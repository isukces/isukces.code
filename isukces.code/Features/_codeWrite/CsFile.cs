using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using iSukces.Code.AutoCode;
using iSukces.Code.Interfaces;
using iSukces.Code.IO;

namespace iSukces.Code;

public class CsFile : IClassOwner, INamespaceCollection, INamespaceOwner
{
    public void AddImportNamespace(string ns)
    {
        _importNamespaces.Add(ns);
    }

    public string GetCode(bool isEmbedded = false)
    {
        var writer = new CsCodeWriter();
        MakeCode(writer, isEmbedded);
        return writer.Code;
    }


    public CsClass GetOrCreateClass(string namespaceName, string className)
    {
        var ns = GetOrCreateNamespace(namespaceName);
        return ns.GetOrCreateClass(className);
    }

    public CsClass GetOrCreateClass(TypeProvider typeP) => GetOrCreateClass(typeP, out _);

    public CsClass GetOrCreateClass(TypeProvider typeP, out bool isCreatedNew)
    {
        if (typeP.IsEmpty)
            throw new ArgumentException("Value can't be empty", nameof(typeP));
        if (_classesCache.TryGetValue(typeP, out var c))
        {
            if (c.DotNetType == null && typeP.Type != null)
                c.DotNetType = typeP.Type;
            isCreatedNew = false;
            return c;
        }

        if (typeP.Type != null)
        {
            var type = typeP.Type;
            var name = type.Name;
            var ti   = type.GetTypeInfo();
            if (ti.IsGenericType)
            {
                if (!ti.IsGenericTypeDefinition)
                    throw new NotSupportedException();
                name = name.Split('`')[0];
                var                 genericArguments = ti.GetGenericArguments();
                IEnumerable<string> nn               = genericArguments.Select(a => a.Name);
                if (type.DeclaringType != null)
                {
                    var genericArguments2 = type.DeclaringType
                        .GetTypeInfo()
                        .GetGenericArguments();
                    var xn = genericArguments2.Select(a => a.Name).ToHashSet();
                    nn = nn.Where(a => !xn.Contains(a));
                }

                var nn1 = nn.ToArray();
                if (nn1.Any())
                    name += nn1.CommaJoin().TriangleBrackets();
            }

            if (type.DeclaringType == null)
            {
                var ns       = GetOrCreateNamespace(type.Namespace);
                var existing = ns.Classes.FirstOrDefault(a => a.Name == name);
                if (existing == null)
                {
                    existing = new CsClass(name)
                    {
                        IsPartial  = true,
                        Owner      = this,
                        Visibility = Visibilities.InterfaceDefault,
                        Kind       = type.GetNamespaceMemberKind()
                    };
                    ns.AddClass(existing);
                    isCreatedNew = true;
                }
                else
                    isCreatedNew = false;

                existing.DotNetType  = type;
                _classesCache[typeP] = existing;
                return existing;
            }

            {
                var parent   = GetOrCreateClass(TypeProvider.FromType(type.DeclaringType));
                var existing = parent.GetOrCreateNested(name, out isCreatedNew);
                existing.IsPartial  = true;
                existing.DotNetType = type;
                existing.Kind       = type.GetNamespaceMemberKind();
                if (isCreatedNew)
                    existing.Visibility = Visibilities.InterfaceDefault;
                return existing;
            }
        }

        {
            var typeNameParts  = typeP.TypeName.Split('.');
            var namespaceParts = typeNameParts.Take(typeNameParts.Length - 1).ToArray();
            var namespaceName  = string.Join(".", namespaceParts);
            var ns             = GetOrCreateNamespace(namespaceName);

            var name   = typeNameParts.Last();
            var result = ns.Classes.FirstOrDefault(aa => aa.Name == name);
            if (result != null)
            {
                isCreatedNew = false;
                return _classesCache[typeP] = result;
            }

            result = _classesCache[typeP] = new CsClass(name)
            {
                IsPartial = true,
                // DotNetType = type, // UNKNOWN
                Owner      = this,
                Visibility = Visibilities.InterfaceDefault,
                Kind       = typeP.Kind
            };

            ns.AddClass(result);
            isCreatedNew = true;
            return result;
        }
    }

    public CsNamespace GetOrCreateNamespace(string name)
    {
        var result = Namespaces.FirstOrDefault(ns => ns.Name == name);
        if (result != null)
            return result;
        result = new CsNamespace(this, name);
        ((List<CsNamespace>)Namespaces).Add(result);
        return result;
    }

    public string GetTypeName(Type type) => GeneratorsHelper.GetTypeName(this, type);

    public bool IsKnownNamespace(string namespaceName) => !string.IsNullOrEmpty(namespaceName) && _importNamespaces.Contains(namespaceName);

