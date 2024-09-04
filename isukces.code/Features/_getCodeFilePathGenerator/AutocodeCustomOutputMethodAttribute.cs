#nullable enable
using System;
using System.Diagnostics;

namespace iSukces.Code;

[Conditional("DEBUG")]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public sealed class AutocodeCustomOutputMethodAttribute : Attribute
{
    public AutocodeCustomOutputMethodAttribute(string methodName) { MethodName = methodName; }

    public string MethodName { get; }
}
