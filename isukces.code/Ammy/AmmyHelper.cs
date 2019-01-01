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
        public static IEnumerable<IAmmyCodePiece> Emit(IConversionCtx ctx,
            IEnumerable<KeyValuePair<string, object>> properties)
        {
            foreach (var a in properties)
            {
                var piece        = ToCodePiece(a.Value, ctx);
                var separateLine = ctx.ResolveSeparateLines(a.Key, piece);
                piece.WriteInSeparateLines = separateLine;
                if (string.IsNullOrEmpty(a.Key))
                    yield return piece;
                {
                }
                switch (piece)
                {
                    case IComplexAmmyCodePiece complexAmmyCodePiece:                        
                        yield return new ComplexAmmyCodePiece(complexAmmyCodePiece.GetNestedCodePieces(), a.Key + ": "+complexAmmyCodePiece.GetOpeningCode())
                        {
                            WriteInSeparateLines = piece.WriteInSeparateLines
                        };
                        break;
                    case ISimpleAmmyCodePiece sac:
                        yield return new SimpleAmmyCodePiece(a.Key + ": " + sac.Code, separateLine);
                        break;
                    default:
                        throw new NotSupportedException(piece.GetType().ToString());
                }
            }
        }


        public static void EmitObject<TObjType>(
            IEnumerable<KeyValuePair<string, object>> properties,
            IConversionCtx f,
            IAmmyCodeWriter writer)
        {
            var typeName = f.TypeName<TObjType>();
            var tmp      = Emit(f, properties).ToArray();
            writer.WriteComplex(typeName, tmp);
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
        public static IAmmyCodePiece ToCodePiece(object obj, IConversionCtx ctx)
        {
            string ToSimpleAmmyCodePiece()
            {
                switch (obj)
                {
                    case null:
                        return "null";
                    case string s:
                        return s.CsEncode();
                    case int i:
                        return i.ToString(CultureInfo.InvariantCulture);
                    case double d:
                        return d.ToString(CultureInfo.InvariantCulture);
                    case IAmmyExpression ac:
                        return ac.GetAmmyCode(ctx);
                }

                var t = obj.GetType();
                if (t.GetTypeInfo().IsEnum)
                {
                    if (ctx.FullNamespaces)
                        return ctx.TypeName(t) + "." + obj;
                    return obj.ToString();
                }

                return null;
            }

            if (obj is IAmmyCodePiece ammyCodePiece)
                return ammyCodePiece;
            if (obj is IAmmyCodePieceConvertible convertible)
                return convertible.ToCodePiece(ctx);
            var simple = ToSimpleAmmyCodePiece();
            if (simple != null)
                return new SimpleAmmyCodePiece(simple);

            throw new NotSupportedException("Unable to convert ToCodePiece " + obj.GetType());
        }
    }


    public class ConversionCtx : IConversionCtx
    {
        public ConversionCtx(IAmmyNamespaceProvider namespaceProvider, bool fullNamespaces = false)
        {
            NamespaceProvider = namespaceProvider;
            FullNamespaces    = fullNamespaces;
        }

        public bool ResolveSeparateLines([CanBeNull] string propertyName, [NotNull] IAmmyCodePiece value)
        {
            var h = OnResolveSeparateLines;
            if (h == null)
                return value.WriteInSeparateLines;
            var a = new ResolveSeparateLinesEventArgs
            {
                PropertyName         = propertyName,
                Value                = value,
                WriteInSeparateLines = value.WriteInSeparateLines
            };
            h.Invoke(this, a);
            return a.WriteInSeparateLines;
        }

        public IAmmyNamespaceProvider NamespaceProvider { get; }
        public bool                   FullNamespaces    { get; }

        public event EventHandler<ResolveSeparateLinesEventArgs> OnResolveSeparateLines;

        public class ResolveSeparateLinesEventArgs : EventArgs
        {
            public string         PropertyName         { get; set; }
            public IAmmyCodePiece Value                { get; set; }
            public bool           WriteInSeparateLines { get; set; }
        }
    }
}