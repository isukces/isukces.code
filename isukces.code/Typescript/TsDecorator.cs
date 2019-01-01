namespace isukces.code.Typescript
{
    public class TsDecorator : ITsCodeProvider
    {
        public void WriteCodeTo(TsCodeFormatter formatter)
        {
            formatter.Writeln("@" + Name + "()");
        }

        public string Name { get; set; }
    }
}