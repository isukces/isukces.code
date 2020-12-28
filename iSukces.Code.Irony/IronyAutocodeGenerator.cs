#if DEBUG
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Irony.Interpreter;
using Irony.Parsing;
using iSukces.Code.AutoCode;
using iSukces.Code.CodeWrite;
using iSukces.Code.Interfaces;

namespace iSukces.Code.Irony
{
    public class IronyAutocodeGenerator
    {
        public IronyAutocodeGenerator(string targetNamespace)
        {
            Cfg.TargetNamespace = targetNamespace;
        }

        public static string CsEncode2(string x)
        {
            const string quote     = "\"";
            const string backslash = "\\";
            if (x is null)
                return "null";
            var sb = new StringBuilder();
            sb.Append(quote);
            foreach (var i in x)
                switch (i)
                {
                    case '\\':
                        sb.Append(backslash + backslash);
                        break;
                    case '\r':
                        sb.Append(backslash + "r");
                        break;
                    case '\n':
                        sb.Append(backslash + "n");
                        break;
                    case '\t':
                        sb.Append(backslash + "t");
                        break;
                    case '\"':
                        sb.Append(backslash + quote);
                        break;
                    default:
                        if (i < '\u2000')
                        {
                            sb.Append(i);
                        }
                        else
                        {
                            var ord = ((int)i).ToString("x4", CultureInfo.InvariantCulture);
                            sb.Append("\\u" + ord);
                        }

                        break;
                }

            sb.Append(quote);
            return sb.ToString();
        }


        public void Generate(IAutoCodeGeneratorContext context)
        {
            foreach (var i in Cfg.NonTerminals)
                if (i.AstBaseClassTypeName is null)
                    i.AstBaseClassTypeName = new MethodCsExpression(r => r.GetTypeName(Cfg.DefaultBaseClass));

            var fullClassName = Cfg.TargetNamespace + ".AmmyGrammar";
            _csc = context.GetOrCreateClass(fullClassName, CsNamespaceMemberKind.Class);
            _csc.WithBaseClass(_csc.Owner.GetTypeName<InterpretedLanguageGrammar>());
            var initCode = Add_Fields()
                .Where(a => a != null)
                .ToArray();
            Add_AutoInit(initCode);
            Add_AstClasses(context);
            Add_AstInterfaces(context);
            Add_DataClasses(context);
        }

        public SequenceRuleBuilder GetSequenceRuleBuilder()
        {
            return new SequenceRuleBuilder
            {
                ProcessToken = exp =>
                {
                    switch (exp)
                    {
                        case NonTerminalInfo nti:
                            return q => Cfg.TargetNamespace + "." + nti.AstClassTypeName;
                        case TerminalName tn:
                        {
                            foreach (var i in Cfg.SpecialTerminals)
                                if (i.Value == tn.Name)
                                    return q =>
                                    {
                                        var t = i.Key.GetAstClass();
                                        return q.GetTypeName(t);
                                    };

                            break;
                        }
                    }

                    /*if (Equals(exp, identifier))
                        return q => q.GetTypeName(typeof(IdentifierTerminal));*/
                    throw new NotImplementedException();
                }
            };
        }

        public IronyAutocodeGenerator With(string terminalName, Action<NonTerminalInfo> process = null)
        {
            var info = NonTerminalInfo.Parse(terminalName);
            return With(info, process);
        }

        public IronyAutocodeGenerator With(NonTerminalInfo info, Action<NonTerminalInfo> process = null)
        {
            if (process != null)
                process(info);
            Cfg.NonTerminals.Add(info);
            return this;
        }

        public IronyAutocodeGenerator With(params NonTerminalInfo[] infos)
        {
            Cfg.NonTerminals.AddRange(infos);
            return this;
        }


