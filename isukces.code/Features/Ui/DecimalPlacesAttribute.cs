using System;

namespace iSukces.Code.Ui
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DecimalPlacesAttribute : Attribute
    {
        public DecimalPlacesAttribute(int decimalPlaces) => DecimalPlaces = decimalPlaces;

        public int DecimalPlaces { get; }

        public string Format
        {
            get
            {
                const string format = "#,0";
                if (DecimalPlaces > 0)
                    return format + "." + new string('0', DecimalPlaces);
                return format;
            }
        }
    }
}