using System;
using System.Collections.Generic;
using System.Linq;
using isukces.code.interfaces.Ammy;

namespace isukces.code.interfaces
{
    public interface IAmmyCodeWriter : ICodeWriter
    {
    }

    public static class AmmyCodeFormatterExt
    {
        public static int AppendComplex(this IAmmyCodeWriter writer, IComplexAmmyCodePiece code)
        {
            var                           openingCode = code.GetOpeningCode();
            IReadOnlyList<IAmmyCodePiece> prop        = code.GetNestedCodePieces().ToList();
            return AppendComplex(writer, openingCode, prop, code.Brackets);
        }

        public static int AppendComplex(this IAmmyCodeWriter writer, string openingCode,
            IReadOnlyList<IAmmyCodePiece> codePieces, AmmyBracketKind kind)
        {
            var openingBracket = GetOpeningBracket(kind);
            var closingBracket = GetClosingBracket(kind);
            // nie dodajemy entera na końcu
            if (codePieces == null || codePieces.Count == 0)
            {
                writer.Append(openingCode + " " + openingBracket + closingBracket);
                return 0;
            }

            var entersCount = 0;

            writer.Append(openingCode + " " + openingBracket);
            writer.Indent++;
            var addComma                         = false;
            var addNewLineBeforeClose            = false;
            var needAddNewLineForPreviousContent = true;
            foreach (var i in codePieces)
                if (i.WriteInSeparateLines)
                {
                    if (needAddNewLineForPreviousContent)
                    {
                        writer.WriteLine();
                        entersCount++;
                    }

                    writer.WriteIndent();
                    switch (i)
                    {
                        case IComplexAmmyCodePiece complexAmmyCodePiece:
                            var nestedEntes = writer.AppendComplex(complexAmmyCodePiece);
                            break;
                        case ISimpleAmmyCodePiece simpleAmmyCode:
                            writer.Append(simpleAmmyCode.Code);

                            break;
                        default:
                            throw new NotImplementedException(i.GetType().Name);
                    }

                    writer.WriteLine();
                    addNewLineBeforeClose            = true;
                    needAddNewLineForPreviousContent = false;
                    addComma                         = false;
                }
                else
                {
                    if (addComma)
                        writer.Append(", ");
                    else
                        writer.Append(" ");
                    switch (i)
                    {
                        case IComplexAmmyCodePiece complexAmmyCodePiece:
                            writer.AppendComplex(complexAmmyCodePiece);
                            addNewLineBeforeClose            = true;
                            needAddNewLineForPreviousContent = false;
                            break;
                        case ISimpleAmmyCodePiece simpleAmmyCode:
                            writer.Append(simpleAmmyCode.Code);
                            break;
                        default:
                            throw new NotImplementedException(i.GetType().Name);
                    }

                    needAddNewLineForPreviousContent = true;
                    addComma                         = true;
                }

            if (addNewLineBeforeClose && needAddNewLineForPreviousContent)
            {
                writer.WriteLine();
                entersCount++;
                writer.Indent--;
                writer.WriteIndent().Append(closingBracket);
            }
            else
            {
                if (needAddNewLineForPreviousContent)
                    writer.Append(" " + closingBracket);
                else
                    writer.Append(closingBracket);
                writer.Indent--;
            }

            return entersCount;
        }

        public static void CloseArray(this IAmmyCodeWriter src)
        {
            src.Indent--;
            src.WriteLine("]");
        }

        public static void OpenArray(this IAmmyCodeWriter src, string text)
        {
            src.WriteLine(text + " [");
            src.Indent++;
        }

        private static string GetClosingBracket(AmmyBracketKind kind)
        {
            switch (kind)
            {
                case AmmyBracketKind.Mustache:
                    return "}";
                case AmmyBracketKind.Square:
                    return "]";
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }

        private static string GetOpeningBracket(AmmyBracketKind kind)
        {
            switch (kind)
            {
                case AmmyBracketKind.Mustache:
                    return "{";
                case AmmyBracketKind.Square:
                    return "[";
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }
    }
}