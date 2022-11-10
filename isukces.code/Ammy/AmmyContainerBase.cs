#if AMMY
using System.Collections.Generic;
using iSukces.Code.Interfaces.Ammy;

namespace iSukces.Code.Ammy
{
    public class AmmyContainerBase : IAmmyContainer
    {
        public IDictionary<string, object> Properties   { get; } = new PropertiesDictionary();
        public IList<object>               ContentItems { get; } = new List<object>();
    }
}
#endif