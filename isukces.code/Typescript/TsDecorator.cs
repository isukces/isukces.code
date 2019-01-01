namespace isukces.code.Typescript
{
    public class TsDecorator : ITsCodeProvider
    {
        public void WriteCodeTo(TsCodeWritter writter)
        {
            writter.Writeln("@" + Name + "()");
        }

        public string Name { get; set; }
    }
}