#nullable enable
using System.Xml.Linq;

namespace iSukces.Code.VsSolutions;

public sealed class XmlPropertyGroupEnableDisableValueCache(XDocument document, string name)
    : XmlPropertyGroupValueCache<bool?>(document, name)
{
    protected override bool? Map(string? internalValue)
    {
        if (string.IsNullOrWhiteSpace(internalValue))
            return null;
        return internalValue.Trim().ToLower() is "true" or "enable";
    }

    protected override string? Map(bool? value)
    {
        if (value == null)
            return null;
        return value.Value ? "enable" : "disable";
    }

}
