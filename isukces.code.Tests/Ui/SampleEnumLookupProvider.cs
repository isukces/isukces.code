using System.Collections.Generic;
using iSukces.Code.Ui;

namespace iSukces.Code.Tests.Ui
{
    // ========= JUST EXAMPLE CODE =========

    [LookupInfo(typeof(SampleEnumLookupProvider))]
    public enum SampleEnumWithLookupAttribute
    {
        Left,
        Right,
        OtherDirection
    }

    public class SampleEnumLookupProvider : AbstractEnumLookupProvider<SampleEnumWithLookupAttribute>
    {
        /// <summary>
        /// In order to proper working class that derives from AbstractEnumLookupProvider
        /// MUST HAVE static property EnumValues
        /// </summary>
        public static IReadOnlyList<Item> EnumValues
        {
            get
            {
                return new[]
                {
                    Make(SampleEnumWithLookupAttribute.Left),
                    Make(SampleEnumWithLookupAttribute.Right),
                    Make(SampleEnumWithLookupAttribute.OtherDirection, "Other direction")
                };
            }
        }
    }
}