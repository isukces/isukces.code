#if DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using X=Bitbrains.AmmyParser.AmmyGrammar;

namespace Bitbrains.AmmyParser
{
    public partial class AmmyGrammarAutogeneratorInfo
    {
        public static IEnumerable<object> GetItemsInternal()
        {
            yield return new AmmyGrammarAutogeneratorInfo("int_number_optional")
                .AsOptional(null, nameof(X.Number));
            
            yield return new AmmyGrammarAutogeneratorInfo("literal").AsOneOf();
            yield return "qual_name_segment";
            yield return "qual_name_segments_opt2";
            yield return "qual_name_with_targs";
            yield return "identifier_or_builtin";
            yield return new AmmyGrammarAutogeneratorInfo("using_ns_directive").AsAlternative(0); // , "using_ns_directive");
            {
                var el =  new AmmyGrammarAutogeneratorInfo("using_directive")
                    .AsOneOf("using_ns_directive");
                // yield return new AmmyGrammarAutogeneratorInfo("using_directives").AsListOf("UsingStatement");
                // yield return "using_directives_opt,AstOptNode,false";
                
                var list = el.GetList();
                yield return el;
                yield return list;
                yield return list.GetOptional();
            }
            yield return new AmmyGrammarAutogeneratorInfo(nameof(X.mixin_definition));
                //.WithMap(0, 2, 4, 6);

            {
                var el = new AmmyGrammarAutogeneratorInfo("statement")
                    .AsOneOf(
                        nameof(X.mixin_definition)
                    );

                var list = el.GetList();
                yield return el;
                yield return list;
                yield return list.GetOptional();
            }
            {
                var el = new AmmyGrammarAutogeneratorInfo("object_setting")
                    .AsOneOf("object_property_setting");
                var list = el.GetList();
                yield return el;
                yield return list;
                yield return list.GetOptional();
            }

            {
                yield return new AmmyGrammarAutogeneratorInfo("ammy_bind");
                var el = new AmmyGrammarAutogeneratorInfo("ammy_bind_source").Single();
                yield return el;
                yield return el.GetOptional();

                yield return new AmmyGrammarAutogeneratorInfo("ammy_bind_source_source")
                    .AsAlternative(0, 
                        "ammy_bind_source_ancestor", 
                        "ammy_bind_source_element_name",
                        "ammy_bind_source_this").Single();
                yield return new AmmyGrammarAutogeneratorInfo("ammy_bind_source_ancestor");
                yield return new AmmyGrammarAutogeneratorInfo("ammy_bind_source_element_name").Single();
                yield return new AmmyGrammarAutogeneratorInfo("ammy_bind_source_this");
            }
            yield return new AmmyGrammarAutogeneratorInfo("object_property_setting");
            yield return new AmmyGrammarAutogeneratorInfo("ammy_property_name").AsOneOf("identifier");
            yield return new AmmyGrammarAutogeneratorInfo("ammy_property_value")
                .AsOneOf("primary_expression", "ammy_bind");

            yield return new AmmyGrammarAutogeneratorInfo("primary_expression").AsOneOf("literal");
            yield return new AmmyGrammarAutogeneratorInfo("expression").AsOneOf("primary_expression");

            {
                var el   = new AmmyGrammarAutogeneratorInfo("mixin_or_alias_argument").AsOneOf("identifier");
                //var list = el.GetList();
                var list = new AmmyGrammarAutogeneratorInfo(el.TerminalName + "s").AsListOf<object>();
                yield return el;
                yield return list;
                yield return list.GetOptional();
            }

            yield return new AmmyGrammarAutogeneratorInfo("ammyCode")
                .AsListOf<object>();

        }

        public static AmmyGrammarAutogeneratorInfo[] Items()
        {
            var nt = GetItemsInternal().Select(a =>
            {
                switch (a)
                {
                    case string s:
                        return new AmmyGrammarAutogeneratorInfo(s);
                    case AmmyGrammarAutogeneratorInfo i:
                        return i;
                    default:
                        throw new NotSupportedException();
                }
            }).ToArray();
            return nt;
        }
    }

