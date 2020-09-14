using System;

namespace iSukces.Code
{
    public class AutocodeGeneratedAttribute : Attribute
    {
        public AutocodeGeneratedAttribute(string generatorInfo = null) => GeneratorInfo = generatorInfo;

        public string GeneratorInfo { get; }
    }
}