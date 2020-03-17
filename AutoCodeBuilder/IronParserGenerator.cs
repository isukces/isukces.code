using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bitbrains.AmmyParser;
using Irony.Interpreter;
using Irony.Parsing;
using isukces.code;
using isukces.code.AutoCode;
using isukces.code.interfaces;

namespace AutoCodeBuilder
{
    internal class IronParserGenerator : IAssemblyAutoCodeGenerator
    {
        public IronParserGenerator(AmmyGrammarAutogeneratorInfo[] items, Type grammarType)
        {
            _items       = items;
            _grammarType = grammarType;
        }

        public static IEnumerable<string> GetImplementedInterfaces(AmmyGrammarAutogeneratorInfo info,
            AmmyGrammarAutogeneratorInfo[] nt)
        {
            var l = new List<string>();

            foreach (var i in nt)
                if (IsAlternativeFor(info, i))
                    l.Add(i.AlternativeInterfaceName);

            return l.Distinct();
        }

        private static void GenerateGetMap(IReadOnlyCollection<int> map, CsClass grammarClass)
        {
            if (map == null || map.Count == 0) return;
            var body = "return new [] { " + string.Join(", ", map) + " };";
            grammarClass.AddMethod("GetMap", "int[]")
                .WithBody(body)
                .WithOverride()
                .WithVisibility(Visibilities.Protected);
        }

        private static bool IsAlternativeFor(AmmyGrammarAutogeneratorInfo info, AmmyGrammarAutogeneratorInfo i)
        {
            if (i == info || i.Alternatives == null || i.Alternatives.Length == 0)
                return false;
            foreach (var a in i.Alternatives)
                if (a == info.TerminalName)
                    return true;

            return false;
        }

        public void AssemblyEnd(Assembly assembly, IAutoCodeGeneratorContext context)
        {
        }

        public void AssemblyStart(Assembly assembly, IAutoCodeGeneratorContext context)
        {
            context.AddNamespace<NonTerminal>();
            var grammarClass = context.GetOrCreateClass(_grammarType);

            var gi = new List<string>();

            foreach (var i in _items)
            {
                // NonTerminal <#= i.TerminalName #> = new NonTerminal("<#= i.TerminalName #>", typeof(<#= i.EffectiveClassName #>));
                var field = grammarClass.AddField(i.TerminalName, typeof(NonTerminal));
                field.ConstValue = $"new {field.Type}({i.TerminalName.CsEncode()}, typeof({i.EffectiveClassName}))";

                if (!i.CreateClass) continue;

                var tp = TypeProvider.FromTypeName(_grammarType.Namespace + "." + i.EffectiveClassName,
                    CsNamespaceMemberKind.Class);
                var cl = context.GetOrCreateClass(tp);
                cl.BaseClass   = i.BaseClass;
                cl.Description = "AST class for " + i.TerminalName + " terminal";
                foreach (var ii in GetImplementedInterfaces(i, _items))
                {
                    cl.ImplementedInterfaces.Add(ii + "Provider");
                    gi.Add(ii);
                }

                GenerateGetMap(i.Map, cl);
            }

            {
                CodeWriter body = CsCodeWriter.Create(SourceCodeLocation.Make());
                foreach (var i in _items)
                {
                    var altCode = i.GetAlternatives().ToArray();
                    if (altCode.Any()) body.WriteLine(i.TerminalName + ".Rule = " + string.Join(" | ", altCode) + ";");
                }

                grammarClass.AddMethod("AutoInit", "void").WithBody(body);
            }

            {
                foreach (var name in gi.Distinct().OrderBy(a => a))
                {
                    var tp = TypeProvider.FromTypeName(_grammarType.Namespace + "." + name,
                        CsNamespaceMemberKind.Interface);
                    context.GetOrCreateClass(tp).WithVisibility(Visibilities.Public);

                    tp = TypeProvider.FromTypeName(_grammarType.Namespace + "." + name + "Provider",
                        CsNamespaceMemberKind.Interface);
                    var cl = context.GetOrCreateClass(tp).WithVisibility(Visibilities.Public);
                    var m  = cl.AddMethod("GetData", name);
                    m.AddParam<ScriptThread>("thread", cl);
                }
            }
        }

        private readonly AmmyGrammarAutogeneratorInfo[] _items;
        private readonly Type _grammarType;
    }
}