using isukces.code.interfaces;

namespace isukces.code.Typescript
{
    public class TsDecorator : ITsCodeProvider
    {
        public void WriteCodeTo(TsCodeWritter writter)
        {
            writter.WriteLine("@" + Name + "()");
        }

        public string Name { get; set; }
    }
}