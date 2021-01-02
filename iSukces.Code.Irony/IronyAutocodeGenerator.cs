using System;
using System.Collections.Generic;
using System.Linq;
using Irony.Interpreter;
using Irony.Parsing;
using iSukces.Code.AutoCode;
using iSukces.Code.Interfaces;

/*
using AutoCodeGeneratorContextExtensions = iSukces.Code.AutoCode.AutoCodeGeneratorContextExtensions;
using IAutoCodeGeneratorContext = iSukces.Code.AutoCode.IAutoCodeGeneratorContext;
*/

namespace iSukces.Code.Irony
{
    public partial class IronyAutocodeGenerator
    {
        public IronyAutocodeGenerator(GrammarNames names) => Cfg.Names = names;

        private static string GetDebugFromRule(NonTerminalInfo i, ITypeNameResolver res,
            IronyAutocodeGeneratorModel cfg)
        {
            string ff(ICsExpression src, string begin = null, string end = null)
            {
                if (!(src is ITokenNameSource tns)) return null;
                var w = cfg.GetAstTypesInfoDelegate(tns)?.Invoke(res);
                if (w is null)
                    return null;
                var s = $"{w.AstType}, {w.DataType}".Trim();
                if (string.IsNullOrEmpty(s))
                    return s;
                return begin + s + end;
            }

            var debug = "rule = " + i.Rule?.GetType();
            switch (i.Rule)
            {
                case RuleBuilder.Alternative alternative:
                    var alts = alternative.GetAlternatives()
                        .Select(a => a.GetCode(res));
                    debug = "rule = alternative: " + string.Join(", ", alts);
                    break;
                /*
                case RuleBuilder.ListAlternative listAlternative:
                    break;
                case RuleBuilder.OptionAlternative optionAlternative:
                    break;
                    */

                case RuleBuilder.BinaryRule binaryRule:
                    break;
                case RuleBuilder.PlusOrStar plusOrStar:
                    if ((plusOrStar.Options & TermListOptions2.AllowEmpty) != 0)
                        debug = "zero or more";
                    else
                        debug = "one or more";
                    debug += " " + plusOrStar.Element.GetTokenName().GetCode(res);
                    debug += ff(plusOrStar.Element.GetTokenName());
                    break;
                case RuleBuilder.SequenceRule sequenceRule:
                    debug = "";
                    if ((sequenceRule.Map?.Count ?? 0) > 0)
                        foreach (var ii in sequenceRule.Map)
                        {
                            var q = sequenceRule.Expressions[ii.RuleItemIndex];
                            if (debug.Length > 0)
                                debug += ", ";
                            debug += q.Expression.GetCode(res) + " " + ff(q.Expression, " [", "]");
                        }

                    debug = "sequence of " + debug;

                    break;
            }

            return debug;
        }

        private static string OptimalJoin(IReadOnlyList<string> items) => string.Join("\r\n    | ", items);


        public void Generate(IAutoCodeGeneratorContext context)
        {
            foreach (var i in Cfg.NonTerminals)
                if (i.AstBaseClassTypeName is null)
                    i.AstBaseClassTypeName = new TypeNameProvider(Cfg.DefaultAstBaseClass);

            var fullClassName = Cfg.Names.GrammarType.FullName;
            _grammarClass = context.GetOrCreateClass(fullClassName,
                CsNamespaceMemberKind.Class);
            _grammarClass.WithBaseClass(_grammarClass.Owner.GetTypeName<InterpretedLanguageGrammar>());
            var initCode = Add_Fields()
                .Where(a => a != null)
                .ToArray();
            Add_AutoInit(initCode);
            var astClassesGenerator = new AstClassesGenerator(context, Cfg);
            astClassesGenerator.Add_AstClasses();
            Add_AstInterfaces(context);
            new DataClassesGenerator(context, Cfg).Add_DataClasses();

            if (Cfg.DoEvaluateHelper != null)
                astClassesGenerator.Update(Cfg.DoEvaluateHelper);
        }

