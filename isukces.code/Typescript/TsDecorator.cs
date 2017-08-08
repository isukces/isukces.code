namespace isukces.code.Typescript
{
    public class TsDecorator : ITsCodeProvider
    {
        public void WriteCodeTo(TsWriteContext cf)
        {
            cf.Formatter.Writeln("@" + Name + "()");
        }

        public string Name { get; set; }
    }
}