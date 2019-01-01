using isukces.code.interfaces;

namespace isukces.code.Typescript
{
    public class TsDecorator : ITsCodeProvider
    {
        public void WriteCodeTo(ITsCodeWritter writter)
        {
            writter.WriteLine("@" + Name + "()");
        }

        public string Name { get; set; }
    }
}