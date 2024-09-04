#nullable enable
using System.Collections.Generic;
using System.Reflection;

namespace iSukces.Code.Ui.DataGrid
{
    public class BasicDataGridColumn
    {
        public string Name  { get; set; }
        public int?   Width { get; set; }   
  
        public object HeaderSource { get; set; }

        public string DataFormatString { get; set; }

        public bool IsReadOnly { get; set; }

        public bool IsSortable { get; set; }

        public bool IsResizable { get; set; } = true;
            
        public string CategoryName         { get; set; }
        public object CategoryHeaderSource { get; set; }
        
        /// <summary>
        ///     Reflected property if possible. For complicated paths can be declared in other than Row types
        /// </summary>
        public PropertyInfo Member { get; set; }

        public Dictionary<string, object> CustomValues { get; } = new Dictionary<string, object>();
    }
}
