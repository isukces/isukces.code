using System.Collections.Generic;
using System.Linq;
using isukces.code.CodeWrite;
using isukces.code.interfaces;

namespace isukces.code
{
    public class CsEnumItem : IDescriptable
    {
        public void MakeCode(ICsCodeWritter writer, bool addComma)
        {
            var commentLines = new List<string>();
            if (!string.IsNullOrEmpty(Label))
                commentLines.Add(Label);
            if (EnumName != SerializeAs)
                commentLines.Add("serialized as " + SerializeAs);
            if (commentLines.Any())
            {
                writer.WriteLine("/// <summary>");
                foreach (var commentLine in commentLines)
                    writer.WriteLine("/// " + commentLine.XmlEncode());
                writer.WriteLine("/// </summary>");
            }
            writer.WriteLine("{0}{1}", EnumName, addComma ? "," : "");
        }

        public string EnumName { get; set; }
        public string Label { get; set; }
        public string SerializeAs { get; set; }

        public string Description { get; set; }
    }
}