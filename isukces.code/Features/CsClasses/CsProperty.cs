using System.Text;
using iSukces.Code.Interfaces;

namespace iSukces.Code;

public class CsProperty : CsMethodParameter, ICsClassMember, ICommentable, IClassMember2
{
    /// <summary>
    ///     Tworzy instancję obiektu
    ///     <param name="name">nazwa parametru</param>
    /// </summary>
    public CsProperty(string name)
        : base(name)
    {
    }

    /// <summary>
    ///     Tworzy instancję obiektu
    ///     <param name="name">nazwa parametru</param>
    ///     <param name="type">typ parametru</param>
    /// </summary>
    public CsProperty(string name, CsType type)
        : base(name, type)
    {
    }

    /// <summary>
    ///     Tworzy instancję obiektu
    ///     <param name="name">nazwa parametru</param>
    ///     <param name="type">typ parametru</param>
    ///     <param name="description">Opis</param>
    /// </summary>
    public CsProperty(string name, CsType type, string description)
        : base(name, type, description)
    {
    }

    public void AddComment(string x)
    {
        _extraComment.AppendLine(x);
    }

    public CsProperty AsCalculatedFromExpression(string expression)
    {
        EmitField             = false;
        OwnGetter             = expression;
        IsPropertyReadOnly    = true;
        OwnGetterIsExpression = true;
        return this;
    }

    public string GetComments() => _extraComment.ToString();

    public CodeLines GetGetterLines() => string.IsNullOrEmpty(OwnGetter)
        ? CodeLines.FromExpression(PropertyFieldName)
        : new CodeLines(OwnGetter.Split('\r', '\n'), OwnGetterIsExpression);

    public CodeLines GetSetterLines()
    {
        var tmp = string.IsNullOrEmpty(OwnSetter)
            ? new CodeLines(new[] { $"{PropertyFieldName} = value" }, true)
            : new CodeLines(OwnSetter.Split('\r', '\n'), OwnSetterIsExpression);
        return tmp;
    }

    /// <summary>
    ///     Zwraca tekstową reprezentację obiektu
    /// </summary>
    /// <returns>Tekstowa reprezentacja obiektu</returns>
    public override string ToString() => string.Format("property {0} {1}", Name, Type.Modern);

    public CsProperty WithIsPropertyReadOnly(bool isPropertyReadOnly = true)
    {
        IsPropertyReadOnly = isPropertyReadOnly;
        return this;
    }

    public CsProperty WithMakeAutoImplementIfPossible(bool value = true)
    {
        MakeAutoImplementIfPossible = value;
        return this;
    }

    public CsProperty WithNoEmitField()
    {
        EmitField = false;
        return this;
    }

    public CsProperty WithOwnGetter(string ownGetter)
    {
        OwnGetter = ownGetter;
        return this;
    }

    public CsProperty WithOwnGetterAsExpression(string ownGetter)
    {
        OwnGetter             = ownGetter;
        OwnGetterIsExpression = true;
        return this;
    }

    public CsProperty WithOwnSetter(string ownSetter)
    {
        OwnSetter = ownSetter;
        return this;
    }

    public CsProperty WithOwnSetterAsExpression(string ownSetter)
    {
        OwnSetter             = ownSetter;
        OwnSetterIsExpression = true;
        return this;
    }

    /// <summary>
    /// </summary>
    public bool IsPropertyReadOnly { get; set; }

    /// <summary>
    /// </summary>
    public string OwnGetter
    {
        get => _ownGetter;
        set => _ownGetter = value?.Trim() ?? string.Empty;
    }

    /// <summary>
    /// </summary>
    public string OwnSetter
    {
        get => _ownSetter;
        set => _ownSetter = value?.Trim() ?? string.Empty;
    }

    public bool OwnGetterIsExpression { get; set; }
    public bool OwnSetterIsExpression { get; set; }

    /// <summary>
    ///     nazwa zmiennej dla własności; własność jest tylko do odczytu.
    /// </summary>
    public string PropertyFieldName => Name.PropertyBackingFieldName();

    /// <summary>
    /// </summary>
    public bool EmitField { get; set; } = true;

    public bool IsVirtual { get; set; }

    public bool IsOverride { get; set; }

    /// <summary>
    /// </summary>
    public bool MakeAutoImplementIfPossible { get; set; }

    public Visibilities? SetterVisibility { get; set; }
    public Visibilities? GetterVisibility { get; set; }
    public Visibilities  FieldVisibility  { get; set; } = Visibilities.Private;

#if NET8_0_OR_GREATER
    public required CsClass Owner { get; init; }
#else
    public CsClass Owner { get; init; }
#endif

    public string CompilerDirective { get; set; }

    private readonly StringBuilder _extraComment = new StringBuilder();
    private string _ownGetter = string.Empty;
    private string _ownSetter = string.Empty;

    public Visibilities Visibility { get; set; } = Visibilities.Public;
    
    
    public CsProperty WithAttribute<T>() => this.WithAttribute(Owner, typeof(T));
}