        private void Add_AstClasses(IAutoCodeGeneratorContext context)
        {
            void Add_GetMap(CsClass c, NonTerminalInfo i)
            {
                c.BaseClass = i.GetBaseClass(Cfg.NonTerminals, c.Owner);
                var map = (i.Rule as IMap12)?.Map;
                if (map == null || map.Count <= 0) return;
                var map2 = string.Join(", ", map.Select(a => a.Index));
                c.AddMethod("GetMap", "int[]")
                    .WithVisibility(Visibilities.Protected)
                    .WithOverride()
                    .WithBody($"return new [] {{ {map2} }};");
                /*for (var index = 0; index < map.Count; index++)
                {
                    var ii = map[index];
                    if (string.IsNullOrEmpty(ii.PropertyName))
                        continue;
                    var classType = ii.PropertyType?.Invoke(c);
                    if (string.IsNullOrEmpty(classType))
                        continue;
                    c.AddProperty(ii.PropertyName, classType)
                        .WithNoEmitField()
                        .WithMakeAutoImplementIfPossible();
                }*/
            }

            foreach (var i in Cfg.NonTerminals.Where(a => a.CreateClass))
            {
                var fullClassName = GetFileLevelTypeName(i.AstClassTypeName);
                var c = context.GetOrCreateClass(fullClassName, CsNamespaceMemberKind.Class)
                    .WithVisibility(Visibilities.Public);
                Add_GetMap(c, i);
            }
        }

