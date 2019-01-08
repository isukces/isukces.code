using System;
using System.Collections.Generic;
using System.Linq;
using isukces.code.Ammy;
using isukces.code.interfaces.Ammy;

namespace isukces.code.Tests.Ammy
{
    public class AmmyPretty
    {       
        public static bool UseNewLine(ConversionCtx.ResolveSeparateLinesEventArgs args)
        {
            if (args.SourceValueHost is AmmyMixin)
                return true;
            if (args.SourceValueHost is AmmyBind)
            {
                if (args.Code is AmmyBind.SetCollection sc)
                {                    
                    var any =  sc.SetItems.Any(codePiece => codePiece.WriteInSeparateLines);
                    if (any)
                    {
                        foreach (var i in sc.SetItems)
                        {
                            if (i is ISimpleAmmyCodePiece cp)
                            {
                                if (cp.Code.StartsWith("Converter:"))
                                    i.WriteInSeparateLines = true;
                            }
                        }
                    }
                    return any;
                }

                return args.PropertyName == "set.ValidationRules";
            }             

            var sv = GetTypeBrowser(args.SourceValue?.GetType());
            if (sv == typeof(RowDefinition) || sv == typeof(ColumnDefinition))
                return true;

            sv = GetTypeBrowser(args.SourceValueHost?.GetType());
            if (sv == typeof(RowDefinition) || sv == typeof(ColumnDefinition) || sv == typeof(DoubleValidation))
                return false;

            if (sv == typeof(TextBox) || sv == typeof(TextBlock) || sv == typeof(RadComboBox) || sv == typeof(CheckBox))
                return true;
           

            switch (args.Code)
            {
                case ISimpleAmmyCodePiece _:
                    return false;
                case IComplexAmmyCodePiece complexAmmyCodePiece:
                    if (complexAmmyCodePiece.Brackets == AmmyBracketKind.Square)
                        return true;
                    if (string.IsNullOrEmpty(args.PropertyName))
                        return true;
                    if (complexAmmyCodePiece.GetOpeningCode().StartsWith("CellTemplate", StringComparison.Ordinal))
                        return true;
                    var n = complexAmmyCodePiece.GetNestedCodePieces();
                    if (n.Count > 0)
                        return n.Any(a => a.WriteInSeparateLines || a is IComplexAmmyCodePiece);
                    break;
                default:
                    throw new NotSupportedException();
            }

            return false;
        }

        public static bool UseNewLine2(ConversionCtx.ResolveSeparateLinesEventArgs args)
        {
            switch (args.Code)
            {
                case ISimpleAmmyCodePiece _:
                    return false;
                case IComplexAmmyCodePiece complexAmmyCodePiece:
                    if (complexAmmyCodePiece.Brackets == AmmyBracketKind.Square)
                        return true;
                    if (string.IsNullOrEmpty(args.PropertyName))
                        return true;
                    if (complexAmmyCodePiece.GetOpeningCode().StartsWith("CellTemplate", StringComparison.Ordinal))
                        return true;
                    var n = complexAmmyCodePiece.GetNestedCodePieces();
                    if (n.Count > 0)
                        return n.Any(a => a.WriteInSeparateLines || a is IComplexAmmyCodePiece);
                    break;
                default:
                    throw new NotSupportedException();
            }

            return false;
        }

        public static void VeryPretty(object sender, ConversionCtx.ResolveSeparateLinesEventArgs e)
        {
            e.WriteInSeparateLines = UseNewLine(e);
            e.Handled              = true;
        }

        private static Type GetTypeBrowser(Type type)
        {
            if (type == null) return null;
            if (!type.IsGenericType) return null;
            if (type.IsGenericTypeDefinition)
                return null;
            var t1 = type.GetGenericTypeDefinition();
            if (t1 == typeof(AmmyObjectBuilder<>))
                return type.GetGenericArguments()[0];
            return null;
        }
 
    }
}