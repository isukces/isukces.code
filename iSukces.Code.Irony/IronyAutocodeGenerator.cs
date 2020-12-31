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
                if (!(src is ITerminalNameSource tns)) return null;
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
                        debug = "zero of more";
                    else
                        debug = "one of more";
                    debug += " " + plusOrStar.Element.GetTerminalName().GetCode(res);
                    debug += ff(plusOrStar.Element.GetTerminalName());
                    break;
                case RuleBuilder.SequenceRule sequenceRule:
                    debug = "";
                    if ((sequenceRule.Map?.Count ?? 0) > 0)
                        foreach (var ii in sequenceRule.Map)
                        {
                            var q = sequenceRule.Expressions[ii.Index];
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
            _context = context;
            foreach (var i in Cfg.NonTerminals)
                if (i.AstBaseClassTypeName is null)
                    i.AstBaseClassTypeName = new TypeTypeNameProvider(Cfg.DefaultAstBaseClass);

            var fullClassName = Cfg.Names.GrammarType.FullName;
            _csc = context.GetOrCreateClass(fullClassName,
                CsNamespaceMemberKind.Class);
            _csc.WithBaseClass(_csc.Owner.GetTypeName<InterpretedLanguageGrammar>());
            var initCode = Add_Fields()
                .Where(a => a != null)
                .ToArray();
            Add_AutoInit(initCode);
            new AstClassesGenerator(context, Cfg).Add_AstClasses();
            Add_AstInterfaces(context);
            new DataClassesGenerator(context, Cfg).Add_DataClasses();
        }

        public SequenceRuleBuilder GetSequenceRuleBuilder() =>
            new SequenceRuleBuilder
            {
                ProcessToken = Cfg.GetAstTypesInfoDelegate
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
                        var interfaceName = Cfg.Names.AstNamespace + "." + altRule.AlternativeInterfaceName;
                        var iface = context.GetOrCreateClass(interfaceName,
                            CsNamespaceMemberKind.Interface);
                        iface.Description = altRule.GetDesc();
                        iface.Visibility  = Visibilities.Public;
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

        private void Add_AutoInit(Tt[] initCode)
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
            _csc.AddMethod("AutoInit", "void")
                .WithBody(code);
        }

        private IEnumerable<Tt> Add_Fields()
        {
            var fields = new List<CsClassField>();

            Tt MakeFromFactory<T>(SpecialTerminalKind key, string methodName)
            {
                if (!Cfg.SpecialTerminals.TryGetValue(key, out var name)) return null;
                var tn = _csc.GetTypeName<T>();
                var factory = _csc.GetTypeName(typeof(TerminalFactory));
                var constValue = $"{factory}.{methodName}({name.CsEncode()})";
                var field = new CsClassField(new TerminalName(name).GetCode(_csc), tn); //.WithConstValue(constValue);
                fields.Add(field);
                field.UserAnnotations["o"] = 1;
                if (key == SpecialTerminalKind.CreateCSharpNumber)
                    if (Cfg.CSharpNumberLiteralOptions != NumberOptions.None)
                    {
                        var code = _csc.GetEnumFlagsValueCode(Cfg.CSharpNumberLiteralOptions, OptimalJoin);
                        code = field.Name + ".Options = " + code + ";";
                        return new Tt(field.Name, constValue, code);
                    }

                return new Tt(field.Name, constValue);
            }

            yield return MakeFromFactory<IdentifierTerminal>(SpecialTerminalKind.CreateCSharpIdentifier,
                "CreateCSharpIdentifier");
            yield return MakeFromFactory<StringLiteral>(SpecialTerminalKind.CreateCSharpString, "CreateCSharpString");
            yield return MakeFromFactory<NumberLiteral>(SpecialTerminalKind.CreateCSharpNumber, "CreateCSharpNumber");

            Cfg.SingleLineComment?.AddTo(_csc);
            Cfg.DelimitedComment?.AddTo(_csc);

            var tnKeyTerm = _csc.GetTypeName<KeyTerm>();
            foreach (var i in Cfg.Terminals)
            {
                var field = new CsClassField(i.Name.GetCode(_csc), tnKeyTerm);
                field.UserAnnotations["o"] = 2;
                fields.Add(field);
            }

            var tnNonTerminal = _csc.GetTypeName<NonTerminal>();
            foreach (var i in Cfg.NonTerminals)
            {
                var astClassNameSrc = i.CreateAstClass ? i.AstClassTypeName : i.AstBaseClassTypeName;
                var astClassName    = astClassNameSrc.GetTypeName(_csc, Cfg.Names.AstNamespace)?.Name;
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
                var field      = new CsClassField(i.Name.GetCode(_csc), tnNonTerminal);
                field.UserAnnotations["o"] = 3;
                fields.Add(field);
                yield return new Tt(field.Name, constValue);
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
                _csc.Fields.Add(field);
            }
        }


        /*private FullTypeName GetFileLevelTypeName(ITypeNameProvider ex)
        {
            var name = ex?.GetTypeName(_context.FileLevelResolver, Cfg.Namespaces);
            return name;
        }*/

        public IronyAutocodeGeneratorModel Cfg { get; } = new IronyAutocodeGeneratorModel();

        private CsClass _csc;
        private IAutoCodeGeneratorContext _context;

        private class Tt
        {
            public Tt(string fieldName, string setValueCode, params string[] otherCode)
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