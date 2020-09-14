using System.Collections.Generic;
using System.Linq;
using iSukces.Code.Interfaces;
using iSukces.Code.IO;

namespace iSukces.Code.Typescript
{
    public class TsFile : ITsCodeProvider
    {
        public TsNamespace GetOrCreateNamespace(string namespaceName)
        {
            foreach (var i in Members.OfType<TsNamespace>())
                if (i.Name == namespaceName)
                    return i;
            var result = new TsNamespace(namespaceName);
            Members.Add(result);
            return result;
        }

        public bool SaveIfDifferent(string filename, bool addBom = false)
        {
            return CodeFileUtils.SaveIfDifferent(GetCode(), filename, addBom);
        }

        public override string ToString()
        {
            return GetCode();
        }

        public void WriteCodeTo(ITsCodeWriter writer)
        {
            foreach (var i in References.Items)
                i.WriteCodeTo(writer);
            foreach (var i in Members)
                i.WriteCodeTo(writer);
        }

        private string GetCode()
        {
            var ctx = new TsCodeWriter();
            WriteCodeTo(ctx);
            return ctx.Code;
        }


        public TsReferenceCollection References { get; } = new TsReferenceCollection();
        public List<ITsCodeProvider> Members    { get; } = new List<ITsCodeProvider>();
    }


    public class TsReferenceCollection
    {
        public void Add(TsReference item)
        {
            if (_added.Add(item))
                _items.Add(item);
        }

        public IReadOnlyList<TsReference> Items => _items;
        private readonly HashSet<TsReference> _added = new HashSet<TsReference>();
        private readonly List<TsReference> _items = new List<TsReference>();
    }
}