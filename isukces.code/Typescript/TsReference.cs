namespace isukces.code.Typescript
{
    public class TsReference : ITsCodeProvider
    {
        public void WriteCodeTo(TsCodeWritter writter)
        {
            writter.Writeln($"/// <reference path=\"{Path}\"/>");
        }

        public string Path { get; set; }
    }
}