using System;

namespace isukces.code.Typescript
{
    public interface ITsCodeProvider
    {
        void WriteCodeTo(TsWriteContext cf);
    }

    public class TsWriteContext
    {
        public TsWriteContext(CodeFormatter formatter)
        {
            Formatter = formatter;
        }

        public CodeFormatter Formatter { get; }

        public TsWriteContext CopyWithFlag(TsWriteContextFlags f)
        {
            return new TsWriteContext(Formatter)
            {
                Flags = Flags | f
            };
        }
        public TsWriteContextFlags Flags { get; set; }
    }

    [Flags]
    public enum TsWriteContextFlags
    {
        None = 0,
        HeadersOnly = 1
    }
}