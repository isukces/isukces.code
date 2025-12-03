using System.Collections.Generic;

namespace iSukces.Code;

internal class PropertyCodeLines : CodeLines
{
    public PropertyCodeLines(IReadOnlyList<string>? lines, bool isExpressionBody = false, bool filterEmptyLines = false)
        : base(lines, isExpressionBody, filterEmptyLines)
    {
    }

    public PropertyCodeLines(string line, bool isExpressionBody = false, bool filterEmptyLines = false)
        : base([line], isExpressionBody, filterEmptyLines)
    {
    }

    public bool WriteAsAutoProperty { get; init; }

    public static PropertyCodeLines AsWriteAsAutoProperty()
    {
        return new PropertyCodeLines("")
        {
            WriteAsAutoProperty = true
        };
    }
}
