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

        public void WriteCodeTo(TsCodeFormatter formatter)
        {
            foreach (var i in References)
                i.WriteCodeTo(formatter);
            foreach (var i in Members)
                i.WriteCodeTo(formatter);
        }

        private string GetCode()
        {
            var ctx = new TsCodeFormatter();
            WriteCodeTo(ctx);
            return ctx.Code;
        }


        public List<TsReference> References { get; set; } = new List<TsReference>();
        public List<ITsCodeProvider> Members { get; set; } = new List<ITsCodeProvider>();
    }
}