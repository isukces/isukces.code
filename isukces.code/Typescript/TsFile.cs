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

        public void WriteCodeTo(TsCodeWritter writter)
        {
            foreach (var i in References)
                i.WriteCodeTo(writter);
            foreach (var i in Members)
                i.WriteCodeTo(writter);
        }

        private string GetCode()
        {
            var ctx = new TsCodeWritter();
            WriteCodeTo(ctx);
            return ctx.Code;
        }


        public List<TsReference> References { get; set; } = new List<TsReference>();
        public List<ITsCodeProvider> Members { get; set; } = new List<ITsCodeProvider>();
    }
}