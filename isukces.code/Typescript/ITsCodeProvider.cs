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
    }
}