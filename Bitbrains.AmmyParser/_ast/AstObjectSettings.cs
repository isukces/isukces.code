#nullable disable
using System.Collections.Generic;

namespace Bitbrains.AmmyParser
{
    public partial class AstObjectSettings
    {
        protected override object MakeCollection(IReadOnlyList<IAstObjectSetting> items)
        {
            return new AstObjectSettingsCollection(items);
        } 
    }
    public partial class AstObjectSettingsCollection
    {
        public IReadOnlyList<IAstObjectSetting> Items { get; }

        public AstObjectSettingsCollection(IReadOnlyList<IAstObjectSetting> items)
        {
            Items = items;
        }
    }
}
