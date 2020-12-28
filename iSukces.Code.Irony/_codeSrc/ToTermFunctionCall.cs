using iSukces.Code.Interfaces;

namespace iSukces.Code.Irony._codeSrc
{
    internal class ToTermFunctionCall : ICsExpression
    {
        public ToTermFunctionCall(string text)
        {
            Text = text;
        }

        public string GetCode(ITypeNameResolver resolver)
        {
            return "ToTerm(" + Text.CsEncode() + ")";
        }

        public string Text { get; }
    }
}