    public void MakeCode(ICsCodeWriter writer, bool isEmbedded = false)
    {
        if (Namespaces == null || Namespaces.Count == 0)
            return;
        switch (Nullable)
        {
            case FileNullableOption.LocalDisabled:
                writer.WriteLine("#nullable disable");
                break;
            case FileNullableOption.LocalEnabled:
                writer.WriteLine("#nullable enable");
                break;
        }

        if (ReSharperDisableAll)
            writer.WriteLine("// ReSharper disable All");
        const string emptyNamespace = "";
        if (!string.IsNullOrEmpty(BeginContent))
            writer.WriteLine(BeginContent);
        if (!isEmbedded)
            foreach (var i in _importNamespaces.OrderBy(i => i))
                writer.WriteLine("using {0};", i);
        if (_importNamespaces.Any())
            writer.EmptyLine();
        var classByNamespace     = Namespaces.ToDictionary(a => a.Name, a => a.Classes);
        var enumByNamespace      = Namespaces.ToDictionary(a => a.Name, a => a.Enums);
        var directiveByNamespace = Namespaces.ToDictionary(a => a.Name, a => a.CompilerDirective);

        var fileNamespaces = classByNamespace.Keys.Union(enumByNamespace.Keys)
            .OrderBy(a => a).ToList();

        foreach (var ns in fileNamespaces)
        {
            var addEmptyLine = false;
            if (string.IsNullOrEmpty(ns))
                continue; // should never occurs
            directiveByNamespace.TryGetValue(ns, out var compilerDirective);
            {
                writer.OpenCompilerIf(compilerDirective);
                writer.Open("namespace {0}", ns);
                var ns1 = Namespaces.FirstOrDefault(a => a.Name == ns)?.ImportNamespaces;
                if (ns1 != null && ns1.Any())
                {
                    foreach (var i in ns1.OrderBy(a => a))
                        writer.WriteLine($"using {i};");
                    addEmptyLine = true;
                }
            }

            {
                if (classByNamespace.TryGetValue(ns, out var classList))
                    foreach (var i in classList.OrderBy(a => a.Name))
                    {
                        if (addEmptyLine)
                            writer.EmptyLine();
                        addEmptyLine = true;
                        writer.DoWithKeepingIndent(() => i.MakeCode(writer));
                    }

                if (enumByNamespace.TryGetValue(ns, out var enumList))
                    foreach (var i in enumList)
                    {
                        if (addEmptyLine)
                            writer.EmptyLine();
                        addEmptyLine = true;
                        writer.DoWithKeepingIndent(() => i.MakeCode(writer));
                    }
            }
            if (!string.IsNullOrEmpty(ns))
                writer.Close();
            writer.CloseCompilerIf(compilerDirective);
        }

        if (!string.IsNullOrEmpty(EndContent))
            writer.WriteLine(EndContent);
    }

    private void Save(string filename)
    {
        var fi = new FileInfo(filename);
        if (fi.Directory == null)
            throw new NullReferenceException("fi.Directory");
        fi.Directory.Create();
        var       x  = Encoding.UTF8.GetBytes(GetCode());
        using var fs = new FileStream(filename, File.Exists(filename) ? FileMode.Create : FileMode.CreateNew);
        fs.Write(x, 0, x.Length);
#if !COREFX
        fs.Close();
#endif
    }

    public bool SaveIfDifferent(string filename, bool addBom = false) => CodeFileUtils.SaveIfDifferent(GetCode(), filename, addBom);

    public override string ToString() => GetCode();

    #region Properties

    /// <summary>
    ///     Przestrzenie nazw
    /// </summary>
    public IReadOnlyList<CsNamespace> Namespaces { get; } = new List<CsNamespace>();


    /// <summary>
    /// </summary>
    public string SuggestedFileName
    {
        get => _suggestedFileName;
        set
        {
            value              = value?.Trim() ?? string.Empty;
            _suggestedFileName = value;
        }
    }


    /// <summary>
    ///     Optional content on the top of file
    /// </summary>
    public string BeginContent { get; set; }

    /// <summary>
    ///     Optional content on the end of file
    /// </summary>
    public string EndContent { get; set; }


    public FileNullableOption Nullable { get; set; }

    public bool ReSharperDisableAll { get; set; } = GlobalSettings.DefaultReSharperDisableAll;

    #endregion

    #region Fields

    private readonly Dictionary<TypeProvider, CsClass> _classesCache = new Dictionary<TypeProvider, CsClass>();

    /// <summary>
    ///     Przestrzenie nazw
    /// </summary>
    private readonly ISet<string> _importNamespaces = new HashSet<string>();

    private List<CsEnum> _enums = new List<CsEnum>();
    private string _suggestedFileName = string.Empty;

    #endregion
}
