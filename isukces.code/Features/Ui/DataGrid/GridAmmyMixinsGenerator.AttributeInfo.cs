namespace iSukces.Code.Ui.DataGrid
{
    public abstract partial class GridAmmyMixinsGenerator
    {
        protected class AttributeInfo
        {
            public AttributeInfo(string name, object headerSource)
            {
                Name         = name;
                HeaderSource = headerSource;
            }

            public string Name         { get; }
            public object HeaderSource { get; }
        }
    }
}