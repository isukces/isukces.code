using System;
using System.Collections.Generic;
using System.Linq;
using iSukces.Code.Interfaces;

namespace iSukces.Code.Ui.DataGrid
{
    public abstract partial class GridAmmyMixinsGenerator
    {
        protected class Model : IAnnotableByUser
        {
            public static Model MakeFromAttributes(Type type)
            {
                if (type.IsInterface || type.IsAbstract)
                    return null;
                var rowType = GetRowType(type);
                if (rowType == null)
                    return null;
                var result = new Model();

                var instance = (DataGridConfigurationProvider)Activator.CreateInstance(type);
                result.AddExpandColumn = instance.AddExpandColumn;
                var columnDefinitions = instance.GetColumns().ToList();

                var modelProperties = GetProperties(rowType)
                    .ToDictionary(a => a.Name, a => a, StringComparer.OrdinalIgnoreCase);
                foreach (var colDef in columnDefinitions)
                {
                    modelProperties.TryGetValue(colDef.Name, out var rowProperty);

                    if (colDef.CategoryName != null)
                        result.Categories.Add(new AttributeInfo(colDef.CategoryName, colDef.CategoryHeaderSource));
                    var col = new ColumnInfo
                    {
                        Name               = rowProperty?.Name ?? colDef.Name,
                        Binding            = colDef.Binding,
                        HeaderSource       = colDef.HeaderSource ?? rowProperty?.Name,
                        Width              = colDef.Width,
                        CategoryName = result.Categories.LastOrDefault()?.Name,
                        Type               = rowProperty?.PropertyType ?? rowType,
                        Lookup             = colDef.Lookup,
                        CellTemplate       = colDef.CellTemplate,
                        EditTemplate       = colDef.EditTemplate,
                        DataFormatString   = colDef.DataFormatString,
                        IsReadOnly         = colDef.IsReadOnly,
                        CustomValues       = colDef.CustomValues
                    };

                    col.AlignRight = RightAligned.Contains(col.Type);
                    result.Columns.Add(col);
                }

                return result;
            }

            public bool                        AddExpandColumn { get; private set; }
            public List<AttributeInfo>         Categories      { get; } = new List<AttributeInfo>();
            public List<ColumnInfo>            Columns         { get; } = new List<ColumnInfo>();
            
            public IDictionary<string, object> UserAnnotations { get; } = new Dictionary<string, object>();
        }
    }
}