using System;
using System.Linq;
using System.Xml.Linq;

namespace iSukces.Code.VsSolutions;

public class XmlWrapper
{
    public XmlWrapper(XDocument document)
    {
        Document  = document;
        Namespace = document.Root?.Name.Namespace ?? "";
    }

    public static void UpdateFirstRemoveOthers(XElement[] elements, string value)
    {
        elements[0].Value = value;
        for (var i = 1; i < elements.Length; i++)
            elements[i].Remove();
    }

    public XElement? FindElementByPath(string path)
    {
        var el = Document.Root;
        if (el == null)
            return null;
        var pathElements = path.Split('/')
            .Select(PathElement.FromString)
            .Where(a => a != null).ToArray();
        foreach (var pathElement in pathElements)
        {
            el = el.Element(pathElement!.ElementName);
            if (el == null)
                return null;
        }

        return el;
    }

    protected XElement FindOrCreateElement(string path)
    {
        var el = Document.Root;
        if (el == null)
            throw new NullReferenceException("el");
        var pathElements = path.Split('/').Select(PathElement.FromString).Where(a => a != null).ToArray();
        foreach (var pathElement in pathElements)
        {
            var next = el.Element(pathElement.ElementName);
            if (next == null)
            {
                next = new XElement(pathElement.ElementName);
                el.Add(next);
            }

            el = next;
        }

        return el;
    }

    protected string FromElement(string path)
    {
        var el = FindElementByPath(path);
        return el?.Value?.Trim();
    }

    public XName MakeName(string name)
    {
        return Namespace + name;
    }


    public void RemoveAll(XName remove)
    {
        var root = Document.Root;
        if (root is null)
            return;
        var els = root.Descendants(remove).ToArray();
        foreach (var el in els)
            el.Remove();
    }

    protected void SetValue(string path, string value)
    {
        var el = FindOrCreateElement(path);
        el.Value = value;
    }

    public XDocument Document { get; }

    public XNamespace Namespace { get; }
}

