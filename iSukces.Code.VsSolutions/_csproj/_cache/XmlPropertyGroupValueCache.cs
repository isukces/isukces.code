#nullable enable
using System.Linq;
using System.Xml.Linq;

namespace iSukces.Code.VsSolutions;

public class XmlPropertyGroupValueCache : XmlCachedWrapper<string>
{
    public XmlPropertyGroupValueCache(XDocument document, string name)
        : base(document)
    {
        Name = name;
    }

    protected override string? GetValueInternal()
    {
        var root = Document.Root;
        if (root is null)
            return null;
        foreach (var i in root.Elements(Namespace + Tags.PropertyGroup))
        {
            var q1 = i.Descendants(Namespace + Name);
            foreach (var j in q1)
                return j.Value;
        }

        return null;
    }

    protected override string? SetValueInternal(string? value)
    {
        var ns     = Namespace;
        var modify = ns + Name;
        value = value?.Trim();

        if (string.IsNullOrEmpty(value))
        {
            RemoveAll(modify);
            return null;
        }

        var root = Document.Root!;

        var els = root.Descendants(modify).ToArray();
        if (els.Length == 0)
        {
            var pg = CsProjXmlTools.SurePropertyGroup(Document);
            pg.Add(new XElement(modify, value));
            return value;
        }

        UpdateFirstRemoveOthers(els, value);
        return value;
    }

    public string Name { get; }
}
