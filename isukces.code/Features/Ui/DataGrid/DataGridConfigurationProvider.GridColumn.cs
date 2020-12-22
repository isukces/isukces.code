using System.Collections.Generic;
using System.Reflection;
using iSukces.Code.Ammy;
using JetBrains.Annotations;

namespace iSukces.Code.Ui.DataGrid
{
    public abstract partial class DataGridConfigurationProvider
    {
        public class GridColumn
        {
            public GridColumn WithCategory(string categoryName, object categoryHeaderSource = null)
            {
                CategoryName         = categoryName;
                CategoryHeaderSource = categoryHeaderSource ?? categoryName;
                return this;
            }

            public GridColumn WithCellTemplate(object presentation)
            {
                CellTemplate = presentation;
                return this;
            }

            public GridColumn WithDataFormatString(string displayFormat)
            {
                DataFormatString = displayFormat;
                return this;
            }

            public GridColumn WithDataMemberBinding(string dataMemberBindingName)
            {
                Binding.Path = dataMemberBindingName;
                return this;
            }

            public GridColumn WithEditTemplate(object template)
            {
                EditTemplate = template;
                return this;
            }

            public GridColumn WithLookup(LookupInfo info)
            {
                Lookup = info;
                return this;
            }

            public GridColumn WithLookup(object source, string selectedValuePath = null,
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

            public GridColumn WithName(string name)
            {
                Name = name;
                return this;
            }

            public GridColumn WithReadOnly(bool isReadOnly = true)
            {
                IsReadOnly = isReadOnly;
                return this;
            }

            public string       Name                 { get; set; }
            public object       HeaderSource         { get; set; }
            public int?         Width                { get; set; }
            public object       CategoryHeaderSource { get; set; }
            public string       CategoryName         { get; set; }
            public LookupInfo   Lookup               { get; set; }
            public object       CellTemplate         { get; set; }
            public object       EditTemplate         { get; set; }
            public string       DataFormatString     { get; set; }
            public bool         IsReadOnly           { get; set; }
            
            /// <summary>
            /// Reflected property if possible. For complicated paths can be declared in other than Row types 
            /// </summary>
            public PropertyInfo Member               { get; set; }

            [NotNull]
            public AmmyBindBuilder Binding { get; } = new AmmyBindBuilder(null);

            [NotNull]
            public Dictionary<string, object> CustomValues { get; } = new Dictionary<string, object>();
        }
    }
}