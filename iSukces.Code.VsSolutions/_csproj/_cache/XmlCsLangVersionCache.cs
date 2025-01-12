using System.Xml.Linq;

namespace iSukces.Code.VsSolutions;

public sealed class XmlCsLangVersionCache(XDocument document)
    : XmlPropertyGroupValueCache<CsLangVersion>(document, Tags.LangVersion)
{
    protected override CsLangVersion Map(string? internalValue) => internalValue;

    protected override string Map(CsLangVersion value) => value;
}
