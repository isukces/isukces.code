namespace iSukces.Code.Ui.DataGrid
{
    public static class AbstractDataGridColumnExtensions
    {
        public static T WithReadOnly<T>(this T src, bool isReadOnly = true)
            where T:BasicDataGridColumn
        {
            src.IsReadOnly = isReadOnly;
            return src;
        }
        public static T WithDataFormatString<T>(this T src, string displayFormat)
            where T:BasicDataGridColumn
        {
            src.DataFormatString = displayFormat;
            return src;
        }
        
        public static T WithSortable<T>(this T src, bool isSortable)
            where T:BasicDataGridColumn
        {
            src.IsSortable = isSortable;
            return src;
        }
        
        
        public static T WithResizable<T>(this T src, bool isResizable)
            where T:BasicDataGridColumn
        {
            src.IsResizable = isResizable;
            return src;
        }
        
        
        public static T WithCategory<T>(this T src, string categoryName, object? categoryHeaderSource = null)
            where T:BasicDataGridColumn
        {
            src.CategoryName         = categoryName;
            src.CategoryHeaderSource = categoryHeaderSource ?? categoryName;
            return src;
        }
    }
}
