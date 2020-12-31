using System;
using System.Collections.Generic;
using System.Linq;
using iSukces.Code.Interfaces;

namespace iSukces.Code.Irony
{
    public class NonTerminalInfo : TokenInfo, ICsExpression
    {
        public NonTerminalInfo(TerminalName name) : base(name)
        {
            var factory = AstClassNameFactory ?? DefaultAstClassNameFactory;
            AstClassTypeName = factory(name);

            factory       = DataClassNameFactory ?? DefaultDataClassNameFactory;
            DataClassName = factory(name);
        }

        public static ITypeNameProvider DefaultAstClassNameFactory(TerminalName name) =>
            new StringTypeNameProvider(".Ast" + name.GetCamelTerminalName());

        public static ITypeNameProvider DefaultDataClassNameFactory(TerminalName name) =>
            new StringTypeNameProvider(".Data" + name.GetCamelTerminalName());

        public static NonTerminalInfo Parse(string parsecode)
        {
            var parts  = (parsecode + ",,,,").Split(',').Select(a => a.Trim()).ToArray();
            var result = new NonTerminalInfo(new TerminalName(parts[0]));
            if (parts.Length > 1)
            {
                var baseClass = parts[1];
                if (!string.IsNullOrEmpty(baseClass))
                    result.AstBaseClassTypeName = new StringTypeNameProvider(baseClass);
            }

            var createClassCommand = parts[2].ToLower();
            result.CreateAstClass = createClassCommand == "" || createClassCommand == "true";
            return result;
        }

        private static TermListOptions2 EncodeDelimiters(Delimiters2 a)
        {
            var r = TermListOptions2.None;
            if ((a & Delimiters2.Starting) != 0)
                r |= TermListOptions2.AllowStartingDelimiter;
            if ((a & Delimiters2.Trailing) != 0)
                r |= TermListOptions2.AllowTrailingDelimiter;
            return r;
        }

        /*
        public NonTerminalInfo AsExpressionList(string listItemName)
        {
            // // "using_directives,ExpressionListNode<UsingStatement>",
            BaseClassNameFactory = _ => "ExpressionListNode<" + listItemName + ">";
            return this;
        }
        */

        /*public NonTerminalInfo AsExpressionList<T>()
        {
            BaseClassNameFactory = r =>
            {
                var t = typeof(ExpressionListNode<>).MakeGenericType(typeof(T));
                return r.GetTypeName(t);
            };
            return this;
        }*/


        public NonTerminalInfo AsOneOf(params ICsExpression[] elements) => AsOneOf(0, elements);

        public NonTerminalInfo AsOneOf(int index, params ICsExpression[] elements)
        {
            var map = new[] {new MapInfo(index)};
            Rule = new RuleBuilder.ListAlternative("IAst" + Name.GetCamelTerminalName(),
                map, elements);
            return this;
        }

        public NonTerminalInfo AsOptional(TokenInfo basedOn)
        {
            Rule = new RuleBuilder.OptionAlternative(basedOn,
                "IAst" + Name.GetCamelTerminalName());
            return this;
        }


        public string GetBaseClassAndInterfaces(IReadOnlyList<NonTerminalInfo> nonterminals,
            ITypeNameResolver resolver, string classNamespace)
        {
            var baseClass = AstBaseClassTypeName?.GetTypeName(resolver, classNamespace)?.Name;
            if (string.IsNullOrEmpty(baseClass))
                throw new Exception("Empty base class");
            var list = new List<string> {baseClass};
            foreach (var i in nonterminals)
            {
                if (!(i.Rule is RuleBuilder.ListAlternative al))
                    continue;
                if (al.Contains(Name))
                    list.Add(al.AlternativeInterfaceName);
            }

            return string.Join(",", list.Distinct());
        }

        public override string GetCode(ITypeNameResolver resolver) => Name.GetCode(resolver);

        public override string ToString() => "NonTerminal " + Name.Name;

        public NonTerminalInfo WithDataBaseClassName(Type type)
        {
            DataBaseClassName = new TypeTypeNameProvider(type);
            return this;
        }

