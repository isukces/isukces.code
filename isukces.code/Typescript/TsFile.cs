using System.Collections.Generic;
using isukces.code.IO;

namespace isukces.code.Typescript
{
    public class TsFile
    {
        public bool SaveIfDifferent(string filename, bool addBom = false)
        {
            return CodeFileUtils.SaveIfDifferent(GetCode(), filename, addBom);
        }

        private string GetCode()
        {
            var cf = new CSCodeFormatter();
            var ctx = new TsWriteContext(cf);
            foreach (var i in References)
                i.WriteCodeTo(ctx);
            foreach (var i in Namespaces)
                i.WriteCodeTo(ctx);
            return cf.Text;
        }

        public List<TsReference> References { get; set; } = new List<TsReference>();
        public List<TsNamespace> Namespaces { get; set; } = new List<TsNamespace>();
    }
}