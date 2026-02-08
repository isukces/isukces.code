using System;
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

    public void AddComment(string? x)
    {
        _extraComment.AppendLine(x);
    }

    public CsProperty AsCalculatedFromExpression(string expression)
    {
        EmitField  = false;
        SetterType = PropertySetter.None;
        return WithOwnGetterAsValue(expression);
    }

    public string GetComments()
    {
        return _extraComment.ToString();
    }

    /// <summary>
    ///     Zwraca tekstową reprezentację obiektu
    /// </summary>
    /// <returns>Tekstowa reprezentacja obiektu</returns>
    public override string ToString()
    {
        return string.Format("property {0} {1}", Name, Type.Modern);
    }

    public CsProperty WithIsPropertyReadOnly()
    {
        SetterType = PropertySetter.None;
        return this;
    }

    public CsProperty WithMakeAutoImplementIfPossible(bool value = true)
    {
        MakeAutoImplementIfPossible = value;
        return this;
    }

    public CsProperty WithBackingField(PropertyBackingFieldRequest value = PropertyBackingFieldRequest.UseIfPossible)
    {
        BackingField = value;
        return this;
    }

    public CsProperty WithNoEmitField()
    {
        EmitField = false;
        return this;
    }

    public CsProperty WithOwnGetter(string? ownGetter)
    {
        OwnGetter = (PropertyGetterCode?)ownGetter;
        return this;
    }

    
    [Obsolete("use WithOwnGetterAsExpressionBody instead", true)]
    public CsProperty WithOwnGetterAsExpression(string ownGetter)
    {
        return this;
    }

    public CsProperty WithOwnGetterAsExpressionBody(string ownGetter)
    {
        OwnGetter = PropertyGetterCode.ExpressionBody(ownGetter);
        return this;
    }
    public CsProperty WithOwnGetterAsValue(string ownGetter)
    {
        OwnGetter = PropertyGetterCode.Value(ownGetter);
        return this;
    }

    public CsProperty WithOwnSetter(string ownSetter)
    {
        OwnSetter = new PropertySetterCode(ownSetter, PropertyMetodKind.Body);
        return this;
    }

    [Obsolete("use WithOwnSetterAsExpressionBody instead", true)]
    public CsProperty WithOwnSetterAsExpression(string ownSetter)
    {
        throw new NotSupportedException();
    }

    public CsProperty WithOwnSetterAsExpressionBody(string ownSetter)
    {
        OwnSetter = new PropertySetterCode(ownSetter, PropertyMetodKind.ExpressionBody);
        return this;
    }

    [Obsolete("use WithOwnSetterFromValue instead", true)]
    public CsProperty WithOwnSetterAsExpressionFromValue(string expressionToSet)
    {
        /*var setVariable = EffectiveBackingField
            ? "field"
            : PropertyFieldName;

        OwnSetter             = $"{setVariable} = {expressionToSet};";
        OwnSetterIsExpression = true;
        return this;*/

        OwnSetter = new PropertySetterCode(expressionToSet, PropertyMetodKind.Value);
        return this;
    }

    public CsProperty WithOwnSetterFromValue(string expressionToSet)
    {
        OwnSetter = new PropertySetterCode(expressionToSet, PropertyMetodKind.Value);
        return this;
    }

    public CsProperty WithOwnGetterFromValue(string expressionToSet)
    {
        OwnGetter = new PropertyGetterCode(expressionToSet, PropertyMetodKind.Value);
        return this;
    }

    /// <summary>
    /// </summary>
    [Obsolete("use SetterType instead", true)]
    public bool IsPropertyReadOnly
    {
        get => SetterType == PropertySetter.None;
        set => SetterType = value
            ? PropertySetter.None
            : PropertySetter.Set;
    }

    public PropertySetter SetterType { get; set; } = PropertySetter.Set;


    public PropertyGetterCode? OwnGetter { get; set; }

    public PropertySetterCode? OwnSetter { get; set; }

    [Obsolete("use OwnGetter.Kind instead", true)]
    public bool OwnGetterIsExpression { get; set; }


    [Obsolete("use OwnSetter.Kind instead", true)]
    public bool OwnSetterIsExpression { get; set; }


    /// <summary>
    ///     Nazwa zmiennej dla własności; własność jest tylko do odczytu.
    /// </summary>
    public string PropertyFieldName => Name.PropertyBackingFieldName();

    /// <summary>
    /// </summary>
    public bool EmitField { get; set; } = true;

    public CsType FieldTypeOverride { get; set; }

    public bool IsVirtual { get; set; }

    public bool IsOverride { get; set; }

    /// <summary>
    /// </summary>
    public bool MakeAutoImplementIfPossible { get; set; }

    public Visibilities? SetterVisibility { get; set; }
    public Visibilities? GetterVisibility { get; set; }

    public Visibilities FieldVisibility { get; set; }
        = Visibilities.Private;

    public PropertyBackingFieldRequest BackingField { get; set; }
        = PropertyBackingFieldRequest.DoNotUse;

    public string PropertyFieldNameOrFieldKeyword
        => EffectiveBackingField ? "field" : PropertyFieldName;

    public bool EffectiveBackingField
    {
        get
        {
            var ownerFlag = (Owner.Formatting.Flags & CodeFormattingFeatures.PropertyBackField) != 0;
            if (!ownerFlag)
                return false;
            switch (BackingField)
            {
                case PropertyBackingFieldRequest.ForceUse:
                    return true;
                case PropertyBackingFieldRequest.DoNotUse:
                    return false;
                default:
                    var fType = FieldTypeOverride;
                    if (!fType.IsVoid)
                    {
                        if (fType.Declaration != Type.Declaration)
                            return false;
                    }

                    return true;
            }
        }
    }

#if NET8_0_OR_GREATER
    public required CsClass Owner { get; init; }
#else
    public CsClass Owner { get; init; }
#endif

    public string? CompilerDirective { get; set; }

    private readonly StringBuilder _extraComment = new StringBuilder();

    public Visibilities Visibility { get; set; } = Visibilities.Public;

    public bool IsRequired { get; set; }

    public CsProperty WithAttribute<T>()
    {
        return this.WithAttribute(Owner, typeof(T));
    }
}

public enum PropertySetter
{
    None,
    Set,
    Init
}

public enum PropertyBackingField
{
    NotSupported,
}

public enum PropertyBackingFieldRequest
{
    DoNotUse,
    UseIfPossible,
    ForceUse
}
