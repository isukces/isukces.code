using System.Xml.Linq;

namespace iSukces.Code.VsSolutions;

public sealed class XmlPropertyGroupBoolValueCache(XDocument document, string name)
    : XmlPropertyGroupValueCache<bool?>(document, name)
{
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

}
