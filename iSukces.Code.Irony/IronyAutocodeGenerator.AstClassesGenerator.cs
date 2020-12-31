using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;
using iSukces.Code.AutoCode;
using iSukces.Code.Interfaces;

namespace iSukces.Code.Irony
{
    public partial class IronyAutocodeGenerator
    {
        private class AstClassesGenerator : NestedGeneratorBase
        {
            public AstClassesGenerator(IAutoCodeGeneratorContext context, IronyAutocodeGeneratorModel cfg)
                : base(context, cfg)
            {
            }


            public void Add_AstClasses()
            {
                foreach (var i in _cfg.NonTerminals.Where(a => a.CreateAstClass))
                {
                    if (SkipCreateClass(i))
                        continue;
                    _terminal = i;
                    var fullClassName = GetFileLevelTypeNameAst(i.AstClassTypeName).Name;
                    _cl = _context.GetOrCreateClass(fullClassName, CsNamespaceMemberKind.Class)
                        .WithVisibility(Visibilities.Public);
                    _cl.BaseClass =
                        _terminal.GetBaseClassAndInterfaces(_cfg.NonTerminals, _cl.Owner, _cl.GetNamespace());
                    var debug = GetDebugFromRule(i, _cl, _cfg);
                    _cl.Description = debug;

                    Add_GetMap();
                    Add_Init();
                }
            }

            private void Add_GetMap()
            {
                var map = (_terminal.Rule as IMap12)?.Map;
                if (map == null || map.Count <= 0) return;
                var map2 = string.Join(", ", map.Select(a => a.Index));
                _cl.AddMethod("GetMap", "int[]")
                    .WithVisibility(Visibilities.Protected)
                    .WithOverride()
                    .WithBody($"return new [] {{ {map2} }};");
            }

            private void Add_Init()
            {
                var initWriter = new CsCodeWriter();
                initWriter.WriteLine("base.Init(context, treeNode);");
                var asString = GetAsString();
                if (!string.IsNullOrEmpty(asString))
                    initWriter.WriteLine(nameof(AstNode.AsString) + " = " + asString.CsEncode() + ";");

                switch (_terminal.Rule)
                {
                    case RuleBuilder.OptionAlternative optionAlternative:
                        Process_OptionAlternative(optionAlternative, initWriter);
                        break;
                    case RuleBuilder.Alternative alternative:
                        Process_Alternative(alternative, initWriter);
                        break;
                    case RuleBuilder.PlusOrStar plusOrStar:
                        Process_PlusOrStar(plusOrStar);
                        break;
                    case RuleBuilder.SequenceRule sequenceRule:
                        Process_SequenceRule(sequenceRule, initWriter);
                        break;
                }

                var m = _cl.AddMethod("Init", "void")
                    .WithVisibility(Visibilities.Public)
                    .WithOverride()
                    .WithBody(initWriter);
                m.AddParam("context", _cl.GetTypeName<AstContext>());
                m.AddParam("treeNode", _cl.GetTypeName<ParseTreeNode>());
            }


            private string GetAsString()
            {
                switch (_terminal.Rule)
                {
                    case RuleBuilder.PlusOrStar plusOrStar:
                        return "Collection of " + plusOrStar.Element.GetTerminalName().Name;
                    case RuleBuilder.SequenceRule sequenceRule:
                        return "Sequence " + _terminal.Name.Name;
                    default:
                        return null;
                }
            }

            private string GetNodeOrAstType(IAstTypesInfo info)
            {
                var type = info.NodeType;
                if (string.IsNullOrEmpty(type))
                {
                    type = info.AstType;
                    if (string.IsNullOrEmpty(type))
                        return null;
                }

                type = _cl.ReduceTypenameIfPossible(type);
                return type;
            }

