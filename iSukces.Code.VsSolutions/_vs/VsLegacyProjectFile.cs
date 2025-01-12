#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace iSukces.Code.VsSolutions;

public class VsLegacyProjectFile : VsProjectFile
{
    public VsLegacyProjectFile(XDocument document) : base(document)
    {
    }

    public IEnumerable<ProjectReference> GetProjectReferences()
    {
        var tmp = ScanItemGroups(Tags.ProjectReference).ToArray();
        if (tmp.Length == 0)
            return XArray.Empty<ProjectReference>();
        return tmp.Select(ProjectReference.FromNode);
 
    }

    private IEnumerable<XElement> ScanItemGroups(string name)
    {
        var ns = Namespace;
        var nodes = Document.Root?
            .Elements(ns + Tags.ItemGroup)
            .SelectMany(a => a.Elements(ns + name));
        return nodes;
    }

    private IEnumerable<XElement> ScanPropertyGroups(string name)
    {
        var ns = Namespace;
        var nodes = Document.Root?
            .Elements(ns + "PropertyGroup")
            .SelectMany(a => a.Elements(ns + name));
        return nodes;
    }

    public string TargetFrameworkVersion
    {
        get { return ScanPropertyGroups(Tags.TargetFrameworkVersion).FirstOrDefault()?.Value; }
    }

    public string OutputType
    {
        get { return ScanPropertyGroups(Tags.OutputType).FirstOrDefault()?.Value; }
    }

    public string AssemblyOriginatorKeyFile
    {
        get
        {
            var q = ScanPropertyGroups("AssemblyOriginatorKeyFile")
                .Select(a => a.Value.Trim())
                .Where(a => !string.IsNullOrEmpty(a));
            return q.FirstOrDefault();
        }
    }
}
