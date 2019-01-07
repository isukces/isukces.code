// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

using System.Collections.Generic;
using System.Linq;
using isukces.code.interfaces;
using isukces.code.interfaces.Ammy;

namespace isukces.code.Ammy
{
    public class AmmyBind : IAmmyCodePieceConvertible
    {
        public AmmyBind(string bindingPath, DataBindingMode? mode = null)
        {
            BindingPath = bindingPath;
            if (mode != null)
                AddParameter("Mode", mode);
        }

        public void AddParameter(string key, object value)
        {
            Items.Add(new KeyValuePair<string, object>(key, value));
        }

        public IAmmyCodePiece ToAmmyCode(IConversionCtx ctx)
        {
            return new SimpleAmmyCodePiece(ToOneLineCode(ctx));
        }

        public string ToOneLineCode(IConversionCtx ctx)
        {
            var txt = new AmmyCodeWriter();
            txt.Append("bind");
            if (!string.IsNullOrEmpty(BindingPath) && BindingPath != ".")
                txt.Append(" " + BindingPath.CsEncode());
            if (From != null)
            {
                var piece = ctx.AnyToCodePiece(From);
                txt.Append(" from ");
                txt.AppendCodePiece(piece);
            }

            if (Items.Any())
            {
                var bindingSetItems = Items.ToAmmyPropertiesCodeWithLineSeparators(ctx, this);
                var anyInNewLine = bindingSetItems.Any(a => a.WriteInSeparateLines);
                if (anyInNewLine)
                    txt.WriteNewLineAndIndent().Append("set [");
                else
                    txt.Append(" set [");
                txt.Indent++;
                
                var addComma = false;
                for (var index = 0; index < bindingSetItems.Length; index++)
                {
                    var el = bindingSetItems[index];
                    if (el.WriteInSeparateLines)
                    {
                        txt.WriteNewLineAndIndent().AppendCodePiece(el);
                        addComma = false;
                    }
                    else
                    {
                        txt.AppendCommaIf(addComma).AppendCodePiece(el);
                        addComma = true;
                    }
                }

                txt.DecIndent();
                if (anyInNewLine)
                    txt.WriteNewLineAndIndent();
                txt.Append("]");
            }
            return txt.Code;
        }


        public override string ToString()
        {
            return ToOneLineCode(new ConversionCtx(new AmmyNamespaceProvider()));
        }

        public string BindingPath { get; set; }

        // public DataBindingMode? Mode        { get; set; }
        public object                             From  { get; set; }
        public List<KeyValuePair<string, object>> Items { get; } = new List<KeyValuePair<string, object>>();
    }


    public enum DataBindingMode
    {
        TwoWay,
        OneWay,
        OneTime,
        OneWayToSource,
        Default
    }
}