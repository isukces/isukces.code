using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using isukces.code.Ammy;
using isukces.code.Compatibility.System.Windows.Data;
using JetBrains.Annotations;

namespace isukces.code.Ui.DataGrid
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


        protected GridColumn Col<TValue>(Expression<Func<TRow, TValue>> func, string header,
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
                    Path = name
                },
                Header            = GetColumnHeader(name, header, prop),
                Width             = width
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

        protected string GetColumnHeader([CanBeNull] string propertyName, [CanBeNull] string suggestedHeader,
            [CanBeNull] PropertyInfo property)
        {
            if (!string.IsNullOrEmpty(suggestedHeader))
                return suggestedHeader;
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