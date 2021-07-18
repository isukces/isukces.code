using System.Collections.Generic;
using JetBrains.Annotations;

namespace iSukces.Code.Ui.DataGrid
{
    public class AbstractDataGridColumn
    {
        public string Name  { get; set; }
        public int?   Width { get; set; }   
  
        public object HeaderSource { get; set; }

        public string DataFormatString { get; set; }

        public bool IsReadOnly { get; set; }

        public bool IsSortable { get; set; }

        public bool IsResizable { get; set; } = true;
            
        public object CategoryHeaderSource { get; set; }
        public string CategoryName         { get; set; }

        [NotNull]
        public Dictionary<string, object> CustomValues { get; } = new Dictionary<string, object>();
    }
}
