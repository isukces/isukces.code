using System;

namespace isukces.code.interfaces.Ammy
{
    public interface IAmmyCodePieceConvertible
    {
        IAmmyCodePiece ToAmmyCode(IConversionCtx ctx);
    }

    public static class AmmyCodePieceConvertibleExt
    {
        public static void AppendTo(this IAmmyCodePieceConvertible src, IAmmyCodeWriter writer, IConversionCtx ctx)
        {
            var nested = src.ToCodePieceWithLineSeparators(ctx, null, null);
            AppendCodePiece(writer, nested);
        }

        public static void AppendConvertible(this IAmmyCodeWriter writer, IAmmyCodePieceConvertible src, IConversionCtx ctx)
        {
            src.AppendTo(writer, ctx);
        }
        
        public static void AppendCodePiece(this IAmmyCodeWriter writer, IAmmyCodePiece piece)
        {
            switch (piece)
            {
                case null:
                    break;
                case IComplexAmmyCodePiece complexAmmyCodePiece:
                    writer.AppendComplex(complexAmmyCodePiece);
                    break;
                case ISimpleAmmyCodePiece simpleAmmyCodePiece:
                    writer.Append(simpleAmmyCodePiece.Code);
                    break;
                default:
                    throw new NotSupportedException(piece.GetType().ToString());
            }
        }


        public static void WriteLineTo(this IAmmyCodePieceConvertible src, IAmmyCodeWriter writer, IConversionCtx ctx)
        {
            var nested = src.ToCodePieceWithLineSeparators(ctx, null, null);
            switch (nested)
            {
                case null:
                    break;
                case IComplexAmmyCodePiece complexAmmyCodePiece:
                    writer.WriteIndent();
                    writer.AppendComplex(complexAmmyCodePiece);
                    writer.WriteLine();
                    break;
                case ISimpleAmmyCodePiece simpleAmmyCodePiece:
                    writer.WriteIndent();
                    writer.Append(simpleAmmyCodePiece.Code);
                    writer.WriteLine();
                    break;
                default:
                    throw new NotSupportedException(nested.GetType().ToString());
            }
        }

        public static IAmmyCodePiece ToCodePieceWithLineSeparators(this IAmmyCodePieceConvertible propertyValue,
            IConversionCtx ctx, string propertyName, object objectHost)
        {
            var nested = propertyValue.ToAmmyCode(ctx); 
            nested.WriteInSeparateLines = ctx.ResolveSeparateLines(propertyName, nested, propertyValue, objectHost);
            return nested;
        }
    }
}