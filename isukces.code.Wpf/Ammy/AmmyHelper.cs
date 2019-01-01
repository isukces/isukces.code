using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using isukces.code.interfaces;
using isukces.code.interfaces.Ammy;

namespace isukces.code.Wpf.Ammy
{
    public static class AmmyHelper
    {
        public static string EmitObject<TObjType>(Dictionary<string, object> props, IConversionCtx f,
                                                  List<object>               content)
        {
            return TypeName<TObjType>(f) + " { " + Emit(f, props, content) + " }";
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

        public static string AnyObjectToString(object v, IConversionCtx f)
        {
            switch (v)
            {
                case null:
                    return "null";
                case string s:
                    return s.CsCite();
                case int i:
                    return i.ToString(CultureInfo.InvariantCulture);
                case double d:
                    return d.ToString(CultureInfo.InvariantCulture);
                case IAmmyExpression ac:
                    return ac.GetAmmyCode(f);
            }

            var t = v.GetType();
            if (t.IsEnum)
            {
                if (f.FullNamespaces)
                    return TypeName(t, f) + "." + v;
                return v.ToString();
            }

            throw new NotSupportedException();
        }

        public static string TypeName(Type t, IConversionCtx ctx)
        {
            if (!ctx.FullNamespaces && ctx.Aaa.Namespaces.Contains(t.Namespace))
                return t.Name;
            return t.FullName;
        }

        public static string TypeName<T>(IConversionCtx ctx)
        {
            return TypeName(typeof(T), ctx);
        }

        private static string Emit(IConversionCtx ctx, Dictionary<string, object> props, IEnumerable<object> content)
        {
            var p = props.Select(a => a.Key + ": " + AnyObjectToString(a.Value, ctx));
            var c = content.Select(a => AnyObjectToString(a, ctx));
            return string.Join(", ", p.Union(c));
        }
    }


   

    public struct ConversionCtx : IConversionCtx
    {
        public ConversionCtx(IAmmyNamespaceProvider aaa, bool fullNamespaces=false)
        {
            Aaa            = aaa;
            FullNamespaces = fullNamespaces;
        }

        public IAmmyNamespaceProvider Aaa            { get; set; }
        public bool                   FullNamespaces { get; set; }
    }

   
}