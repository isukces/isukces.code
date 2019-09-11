using System;
using System.Collections.Generic;
using System.Linq;

namespace isukces.code.Ui.DataGrid
{
    public partial class GridAmmyMixinsGenerator
    {
        protected class Model
        {
            public static Model MakeFromAttributes(Type type)
            {
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
                        result.Categories.Add(new AttributeInfo(colDef.CategoryName, colDef.CategoryHeader));
                    var col = new ColumnInfo
                    {
                        Name              = rowProperty?.Name ?? colDef.Name,
                        DataMemberBinding = colDef.DataMemberBinding,
                        Header            = colDef.Header ?? rowProperty?.Name,
                        Width             = colDef.Width,
                        CategoryName      = result.Categories.LastOrDefault()?.Name,
                        Type              = rowProperty?.PropertyType ?? rowType,
                        Lookup            = colDef.Lookup,
                        CellTemplate      = colDef.CellTemplate,
                        EditTemplate      = colDef.EditTemplate,
                        DataFormatString  = colDef.DataFormatString,
                        IsReadOnly        = colDef.IsReadOnly
                    };

                    col.AlignRight = RightAligned.Contains(col.Type);
                    result.Columns.Add(col);
                }

                return result;
            }

            public bool                AddExpandColumn { get; private set; }
            public List<AttributeInfo> Categories      { get; } = new List<AttributeInfo>();
            public List<ColumnInfo>    Columns         { get; } = new List<ColumnInfo>();
        }
    }
}