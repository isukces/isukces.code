using isukces.code.interfaces;

namespace isukces.code.Typescript
{
    public class TsReference : ITsCodeProvider
    {
        public void WriteCodeTo(ITsCodeWritter writter)
        {
            writter.WriteLine($"/// <reference path=\"{Path}\"/>");
        }

        public string Path { get; set; }
    }
}