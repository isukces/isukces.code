using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using isukces.code.interfaces;
using isukces.code.interfaces.Ammy;
using JetBrains.Annotations;

namespace isukces.code.Ammy
{
    public static class AmmyHelper
    {
        [NotNull]
        public static IAmmyCodePiece AnyToCodePiece(this IConversionCtx ctx, object src)
        {
            string ToSimpleAmmyCodePiece()
            {
                switch (src)
                {
                    case null:
                        return "null";
                    case string s:
                        return s.CsEncode();
                    case int i:
                        return i.ToString(CultureInfo.InvariantCulture);
                    case double d:
                        return d.ToString(CultureInfo.InvariantCulture);
                }

                var t = src.GetType();
                if (t.GetTypeInfo().IsEnum)
                {
                    if (ctx.FullNamespaces)
                        return ctx.TypeName(t) + "." + src;
                    return src.ToString();
                }

                return null;
            }

            if (src is IAmmyCodePiece ammyCodePiece)
                return ammyCodePiece;
            if (src is IAmmyCodePieceConvertible convertible)
                return convertible.ToCodePiece(ctx);

            var simple = ToSimpleAmmyCodePiece();
            if (simple != null)
            {
                var nested = new SimpleAmmyCodePiece(simple);
                return nested;
            }

            throw new NotSupportedException("Unable to convert ToCodePiece " + src.GetType());
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

        [NotNull]
        public static IAmmyCodePiece ToCodePieceWithLineSeparators(this IConversionCtx ctx, object obj,
            string propertyName)
        {
            var tmp = AnyToCodePiece(ctx, obj);
            tmp.WriteInSeparateLines = ctx.ResolveSeparateLines(propertyName, tmp, obj);
            return tmp;
        }

        [NotNull]
        public static IAmmyCodePiece[] ToAmmyPropertiesCodePieces(this IEnumerable<KeyValuePair<string, object>> values,
            IConversionCtx ctx,
            bool? forceWriteInSeparateLines = null)
        {
            if (values == null)
                return new IAmmyCodePiece[0];
            var result = values.Select(a =>
            {
                var piece = ctx.ToAmmyPropertyCodePiece(a.Key, a.Value);
                if (forceWriteInSeparateLines != null)
                    piece.WriteInSeparateLines = forceWriteInSeparateLines.Value;
                return piece;
            }).ToArray();
            return result;
        }

        public static IAmmyCodePiece ToAmmyPropertyCodePiece(this IConversionCtx ctx, string propertyName, object value)
        {
            var piece = ToCodePieceWithLineSeparators(ctx, value, propertyName);
            if (string.IsNullOrEmpty(propertyName))
                return piece;

            switch (piece)
            {
                case IComplexAmmyCodePiece complexAmmyCodePiece:
                    return new ComplexAmmyCodePiece(complexAmmyCodePiece.GetNestedCodePieces(),
                        propertyName + ": " + complexAmmyCodePiece.GetOpeningCode())
                    {
                        WriteInSeparateLines = piece.WriteInSeparateLines
                    };
                case ISimpleAmmyCodePiece sac:
                    return new SimpleAmmyCodePiece(propertyName + ": " + sac.Code, piece.WriteInSeparateLines);
                default:
                    throw new NotSupportedException(piece.GetType().ToString());
            }
        }
    }
}