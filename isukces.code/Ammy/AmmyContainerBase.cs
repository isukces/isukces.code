using System.Collections.Generic;
using isukces.code.interfaces.Ammy;

namespace isukces.code.Ammy
{
    public class AmmyContainerBase : IAmmyContainer
    {
        public IDictionary<string, object> Properties   { get; } = new PropertiesDictionary();
        public IList<object>               ContentItems { get; } = new List<object>();
    }
}