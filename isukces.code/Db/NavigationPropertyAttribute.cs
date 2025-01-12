using System;

namespace iSukces.Code.Db
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class NavigationPropertyAttribute : Attribute
    {
        public NavigationPropertyAttribute(bool forceNavigation = true)
        {
            ForceNavigation = forceNavigation;
        }

        public bool ForceNavigation { get; }
    }
}
