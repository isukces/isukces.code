using iSukces.Code.Interfaces;

namespace iSukces.Code.Irony
{
    /// <summary>
    ///     Similar to DirectCode, but indicates that AST will not contain this element
    /// </summary>
    public sealed class WhiteCharCode : ICsExpression
    {
        public WhiteCharCode(string code) => Code = code;

        public string GetCode(ITypeNameResolver resolver) => Code;

        public string Code { get; }
    }
}