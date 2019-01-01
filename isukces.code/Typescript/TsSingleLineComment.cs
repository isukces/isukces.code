using isukces.code.interfaces;

namespace isukces.code.Typescript
{
    public class TsSingleLineComment : ITsCodeProvider
    {
        public TsSingleLineComment(string text = null)
        {
            Text = text;
        }

        public void WriteCodeTo(ITsCodeWriter writer)
        {
            if (string.IsNullOrWhiteSpace(Text))
                return;
            var lines = Text.Trim().SplitToLines();
            foreach (var line in lines)
                writer.WriteLine("// " + line);
        }

        public string Text { get; set; }
    }
}