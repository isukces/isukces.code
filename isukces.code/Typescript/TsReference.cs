namespace isukces.code.Typescript
{
    public class TsReference : ITsCodeProvider
    {
        public void WriteCodeTo(TsCodeFormatter formatter)
        {
            formatter.Writeln($"/// <reference path=\"{Path}\"/>");
        }

        public string Path { get; set; }
    }
}