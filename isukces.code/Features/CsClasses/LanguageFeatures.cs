using System;

namespace isukces.code
{
    [Flags]
    public enum LanguageFeatures
    {
        None = 0,
        ExpressionBody = 1,
        Regions = 2
    }
}