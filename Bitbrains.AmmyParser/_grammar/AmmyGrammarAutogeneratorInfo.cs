using System.Collections.Generic;
using System.Linq;
using System.Text;
using iSukces.Code;

namespace Bitbrains.AmmyParser
{
    public partial class AmmyGrammarAutogeneratorInfo
    {
        public AmmyGrammarAutogeneratorInfo(string terminalName)
        {
            var parts = (terminalName + ",,,,").Split(',').Select(a => a.Trim()).ToArray();
            TerminalName      = parts[0];
            CamelTerminalName = GetCamelName(TerminalName);
            ClassType         = "Ast" + CamelTerminalName;

            BaseClass = parts.Length < 2 ? default : (CsType)parts[1];
            if (BaseClass.IsVoid)
                BaseClass = (CsType)nameof(BbExpressionListNode);
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
            BaseClass = new CsType("BbExpressionListNode")
                .WithGenericParameter((CsType)listItemName); 
            return this;
        }

        public AmmyGrammarAutogeneratorInfo AsOneOf(params string[] elements) => AsAlternative(0, elements);

        public AmmyGrammarAutogeneratorInfo AsOptional(string baseName = null, string alternativeOf = null)
        {
            BaseClass   = (CsType)(baseName ?? "AstOptNode");
            CreateClass = BaseClass != (CsType)"AstOptNode";
            if (!string.IsNullOrEmpty(alternativeOf))
                Alternatives = new[] {"Empty", alternativeOf};
            return this;
        }


        public string GetRule()
        {
            IEnumerable<string> GetAlternatives()
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

            if (!string.IsNullOrEmpty(Rule))
                return Rule;
            var altCode = GetAlternatives().ToArray();
            if (altCode.Any()) return string.Join(" | ", altCode);
            return null;
        }

        public AmmyGrammarAutogeneratorInfo Single(params int[] map) => WithMap(0);

        public AmmyGrammarAutogeneratorInfo WithMap(params int[] map)
        {
            if (map != null && map.Length > 0)
                Map = map;
            return this;
        }

        private AmmyGrammarAutogeneratorInfo GetList(bool makeRule = false, string separator="null")
        {
         var el =   new AmmyGrammarAutogeneratorInfo(TerminalName + "s").AsListOf(this);
         if (makeRule) 
             el.Rule = $"MakePlusRule({el.TerminalName}, {separator}, {this.TerminalName})";
         return el;
        }

        private AmmyGrammarAutogeneratorInfo GetOptional() =>
            new AmmyGrammarAutogeneratorInfo(TerminalName + "_opt").AsOptional();


        public StringBuilder CamelTerminalName { get; }

        public int[] Map { get; set; }

        public bool CreateClass  { get; private set; }
        public string TerminalName { get; }
        public string ClassType    { get; }
        public CsType BaseClass    { get; set; }

        public string EffectiveClassName
        {
            get { return CreateClass ? ClassType : BaseClass.Declaration; }
        }

        public string AlternativeInterfaceName
        {
            get { return "IAst" + CamelTerminalName; }
        }

        public string[] Alternatives { get; set; }

        public string Rule { get; set; }
        public HashSet<string> Punctuations { get; } = new HashSet<string>();
        
        public string Keyword { get; set; }
    }
}
