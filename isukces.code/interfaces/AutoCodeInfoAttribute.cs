using System;

namespace isukces.code.interfaces
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class AutoCodeInfoAttribute : Attribute
    {
        public string AmmyFileName          { get; set; }
        public string AmmyResourcesFileName { get; set; }
        public string AmmyVariablesPrefix   { get; set; }
    }
}