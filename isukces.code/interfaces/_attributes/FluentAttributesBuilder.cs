using iSukces.Code.AutoCode;

namespace iSukces.Code.Interfaces;

public class FluentAttributesBuilder<T>
    where T : IAttributable
{
    public FluentAttributesBuilder(T element, INamespaceContainer nsContainer)
    {
        _element     = element;
        _nsContainer = nsContainer;
    }


    public T End()
    {
        return _element;
    }

    public FluentAttributesBuilder<T> WithArgument(object value)
    {
        _attribute.WithArgument("", value);
        return this;
    }

    public FluentAttributesBuilder<T> WithArgument(string name, object value)
    {
        _attribute.WithArgument(name, value);
        return this;
    }

    public FluentAttributesBuilder<T> WithArgumentCode(string valueCode)
    {
        return WithArgumentCode("", valueCode);
    }

    public FluentAttributesBuilder<T> WithArgumentCode(string name, string valueCode)
    {
        _attribute.WithArgumentCode(name, valueCode);
        return this;
    }

    public FluentAttributesBuilder<T> WithAttribute<TAttribute>()
    {
        var typeName = _nsContainer.GetTypeName(typeof(TAttribute));
        _attribute = new CsAttribute(typeName);
        _element.Attributes.Add(_attribute);
        return this;
    }

    private readonly T _element;
    private readonly INamespaceContainer _nsContainer;
    private CsAttribute _attribute;
}
