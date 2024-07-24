#nullable enable
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace iSukces.Code;

public class CodeDocumentation
{
    private CodeDocumentation(XDocument? document)
    {
        Elements = new Dictionary<CodeDocumentationKey, CodeDocumentationElement>();
        var members = document?.Root?.Element("members");
        if (members is null)
            return;
        foreach (var i in members.Elements("member"))
        {
            var key = CodeDocumentationKey.FromString((string?)i.Attribute("name"));
            if (key is null)
                continue;
            Elements[key] = new CodeDocumentationElement(i);
        }
    }
#if X

    public static string FixType(string type)
    {
        var add = type is "Boolean"
            or "Byte" or "SByte"
            or "Int16" or "Int32" or "Int64"
            or "UInt16" or "UInt32" or "UInt64"
            or "Single" or "Double";
        if (add)
            return $"System.{type}";

        return type;
    }
#endif

    public static CodeDocumentation Parse(string? fileName)
    {
        if (!File.Exists(fileName))
            return new CodeDocumentation(null);
        return new CodeDocumentation(XDocument.Load(fileName));
    }

    public static CodeDocumentation Parse(Assembly assembly)
    {
        var loc             = new FileInfo(assembly.Location);
        var locFullName     = loc.FullName;
        var doc             = locFullName.Substring(0, locFullName.Length - loc.Extension.Length) + ".xml";
        var fi              = new FileInfo(doc);
        if (!fi.Exists)
            doc = Path.Combine(fi.Directory?.Parent?.FullName ?? "", fi.Name);

        return Parse(doc);
    }

    public CodeDocumentationElement? Get(MethodInfo m)
    {
        var key = MethodInfoConverter.GetKey(m);
        return this[key];
    }

    public Dictionary<CodeDocumentationKey, CodeDocumentationElement> Elements { get; }

    public CodeDocumentationElement? this[CodeDocumentationKey? key]
    {
        get
        {
            if (key is null)
                return null;
#if NET48
            Elements.TryGetValue(key, out var value);
            return value;
#else
            return Elements.GetValueOrDefault(key);
#endif
        }
    }

    #region Fields

    private const string MethodNameFilter = @"^([^(]+\()([^)]+)\)";

    private static readonly Regex MethodNameRegex =
        new(MethodNameFilter, RegexOptions.IgnoreCase | RegexOptions.Compiled);

    #endregion
}