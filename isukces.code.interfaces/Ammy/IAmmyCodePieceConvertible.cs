using System;

namespace isukces.code.interfaces.Ammy
{
    public interface IAmmyCodePieceConvertible
    {
        IAmmyCodePiece ToCodePiece(IConversionCtx ctx);
    }

    public static class AmmyCodePieceConvertibleExt
    {
        public static void AppendTo(this IAmmyCodePieceConvertible src, IAmmyCodeWriter writer, IConversionCtx ctx)
        {
            var nested = src.ToCodePieceWithLineSeparators(ctx, null);
            AppendCodePiece(writer, nested);
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
            var nested = src.ToCodePieceWithLineSeparators(ctx, null);
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

        public static IAmmyCodePiece ToCodePieceWithLineSeparators(this IAmmyCodePieceConvertible src,
            IConversionCtx ctx, string propertyName)
        {
            var nested = src.ToCodePiece(ctx); 
            nested.WriteInSeparateLines = ctx.ResolveSeparateLines(propertyName, nested, src);
            return nested;
        }
    }
}