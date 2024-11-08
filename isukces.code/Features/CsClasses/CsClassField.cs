#nullable enable
using System.Text;
using iSukces.Code.Interfaces;

namespace iSukces.Code;

public class CsClassField : CsMethodParameter, ICsClassMember, ICommentable, IClassMember2
{
    public CsClassField(string name)
        : base(name)
    {
    }

    public CsClassField(string name, CsType type)
        : base(name, type)
    {
    }

    public CsClassField(string name, CsType type, string description)
        : base(name, type, description)
    {
    }

    public CsClassField WithIsConst(bool isConst = true)
    {
        IsConst = isConst;
        return this;
    }
    public CsClassField WithIsConst(string value)
    {
        IsConst = true;
        ConstValue = value;
        return this;
    }

    public override string ToString()
    {
        var type       = Type.Modern;
        var visibility = Visibility.ToString().ToLower();
        return IsConst
            ? $"{visibility} const {type} {Name} = {ConstValue}"
            : $"{visibility} {type} {Name}";
    }
    
    /// <summary>
    /// </summary>
    public Visibilities Visibility { get; set; } = Visibilities.Public;

    public void AddComment(string? x) => _extraComment.AppendLine(x);

    public string GetComments() => _extraComment.ToString();

    public string? CompilerDirective { get; set; }

    private readonly StringBuilder _extraComment = new StringBuilder();

    public bool IsConst { get; set; }


#if NET8_0_OR_GREATER
    public required CsClass Owner { get; init; }
#else
    public CsClass Owner { get; init; }
#endif
    
    public CsClassField WithAttribute<T>() => this.WithAttribute(Owner, typeof(T));
}
