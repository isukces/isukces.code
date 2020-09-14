namespace iSukces.Code.Ui.DataGrid
{
    public abstract partial class GridAmmyMixinsGenerator
    {
        protected class AttributeInfo
        {
            public AttributeInfo(string name, string header)
            {
                Name   = name;
                Header = header;
            }

            public string Name   { get; }
            public string Header { get; }
        }
    }
}