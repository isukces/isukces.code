using System.Collections.Generic;
using iSukces.Code.Interfaces;

namespace iSukces.Code.Typescript
{
    public abstract class TsClassOrInterface : TsNamespaceMember
    {
        protected TsClassOrInterface(bool isInterface, string name)
        {
            IsInterface = isInterface;
            Name        = name;
        }

        public TsField AddField(string name, string? type = null)
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
        public TsMethod AddConstructor()
        {
            var m = new TsMethod {Name = "constructor"};
            Members.Add(m);
            return m;
        }


        public override void WriteCodeTo(ITsCodeWriter writer)
        {
            writer.DoWithHeadersOnly(IsInterface, () =>
            {
                WriteCommonHeaderCode(writer);
                writer.Open(string.Join(" ", GetClassHeader()));
                foreach (var i in Members)
                    i.WriteCodeTo(writer);
                writer.Close();
            });
        }


        protected abstract IEnumerable<string> GetClassHeader();

        public List<ITsCodeProvider> Members     { get; } = new List<ITsCodeProvider>();
        public bool                  IsInterface { get; }
    }
}