        public NonTerminalInfo WithDataClassName(ITypeNameProvider name)
        {
            DataClassName = name;
            return this;
        }

        public NonTerminalInfo WithDataClassName(string name, Type baseType = null)
        {
            DataClassName = new StringTypeNameProvider(name);
            if (baseType != null)
                WithDataBaseClassName(baseType);
            return this;
        }

        public NonTerminalInfo WithNoDataClass()
        {
            CreateDataClass = false;
            return this;
        }


        public NonTerminalInfo WithPlusRule(ICsExpression delimiter, TerminalName element,
            Delimiters2 delimiters = Delimiters2.None)
        {
            var options = TermListOptions2.PlusList;
            options |= EncodeDelimiters(delimiters);
            Rule    =  new RuleBuilder.PlusOrStar(delimiter, element, options, Name);
            return this;
        }

        public NonTerminalInfo WithPlusRule(TerminalName element) => WithPlusRule(null, element);

        public NonTerminalInfo WithPlusRule(ICsExpression delimiter, ITerminalNameSource element,
            Delimiters2 delimiters = Delimiters2.None)
        {
            var options = TermListOptions2.PlusList;
            options |= EncodeDelimiters(delimiters);
            Rule    =  new RuleBuilder.PlusOrStar(delimiter, element, options, Name);
            return this;
        }

        public NonTerminalInfo WithRule(RuleBuilder rule)
        {
            Rule = rule;
            return this;
        }

        public NonTerminalInfo WithSequenceRule(SequenceRuleBuilder builder)
        {
            var map = builder.GetMap();
            WithSequenceRule(null, map, builder.GetItems());
            return this;
        }


        public NonTerminalInfo WithSequenceRule(Action<RuleBuilder.SequenceRule> process,
            IReadOnlyList<MapInfo> map, IReadOnlyList<RuleBuilder.SequenceRule.SequenceItem> items)
        {
            var sequenceRule = new RuleBuilder.SequenceRule(items, map);
            process?.Invoke(sequenceRule);
            Rule = sequenceRule;
            return this;
        }

        public NonTerminalInfo WithStarRule(ICsExpression delimiter, ITerminalNameSource element,
            Delimiters2 delimiters = Delimiters2.None)
        {
            var options = TermListOptions2.StarList;
            options |= EncodeDelimiters(delimiters);
            Rule    =  new RuleBuilder.PlusOrStar(delimiter, element, options, Name);
            return this;
        }

        public NonTerminalInfo WithStarRule(ITerminalNameSource element, Delimiters2 addPreferShiftHint = Delimiters2.None) =>
            WithStarRule(null, element, addPreferShiftHint);


        public bool CreateDataClass { get; set; } = true;
        public bool CreateAstClass  { get; set; } = true;

        public ITypeNameProvider AstClassTypeName { get; }

        public ITypeNameProvider AstBaseClassTypeName { get; set; }

        public RuleBuilder Rule
        {
            get { return _rule; }
            set
            {
                if (!(_rule is null))
                    throw new Exception("Rule already set");
                _rule = value;
            }
        }


        public ITypeNameProvider DataClassName     { get; set; }
        public ITypeNameProvider DataBaseClassName { get; set; }
        public static Func<TerminalName, ITypeNameProvider> AstClassNameFactory = DefaultAstClassNameFactory;
        public static Func<TerminalName, ITypeNameProvider> DataClassNameFactory = DefaultDataClassNameFactory;
        private RuleBuilder _rule;
    }

    [Flags]
    public enum TermListOptions2
    {
        None = 0,
        AllowEmpty = 1,
        AllowTrailingDelimiter = 2,
        AddPreferShiftHint = 4,
        
        PlusList =  AddPreferShiftHint, 
        StarList = PlusList | AllowEmpty,
        AllowStartingDelimiter = 8,
        
        BothDelimiters = AllowStartingDelimiter | AllowTrailingDelimiter
    }

    [Flags]
    public enum Delimiters2
    {
        None = 0,
        Starting = 1,
        Trailing = 2,
        Both = Starting | Trailing
    }
}