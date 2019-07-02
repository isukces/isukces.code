using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace isukces.code
{
    public static  class CodeUtils
    {
        public static DirectoryInfo SearchFoldersUntilFileExists(Assembly a, string fileName)
        {
            var di = new FileInfo(a.Location).Directory;
            di = SearchFoldersUntilFileExists(di,fileName);
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

        public static MemberExpression GetMemberInfo(Expression method)
        {
            if (!(method is LambdaExpression lambda))
                throw new ArgumentNullException(nameof(method));

            MemberExpression memberExpr = null;
            if (lambda.Body.NodeType == ExpressionType.Convert)
                memberExpr =
                    ((UnaryExpression)lambda.Body).Operand as MemberExpression;
            else if (lambda.Body.NodeType == ExpressionType.MemberAccess) memberExpr = lambda.Body as MemberExpression;

            if (memberExpr == null)
                throw new ArgumentException("method");

            return memberExpr;
        }

        public static string GetMemberPath<T,T2>(Expression<Func<T, T2>> func)
        {
            var mi   = GetMemberInfo(func);
            var list = new List<string>();
            while (mi != null)
            {
                list.Add(mi.Member.Name);
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
    }
}