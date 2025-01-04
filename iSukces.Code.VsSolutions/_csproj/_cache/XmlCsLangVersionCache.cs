using System.Xml.Linq;

namespace iSukces.Code.VsSolutions;

internal sealed class XmlCsLangVersionCache: XmlCachedWrapper<CsLangVersion>
{
    private readonly XmlPropertyGroupValueCache _internal;

    public XmlCsLangVersionCache(XDocument document)
        : base(document)
    {
        _internal = new XmlPropertyGroupValueCache(document, CsProjXmlTools.Names.LangVersion);
    }

    public override void Invalidate()
    {
        base.Invalidate();
        _internal.Invalidate();
    }

    protected override CsLangVersion GetValueInternal()
    {
        return _internal.Value;
    }

    protected override CsLangVersion SetValueInternal(CsLangVersion value)
    {
        _internal.Value = value;
        return _internal.Value;
    }
}
