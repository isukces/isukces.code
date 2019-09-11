namespace isukces.code.Ui.DataGrid
{
    public partial class GridAmmyMixinsGenerator
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