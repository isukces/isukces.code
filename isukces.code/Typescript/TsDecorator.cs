using isukces.code.interfaces;

namespace isukces.code.Typescript
{
    public class TsDecorator : ITsCodeProvider
    {
        public void WriteCodeTo(ITsCodeWriter writer)
        {
            writer.WriteLine("@" + Name + "()");
        }

        public string Name { get; set; }
    }
}