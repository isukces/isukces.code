using System.Collections.Generic;

namespace isukces.code.Typescript
{
    public class TsClass : TsClassOrInterface
    {
        public TsClass() : this("")
        {
        }

        public TsClass(string name) : base(false, name)
        {
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


        public string Extends { get; set; }
    }
}