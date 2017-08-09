using System;

namespace isukces.code.Typescript
{
    public interface ITsCodeProvider
    {
        void WriteCodeTo(TsWriteContext context);
    }

    public class TsWriteContext
    {
        public TsWriteContext(CodeFormatter formatter)
        {
            Formatter = formatter;
        }

        public TsWriteContext CopyWithFlag(TsWriteContextFlags f)
        {
            return new TsWriteContext(Formatter)
            {
                Flags = Flags | f
            };
        }

        public CodeFormatter Formatter { get; }
        public TsWriteContextFlags Flags { get; set; }
    }

    [Flags]
    public enum TsWriteContextFlags
    {
        None = 0,
        HeadersOnly = 1
    }
}