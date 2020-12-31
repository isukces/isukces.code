using Irony.Parsing;
using iSukces.Code.Interfaces;

namespace iSukces.Code.Irony
{
    public abstract partial class RuleBuilder
    {
        public sealed class PlusOrStar : RuleBuilder
        {
            public PlusOrStar(ICsExpression delimiter, ITerminalNameSource element, TermListOptions2 options,
                TerminalName output)
            {
                Delimiter = delimiter ?? new DirectCode("null");
                Element   = element;
                Options   = options;
                Output    = output;
            }

            public override string GetCode(ITypeNameResolver resolver)
            {
                var args = new CsArgumentsBuilder()
                    .AddCode(Output.GetCode(resolver))
                    .AddCode(Delimiter.GetCode(resolver))
                    .AddCode(Element.GetTerminalName().GetCode(resolver));
                if (Options == TermListOptions2.PlusList)
                    return "MakePlusRule" + args.CodeEx;
                if (Options == TermListOptions2.StarList)
                    return "MakeStarRule" + args.CodeEx;

                if ((Options & TermListOptions2.AllowStartingDelimiter) != 0)
                {
                    var flags = resolver.GetEnumFlagsValueCode(Options);
                    args.AddCode(flags);
                    return "MakeListRuleEx" + args.CodeEx;
                }
                else
                {
                    var flags = resolver.GetEnumFlagsValueCode((TermListOptions)Options);
                    args.AddCode(flags);
                    return "MakeListRule" + args.CodeEx;
                }
            }

            public ICsExpression       Delimiter { get; }
            public ITerminalNameSource Element   { get; }
            public TermListOptions2    Options   { get; }
            public TerminalName        Output    { get; }
        }
    }
}