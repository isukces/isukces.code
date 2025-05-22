using System;
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

#nullable disable
namespace iSukces.Code.Irony
{
    public partial class IronyAutocodeGenerator
    {
        internal class AstClassesGenerator : NestedGeneratorBase
        {
            public AstClassesGenerator(IAutoCodeGeneratorContext context, IronyAutocodeGeneratorModel cfg)
                : base(context, cfg)
            {
            }


            public void Add_AstClasses()
            {
                _tokensToCreateAstClasses = new List<NonTerminalInfo>();
                var man = new NamespacesManager();
                foreach (var info in _cfg.NonTerminals.Where(a => a.AstClass.CreateAutoCode))
                {
                    if (SkipCreateClass(info))
                        continue;
                    if (!info.AstClass.CreateAutoCode)
                        return;

                    _tokensToCreateAstClasses.Add(info);
                    if (!info.DataClass.CreateAutoCode) continue;
                    var fulldata      = GetFileLevelTypeNameData(info.DataClass.Provider);
                    var fullClassName = GetFileLevelTypeNameAst(info.AstClass.Provider);
                    man.ShouldSee(fullClassName, fulldata);
                }

                foreach (var i in _tokensToCreateAstClasses)
                {
                    _terminal = i;
                    var fullClassName = GetFileLevelTypeNameAst(i.AstClass.Provider).Name;
                    _astClass = _context.GetOrCreateClass(fullClassName, CsNamespaceMemberKind.Class)
                        .WithVisibility(Visibilities.Public);

                    if (_astClass.Owner is CsNamespace ns) man.Apply(fullClassName, ns);

                    _astClass.BaseClass =
                        (CsType)_terminal.GetBaseClassAndInterfaces(_cfg.NonTerminals, _astClass.Owner,
                            _astClass.GetNamespace());
                    var debug = GetDebugFromRule(i, _astClass, _cfg);
                    _astClass.Description = debug;

                    i.CreationInfo.AstClass = _astClass;
                    Add_GetMap();
                    Add_Init();
                }
            }

            public void Update(IDoEvaluateHelper cew)
            {
                foreach (var i in _tokensToCreateAstClasses)
                {
                    _astClass = i.CreationInfo.AstClass;
                    if (_astClass is null)
                        continue;
                    DoEvaluateMethodMakerBase maker = null;
                    switch (i.Rule)
                    {
                        case RuleBuilder.ListAlternative _:
                            maker = new DoEvaluateMethodMakerBase.ForAlternative(i, cew, _astClass);
                            break;
                        case RuleBuilder.PlusOrStar _:
                            maker = new DoEvaluateMethodMakerBase.ForPlusOrStar(i, cew, _astClass);
                            break;
                        case RuleBuilder.SequenceRule _:
                            maker = new DoEvaluateMethodMakerBase.ForSequenceRule(i, cew, _astClass);
                            break;
                    }

                    maker?.Create();
                }
            }

            private void Add_GetMap()
            {
                var map = (_terminal.Rule as IMap12)?.Map;
                if (map == null || map.Count <= 0) return;
                var map2 = string.Join(", ", map.Select(a => a.AstIndex));
                var expression = $"new [] {{ {map2} }}";
                var m = _astClass.AddMethod("GetMap", CsType.Int32.MakeArray())
                    .WithVisibility(Visibilities.Protected)
                    .WithOverride()
                    .WithBodyAsExpression(expression);
                m.AddCommentLocation<AstClassesGenerator>("created");
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

                var m = _astClass.AddMethod("Init", CsType.Void)
                    .WithVisibility(Visibilities.Public)
                    .WithOverride()
                    .WithBody(initWriter);
                m.AddParam("context", _astClass.GetTypeName<AstContext>());
                m.AddParam("treeNode", _astClass.GetTypeName<ParseTreeNode>());
            }


            private string GetAsString()
            {
                switch (_terminal.Rule)
                {
                    case RuleBuilder.PlusOrStar plusOrStar:
                        return "Collection of " + plusOrStar.Element.GetTokenName().Name;
                    case RuleBuilder.SequenceRule sequenceRule:
                        return "Sequence " + _terminal.Name.Name;
                    default:
                        return null;
                }
            }

