using System.Collections.Generic;
using iSukces.Code.Interfaces;

namespace iSukces.Code;

public class FluentCode
{
    public FluentCode Add(string code)
    {
        _code.Add(code);
        return this;
    }

    public FluentCode AddMethod(string methodName, params string[] args)
    {
        var code = args.CommaJoin().Parentheses(methodName);
        _code.Add(code);
        return this;
    }

    public void Write(CsCodeWriter writer)
    {
        if (_code.Count < 1)
            return;
        var lastIdx = _code.Count - 1;
        for (var index = 0; index < _code.Count; index++)
        {
            var line = _code[index];
            if (index > 0)
                line = "." + line;
            if (index == lastIdx)
                line += ";";
            writer.WriteLine(line);
            if (index == 0)
                writer.IncIndent();
        }

        writer.DecIndent();
    }

    #region properties

    public int Count => _code.Count;

    #endregion

    #region Fields

    private readonly List<string> _code = new();

    #endregion
}

