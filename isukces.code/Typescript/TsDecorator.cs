namespace isukces.code.Typescript
{
    public class TsDecorator : ITsCodeProvider
    {
        public void WriteCodeTo(TsWriteContext context)
        {
            context.Formatter.Writeln("@" + Name + "()");
        }

        public string Name { get; set; }
    }
}