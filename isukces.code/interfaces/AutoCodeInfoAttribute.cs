#nullable enable
using System;

namespace iSukces.Code.Interfaces
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class AutoCodeInfoAttribute : Attribute
    {
        public string AmmyFileName          { get; set; }
        public string AmmyResourcesFileName { get; set; }
        public string AmmyVariablesPrefix   { get; set; }
    }
}