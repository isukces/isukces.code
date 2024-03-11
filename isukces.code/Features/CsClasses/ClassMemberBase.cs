using System.Collections.Generic;
using iSukces.Code.Interfaces;

namespace iSukces.Code;

public abstract class ClassMemberBase : IAttributable, ICsClassMember
{
    public IList<ICsAttribute> Attributes
    {
        get => _attributes;
        set => _attributes = value ?? new List<ICsAttribute>();
    }

    public string Description { get; set; }

    public Visibilities Visibility { get; set; }

    public bool IsStatic { get; set; }

    public string CompilerDirective { get; set; }

    private IList<ICsAttribute> _attributes = new List<ICsAttribute>();
}
