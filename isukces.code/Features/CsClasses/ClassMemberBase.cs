using System.Collections.Generic;
using System.Text;
using isukces.code.interfaces;

namespace isukces.code
{
    public abstract class ClassMemberBase : IAttributable, ICsClassMember
    {

        public string Description { get; set; }

        public IList<ICsAttribute> Attributes
        {
            get { return _attributes; }
            set { _attributes = value ?? new List<ICsAttribute>(); }
        }

        public Visibilities Visibility { get; set; }

        public bool IsStatic { get; set; }

        public string CompilerDirective { get; set; }

        private IList<ICsAttribute> _attributes = new List<ICsAttribute>();
    }
}