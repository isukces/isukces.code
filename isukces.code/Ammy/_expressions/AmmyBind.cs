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

        public IAmmyCodePiece ToCodePiece(IConversionCtx ctx)
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
                var cp = Items.ToAmmyPropertiesCodePieces(ctx);
                txt.Append(" set [");
                for (var index = 0; index < cp.Length; index++)
                {
                    if (index > 0)
                        txt.Append(", ");
                    txt.AppendCodePiece(cp[index]);
                }

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