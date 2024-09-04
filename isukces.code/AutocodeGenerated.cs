#nullable enable
using System;

namespace iSukces.Code
{
    public class AutocodeGeneratedAttribute : Attribute
    {
        public AutocodeGeneratedAttribute(string? generatorInfo = null) => GeneratorInfo = generatorInfo;

        #region Properties

        public string GeneratorInfo { get; }

        #endregion
    }
}
