#if FULLFX
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using isukces.code.interfaces;
using isukces.code.interfaces.Ammy;

namespace isukces.code.Ammy
{
    public class AmmyBind:IAmmyExpression
    {
        public override string ToString()
        {
            var txt = "bind";
            if (!string.IsNullOrEmpty(PropertyName) && PropertyName!=".")
                txt += " " + PropertyName.CsEncode();
            {
                var se = new List<string>();
                if (Mode!=null)
                    se.Add($"Mode: {Mode}");
                if (se.Any())
                    txt = txt + " set [" + string.Join(", ", se) + "]";
            }
            return txt;
        }

        public string GetAmmyCode(IConversionCtx ctx)
        {
            return ToString();
        }

        public string PropertyName { get; set; }
        public BindingDirection? Mode { get; set; }

        public AmmyBind(string propertyName, BindingDirection? mode=null)
        {
            PropertyName = propertyName;
            Mode = mode;
        }
    }
}
#endif