        public SequenceRuleBuilder GetSequenceRuleBuilder() =>
            new SequenceRuleBuilder
            {
                ProcessToken       = Cfg.GetAstTypesInfoDelegate,
                GetTokenInfoByName = Cfg.GetTokenInfoByName
            };

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

        private void Add_AstInterfaces(IAutoCodeGeneratorContext context)
        {
            foreach (var info in Cfg.NonTerminals)
                switch (info.Rule)
                {
                    case null:
                        continue;
                    case RuleBuilder.Alternative altRule:
                        var ip = altRule.AlternativeInterfaceName;
                        if (ip.IsInterface && ip.CreateAutoCode)
                        {
                            var pn = ip.Provider.GetTypeName(context.FileLevelResolver, Cfg.Names.AstNamespace);
                            var iface = context.GetOrCreateClass(pn.Name,
                                CsNamespaceMemberKind.Interface);
                            iface.Description = altRule.GetDesc();
                            iface.Visibility  = Visibilities.Public;
                        }

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

        private void Add_AutoInit(FieldCreationInfo[] initCode)
        {
            var code = CsCodeWriter.Create<IronyAutocodeGenerator>();

            if (initCode.Any())
            {
                code.WriteLine("// init 1");
                foreach (var i in initCode)
                {
                    code.WriteLine($"{i.FieldName} = {i.SetValueCode};");
                    if (i.OtherCode == null || !i.OtherCode.Any()) continue;
                    foreach (var oc in i.OtherCode) code.WriteLine(oc);
                }
            }

            code.WriteLine("// == init Terminals");
            if (Cfg.Terminals.Any())
                foreach (var i in Cfg.Terminals)
                {
                    var fieldName = i.Name.GetCode(_grammarClass);
                    code.WriteLine($"{fieldName} = ToTerm({i.Code.CsEncode()});");
                }

            code.WriteLine("// == init NonTerminals");
            foreach (var i in Cfg.NonTerminals)
            {
                var fieldName = i.Name.GetCode(_grammarClass);
                var rule      = i.Rule?.GetCode(_grammarClass);
                if (string.IsNullOrEmpty(rule)) continue;
                code.WriteLine($"{fieldName}.Rule = {rule};");
            }

            // add brackets
            if (Cfg.BracketsPairs.Any())
            {
                code.WriteLine("// == brackets");
                foreach (var i in Cfg.BracketsPairs)
                {
                    var ab = new CsArgumentsBuilder()
                        .AddValue(i.Item1)
                        .AddValue(i.Item2);
                    code.WriteLine(ab.CallMethod("RegisterBracePair", true));
                }
            }

            void Add(TerminalsList x, string method, string title)
            {
                if (!x.Any()) return;
                code.WriteLine("// == mark " + title);
                var ab = new CsArgumentsBuilder();
                foreach (var i in x)
                    ab.AddValue(i.Code);
                code.WriteLine(ab.CallMethod(method, true));
                code.WriteLine();
            }

            Add(Cfg.ReservedWords, "MarkReservedWords", "reserved words");
            Add(Cfg.Punctuations, "MarkPunctuation", "punctuations");

            if (Cfg.SingleLineComment != null)
                code.WriteLine($"NonGrammarTerminals.Add({Cfg.SingleLineComment.Name});");
            if (Cfg.DelimitedComment != null)
                code.WriteLine($"NonGrammarTerminals.Add({Cfg.DelimitedComment.Name});");

            if (Cfg.Root != null) code.WriteLine("Root = " + Cfg.Root.Name.GetCode(_grammarClass) + ";");
            _grammarClass.AddMethod("AutoInit", "void")
                .WithBody(code);
        }

        private IEnumerable<FieldCreationInfo> Add_Fields()
        {
            var fields = new List<CsClassField>();

            FieldCreationInfo MakeFromFactory<T>(SpecialTerminalKind key, string methodName)
            {
                if (!Cfg.SpecialTerminals.TryGetValue(key, out var name)) return null;
                var tn         = _grammarClass.GetTypeName<T>();
                var factory    = _grammarClass.GetTypeName(typeof(TerminalFactory));
                var constValue = $"{factory}.{methodName}({name.CsEncode()})";
                var field = new CsClassField(new TokenName(name).GetCode(_grammarClass),
                    tn); //.WithConstValue(constValue);
                fields.Add(field);
                field.UserAnnotations["o"] = 1;
                if (key == SpecialTerminalKind.CreateCSharpNumber)
                    if (Cfg.CSharpNumberLiteralOptions != NumberOptions.None)
                    {
                        var code = _grammarClass.GetEnumFlagsValueCode(Cfg.CSharpNumberLiteralOptions, OptimalJoin);
                        code = field.Name + ".Options = " + code + ";";
                        return new FieldCreationInfo(field.Name, constValue, code);
                    }

                return new FieldCreationInfo(field.Name, constValue);
            }

            yield return MakeFromFactory<IdentifierTerminal>(SpecialTerminalKind.CreateCSharpIdentifier,
                "CreateCSharpIdentifier");
            yield return MakeFromFactory<StringLiteral>(SpecialTerminalKind.CreateCSharpString, "CreateCSharpString");
            yield return MakeFromFactory<NumberLiteral>(SpecialTerminalKind.CreateCSharpNumber, "CreateCSharpNumber");

            Cfg.SingleLineComment?.AddTo(_grammarClass);
            Cfg.DelimitedComment?.AddTo(_grammarClass);

            var tnKeyTerm = _grammarClass.GetTypeName<KeyTerm>();
            foreach (var i in Cfg.Terminals)
            {
                var field = new CsClassField(i.Name.GetCode(_grammarClass), tnKeyTerm);
                field.UserAnnotations["o"] = 2;
                fields.Add(field);
            }

            var tnNonTerminal = _grammarClass.GetTypeName<NonTerminal>();
            foreach (var i in Cfg.NonTerminals)
            {
                ITypeNameProvider astClassNameSrc; // ? i.AstClassTypeName2 : i.AstBaseClassTypeName;
                if (i.AstClass.BuiltInOrAutocode)
                    astClassNameSrc = i.AstClass.Provider;
                else
                    astClassNameSrc = i.AstBaseClassTypeName;
                var astClassName = astClassNameSrc.GetTypeName(_grammarClass, Cfg.Names.AstNamespace)?.Name;

                /*
                if (astClassName.StartsWith("."))
                    astClassName = Cfg.TargetNamespace + astClassName;
                */
                var args = new CsArgumentsBuilder()
                    .AddValue(i.Name.Name);
                var skip = string.IsNullOrEmpty(astClassName)
                           || NestedGeneratorBase.SkipCreateClass(i);
                if (!skip)
                    args.AddCode($"typeof({astClassName})");

                var constValue = $"new {tnNonTerminal}{args.CodeEx}";
                var field      = new CsClassField(i.Name.GetCode(_grammarClass), tnNonTerminal);
                field.UserAnnotations["o"] = 3;
                fields.Add(field);
                yield return new FieldCreationInfo(field.Name, constValue);
            }

            foreach (var field in fields
                .OrderBy(a =>
                {
                    var ord = (int)a.UserAnnotations["o"];
                    return ord;
                })
                .ThenBy(a => a.Name))
            {
                field.Visibility = Visibilities.Private;
                _grammarClass.Fields.Add(field);
            }
        }

        public IronyAutocodeGeneratorModel Cfg { get; } = new IronyAutocodeGeneratorModel();

        private CsClass _grammarClass;

        private class FieldCreationInfo
        {
            public FieldCreationInfo(string fieldName, string setValueCode, params string[] otherCode)
            {
                FieldName    = fieldName;
                SetValueCode = setValueCode;
                OtherCode    = otherCode;
            }

            public string   FieldName    { get; }
            public string   SetValueCode { get; }
            public string[] OtherCode    { get; }
        }
    }
}