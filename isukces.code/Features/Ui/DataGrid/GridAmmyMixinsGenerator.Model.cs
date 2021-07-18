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

                var instance = (BasicDataGridConfigurationProvider)Activator.CreateInstance(type);
                result.AddExpandColumn = instance.AddExpandColumn;
                var columnDefinitions = instance.GetColumnsGeneral().ToList();

                foreach (var colDef in columnDefinitions)
                {
                    var propertyInfo = colDef.Member;
                    if (colDef.CategoryName != null)
                        result.Categories.Add(new AttributeInfo(colDef.CategoryName, colDef.CategoryHeaderSource));
                    var col = new ColumnInfo
                    {
                        Name             = propertyInfo?.Name ?? colDef.Name,
                        HeaderSource     = colDef.HeaderSource ?? propertyInfo?.Name,
                        Width            = colDef.Width,
                        CategoryName     = result.Categories.LastOrDefault()?.Name,
                        Type             = propertyInfo?.PropertyType ?? rowType,
                        
                        DataFormatString = colDef.DataFormatString,
                        IsReadOnly       = colDef.IsReadOnly,
                        IsSortable       = colDef.IsSortable,
                        IsResizable      = colDef.IsResizable,
                        CustomValues     = colDef.CustomValues
                    };
                    if (colDef is WpfDataGridColumn wpf)
                    {
                        col.Binding      = wpf.Binding;
                        col.Lookup       = wpf.Lookup;
                        col.CellTemplate = wpf.CellTemplate;
                        col.EditTemplate = wpf.EditTemplate;
                    }

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