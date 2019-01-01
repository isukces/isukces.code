namespace isukces.code.Typescript
{
    public class TsSingleLineComment : ITsCodeProvider
    {
        public TsSingleLineComment(string text = null)
        {
            Text = text;
        }

        public void WriteCodeTo(TsCodeWritter writter)
        {
            if (string.IsNullOrWhiteSpace(Text))
                return;
            var lines = Text.Trim().SplitToLines();
            foreach (var line in lines)
                writter.Writeln("// " + line);
        }

        public string Text { get; set; }
    }
}