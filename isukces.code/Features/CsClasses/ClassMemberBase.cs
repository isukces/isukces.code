using System.Collections.Generic;
using isukces.code.interfaces;

namespace isukces.code
{
    public abstract class ClassMemberBase : IAttributable, ICsClassMember
    {
        public string Description { get; set; }

        public IList<ICsAttribute> Attributes
        {
            get { return _attributes; }
            set
            {
                if (value == null) value = new List<ICsAttribute>();
                _attributes = value;
            }
        }

        public Visibilities Visibility { get; set; }

        private IList<ICsAttribute> _attributes = new List<ICsAttribute>();
    }
}