using System;
using System.Diagnostics;

namespace iSukces.Code.Db;

[Conditional("AUTOCODE_ANNOTATIONS")]
[AttributeUsage(AttributeTargets.Property)]
public sealed class NavigationPropertyAttribute : Attribute
{
    public NavigationPropertyAttribute(bool forceNavigation = true)
    {
        ForceNavigation = forceNavigation;
    }

    public bool ForceNavigation { get; }
}