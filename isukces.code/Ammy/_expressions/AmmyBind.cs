// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
using System.Collections.Generic;
using System.Linq;
using isukces.code.interfaces;
using isukces.code.interfaces.Ammy;

namespace isukces.code.Ammy
{
    public class AmmyBind : IAmmyCodePieceConvertible
    {
        public AmmyBind(string propertyName, DataBindingMode? mode = null)
        {
            PropertyName = propertyName;
            Mode         = mode;
        }

      

        public override string ToString()
        {
            var txt = "bind";
            if (!string.IsNullOrEmpty(PropertyName) && PropertyName != ".")
                txt += " " + PropertyName.CsEncode();
            {
                var se = new List<string>();
                if (Mode != null)
                    se.Add($"Mode: {Mode}");
                if (se.Any())
                    txt = txt + " set [" + string.Join(", ", se) + "]";
            }
            return txt;
        }

        public IAmmyCodePiece ToCodePiece(IConversionCtx ctx)
        {
            return new SimpleAmmyCodePiece(ToString());
        }

        public string           PropertyName { get; set; }
        public DataBindingMode? Mode         { get; set; }
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