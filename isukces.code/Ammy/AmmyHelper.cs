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
                    case null: return "null";
                    case bool boolValue: return boolValue ? "true" : "false";
                    case string stringValue: return stringValue.CsEncode();
                    case int intValue: return intValue.ToString(CultureInfo.InvariantCulture);
                    case long longValue: return longValue.ToString(CultureInfo.InvariantCulture);
                    case short shortValue: return shortValue.ToString(CultureInfo.InvariantCulture);
                    case byte byteValue: return byteValue.ToString(CultureInfo.InvariantCulture);
                    case uint uintValue: return uintValue.ToString(CultureInfo.InvariantCulture);
                    case ulong ulongValue: return ulongValue.ToString(CultureInfo.InvariantCulture);
                    case ushort ushortValue: return ushortValue.ToString(CultureInfo.InvariantCulture);
                    case sbyte sbyteValue: return sbyteValue.ToString(CultureInfo.InvariantCulture);
                    case double doubleValue: return doubleValue.ToString(CultureInfo.InvariantCulture);
                    case float floatValue: return floatValue.ToString(CultureInfo.InvariantCulture);
                    case decimal decimalValue: return decimalValue.ToString(CultureInfo.InvariantCulture);
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
                return convertible.ToAmmyCode(ctx);

            var simple = ToSimpleAmmyCodePiece();
            if (simple != null)
            {
                var nested = new SimpleAmmyCodePiece(simple);
                return nested;
            }

            throw new NotSupportedException("Unable to convert ToCodePiece " + src.GetType());
        }

        [NotNull]
        public static IAmmyCodePiece[] ConvertArguments(
            [NotNull] this IConversionCtx ctx,
            [CanBeNull] Func<object, int, bool> cutLastIf,
            params object[] arguments)
        {
            if (ctx == null) throw new ArgumentNullException(nameof(ctx));
            var result      = new IAmmyCodePiece[arguments.Length];
            var callCutLast = cutLastIf != null;
            var take        = arguments.Length;
            for (var argumentIndex = arguments.Length - 1; argumentIndex >= 0; argumentIndex--)
            {
                var arg = arguments[argumentIndex];
                if (callCutLast)
                {
                    if (cutLastIf(arg, argumentIndex))
                    {
                        take = argumentIndex;
                        continue;
                    }

                    callCutLast = false;
                }

                var code = ctx.AnyToCodePiece(arg ?? AmmyNone.Instance);
                code.WriteInSeparateLines = false;
                result[argumentIndex]     = code;
            }

            if (take == arguments.Length)
                return result;
            var res = new IAmmyCodePiece[take];
            Array.Copy(result, res, take);
            return res;
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
        public static IAmmyCodePiece[] ToAmmyContentItemsCode(this IEnumerable<object> values,
            IConversionCtx ctx,
            object objHost,
            bool? forceWriteInSeparateLines = null)
        {
            if (values == null)
                return new IAmmyCodePiece[0];
            var result = values.Select(a =>
            {
                var piece = ctx.ToAmmyPropertyCodePiece(null, a, objHost);
                if (forceWriteInSeparateLines != null)
                    piece.WriteInSeparateLines = forceWriteInSeparateLines.Value;
                return piece;
            }).ToArray();
            return result;
        }

        [NotNull]
        public static IAmmyCodePiece[] ToAmmyPropertiesCode(this IEnumerable<KeyValuePair<string, object>> values,
            IConversionCtx ctx,
            object objHost,
            bool? forceWriteInSeparateLines = null)
        {
            if (values == null)
                return new IAmmyCodePiece[0];
            var result = values.Select(a =>
            {
                var piece = ctx.ToAmmyPropertyCodePiece(a.Key, a.Value, objHost);
                if (forceWriteInSeparateLines != null)
                    piece.WriteInSeparateLines = forceWriteInSeparateLines.Value;
                return piece;
            }).ToArray();
            return result;
        }

        [NotNull]
        public static IAmmyCodePiece[] ToAmmyPropertiesCodeWithLineSeparators(
            this IEnumerable<KeyValuePair<string, object>> values,
            IConversionCtx ctx,
            object objHost)
        {
            if (values == null)
                return new IAmmyCodePiece[0];
            var result = values.Select(a =>
            {
                var piece = ctx.ToAmmyPropertyCodePiece(a.Key, a.Value, objHost);
                piece.WriteInSeparateLines = ctx.ResolveSeparateLines(a.Key, piece, a.Value, objHost);
                return piece;
            }).ToArray();
            return result;
        }

        public static IAmmyCodePiece ToAmmyPropertyCodePiece(this IConversionCtx ctx, string propertyName, object value,
            object objHost)
        {
            var piece = ToCodePieceWithLineSeparators(ctx, value, propertyName, objHost);
            if (string.IsNullOrEmpty(propertyName))
                return piece;

            switch (piece)
            {
                case IComplexAmmyCodePiece complexAmmyCodePiece:
                    return new ComplexAmmyCodePiece(complexAmmyCodePiece.GetNestedCodePieces(),
                        propertyName + ": " + complexAmmyCodePiece.GetOpeningCode(),
                        complexAmmyCodePiece.Brackets)
                    {
                        WriteInSeparateLines = piece.WriteInSeparateLines
                    };
                case ISimpleAmmyCodePiece sac:
                    return new SimpleAmmyCodePiece(propertyName + ": " + sac.Code, piece.WriteInSeparateLines);
                default:
                    throw new NotSupportedException(piece.GetType().ToString());
            }
        }

        [NotNull]
        public static IAmmyCodePiece ToCodePieceWithLineSeparators(this IConversionCtx ctx, object obj,
            string propertyName, object objHost)
        {
            var tmp = AnyToCodePiece(ctx, obj);
            tmp.WriteInSeparateLines = ctx.ResolveSeparateLines(propertyName, tmp, obj, objHost);
            return tmp;
        }
    }
}