            private string GetNodeOrAstType(IAstTypesInfo info)
            {
                var type = info.NodeType;
                if (type.IsVoid)
                {
                    type = info.AstType;
                    if (type.IsVoid)
                        return null;
                }
#if IGNORECSTYPE
                throw new NotImplementedException();
#else
                type = _astClass.ReduceTypenameIfPossible(type);
                return type;
#endif
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

                //var pn = _cfg.Names.AstNamespace + "." + rule.AlternativeInterfaceName;
                var pn = rule.AlternativeInterfaceName.Provider.GetTypeName(_astClass.Owner, _cfg.Names.AstNamespace);
                if (alts.Length == 1)
                {
                }

                pn = new FullTypeName(_astClass.Owner.GetTypeName<AstNode>());

                var csProp = _astClass.AddProperty("OptionNode", pn.Name);
                csProp.AddCommentLocation<AstClassesGenerator>("created");
                ProcessProperty(csProp, false);
                csProp.SetterVisibility = Visibilities.Private;

                csProp.SetterType            = PropertySetter.None;
                csProp.OwnGetterIsExpression = true;
                csProp.OwnGetter             = "ChildNodes[0]";
                if (alts.Length > 1)
                {
                    var l = new List<CsEnumItem>();
                    l.Add(new CsEnumItem("Unknown"));
                    foreach (var i in alts)
                    {
                        var name = i.GetCode(_astClass.Owner);
                        name = CSharpExtension.GetCamelTerminalName(name);
                        l.Add(new CsEnumItem(name));
                    }

                    var en = new CsEnum(_astClass.Name + "NodeKinds", l.ToArray());
                    ((CsNamespace)_astClass.Owner).AddEnum(en);

                    var b = CsCodeWriter.Create<AstClassesGenerator>();
                    b.Open("switch (" + csProp.Name + ")");
                    for (var index = 0; index < alts.Length; index++)
                    {
                        var i = alts[index];
                        if (i is ITokenNameSource ts)
                        {
                            var q = _cfg.GetAstTypesInfoDelegate(ts);
                            if (q != null)
                            {
                                var aa = q.Invoke(_astClass);
                                b.WriteLine("// AstType = " + aa.AstType);
                                b.WriteLine("// DataType = " + aa.DataType);
                                b.WriteLine("// NodeType = " + aa.NodeType);
                                b.WriteLine("case " + aa.NodeType + " _:")
                                    .IncIndent()
                                    .WriteLine("return " + en.Name + "." + l[index + 1].EnumName + ";")
                                    .DecIndent();
                            }
                        }
                    }

                    b.Close();
                    b.WriteLine($"return {en.Name}.{l[0].EnumName};");
                    var m = _astClass.AddMethod("GetNodeKind", (CsType)en.Name)
                        .WithBody(b);
                    rule.CreationInfo.Enum1               = en;
                    _astClass.UserAnnotations[Exchange.X] = new Exchange.Du(m.Name);
                }
            }

            private void Process_OptionAlternative(RuleBuilder.OptionAlternative rule, CsCodeWriter initWriter)
            {
                var alts = (NonTerminalInfo)rule.Info;
                var pn   = GetFileLevelTypeNameAst(alts.AstClass.Provider).Name;
#if IGNORECSTYPE
                throw new NotImplementedException();
#else
                // pn = _astClass.ReduceTypenameIfPossible(pn);
                var csProp = _astClass.AddProperty("Optional", pn);
                csProp.AddCommentLocation<AstClassesGenerator>("created");
                ProcessProperty(csProp, false);
                csProp.SetterVisibility = Visibilities.Private;
#endif
            }

