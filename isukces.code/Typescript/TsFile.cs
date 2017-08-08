using System.Collections.Generic;

namespace isukces.code.Typescript
{
    public class TsFile
    {
        public List<TsReference> References { get; set; } = new List<TsReference>();
        public List<TsNamespace> Namespaces { get; set; } = new List<TsNamespace>();
    }
}