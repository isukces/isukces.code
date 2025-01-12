using iSukces.Code.Interfaces;

namespace iSukces.Code.Typescript
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