    public partial class AmmyGrammarAutogeneratorInfo
    {
        public AmmyGrammarAutogeneratorInfo(string terminalName)
        {
            var parts = (terminalName + ",,,,").Split(',').Select(a => a.Trim()).ToArray();
            TerminalName      = parts[0];
            CamelTerminalName = GetCamelName(TerminalName);
            ClassType         = "Ast" + CamelTerminalName;

            BaseClass = parts.Length < 2 ? null : parts[1];
            if (string.IsNullOrEmpty(BaseClass))
                BaseClass = nameof(BbExpressionListNode);
            var createClassCommand = parts[2].ToLower();
            CreateClass = createClassCommand == "" || createClassCommand == "true";
            if (parts[3].Length > 0)
            {
                Map = parts[3].Split(' ').Select(a => a.Trim())
                    .Where(a => a.Length > 0)
                    .Select(int.Parse)
                    .ToArray();
                if (Map.Length == 0)
                    Map = null;
            }
        }


        private static StringBuilder GetCamelName(string name)
        {
            var s       = new StringBuilder();
            var toUpper = true;
            foreach (var i in name.Trim())
            {
                if (i == '_')
                {
                    toUpper = true;
                    continue;
                }

                s.Append(toUpper ? char.ToUpper(i) : i);
                toUpper = false;
            }

            return s;
        }

        public AmmyGrammarAutogeneratorInfo AsAlternative(int index, params string[] elements)
        {
            Map          = new[] {index};
            Alternatives = elements;
            return this;
        }


        public AmmyGrammarAutogeneratorInfo AsListOf<T>() => AsListOf(typeof(T).ToString());

        public AmmyGrammarAutogeneratorInfo AsListOf(AmmyGrammarAutogeneratorInfo info)
        {
            if ((info.Alternatives?.Length ?? 0) > 0)
                return AsListOf(info.AlternativeInterfaceName);
            return AsListOf("object");
        }

        public AmmyGrammarAutogeneratorInfo AsListOf(string listItemName)
        {
            // // "using_directives,ExpressionListNode<UsingStatement>",
            BaseClass = "ExpressionListNode<" + listItemName + ">";
            return this;
        }

        public AmmyGrammarAutogeneratorInfo AsOneOf(params string[] elements) => AsAlternative(0, elements);

        public AmmyGrammarAutogeneratorInfo AsOptional(string baseName = null, string alternativeOf =null)
        {
            BaseClass   = baseName ?? "AstOptNode";
            CreateClass = BaseClass != "AstOptNode";
            if (!string.IsNullOrEmpty(alternativeOf))
                Alternatives = new[] {"Empty", alternativeOf};
            return this;
        }

        public IEnumerable<string> GetAlternatives()
        {
            if (TerminalName.EndsWith("_opt"))
            {
                yield return "Empty";
                yield return TerminalName.Substring(0, TerminalName.Length - 4);
            }
            else if (Alternatives != null && Alternatives.Any())
            {
                foreach (var i in Alternatives)
                    yield return i;
            }
        }

        public AmmyGrammarAutogeneratorInfo Single(params int[] map)
        {
            return WithMap(0);
        }
        public AmmyGrammarAutogeneratorInfo WithMap(params int[] map)
        {
            if (map != null && map.Length > 0)
                Map = map;
            return this;
        }

        private AmmyGrammarAutogeneratorInfo GetList() =>
            new AmmyGrammarAutogeneratorInfo(TerminalName + "s").AsListOf(this);

        private AmmyGrammarAutogeneratorInfo GetOptional() =>
            new AmmyGrammarAutogeneratorInfo(TerminalName + "_opt").AsOptional();


        public StringBuilder CamelTerminalName { get; }

        public int[] Map { get; set; }

        public bool   CreateClass  { get; set; }
        public string TerminalName { get; }
        public string ClassType    { get; }
        public string BaseClass    { get; set; }

        public string EffectiveClassName
        {
            get { return CreateClass ? ClassType : BaseClass; }
        }

        public string AlternativeInterfaceName
        {
            get { return "IAst" + CamelTerminalName; }
        }

        public string[] Alternatives { get; set; }
    }
}
#endif