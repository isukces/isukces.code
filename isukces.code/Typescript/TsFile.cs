using System.Collections.Generic;
using isukces.code.IO;

namespace isukces.code.Typescript
{
    public class TsFile : ITsCodeProvider
    {
        public bool SaveIfDifferent(string filename, bool addBom = false)
        {
            return CodeFileUtils.SaveIfDifferent(GetCode(), filename, addBom);
        }

        public override string ToString()
        {
            return GetCode();
        }

        public void WriteCodeTo(TsWriteContext ctx)
        {
            foreach (var i in References)
                i.WriteCodeTo(ctx);
            foreach (var i in Members)
                i.WriteCodeTo(ctx);
        }

        private string GetCode()
        {
            var cf = new CSCodeFormatter();
            var ctx = new TsWriteContext(cf);
            WriteCodeTo(ctx);
            return cf.Text;
        }


        public List<TsReference> References { get; set; } = new List<TsReference>();
        public List<ITsCodeProvider> Members { get; set; } = new List<ITsCodeProvider>();
    }
}