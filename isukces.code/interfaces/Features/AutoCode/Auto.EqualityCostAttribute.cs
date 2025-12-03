using System;
using System.Diagnostics;

namespace iSukces.Code.Interfaces;

public static partial class Auto
{
    [Conditional("AUTOCODE_ANNOTATIONS")]
    public class EqualityCostAttribute : Attribute
    {
        public EqualityCostAttribute(int cost)
        {
            Cost = cost;
        }

        public int Cost { get; }
        public const int Bool = 1;
        public const int Int = 2;
        public const int Float = 3;
        public const int Decimal = 4;
        public const int Collection = 9999;
        public const int String = 8;
    }
}