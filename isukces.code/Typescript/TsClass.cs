#nullable enable
using System.Collections.Generic;

namespace iSukces.Code.Typescript
{
    public class TsClass : TsClassOrInterface
    {
        public TsClass() : this("")
        {
        }

        public TsClass(string name) : base(false, name)
        {
        }


        public TsClass WithExtends(string baseType)
        {
            Extends = baseType;
            return this;
        }

        protected override IEnumerable<string> GetClassHeader()
        {
            if (IsExported)
                yield return "export";
            yield return "class";
            yield return Name;
            if (string.IsNullOrEmpty(Extends)) yield break;
            yield return "extends";
            yield return Extends;
        }

        public string Extends      { get; set; }       
    }
}