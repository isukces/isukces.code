using System.Linq;
using System.Xml.Linq;

namespace iSukces.Code.VsSolutions;

public class XmlTargetFrameworkCache : XmlCachedWrapper<TargetFramework>
{
    public XmlTargetFrameworkCache(XDocument document)
        : base(document)
    {
    }

    protected override TargetFramework GetValueInternal()
    {
        var root = Document.Root;
        if (root is null)
            return default;
        foreach (var i in root.Elements(Namespace + Tags.PropertyGroup))
        {
            var q1 = i.Descendants(Namespace + Tags.TargetFramework);
            var q2 = i.Descendants(Namespace + Tags.TargetFrameworks);
            foreach (var j in q1.Concat(q2))
                return j.Value;
        }

        return default;
    }

    protected override TargetFramework SetValueInternal(TargetFramework value)
    {
        var ns     = Namespace;
        var remove = ns + Tags.TargetFrameworks;
        var modify = ns + Tags.TargetFramework;

        if (value.Text == "")
        {
            RemoveAll(remove);
            RemoveAll(modify);
            return value;
        }

        var root       = Document.Root!;
        var isMultiple = value.Count > 1;
        if (isMultiple)
            (remove, modify) = (modify, remove);

        RemoveAll(remove);
        var els = root.Descendants(modify).ToArray();
        if (els.Length == 0)
        {
            var pg = CsProjXmlTools.SurePropertyGroup(Document);
            pg.Add(new XElement(modify, value));
            return value;
        }

        UpdateFirstRemoveOthers(els, value.Text);
        return value;
    }
}