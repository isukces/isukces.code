using System.Collections.Generic;
using isukces.code.interfaces;
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

        public void WriteCodeTo(ITsCodeWriter writer)
        {
            foreach (var i in References)
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


        public List<TsReference> References { get; } = new List<TsReference>();
        public List<ITsCodeProvider> Members { get; } = new List<ITsCodeProvider>();
    }
}