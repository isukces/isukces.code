using isukces.code.interfaces;

namespace isukces.code.Typescript
{
    public class TsDirectCode : ITsCodeProvider
    {
        public TsDirectCode(string code)
        {
            _code = code;
        }

        public void WriteCodeTo(TsCodeWritter writter)
        {
            writter.WriteLine(_code);
        }

        private readonly string _code;
    }
}