        private void Add_AstInterfaces(IAutoCodeGeneratorContext context)
        {
            foreach (var info in Cfg.NonTerminals)
                switch (info.Rule)
                {
                    case null:
                        continue;
                    case RuleBuilder.Alternative altRule:
                        var interfaceName = Cfg.TargetNamespace + "." + altRule.AlternativeInterfaceName;
                        var iface         = context.GetOrCreateClass(interfaceName, CsNamespaceMemberKind.Interface);
                        iface.Description = altRule.GetDesc();
                        break;
                    case RuleBuilder.BinaryRule binaryRule:
                        break;
                    case RuleBuilder.PlusOrStar plus:
                        break;
                    case RuleBuilder.SequenceRule sequenceRule:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
        }

        private void Add_AutoInit(Tuple<string, string>[] initCode)
        {
            var code = CsCodeWriter.Create<IronyAutocodeGenerator>();

            if (initCode.Any())
            {
                code.WriteLine("// init 1");
                foreach (var i in initCode) code.WriteLine($"{i.Item1} = {i.Item2};");
            }

            code.WriteLine("// init Terms");
            if (Cfg.Terminals.Any())
                foreach (var i in Cfg.Terminals)
                {
                    var fieldName = i.Name.GetCode(_csc);
                    code.WriteLine($"{fieldName} = ToTerm({i.Code.CsEncode()});");
                }

            code.WriteLine("// init NonTerminals");
            foreach (var i in Cfg.NonTerminals)
            {
                var fieldName = i.Name.GetCode(_csc);
                var rule      = i.Rule?.GetCode(_csc);
                if (string.IsNullOrEmpty(rule)) continue;
                code.WriteLine($"{fieldName}.Rule = {rule};");
            }

            if (Cfg.SingleLineComment != null)
                code.WriteLine($"NonGrammarTerminals.Add({Cfg.SingleLineComment.Name});");
            if (Cfg.DelimitedComment != null)
                code.WriteLine($"NonGrammarTerminals.Add({Cfg.DelimitedComment.Name});");

            if (Cfg.Root != null) code.WriteLine("Root = " + Cfg.Root.Name.GetCode(_csc) + ";");
            _csc.AddMethod("AutoInit", "void").WithBody(code);
        }

        private void Add_DataClasses(IAutoCodeGeneratorContext context)
        {
            void Add_GetMap(CsClass c, NonTerminalInfo i)
            {
                c.BaseClass = i.GetBaseClass(Cfg.NonTerminals, c.Owner);
                var map = (i.Rule as IMap12)?.Map;
                if (map == null || map.Count <= 0) return;
                var map2 = string.Join(", ", map.Select(a => a.Index));
                c.AddMethod("GetMap", "int[]")
                    .WithVisibility(Visibilities.Protected)
                    .WithOverride()
                    .WithBody($"return new [] {{ {map2} }};");
                /*for (var index = 0; index < map.Count; index++)
                {
                    var ii = map[index];
                    if (string.IsNullOrEmpty(ii.PropertyName))
                        continue;
                    var classType = ii.PropertyType?.Invoke(c);
                    if (string.IsNullOrEmpty(classType))
                        continue;
                    c.AddProperty(ii.PropertyName, classType)
                        .WithNoEmitField()
                        .WithMakeAutoImplementIfPossible();
                }*/
            }

            CsFile file = null;

            void Create(NonTerminalInfo i)
            {
                if (!i.CreateDataClass)
                    return;
                var fullClassName = GetFileLevelTypeName(i.DataClassName);
                if (string.IsNullOrEmpty(fullClassName))
                    return;
                var c = context
                    .GetOrCreateClass(fullClassName, CsNamespaceMemberKind.Class)
                    .WithVisibility(Visibilities.Public);
            }

            foreach (var i in Cfg.NonTerminals) Create(i);
        }

        private IEnumerable<Tuple<string, string>> Add_Fields()
        {
            Tuple<string, string> MakeFromFactory<T>(SpecialTerminalKind key, string methodName)
            {
                if (!Cfg.SpecialTerminals.TryGetValue(key, out var name)) return null;
                var tn         = _csc.GetTypeName<T>();
                var factory    = _csc.GetTypeName(typeof(TerminalFactory));
                var constValue = $"{factory}.{methodName}({name.CsEncode()})";
                var field      = _csc.AddField(new TerminalName(name).GetCode(_csc), tn); //.WithConstValue(constValue);

                return new Tuple<string, string>(field.Name, constValue);
            }

            yield return MakeFromFactory<IdentifierTerminal>(SpecialTerminalKind.CreateCSharpIdentifier,
                "CreateCSharpIdentifier");
            yield return MakeFromFactory<StringLiteral>(SpecialTerminalKind.CreateCSharpString, "CreateCSharpString");
            yield return MakeFromFactory<NumberLiteral>(SpecialTerminalKind.CreateCSharpNumber, "CreateCSharpNumber");

            Cfg.SingleLineComment?.AddTo(_csc);
            Cfg.DelimitedComment?.AddTo(_csc);

            var tnKeyTerm = _csc.GetTypeName<KeyTerm>();
            foreach (var i in Cfg.Terminals) _csc.AddField(i.Name.GetCode(_csc), tnKeyTerm);

            var tnNonTerminal = _csc.GetTypeName<NonTerminal>();
            foreach (var i in Cfg.NonTerminals)
            {
                var astClassNameSrc = i.CreateClass ? i.AstClassTypeName : i.AstBaseClassTypeName;
                var astClassName    = astClassNameSrc.GetCode(_csc);
                if (astClassName.StartsWith("."))
                    astClassName = Cfg.TargetNamespace + astClassName;
                var constValue      = $"new {tnNonTerminal}({i.Name.Name.CsEncode()}, typeof({astClassName}))";
                var field           = _csc.AddField(i.Name.GetCode(_csc), tnNonTerminal);
                yield return new Tuple<string, string>(field.Name, constValue);
            }
        }

        private string GetFileLevelTypeName(ICsExpression ex)
        {
            var name = ex?.GetCode(FileLevelResolver)?.Trim();

            if (string.IsNullOrEmpty(name))
                return null;
            if (name.StartsWith("."))
                return Cfg.TargetNamespace + name;
            return name;
        }

        public ITypeNameResolver FileLevelResolver { get; set; } = new FullNameTypeNameResolver();

        public IronyAutocodeGeneratorModel Cfg { get; } = new IronyAutocodeGeneratorModel();

        private CsClass _csc;

        public class FullNameTypeNameResolver : ITypeNameResolver, INamespaceContainer
        {
            public string GetTypeName(Type type)
            {
                return GeneratorsHelper.GetTypeName(this, type);
            }

            public bool IsKnownNamespace(string namespaceName)
            {
                return false;
            }
        }
    }
}
#endif