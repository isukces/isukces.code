using System;
using System.Diagnostics;

namespace iSukces.Code;

[Conditional("AUTOCODE_ANNOTATIONS")]
public class AutocodeGeneratedAttribute : Attribute
{
    public AutocodeGeneratedAttribute(string? generatorInfo = null) => GeneratorInfo = generatorInfo;

    public string? GeneratorInfo { get; }
}
