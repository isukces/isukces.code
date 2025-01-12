#nullable disable
namespace iSukces.Code.Ui.DataGrid
{
    /// <summary>
    ///     Helper structure to handle texts that can vary i.e. translations
    /// </summary>
    public readonly struct UiTestTextSourceInfo
    {
        public UiTestTextSourceInfo(string constantValue, string csExpression)
        {
            ConstantValue = constantValue;
            CsExpression  = csExpression;
        }

        public static UiTestTextSourceInfo FromString(string s)
        {
            if (s is null)
                return Null;
            return new UiTestTextSourceInfo(s, s.CsEncode());
        }

        public static UiTestTextSourceInfo Null
        {
            get { return new UiTestTextSourceInfo(null, "null"); }
        }

        public string ConstantValue { get; }

        public bool HasConstantValue
        {
            get { return !string.IsNullOrEmpty(ConstantValue); }
        }

        public string CsExpression { get; }
    }
}
