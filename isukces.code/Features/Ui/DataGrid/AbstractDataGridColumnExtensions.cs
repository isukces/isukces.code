namespace iSukces.Code.Ui.DataGrid;

public static class AbstractDataGridColumnExtensions
{
    extension<T>(T src)
        where T:BasicDataGridColumn
    {
        public T WithReadOnly(bool isReadOnly = true)
        {
            src.IsReadOnly = isReadOnly;
            return src;
        }

        public T WithDataFormatString(string displayFormat)
        {
            src.DataFormatString = displayFormat;
            return src;
        }

        public T WithSortable(bool isSortable)
        {
            src.IsSortable = isSortable;
            return src;
        }

        public T WithResizable(bool isResizable)
        {
            src.IsResizable = isResizable;
            return src;
        }

        public T WithCategory(string categoryName, object? categoryHeaderSource = null)
        {
            src.CategoryName         = categoryName;
            src.CategoryHeaderSource = categoryHeaderSource ?? categoryName;
            return src;
        }
    }
}