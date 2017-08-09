using System.Collections.Generic;

namespace isukces.code.Typescript
{
    public static class TsExtensions
    {
        public static IList<string> SplitToLines(this string text)
        {
            var lines = text.Replace("\r\n", "\n").Split('\n');
            return lines;
        }
    }
}