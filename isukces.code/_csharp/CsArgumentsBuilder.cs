using System.Collections.Generic;
using System.Runtime.CompilerServices;
using iSukces.Code.AutoCode;

namespace iSukces.Code;

public sealed class CsArgumentsBuilder
{
    public void Add(CsExpression expression)
    {
        if (expression is null)
            AddCode("null");
        else
            AddCode(expression.Code);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CsArgumentsBuilder AddCode(string ammyCode)
    {
        Items.Add(ammyCode);
        return this;
    }

    public CsArgumentsBuilder AddValue(string text) => AddCode(text.CsEncode());
    public CsArgumentsBuilder AddValue(int value) => AddCode(value.ToCsString());

    public bool Any() => Items.Count > 0;

    public string CallMethod(string methodName, bool addSemicolon)
    {
        if (addSemicolon)
            return methodName + CodeEx + ";";
        return methodName + CodeEx;
    }

    public override string ToString() => Code;

    public string Code => Items.CommaJoin();

    public string CodeEx => Code.Parentheses();

    public List<string> Items { get; } = new List<string>();
}