            private void Process_Alternative(RuleBuilder.Alternative rule, CsCodeWriter initWriter)
            {
                /*
                foreach (var i in rule.GetAlternatives())
                {
                    object d = i.GetCode(_cl);
                    _cl.Description += ", " + d;
                }

                _cl.Description += ", " + rule.AlternativeInterfaceName;
                */
                var alts = rule.GetAlternatives().ToArray();
                if (alts.Length == 0)
                    return;
                var pn = _cfg.Names.AstNamespace + "." + rule.AlternativeInterfaceName;
                if (alts.Length == 1)
                {
                }

                var csProp = _cl.AddProperty("Value", pn);
                ProcessProperty(csProp, false);
                csProp.SetterVisibility = Visibilities.Private;
            }

            private void Process_OptionAlternative(RuleBuilder.OptionAlternative rule, CsCodeWriter initWriter)
            {
                var alts = (NonTerminalInfo)rule.Info;
                var pn   = GetFileLevelTypeNameAst(alts.AstClassTypeName).Name;
                pn = _cl.ReduceTypenameIfPossible(pn);
                var csProp = _cl.AddProperty("Optional", pn);
                ProcessProperty(csProp, false);
                csProp.SetterVisibility = Visibilities.Private;
            }

            private void Process_PlusOrStar(RuleBuilder.PlusOrStar rule)
            {
                var el       = rule.Element;
                var info     = _cfg.GetAstTypesInfoDelegate(el)?.Invoke(_cl);
                var itemType = GetNodeOrAstType(info);

                {
                    var ex = new CsExpression("childNode");
                    if (info.GetEvaluateExpression != null)
                    {
                        var cfg = new GetEvaluateExpressionInput(ex, new CsExpression("thread"));
                        var ex1 = info.GetEvaluateExpression(cfg);
                        if (ex1.Expression != null)
                        {
                            if (string.IsNullOrEmpty(info.DataType))
                            {
                                Debug.WriteLine("No datatype " + el);
                            }
                            else
                            {
                                var listType = MakeList(_cl, info.DataType, typeof(IReadOnlyList<>));
                                var code     = new CsCodeWriter();
                                code.WriteLine("var cnt = ChildNodes.Count;");
                                code.WriteLine("var result = new " + info.DataType + "[cnt];");
                                code.Open("for (var i = cnt - 1; i >= 0; i--)");
                                code.WriteLine("var childNode = ChildNodes[i];");
                                code.WriteLine($"result[i] = {ex1.Expression.Code};");
                                code.Close();
                                code.WriteLine("return result;");
                                var m = _cl.AddMethod("EvaluateItems", listType).WithBody(code);
                                if (ex1.NeedScriptThread)
                                    m.AddParam<ScriptThread>("thread", _cl);
                            }
                        }
                        else
                        {
                            Debug.WriteLine("No GetEvaluateExpression " + el);
                        }
                    }
                }
                {
                    var listType = MakeList(_cl, itemType, typeof(IEnumerable<>));
                    var code     = new CsCodeWriter();
                    code.WriteLine("var cnt = ChildNodes.Count;");
                    code.Open("for (var i = 0; i < cnt; i++)");
                    code.WriteLine("var el = ChildNodes[i];");
                    code.WriteLine($"yield return ({itemType})el;");
                    code.Close();
                    _cl.AddMethod("GetItems", listType).WithBody(code);
                }
                // var items = GetItems().Select(a => a.Symbol).ToArray();
            }


            private void Process_SequenceRule(RuleBuilder.SequenceRule rule, CsCodeWriter initWriter)
            {
                foreach (var i in rule.Enumerate())
                {
                    var map = i.Map;
                    if (map is null || string.IsNullOrEmpty(map.PropertyName))
                        continue;

                    var t            = map.PropertyType?.Invoke(_cl);
                    var propertyType = GetNodeOrAstType(t);
                    if (string.IsNullOrEmpty(propertyType)) continue;
                    var csProp = _cl.AddProperty(map.PropertyName, propertyType);
                    ProcessProperty(csProp, false);
                    csProp.SetterVisibility = Visibilities.Private;
                    csProp.Description      = "Index = " + i.SourceIndex;
                    initWriter.WriteLine(string.Format("{0} = ({1})ChildNodes[{2}];",
                        csProp.Name, csProp.Type, i.SourceIndex.ToString(CultureInfo.InvariantCulture)));
                }
            }


            private CsClass _cl;
            private NonTerminalInfo _terminal;
        }
    }
}