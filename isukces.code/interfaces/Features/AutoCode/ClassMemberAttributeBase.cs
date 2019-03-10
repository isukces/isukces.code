using System;

namespace isukces.code.interfaces
{
    public abstract class ClassMemberAttributeBase : Attribute
    {
        public string Name { get; set; }

        public string Description { get; set; }
        public Visibilities Visibility { get; set; }
    }
}