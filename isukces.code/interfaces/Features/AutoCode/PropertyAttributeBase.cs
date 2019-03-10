using System;

namespace isukces.code.interfaces
{
    public abstract class PropertyAttributeBase : ClassMemberAttributeBase
    {
        public Type PropertyType { get; set; }

        public Visibilities? SetterVisibility { get; set; }
        public Visibilities? GetterVisibility { get; set; }
    }
}