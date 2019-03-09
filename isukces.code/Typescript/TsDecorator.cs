using isukces.code.interfaces;

namespace isukces.code.Typescript
{
    public class TsDecorator : ITsCodeProvider
    {
        public TsDecorator(string name)
        {
            Name = name;
        }

        public void WriteCodeTo(ITsCodeWriter writer)
        {
            writer.WriteLine("@" + Name + "()");
        }

        public string Name { get; set; }
    }
}