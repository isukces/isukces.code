using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using isukces.code.Ammy;

namespace isukces.code.Ui.DataGrid
{
    public abstract partial class DataGridConfigurationProvider
    {
        public abstract IEnumerable<GridColumn> GetColumns();
        public abstract bool AddExpandColumn { get; }
    }

    public abstract class DataGridConfigurationProvider<TRow> : DataGridConfigurationProvider
    {
        protected AmmyBind BindToModel<TValue>(Expression<Func<TRow, TValue>> func, DataBindingMode? mode = null)
        {
            var name = CodeUtils.GetMemberPath(func);
            return new AmmyBind(name, mode);
        }

        protected GridColumn Col<TValue>(Expression<Func<TRow, TValue>> func, string header,
            int? width = null)
        {
            var name = CodeUtils.GetMemberPath(func);
            var result = new GridColumn
            {
                Name              = name,
                DataMemberBinding = name,
                Header            = header ?? name,
                Width             = width
            };
            var prop = typeof(TRow)
#if COREFX
                .GetTypeInfo()
#endif
                .GetProperty(name);
            if (prop != null)
            {
                var att = prop.GetCustomAttribute<DecimalPlacesAttribute>();
                if (att != null)
                    result = result.WithDataFormatString(att.Format);
            }

            return result;
        }

        protected GridColumn Col<TValue>(Expression<Func<TRow, TValue>> func, int width)
        {
            var col = Col(func, null, width);
            return col;
        }
    }
}