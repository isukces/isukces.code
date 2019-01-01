using System.Collections.Generic;
using System.Linq;
using isukces.code.interfaces;

namespace isukces.code.Typescript
{
    public abstract class TsClassOrInterface : TsNamespaceMember
    {
        protected TsClassOrInterface(bool isInterface, string name)
        {
            IsInterface = isInterface;
            Name        = name;
        }

        public TsField AddField(string name, string type = null)
        {
            var f = new TsField(name) {Type = type};
            Members.Add(f);
            return f;
        }

        public TsMethod AddMethod(string name)
        {
            var m = new TsMethod {Name = name};
            Members.Add(m);
            return m;
        }


        public override void WriteCodeTo(TsCodeWritter writter)
        {
            writter.DoWithHeadersOnly(IsInterface, () =>
            {
                Introduction?.WriteCodeTo(writter);
                if (Decorators != null && Decorators.Any())
                    foreach (var i in Decorators)
                        i.WriteCodeTo(writter);
                writter.Open(string.Join(" ", GetClassHeader()));
                foreach (var i in Members)
                    i.WriteCodeTo(writter);
                writter.Close();
            });
        }


        protected abstract IEnumerable<string> GetClassHeader();

        public List<ITsCodeProvider> Members     { get; } = new List<ITsCodeProvider>();
        public bool                  IsInterface { get; }
    }
}