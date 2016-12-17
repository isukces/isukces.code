#region using

using System.Collections.Generic;
using isukces.code.interfaces;

#endregion

namespace isukces.code
{
    public abstract class ClassMemberBase : IAttributable, ICsClassMember
    {
        #region Properties

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

        #endregion

        #region Fields

        private IList<ICsAttribute> _attributes = new List<ICsAttribute>();

        #endregion
    }
}