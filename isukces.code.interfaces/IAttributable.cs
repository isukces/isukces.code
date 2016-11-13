using System.Collections.Generic;

namespace isukces.code.interfaces
{
    public interface IAttributable
    {
        IList<ICsAttribute> Attributes { get; }
    }
}
