using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        protected static PropertyInfo GetPropertyInfo<TValue>(Expression<Func<TRow, TValue>> func, string bindingPath)
        {
            var expression = CodeUtils.StripExpression(func);
            if (expression is null)
                return null;
            if (expression.NodeType == ExpressionType.Parameter)
                return null;

            var memberExpression = expression as MemberExpression;
            if (memberExpression?.Member is PropertyInfo pi)
                return pi;

            return typeof(TRow)
#if COREFX
                .GetTypeInfo()
#endif
                .GetProperty(bindingPath);
        }

        protected AmmyBind BindToModel<TValue>(Expression<Func<TRow, TValue>> func, XBindingMode? mode = null)
        {
            var name = CodeUtils.GetMemberPath(func);
            return new AmmyBind(name, mode);
        }


        protected GridColumn Col<TValue>(Expression<Func<TRow, TValue>> func, object headerSource,
            int? width = null)
        {
            var bindingPath  = CodeUtils.GetMemberPath(func);
            var propertyInfo = GetPropertyInfo(func, bindingPath);

            var result = new GridColumn
            {
                Name = bindingPath,
                Binding =
                {
                    Path = bindingPath,
                },
                HeaderSource = GetColumnHeaderSource(bindingPath, headerSource, propertyInfo),
                Width        = width,
                Member       = propertyInfo
            };

            if (propertyInfo != null)
            {
                var att = propertyInfo.GetCustomAttribute<DecimalPlacesAttribute>();
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
                switch (suggestedHeader)
                {
                    case string s when s.Length > 0:
                        return suggestedHeader;
                    default:
                        return suggestedHeader;
                }
#if COREFX20 || FULLFX
            var descriptionFromAttribute = property?
                .GetCustomAttribute<DescriptionAttribute>()?
                .Description;
            if (!string.IsNullOrEmpty(descriptionFromAttribute))
                return descriptionFromAttribute;
#endif
            return propertyName.Decamelize();
        }
    }
}