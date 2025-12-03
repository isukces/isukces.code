#if false
using System;
using System.Diagnostics;

namespace iSukces.Code.Interfaces;

[AttributeUsage(AttributeTargets.Assembly)]
[Conditional("AUTOCODE_ANNOTATIONS")]
public class AutoCodeInfoAttribute : Attribute
{
    public string AmmyFileName          { get; set; }
    public string AmmyResourcesFileName { get; set; }
    public string AmmyVariablesPrefix   { get; set; }
}
#endif