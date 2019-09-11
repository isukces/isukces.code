using System.Collections.Generic;
using isukces.code.Ui;

namespace isukces.code.Ammy.Ui
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