#nullable enable
using System;
using System.Xml.Linq;

namespace iSukces.Code.VsSolutions;

public sealed class XmlPropertyGroupBoolValueCache : XmlPropertyGroupValueCache<bool?>
{
    public XmlPropertyGroupBoolValueCache(XDocument document, string name)
        : base(document, name)
    {
        _internal = new XmlPropertyGroupValueCache(document, Tags.ProjectGuid);
    }


    protected override bool? Map(string? internalValue)
    {
        if (string.IsNullOrWhiteSpace(internalValue))
            return null;
        return internalValue.Trim().ToLower() == "true";
    }

    protected override string? Map(bool? value)
    {
        if (value == null)
            return null;
        return value.Value ? "true" : "false";
    }

    private readonly XmlPropertyGroupValueCache _internal;
}
