using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace iSukces.Code.Ui.DataGrid
{
    
    public abstract class BasicDataGridConfigurationProvider {
        public abstract IEnumerable<BasicDataGridColumn> GetColumnsGeneral();

        public abstract bool AddExpandColumn { get; }
    }
    public abstract class BasicDataGridConfigurationProvider<TColumn>: BasicDataGridConfigurationProvider
        where TColumn:BasicDataGridColumn
    {
        public abstract IEnumerable<TColumn> GetColumns();
        public override IEnumerable<BasicDataGridColumn> GetColumnsGeneral()
        {
            return GetColumns();
        }
    }

    public abstract class DataGridConfigurationProvider<TRow, TColumn> : BasicDataGridConfigurationProvider<TColumn>
        where TColumn : BasicDataGridColumn, new()
    {
        protected static PropertyInfo? GetPropertyInfo<TValue>(Expression<Func<TRow, TValue>> func, string bindingPath)
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

        protected TColumn Col<TValue>(Expression<Func<TRow, TValue>> func, object? headerSource,
            UiWidth width = default)
        {
            var bindingPath  = CodeUtils.GetMemberPath(func);
            var propertyInfo = GetPropertyInfo(func, bindingPath);

            var result = new TColumn
            {
                Name         = bindingPath,
                HeaderSource = GetColumnHeaderSource(bindingPath, headerSource, propertyInfo),
                Width        = width,
                Member       = propertyInfo,
            };

            if (propertyInfo is not null)
            {
                var att = propertyInfo.GetCustomAttribute<DecimalPlacesAttribute>();
                if (att is not null)
                    result = result.WithDataFormatString(att.Format);
            }

            return result;
        }

        protected TColumn Col<TValue>(Expression<Func<TRow, TValue>> func, UiWidth width)
        {
            var col = Col(func, null, width);
            return col;
        }

        protected virtual object GetColumnHeaderSource(string? propertyName, object? suggestedHeader,
            PropertyInfo? property)
        {
            if (suggestedHeader is not null)
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
