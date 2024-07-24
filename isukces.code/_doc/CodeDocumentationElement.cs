#nullable enable
using System;
using System.Linq;
using System.Xml.Linq;

namespace iSukces.Code;

public class CodeDocumentationElement(XElement wrapped)
{
    public override string ToString()
    {
        return (string?)Wrapped.Attribute("name") ?? "?";
    }

    public XElement Wrapped { get; }
        = wrapped ?? throw new ArgumentNullException(nameof(wrapped));

    public string? Summary => Wrapped.Element("summary")?.Value;

    public string? GetParameter(string paramName)
    {
        // <param name="searchText">Fragment tekstu do wyszukania w fullname i city</param>
        var el = Wrapped.Elements("param").FirstOrDefault(a => paramName == (string?)a.Attribute("name"));
        return el?.Value;
    }
}