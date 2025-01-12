using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace iSukces.Code.VsSolutions;

public static class CsProjXmlTools
{
    public static IEnumerable<XElement> Descendants(XDocument document, string name)
    {
        var root = GetRootElement(document);
        return root.Descendants(root.Name.Namespace + name);
    }

    public static XElement GetRootElement(XDocument? document)
    {
        ArgumentNullException.ThrowIfNull(document);
        var root = document.Root ?? throw new InvalidOperationException("Root element is null");
        return root;
    }

    public static IEnumerable<XElement> ItemGroups(XDocument document)
    {
        return Descendants(document, Tags.ItemGroup);
    }


    public static IEnumerable<XElement> PropertyGroups(XDocument document)
    {
        return Descendants(document, Tags.PropertyGroup);
    }

    public static void RemoveEmptyItemGroups(XDocument document)
    {
        foreach (var itemGroup in ItemGroups(document).ToArray())
        {
            if (!itemGroup.HasElements)
                itemGroup.Remove();
        }
    }

    public static void RemoveEmptyPropertyGroups(XDocument document)
    {
        foreach (var itemGroup in PropertyGroups(document).ToArray())
        {
            if (!itemGroup.HasElements)
                itemGroup.Remove();
        }
    }

    public static XElement SureItemGroup(XDocument document)
    {
        return SureRootElement(document, Tags.ItemGroup);
    }

    public static XElement SurePropertyGroup(XDocument document)
    {
        return SureRootElement(document, Tags.PropertyGroup);
    }

    public static XElement SureRootElement(XDocument document, string name)
    {
        var root = GetRootElement(document);
        return SureRootElement(document, root.Name.Namespace + name);
    }

    public static XElement SureRootElement(XDocument document, XName name)
    {
        var root          = GetRootElement(document);
        var propertyGroup = root.Elements(name).FirstOrDefault();
        if (propertyGroup is not null) return propertyGroup;
        propertyGroup = new XElement(name);
        root.Add(propertyGroup);

        return propertyGroup;
    }
    
}

