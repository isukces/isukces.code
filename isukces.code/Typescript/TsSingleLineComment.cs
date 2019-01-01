namespace isukces.code.Typescript
{
    public class TsSingleLineComment : ITsCodeProvider
    {
        public TsSingleLineComment(string text = null)
        {
            Text = text;
        }

        public void WriteCodeTo(TsCodeFormatter formatter)
        {
            if (string.IsNullOrWhiteSpace(Text))
                return;
            var lines = Text.Trim().SplitToLines();
            foreach (var line in lines)
                formatter.Writeln("// " + line);
        }

        public string Text { get; set; }
    }
}