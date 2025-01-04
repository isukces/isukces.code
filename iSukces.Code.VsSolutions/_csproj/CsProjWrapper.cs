using System.Xml.Linq;

namespace iSukces.Code.VsSolutions;

public class CsProjWrapper : XmlWrapper
{
    public CsProjWrapper(XDocument document, CsprojDocumentKind kind)
        : base(document)
    {
        TargetFramework = new XmlTargetFrameworkCache(Document);
        Authors         = new XmlPropertyGroupValueCache(Document, "Authors");
        Company         = new XmlPropertyGroupValueCache(Document, "Company");
        Copyright       = new XmlPropertyGroupValueCache(Document, "Copyright");
        LangVersion     = new XmlCsLangVersionCache(Document);
        Kind            = kind;
    }

    #region Properties

    public CsprojDocumentKind              Kind            { get; }
    public IValueProvider<TargetFramework> TargetFramework { get; }
    public IValueProvider<string>          Authors         { get; }
    public IValueProvider<string>          Company         { get; }
    public IValueProvider<string>          Copyright       { get; }
    public IValueProvider<CsLangVersion>   LangVersion     { get; }

    #endregion
}
