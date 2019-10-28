using System.Xml.Linq;

namespace isukces.code.vssolutions
{
    public abstract class VsProjectFile : XmlWrapper
    {
        public VsProjectFile(XDocument document) : base(document)
        {
        }

        public static VsProjectFile FromFile(string file)
        {
            var doc = XDocument.Load(file);
            var sdk = (string)doc.Root?.Attribute(Tags.Sdk);
            if (sdk == Tags.MicrosoftNetSdk || sdk == Tags.MicrosoftNetSdkWeb)
                return new VsCoreProjectFile(doc);
            return new VsOldProjectFile(doc);
        }
    }
}