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

        public static int WriteComplex(this IAmmyCodeWriter writer, IComplexAmmyCodePiece code)
        {
            var                           openingCode = code.GetOpeningCode();
            IReadOnlyList<IAmmyCodePiece> prop        = code.GetNestedCodePieces().ToList();
            return WriteComplex(writer, openingCode, prop);
        }

        public static int WriteComplex(this IAmmyCodeWriter writer, string openingCode,
            IReadOnlyList<IAmmyCodePiece> codePieces)
        {
            // nie dodajemy entera na końcu
            if (codePieces == null || codePieces.Count == 0)
            {
                writer.Append(openingCode + " {}");
                return 0;
            }
            int entersCount = 0;

            writer.Append(openingCode + " {");
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
                            var nestedEntes = writer.WriteComplex(complexAmmyCodePiece);
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
                    addComma = false;
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
                            writer.WriteComplex(complexAmmyCodePiece);
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
                writer.WriteIndent().Append("}");
            }
            else
            {
                if (needAddNewLineForPreviousContent)
                    writer.Append(" }");
                else
                    writer.Append("}");
                writer.Indent--;
            }

            return entersCount;
        }
    }
}