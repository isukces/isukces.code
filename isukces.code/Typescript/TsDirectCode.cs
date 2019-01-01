namespace isukces.code.Typescript
{
    public class TsDirectCode : ITsCodeProvider
    {
        public TsDirectCode(string code)
        {
            _code = code;
        }

        public void WriteCodeTo(TsCodeFormatter formatter)
        {
            formatter.Writeln(_code);
        }

        private readonly string _code;
    }
}