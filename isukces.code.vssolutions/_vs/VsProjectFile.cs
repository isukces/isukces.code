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
            var sdk = (string)doc.Root?.Attribute("Sdk");
            if (sdk == "Microsoft.NET.Sdk" || sdk == "Microsoft.NET.Sdk.Web")
                return new VsCoreProjectFile(doc);
            return new VsOldProjectFile(doc);
        }
    }
}