namespace iSukces.Code.Ui.DataGrid
{
    public class WpfDataGridColumn:BasicDataGridColumn
    {
   

        public WpfDataGridColumn WithCellTemplate(object presentation)
        {
            CellTemplate = presentation;
            return this;
        }

        public WpfDataGridColumn WithEditTemplate(object template)
        {
            EditTemplate = template;
            return this;
        }


        public WpfDataGridColumn WithLookup(LookupInfo info)
        {
            Lookup = info;
            return this;
        }

        public WpfDataGridColumn WithLookup(object source, string selectedValuePath = null,
            string displayMemberPath = null)
        {
            Lookup = new LookupInfo
            {
                Source            = source,
                DisplayMemberPath = displayMemberPath,
                SelectedValuePath = selectedValuePath
            };
            return WithLookup(Lookup);
        }

        public WpfDataGridColumn WithName(string name)
        {
            Name = name;
            return this;
        }



        public LookupInfo Lookup       { get; set; }
        public object     CellTemplate { get; set; }
        public object     EditTemplate { get; set; }

    }
}
