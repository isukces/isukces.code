using System;

namespace isukces.code.interfaces.Ammy
{
    public interface IAmmyCodePieceConvertible
    {
        IAmmyCodePiece ToCodePiece(IConversionCtx ctx);
    }

    public static class AmmyCodePieceConvertibleExt
    {
        public static void WriteLineTo(this IAmmyCodePieceConvertible src, IAmmyCodeWriter writer, IConversionCtx ctx)
        {
            var nested = src.ToCodePiece(ctx);
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
        
        public static void AppendTo(this IAmmyCodePieceConvertible src, IAmmyCodeWriter writer, IConversionCtx ctx)
        {
            var nested = src.ToCodePiece(ctx);
            switch (nested)
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
                    throw new NotSupportedException(nested.GetType().ToString());
            }
        }
    }
}