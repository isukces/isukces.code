namespace isukces.code.Typescript
{
    public class TsDirectCode : ITsCodeProvider
    {
        public TsDirectCode(string code)
        {
            _code = code;
        }

        public void WriteCodeTo(TsWriteContext context)
        {
            context.Formatter.Writeln(_code);
        }

        private readonly string _code;
    }
}