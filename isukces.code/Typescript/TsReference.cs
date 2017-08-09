namespace isukces.code.Typescript
{
    public class TsReference : ITsCodeProvider
    {
        public void WriteCodeTo(TsWriteContext context)
        {
            context.Formatter.Writeln($"/// <reference path=\"{Path}\"/>");
        }

        public string Path { get; set; }
    }
}