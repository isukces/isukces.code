#nullable enable
using System.Xml.Linq;

namespace iSukces.Code.VsSolutions;

public abstract class XmlPropertyGroupValueCache<T> : XmlCachedWrapper<T>
{
    public XmlPropertyGroupValueCache(XDocument document, string name)
        : base(document)
    {
        _internal = new XmlPropertyGroupValueCache(document, name);
    }

    protected override T GetValueInternal()
    {
        return Map(_internal.Value);
    }

    public override void Invalidate()
    {
        base.Invalidate();
        _internal.Invalidate();
    }

    protected abstract T Map(string? internalValue);

    protected abstract string? Map(T? value);

    protected override T SetValueInternal(T? value)
    {
        _internal.Value = Map(value);
        return Map(_internal.Value);
    }

    private readonly XmlPropertyGroupValueCache _internal;
}
