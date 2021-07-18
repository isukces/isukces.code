namespace iSukces.Code.Ui.DataGrid
{
    public static class AbstractDataGridColumnExtensions
    {
        public static T WithReadOnly<T>(this T src, bool isReadOnly = true)
            where T:AbstractDataGridColumn
        {
            src.IsReadOnly = isReadOnly;
            return src;
        }
        public static T WithDataFormatString<T>(this T src, string displayFormat)
            where T:AbstractDataGridColumn
        {
            src.DataFormatString = displayFormat;
            return src;
        }
        
        public static T WithSortable<T>(this T src, bool isSortable)
            where T:AbstractDataGridColumn
        {
            src.IsSortable = isSortable;
            return src;
        }
        
        
        public static T WithResizable<T>(this T src, bool isResizable)
            where T:AbstractDataGridColumn
        {
            src.IsResizable = isResizable;
            return src;
        }
        
        
        public static T WithCategory<T>(this T src, string categoryName, object categoryHeaderSource = null)
            where T:AbstractDataGridColumn
        {
            src.CategoryName         = categoryName;
            src.CategoryHeaderSource = categoryHeaderSource ?? categoryName;
            return src;
        }
    }
}