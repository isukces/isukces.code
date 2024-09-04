#nullable enable
using System.Collections.Generic;

namespace iSukces.Code.Typescript
{
    public class TsInterface : TsClassOrInterface
    {
        public TsInterface() : this("")
        {
        }

        public TsInterface(string name) : base(true, name)
        {
        }

        protected override IEnumerable<string> GetClassHeader()
        {
            if (IsExported)
                yield return "export";
            yield return "interface";
            yield return Name;
            /*         if (string.IsNullOrEmpty(Extends)) yield break;
                     yield return "extends";
                     yield return Extends;*/
        }
    }
}