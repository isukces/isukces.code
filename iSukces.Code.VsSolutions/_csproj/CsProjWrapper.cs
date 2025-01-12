#nullable disable
using System;
using System.Xml.Linq;

namespace iSukces.Code.VsSolutions;

[Obsolete("Use VsProjectFile instead", true)]
public class CsProjWrapper : XmlWrapper
{
    public CsProjWrapper(XDocument document, CsprojDocumentKind kind)
        : base(document)
    {
        Kind            = kind;
        TargetFramework = new XmlTargetFrameworkCache(Document);
        Authors         = new XmlPropertyGroupValueCache(Document, Tags.Authors);
        Company         = new XmlPropertyGroupValueCache(Document, Tags.Company);
        Copyright       = new XmlPropertyGroupValueCache(Document, Tags.Copyright);
        LangVersion     = new XmlCsLangVersionCache(Document);
        ProjectGuid     = new XmlProjectGuidCache(Document);

        AssemblyVersion     = new XmlPropertyGroupValueCache(Document, Tags.AssemblyVersion);
        AssemblyFileVersion = new XmlPropertyGroupValueCache(Document, Tags.AssemblyFileVersion);
        FileVersion         = new XmlPropertyGroupValueCache(Document, Tags.FileVersion);
        Version             = new XmlPropertyGroupValueCache(Document, Tags.Version);
        RootNamespace       = new XmlPropertyGroupValueCache(Document, Tags.RootNamespace);
    }

    public CsprojDocumentKind              Kind            { get; }
    public IValueProvider<TargetFramework> TargetFramework { get; }
    public IValueProvider<string>          Authors         { get; }
    public IValueProvider<string>          Company         { get; }
    public IValueProvider<string>          Copyright       { get; }
    public IValueProvider<CsLangVersion>   LangVersion     { get; }
    public IValueProvider<Guid?>           ProjectGuid     { get; }

    public IValueProvider<string> AssemblyFileVersion { get; }
    public IValueProvider<string> AssemblyVersion { get; }
    public IValueProvider<string> FileVersion     { get; }
    public IValueProvider<string> Version         { get; }
    public IValueProvider<string> RootNamespace   { get; }
}

