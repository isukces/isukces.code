namespace isukces.code.Typescript
{
    public class TsMultiLineComment : ITsCodeProvider
    {
        public TsMultiLineComment(string text = null, bool compactRender = true)
        {
            CompactRender = compactRender;
            Text = text;
        }

        public void WriteCodeTo(TsWriteContext context)
        {
            if (string.IsNullOrWhiteSpace(Text))
                return;
            var lines = Text.Trim().SplitToLines();
            if (CompactRender)
            {
                for (int index = 0, lastIdx = lines.Count - 1; index <= lastIdx; index++)
                {
                    var line = lines[index].Replace("*/", "* /");
                    if (index == 0)
                        line = "/* " + line;
                    else
                        line = "   " + line;
                    if (index == lastIdx)
                        line += " */";
                    context.Formatter.Writeln(line);
                }
            }
            else
            {
                context.Formatter.Writeln("/*");
                foreach (var line in lines)
                    context.Formatter.Writeln(line);
                context.Formatter.Writeln("*/");

            }
        }

        public string Text { get; set; }
        public bool CompactRender { get; set; }
    }
}