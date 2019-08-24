using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
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
                    case int intValue: return intValue.ToCsString();
                    case long longValue: return longValue.ToCsString();
                    case short shortValue: return shortValue.ToCsString();
                    case byte byteValue: return byteValue.ToCsString();
                    case uint uintValue: return uintValue.ToCsString();
                    case ulong ulongValue: return ulongValue.ToCsString();
                    case ushort ushortValue: return ushortValue.ToCsString();
                    case sbyte sbyteValue: return sbyteValue.ToCsString();
                    case double doubleValue: return doubleValue.ToCsString();
                    case float floatValue: return floatValue.ToCsString();
                    case decimal decimalValue: return decimalValue.ToCsString();
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

        /*
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
        */

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
            return WithPropertyNameBefore(piece, propertyName);
        }

        [NotNull]
        public static IAmmyCodePiece ToCodePieceWithLineSeparators(this IConversionCtx ctx, object obj,
            string propertyName, object objHost)
        {
            var tmp = AnyToCodePiece(ctx, obj);
            tmp.WriteInSeparateLines = ctx.ResolveSeparateLines(propertyName, tmp, obj, objHost);
            return tmp;
        }

        public static IAmmyCodePiece WithPropertyNameBefore(this IAmmyCodePiece piece, string propertyName)
        {
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
    }
}