            private void Process_PlusOrStar(RuleBuilder.PlusOrStar rule)
            {
                var el       = rule.Element;
                var info     = _cfg.GetAstTypesInfoDelegate(el)?.Invoke(_astClass);
                CsType itemType = new CsType(GetNodeOrAstType(info));

                {
                    var ex = new CsExpression("childNode");
                    if (info.GetEvaluateExpression != null)
                    {
                        var cfg = new GetEvaluateExpressionInput(ex, new CsExpression("thread"), true);
                        var ex1 = info.GetEvaluateExpression(cfg);
                        if (ex1.Expression != null)
                        {
                            if (info.DataType.IsVoid)
                            {
                                Debug.WriteLine("No datatype " + el);
                            }
                            else
                            {
                                var listType = MakeList(_astClass, info.DataType, typeof(IReadOnlyList<>));
                                var code     = new CsCodeWriter();
                                code.WriteLine("var cnt = ChildNodes.Count;");
                                code.WriteLine("var result = new " + info.DataType + "[cnt];");
                                code.Open("for (var i = cnt - 1; i >= 0; i--)");
                                code.WriteLine("var childNode = ChildNodes[i];");
                                code.WriteLine($"result[i] = {ex1.Expression.Code};");
                                code.Close();
                                code.WriteLine("return result;");
                                var m = _astClass.AddMethod("EvaluateItems", (CsType)listType).WithBody(code);
                                if (ex1.NeedScriptThread)
                                    m.AddParam<ScriptThread>("thread", _astClass);
                                m.UserAnnotations[Exchange.MethodForEvaluatingPropertyKey] =
                                    new Exchange.MethodForEvaluatingProperty("Items", m.Name, ex1.NeedScriptThread);
                            }
                        }
                        else
                        {
                            Debug.WriteLine("No GetEvaluateExpression " + el);
                        }
                    }
                }
                {
                    var listType = MakeList(_astClass, itemType, typeof(IEnumerable<>));
                    var code     = new CsCodeWriter();
                    code.WriteLine("var cnt = ChildNodes.Count;");
                    code.Open("for (var i = 0; i < cnt; i++)");
                    code.WriteLine("var el = ChildNodes[i];");
                    code.WriteLine($"yield return ({itemType})el;");
                    code.Close();
                    _astClass.AddMethod("GetItems", (CsType)listType).WithBody(code);
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

                    var t = map.PropertyType?.Invoke(_astClass);

                    var propertyType = GetNodeOrAstType(t);
                    if (string.IsNullOrEmpty(propertyType)) continue;

                    var csProp = _astClass.AddProperty(map.PropertyName, (CsType)propertyType);
                    csProp.AddCommentLocation<AstClassesGenerator>("created");
                    ProcessProperty(csProp, false);
                    csProp.SetterVisibility = Visibilities.Private;
                    csProp.Description      = "Index = " + i.AstIndex;
                    var propType = csProp.Type.AsString(_astClass.AllowReferenceNullable());
                    var code = string.Format("({0})ChildNodes[{1}];",
                        propType,
                        i.AstIndex.ToString(CultureInfo.InvariantCulture));
                    csProp.SetterType            = PropertySetter.None;
                    csProp.OwnGetter             = code;
                    csProp.OwnGetterIsExpression = true;

                    var input = new GetEvaluateExpressionInput(
                        new CsExpression(csProp.Name),
                        new CsExpression("thread"),
                        false);

                    var ee           = t.GetEvaluateExpression(input);
                    var evaluateCode = ee.Expression?.Code;

                    bool need;
                    if (t.DataType.Declaration == "string") Debug.WriteLine("");
                    var codeWriter = CsCodeWriter.Create<AstClassesGenerator>();
                    if (string.IsNullOrEmpty(evaluateCode))
                    {
                        need         = true;
                        evaluateCode = $"({t.DataType}){csProp.Name}.Evaluate(thread)";
                        codeWriter.WriteLine($"var tmp = {csProp.Name}.Evaluate(thread);");
                        codeWriter.WriteLine($"var result = ({t.DataType})tmp;");
                    }
                    else
                    {
                        need = ee.NeedScriptThread;
                        if (ee.Expression is CsExpression.Cast c)
                        {
                            codeWriter.WriteLine($"var tmp = {c.CastedExpression?.Code};");
                            codeWriter.WriteLine($"var result = ({c.Type})tmp;");
                        }
                        else
                        {
                            codeWriter.WriteLine($"var result = {evaluateCode};");
                        }
                    }

                    codeWriter.WriteLine("return result;");

                    if (evaluateCode == "((LiteralValueNode)FullTypeName).Symbol")
                        Debug.WriteLine("");
                    var m = _astClass.AddMethod($"Get{csProp.Name}Value", t.DataType)
                        .WithBody(codeWriter);
                    if (need)
                        m.AddParam<ScriptThread>("thread", _astClass);
                    m.UserAnnotations[Exchange.MethodForEvaluatingPropertyKey] =
                        new Exchange.MethodForEvaluatingProperty(i.Map?.PropertyName, m.Name, need);
                }
            }

            private CsClass _astClass;
            private NonTerminalInfo _terminal;
            private List<NonTerminalInfo> _tokensToCreateAstClasses;
        }
    }
}

