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
            AstClassTypeName = new SimpleCodeExpression(".Ast.Ast" + Name.GetCamelTerminalName());
        }

        public static NonTerminalInfo Parse(string parsecode)
        {
            var parts  = (parsecode + ",,,,").Split(',').Select(a => a.Trim()).ToArray();
            var result = new NonTerminalInfo(new TerminalName(parts[0]));
            if (parts.Length > 1)
            {
                var baseClass = parts[1];
                if (!string.IsNullOrEmpty(baseClass))
                    result.AstBaseClassTypeName = new SimpleCodeExpression(baseClass);
            }

            var createClassCommand = parts[2].ToLower();
            result.CreateClass = createClassCommand == "" || createClassCommand == "true";
            return result;
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


        public NonTerminalInfo AsOneOf(params ICsExpression[] elements)
        {
            return AsOneOf(0, elements);
        }

        public NonTerminalInfo AsOneOf(int index, params ICsExpression[] elements)
        {
            var map = new[] {new MapInfo(index)};
            Rule = new RuleBuilder.ListAlternative("IAst" + Name.GetCamelTerminalName(),
                map, elements);
            return this;
        }

        public NonTerminalInfo AsOptional(TokenInfo basedOn)
        {
            Rule = new RuleBuilder.OptionAlternative(basedOn.Name,
                "IAst" + Name.GetCamelTerminalName());
            return this;
        }


        public string GetBaseClass(IReadOnlyList<NonTerminalInfo> nt, ITypeNameResolver resolver)
        {
            var baseClass = AstBaseClassTypeName?.GetCode(resolver);
            if (string.IsNullOrEmpty(baseClass))
                throw new Exception("Empty base class");
            var l = new List<string> {baseClass};
            foreach (var i in nt)
            {
                if (!(i.Rule is RuleBuilder.ListAlternative al)) continue;
                if (al.Contains(Name))
                    l.Add(al.AlternativeInterfaceName);
            }

            return string.Join(",", l.Distinct());
        }

        public override string GetCode(ITypeNameResolver resolver)
        {
            return Name.GetCode(resolver);
        }

        public NonTerminalInfo WithDataClassName(ICsExpression name)
        {
            DataClassName = name;
            return this;
        }

        public NonTerminalInfo WithDataClassName(string name)
        {
            DataClassName = new SimpleCodeExpression(name);
            return this;
        }

        public NonTerminalInfo WithNoDataClass()
        {
            CreateDataClass = false;
            return this;
        }


        public NonTerminalInfo WithPlusRule(ICsExpression delimiter, TerminalName element)
        {
            Rule = new RuleBuilder.PlusOrStar(delimiter, element, true, Name);
            return this;
        }

        public NonTerminalInfo WithPlusRule(TerminalName element)
        {
            return WithPlusRule(null, element);
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
            IReadOnlyList<MapInfo> map, params ICsExpression[] items)
        {
            var sequenceRule = new RuleBuilder.SequenceRule(items, map);
            process?.Invoke(sequenceRule);
            Rule = sequenceRule;
            return this;
        }

        public NonTerminalInfo WithStarRule(ICsExpression delimiter, TerminalName element)
        {
            Rule = new RuleBuilder.PlusOrStar(delimiter, element, false, Name);
            return this;
        }

        public NonTerminalInfo WithStarRule(TerminalName element)
        {
            return WithStarRule(null, element);
        }


        public bool CreateClass { get; set; }

        public ICsExpression AstClassTypeName { get; }

        public ICsExpression AstBaseClassTypeName { get; set; }

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

        public bool          CreateDataClass { get; set; } = true;
        public ICsExpression DataClassName   { get; set; }

        private RuleBuilder _rule;
    }
}