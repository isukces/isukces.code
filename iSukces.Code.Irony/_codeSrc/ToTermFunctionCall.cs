using iSukces.Code.Interfaces;

#nullable disable
namespace iSukces.Code.Irony
{
    internal class ToTermFunctionCall : ICsExpression, ITokenNameSource
    {
        public ToTermFunctionCall(string text) => Text = text;

        string ICsExpression.GetCode(ITypeNameResolver resolver) => "ToTerm(" + Text.CsEncode() + ")";

        TokenName ITokenNameSource.GetTokenName() => new TokenName(Text);

        TokenNameTarget ITokenNameSource.GetTokenNameIsNonterminal() => TokenNameTarget.Terminal;

        public string Text { get; }
    }
}

