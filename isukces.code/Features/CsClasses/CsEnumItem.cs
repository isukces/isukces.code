using System.Collections.Generic;
using iSukces.Code.Interfaces;

namespace iSukces.Code;

public class CsEnumItem : IDescriptable, IAttributable
{
    public CsEnumItem()
    {
    }

    public CsEnumItem(string enumName) => EnumName = enumName;

    public CsEnumItem(string enumName, int value)
    {
        EnumName     = enumName;
        EncodedValue = value.ToCsString();
    }


    public void MakeCode(ICsCodeWriter writer, bool addComma)
    {
        var commentLines = new List<string>();
        if (!string.IsNullOrEmpty(Description))
            commentLines.Add(Description!);

        if (!string.IsNullOrEmpty(Label))
            commentLines.Add(Label!);
        if (EnumName != SerializeAs && !string.IsNullOrEmpty(SerializeAs))
            commentLines.Add("serialized as " + SerializeAs);
        writer.WriteMultiLineSummary(commentLines, true);
        writer.WriteAttributes(Attributes);

        var code = EnumName;
        if (!string.IsNullOrEmpty(EncodedValue))
            code += " = " + EncodedValue;
        if (addComma)
            code += ",";
        writer.WriteLine(code);
    }

    public string? EncodedValue { get; set; }

    public string  EnumName    { get; set; }
    public string? Label       { get; set; }
    public string? SerializeAs { get; set; }

    public string?             Description { get; set; }
    public IList<ICsAttribute> Attributes  { get; } = new List<ICsAttribute>();
}
