using System;
using System.Xml.Linq;

namespace iSukces.Code.VsSolutions;

public sealed class XmlProjectGuidCache : XmlPropertyGroupValueCache<Guid?>
{
    public XmlProjectGuidCache(XDocument document)
        : base(document, Tags.LangVersion)
    {
        _internal = new XmlPropertyGroupValueCache(document, Tags.ProjectGuid);
    }


    protected override Guid? Map(string? internalValue)
    {
        if (internalValue == null)
            return null;
        if (Guid.TryParse(internalValue, out var guid))
            return guid;
        return null;
    }

    protected override string? Map(Guid? value)
    {
        return value?.ToString("B").ToUpper();
    }

    private readonly XmlPropertyGroupValueCache _internal;
}
