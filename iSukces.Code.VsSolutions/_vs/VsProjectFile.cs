#nullable disable
using System.IO;
using System.Xml.Linq;

namespace iSukces.Code.VsSolutions;

public abstract class VsProjectFile : XmlWrapper
{
    public VsProjectFile(XDocument document)
        : base(document)
    {
    }

    public static VsProjectFile FromFile(string file)
    {
        var doc = XDocument.Load(file);
        var sdk = (string)doc.Root?.Attribute(Tags.Sdk);
        if (sdk is Tags.MicrosoftNetSdk or Tags.MicrosoftNetSdkWeb)
            return new VsCoreProjectFile(doc, GetProjectKind(file));
        return new VsLegacyProjectFile(doc);
    }

    public static CsprojDocumentKind GetProjectKind(string file)
    {
        var ext = Path.GetExtension(file).ToLower().TrimStart('.');
        return ext switch
        {
            "csproj" => CsprojDocumentKind.Project,
            "props" => CsprojDocumentKind.Props,
            _ => CsprojDocumentKind.Unknown
        };
    }
}

