#nullable enable
using System;
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

    public static CodeDocumentationKey? GetKey(MethodInfo? m)
    {
        if (m is null)
            return null;
        var name = m.ToString() ?? "";
        var name2 = m.Name + "(";
        var idx = name.IndexOf(name2, StringComparison.Ordinal);
        name = name[idx..];
        name = name.Replace(", ", ",");
        if (name.EndsWith("()"))
            name = name.Substring(0, name.Length - 2);
        {
            var match = MethodNameRegex.Match(name);
            if (match.Success)
            {
                var q = match.Groups[2].Value.Split(',');
                for (var index = 0; index < q.Length; index++)
                    if (q[index] == "Boolean")
                        q[index] = "System.Boolean";

                name = match.Groups[1].Value + string.Join(",", q) + ")";
            }
        }
        // M:PdServer.Web.Api.CompanyApiController.FindCompany(System.String)
        // Pd.Cloud.Client.CloudCompanySearchResult FindCompany(System.String)
        // PdServer.Web.Api.CompanyApiController.FindCompany(System.String)
        name = m.DeclaringType!.FullName + "." + name.Replace(", ", ",");
        var key = new CodeDocumentationKey(CodeDocumentationKind.Method, name);
        return key;
    }

    public static CodeDocumentation Parse(string? fileName)
    {
        if (!File.Exists(fileName))
            return new CodeDocumentation(null);
        return new CodeDocumentation(XDocument.Load(fileName));
    }

    public static CodeDocumentation Parse(Assembly assembly)
    {
        var loc = new FileInfo(assembly.Location);
        var locFullName = loc.FullName;
        var doc = locFullName.Substring(0, locFullName.Length - loc.Extension.Length) + ".xml";
        var fi = new FileInfo(doc);
        if (!fi.Exists)
        {
            doc = Path.Combine(fi.Directory.Parent.FullName, fi.Name);
        }

        return Parse(doc);
    }

    public CodeDocumentationElement? Get(MethodInfo m)
    {
        var key = GetKey(m);
        return this[key];
    }

    public Dictionary<CodeDocumentationKey, CodeDocumentationElement> Elements { get; }

    private static readonly Regex MethodNameRegex =
        new Regex(MethodNameFilter, RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public CodeDocumentationElement? this[CodeDocumentationKey? key]
    {
        get
        {
            if (key is null)
                return null;
            return Elements.GetValueOrDefault(key);
        }
    }

    private const string MethodNameFilter = @"^([^(]+\()([^)]+)\)";
}