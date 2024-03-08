using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace iSukces.Code
{
    public static class CodeUtils
    {
        public static MemberExpression GetMemberInfo(Expression method)
        {
            var e = StripExpression(method);
            if (e is MemberExpression me)
                return me;
            throw new ArgumentException("method");
        }

        public static string GetMemberPath<TBrowser, TProperty>(Expression<Func<TBrowser, TProperty>> func)
        {
            var expression = StripExpression(func);
            if (expression is null)
                return string.Empty;
            if (expression.NodeType == ExpressionType.Parameter)
                return string.Empty;
            var mi = expression as MemberExpression;
            if (expression is null)
                throw new NotSupportedException(expression.NodeType.ToString());
            var list = new List<string>();
            while (mi != null)
            {
                list.Add(mi.Member.Name);
                if (mi.Expression is UnaryExpression ue)
                {
                    if (ue.NodeType == ExpressionType.Convert)
                    {
                        var ex = ue.Operand;
                        if (ex is MemberExpression me)
                        {
                            mi = me;
                            continue;
                        }
                    }

                    break;
                }

                mi = mi.Expression as MemberExpression;
            }

            if (list.Count == 0)
                return string.Empty;
            if (list.Count == 1)
                return list[0];
            var sb  = new StringBuilder();
            var add = false;
            for (var index = list.Count - 1; index >= 0; index--)
            {
                var name = list[index];
                if (add)
                    sb.Append('.');
                else
                    add = true;
                sb.Append(name);
            }

            return sb.ToString();
        }

        public static DirectoryInfo SearchFoldersUntilFileExists(Assembly a, string fileName)
        {
            var di = new FileInfo(a.Location).Directory;
            di = SearchFoldersUntilFileExists(di, fileName);
            return di;
        }

        public static DirectoryInfo SearchFoldersUntilFileExists(DirectoryInfo di, string fileName)
        {
            while (di != null)
            {
                if (!di.Exists)
                    return null;
                var fi = Path.Combine(di.FullName, fileName);
                if (File.Exists(fi))
                    return di;
                di = di.Parent;
            }

            return null;
        }

        public static Expression StripExpression(Expression method)
        {
            if (!(method is LambdaExpression lambda))
                throw new ArgumentNullException(nameof(method));

            switch (lambda.Body.NodeType)
            {
                case ExpressionType.Convert:
                    return ((UnaryExpression)lambda.Body).Operand;
                case ExpressionType.MemberAccess:
                    return lambda.Body;
                case ExpressionType.Parameter:
                    return lambda.Body;
                default:
                    return null;
            }
        }
    }
}