using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Irony.Parsing;
using iSukces.Code.Irony;

namespace Samples.Irony.AmmyGrammar
{
    public partial class AmmyGrammar
    {
        private NonTerminal _newLinePlusX;
        private NonTerminal _newLineStarX;

        public AmmyGrammar()
            : base(true)
        {
            AutoInit();
            LanguageFlags = LanguageFlags.NewLineBeforeEOF
                            | LanguageFlags.CreateAst
                            | LanguageFlags.SupportsBigInt;

            MarkPunctuation(__dot);
            RegisterBracePair("{","}");
            RegisterBracePair("[","]");
            RegisterBracePair("(",")");
            
            MarkReservedWords("using");
        }


        public override void SkipWhitespace(ISourceStream source)
        {
            while (!source.EOF())
            {
                var ch = source.PreviewChar;
                switch (ch)
                {
                    case ' ':
                    case '\t':
                    //case '\r':
                    // case '\n':
                    case '\v':
                    case '\u2085':
                    case '\u2028':
                    case '\u2029':
                        source.PreviewPosition++;
                        break;
                    default:
                        //Check unicode class Zs
                        var chCat = char.GetUnicodeCategory(ch);
                        if (chCat == UnicodeCategory.SpaceSeparator) //it is whitespace, continue moving
                            continue; //while loop 
                        //Otherwize return
                        return;
                } //switch
            } //while
        }

         
        public override void CreateTokenFilters(LanguageData language, TokenFilterList filters) {
            /*var outlineFilter = new CodeOutlineFilter(language.GrammarData, 
                OutlineOptions.ProduceIndents | OutlineOptions.CheckBraces, ToTerm(@"\")); // "\" is continuation symbol*/
            filters.Add(new MyFilter());
        }

        private BnfExpression MakeListRuleEx(NonTerminal list, BnfTerm delimiter, BnfTerm listMember,
            TermListOptions2 options = TermListOptions2.AddPreferShiftHint)
        {
            var allowEmpty       = options.IsSet(TermListOptions2.AllowEmpty);
            var mustHaveElements = !allowEmpty;
            // var minumumCount     = mustHaveElements ? 1 : 0;

            var endingDelimiter   = options.IsSet(TermListOptions2.AllowTrailingDelimiter) && delimiter != null;
            var startingDelimiter = options.IsSet(TermListOptions2.AllowStartingDelimiter) && delimiter != null;

            var notEmptyList = mustHaveElements ? list : new NonTerminal(listMember.Name + "(s)");
            notEmptyList.SetFlag(TermFlags.IsList);
            notEmptyList.Rule = notEmptyList;
            if (delimiter != null)
                notEmptyList.Rule += delimiter;

            if (options.IsSet(TermListOptions2.AddPreferShiftHint))
                notEmptyList.Rule += PreferShiftHere();

            notEmptyList.Rule += listMember;
            notEmptyList.Rule |= listMember;

            BnfExpression Make(BnfExpression exx)
            {
                var delimiters = Delimiters2.None;
                if (startingDelimiter)
                    delimiters |= Delimiters2.Starting;
                if (endingDelimiter)
                    delimiters |= Delimiters2.Trailing;
                if (delimiters == Delimiters2.None)
                    return null;

                var list = new[]
                {
                    delimiters & Delimiters2.Starting,
                    delimiters & Delimiters2.Trailing,
                    delimiters
                };
                var distinct = list.Distinct()
                    .Where(a => a != Delimiters2.None)
                    .ToArray();

                static BnfExpression Plus(BnfTerm term1, BnfTerm term2)
                {
                    var bnfExpression = new BnfExpression(term1);
                    return bnfExpression + term2;
                }

                BnfExpression result = null;
                foreach (var i in distinct)
                {
                    BnfExpression expression = null;
                    switch (i)
                    {
                        case Delimiters2.Starting:
                            expression = Plus(delimiter, PreferShiftHere())+ exx;
                            break;
                        case Delimiters2.Trailing:
                            expression = Plus(exx, delimiter);
                            break;
                        case Delimiters2.Both:
                            expression = Plus(delimiter, PreferShiftHere()) + exx + delimiter;
                            break;
                        default:
                            continue;
                    }

                    if (result is null)
                        result = expression;
                    else
                        result |= expression;
                }

                return result;
            }

            if (allowEmpty)
            {
                // allow empty
                list.Rule = Empty | notEmptyList;
                var expression = Make(notEmptyList);
                if (expression != null)
                {
                    expression       |= delimiter;
                    list.Rule |= expression;
                }

                notEmptyList.SetFlag(TermFlags.NoAstNode);
                list.SetFlag(TermFlags.IsListContainer);
            }
            else
            {
                // at least one element
                var expression = Make(list);
                if (expression != null)
                    list.Rule |= expression;
            }

            Debug.WriteLine(list.Rule);
            if (list != notEmptyList)
                Debug.WriteLine(notEmptyList.Rule);

            return list.Rule;
        }

        static BnfTerm Conv(BnfTerm a)
        {
            var exp = new BnfExpression(a);
            exp.Name = a.Name + " wrapped";
            return exp;
        }
        
        
        public NonTerminal NewLinePlusX
        {
            get
            {
                if (_newLinePlusX != null) return _newLinePlusX;
                _newLinePlusX      = new NonTerminal("xLF+");
                _newLinePlusX.Rule = Conv(NewLine) | Conv(_newLinePlusX) + PreferShiftHere() + NewLine;
                MarkPunctuation((BnfTerm) _newLinePlusX);
                _newLinePlusX.SetFlag(TermFlags.IsList);
                return _newLinePlusX;
            }
        }
        public NonTerminal NewLineStarX
        {
            get
            {
                if (_newLineStarX != null) return _newLineStarX;
                _newLineStarX = new NonTerminal("xLF*");
                MarkPunctuation((BnfTerm) _newLineStarX);
                _newLineStarX.Rule = MakeStarRule(_newLineStarX, NewLine);
                return _newLineStarX;
            }
        }
    }

    public static class ParsingEnumExtensions2
    {
        public static bool IsSet(this TermFlags flags, TermFlags flag) => (uint)(flags & flag) > 0U;

        public static bool IsSet(this LanguageFlags flags, LanguageFlags flag) => (uint)(flags & flag) > 0U;

        public static bool IsSet(this ParseOptions options, ParseOptions option) => (uint)(options & option) > 0U;

        public static bool IsSet(this TermListOptions2 options, TermListOptions2 option) =>
            (uint)(options & option) > 0U;

        public static bool IsSet(this ProductionFlags flags, ProductionFlags flag) => (uint)(flags & flag) > 0U;
    }
}