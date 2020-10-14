using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using iSukces.Code.Ammy;
using iSukces.Code.Compatibility.System.Windows.Data;
using JetBrains.Annotations;

namespace iSukces.Code.Ui.DataGrid
{
    public abstract partial class DataGridConfigurationProvider
    {
        public abstract IEnumerable<GridColumn> GetColumns();
        public abstract bool AddExpandColumn { get; }
    }

    public abstract class DataGridConfigurationProvider<TRow> : DataGridConfigurationProvider
    {
        protected AmmyBind BindToModel<TValue>(Expression<Func<TRow, TValue>> func, XBindingMode? mode = null)
        {
            var name = CodeUtils.GetMemberPath(func);
            return new AmmyBind(name, mode);
        }


        protected GridColumn Col<TValue>(Expression<Func<TRow, TValue>> func, object headerSource,
            int? width = null)
        {
            var name = CodeUtils.GetMemberPath(func);

            var prop = typeof(TRow)
#if COREFX
                .GetTypeInfo()
#endif
                .GetProperty(name);

            var result = new GridColumn
            {
                Name              = name,
                Binding =
                {
                    Path = name,
                },
                //DataMemberBinding = name,
                HeaderSource = GetColumnHeaderSource(name, headerSource, prop),
                Width        = width
            };

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

        protected object GetColumnHeaderSource([CanBeNull] string propertyName, [CanBeNull] object suggestedHeader,
            [CanBeNull] PropertyInfo property)
        {
            if (!(suggestedHeader is null))
            {
                switch (suggestedHeader)
                {
                    case string s when s.Length > 0:
                        return suggestedHeader;
                    default:
                        return suggestedHeader;
                }
            }
#if COREFX20 || FULLFX
            var descriptionFromAttribute = property?
                .GetCustomAttribute<System.ComponentModel.DescriptionAttribute>()?
                .Description;
            if (!string.IsNullOrEmpty(descriptionFromAttribute))
                return descriptionFromAttribute;
#endif
            return propertyName.Decamelize();
        }
    }
}