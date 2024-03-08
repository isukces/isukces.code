using System;
using System.Collections.Generic;
using System.Linq;
using iSukces.Code.Interfaces;

namespace iSukces.Code.Irony
{
    public class NonTerminalInfo : TokenInfo, ICsExpression
    {
        public NonTerminalInfo(TokenName name) : base(name)
        {
            var factory = AstClassNameFactory ?? DefaultAstClassNameFactory;
            AstClass = factory(name);

            factory   = DataClassNameFactory ?? DefaultDataClassNameFactory;
            DataClass = factory(name);
        }

        public static TypeNameProviderEx DefaultAstClassNameFactory(TokenName name)
        {
            var a = new StringTypeNameProvider(".Ast" + name.GetCamelTerminalName());
            return new TypeNameProviderEx(a, TypeNameProviderFlags.CreateAutoCode);
        }

        public static TypeNameProviderEx DefaultDataClassNameFactory(TokenName name)
        {
            var a = new StringTypeNameProvider(".Data" + name.GetCamelTerminalName());
            return new TypeNameProviderEx(a, TypeNameProviderFlags.CreateAutoCode);
        }

        public static NonTerminalInfo Parse(string parsecode)
        {
            var parts  = (parsecode + ",,,,").Split(',').Select(a => a.Trim()).ToArray();
            var result = new NonTerminalInfo(new TokenName(parts[0]));
            if (parts.Length > 1)
            {
                var baseClass = parts[1];
                if (!string.IsNullOrEmpty(baseClass))
                    result.AstBaseClassTypeName = new StringTypeNameProvider(baseClass);
            }

            /*var createClassCommand = parts[2].ToLower();
            result.as
            result.CreateAstClass = createClassCommand == "" || createClassCommand == "true";*/
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


        private static TypeNameProviderEx GetAlternativeInterfaceName(TokenName name)
        {
            var provider = new StringTypeNameProvider(".IAst" + name.GetCamelTerminalName());
            return new TypeNameProviderEx(provider,
                TypeNameProviderFlags.CreateAutoCode | TypeNameProviderFlags.IsInterface);
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


        public NonTerminalInfo AsOneOf(params ICsExpression[] elements) => AsOneOf(null, elements);


        public NonTerminalInfo AsOneOf(Action<RuleBuilder.ListAlternative> setup,
            params ICsExpression[] elements)
        {
            var map                      = new[] {new MapInfo(0, 0)};
            var alternativeInterfaceName = GetAlternativeInterfaceName(Name);
            var rule = new RuleBuilder.ListAlternative(alternativeInterfaceName,
                map, elements);
            Rule = rule;
            if (setup != null)
                setup(rule);
            return this;
        }

        public NonTerminalInfo AsOptional(TokenInfo basedOn)
        {
            var alternativeInterfaceName = GetAlternativeInterfaceName(Name);
            Rule = new RuleBuilder.OptionAlternative(basedOn, alternativeInterfaceName);
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
                {
                    var tmp = al.AlternativeInterfaceName;
                    if (tmp.IsInterface)
                    {
                        var n = tmp.Provider.GetTypeName(resolver, classNamespace);
                        list.Add(n.Name);
                    }
                }
            }

            return string.Join(", ", list.Distinct());
        }

        public override string GetCode(ITypeNameResolver resolver) => Name.GetCode(resolver);
        public override TokenNameTarget GetTokenNameIsNonterminal() => TokenNameTarget.Terminal;

        public override string ToString() => "NonTerminal " + Name.Name;

        public NonTerminalInfo WithDataBaseClassName(Type type)
        {
            DataBaseClassName = new TypeNameProvider(type);
            return this;
        }

        public NonTerminalInfo WithNoAstClass()
        {
            var newFlags = AstClass.Flags & ~TypeNameProviderFlags.CreateAutoCode;
            AstClass = new TypeNameProviderEx(AstClass.Provider, newFlags);
            return this;
        }

        /*public NonTerminalInfo WithDataClassName(ITypeNameProvider name)
        {
            DataClassName2 = name;
            return this;
        }

        public NonTerminalInfo WithDataClassName(string name, Type baseType = null)
        {
            DataClassName = new StringTypeNameProvider(name);
            if (baseType != null)
                WithDataBaseClassName(baseType);
            return this;
        }*/

        public NonTerminalInfo WithNoDataClass()
        {
            var newFlags = DataClass.Flags & ~TypeNameProviderFlags.CreateAutoCode;
            DataClass = new TypeNameProviderEx(DataClass.Provider, newFlags);
            return this;
        }


        public NonTerminalInfo WithPlusRule(ICsExpression delimiter, TokenName element,
            Delimiters2 delimiters = Delimiters2.None)
        {
            var options = TermListOptions2.PlusList;
            options |= EncodeDelimiters(delimiters);
            Rule    =  new RuleBuilder.PlusOrStar(delimiter, element, options, Name);
            return this;
        }

        public NonTerminalInfo WithPlusRule(TokenName element) => WithPlusRule(null, element);

        public NonTerminalInfo WithPlusRule(ICsExpression delimiter, ITokenNameSource element,
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
            WithSequenceRule(null, map, builder.GetRuleItems());
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

        public NonTerminalInfo WithStarRule(ICsExpression delimiter, ITokenNameSource element,
            Delimiters2 delimiters = Delimiters2.None)
        {
            var options = TermListOptions2.StarList;
            options |= EncodeDelimiters(delimiters);
            Rule    =  new RuleBuilder.PlusOrStar(delimiter, element, options, Name);
            return this;
        }

        public NonTerminalInfo WithStarRule(ITokenNameSource element,
            Delimiters2 addPreferShiftHint = Delimiters2.None) =>
            WithStarRule(null, element, addPreferShiftHint);


        public TypeNameProviderEx AstClass             { get; set; }
        public TypeNameProviderEx DataClass            { get; set; }
        public ITypeNameProvider  AstBaseClassTypeName { get; set; }

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


        public ITypeNameProvider DataBaseClassName { get; set; }
        public static Func<TokenName, TypeNameProviderEx> AstClassNameFactory = DefaultAstClassNameFactory;
        public static Func<TokenName, TypeNameProviderEx> DataClassNameFactory = DefaultDataClassNameFactory;
        private RuleBuilder _rule;
    }

    [Flags]
    public enum TermListOptions2
    {
        None = 0,
        AllowEmpty = 1,
        AllowTrailingDelimiter = 2,
        AddPreferShiftHint = 4,

        PlusList = AddPreferShiftHint,
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