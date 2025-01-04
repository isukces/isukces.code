using System.Linq;
using System.Xml.Linq;

namespace iSukces.Code.VsSolutions;

internal class XmlTargetFrameworkCache : XmlCachedWrapper<TargetFramework>
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
        foreach (var i in root.Elements(Namespace + CsProjXmlTools.Names.PropertyGroup))
        {
            var q1 = i.Descendants(Namespace + CsProjXmlTools.Names.TargetFramework);
            var q2 = i.Descendants(Namespace + CsProjXmlTools.Names.TargetFrameworks);
            foreach (var j in q1.Concat(q2))
                return j.Value;
        }

        return default;
    }

    protected override TargetFramework SetValueInternal(TargetFramework value)
    {
        var ns     = Namespace;
        var remove = ns + CsProjXmlTools.Names.TargetFrameworks;
        var modify = ns + CsProjXmlTools.Names.TargetFramework;

        if (value.Text == "")
        {
            RemoveAll(remove);
            RemoveAll(modify);
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