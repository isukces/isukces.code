using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using iSukces.Code.AutoCode;
using iSukces.Code.Interfaces;
using iSukces.Code.IO;
using JetBrains.Annotations;

namespace iSukces.Code;

public class CsFile : IClassOwner, INamespaceCollection, INamespaceOwner
{
    public void AddImportNamespace(string ns)
    {
        _importNamespaces.Add(ns);
    }

    public void AddImportNamespace<T>()
    {
        var item = typeof(T).Namespace;
        if (string.IsNullOrEmpty(item)) return;
        _importNamespaces.Add(item);
    }

    public string GetCode(bool isEmbedded = false)
    {
        var writer = new CsCodeWriter();
        MakeCode(writer, isEmbedded);
        return writer.Code;
    }

    public CsClass GetOrCreateClass(string namespaceName, CsType className)
    {
        var ns = GetOrCreateNamespace(namespaceName);
        return ns.GetOrCreateClass(className);
    }

    [Obsolete("Use CsType instead of string", GlobalSettings.WarnObsolete)]
    public CsClass GetOrCreateClass(string namespaceName, string className)
    {
        var ns = GetOrCreateNamespace(namespaceName);
        return ns.GetOrCreateClass((CsType)className);
    }

    public CsClass GetOrCreateClass(TypeProvider typeP)
    {
        return GetOrCreateClass(typeP, out _);
    }

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
            var name = new CsType(type.Name);
            var ti = type.GetTypeInfo();
            if (ti.IsGenericType)
            {
                if (!ti.IsGenericTypeDefinition)
                    throw new NotSupportedException();
                name = new CsType(type.Name.Split('`')[0]);
                var genericArguments = ti.GetGenericArguments();
                var nn = genericArguments.Select(a => a.Name);
                if (type.DeclaringType != null)
                {
                    var genericArguments2 = type
                        .DeclaringType
                        .GetTypeInfo()
                        .GetGenericArguments();
                    var xn = genericArguments2.Select(a => a.Name).ToHashSet();
                    nn = nn.Where(a => !xn.Contains(a));
                }

                var nn1 = nn.Select(a => new CsType(a)).ToArray();
                if (nn1.Any())
                    name.GenericParamaters = nn1;
                //name += nn1.CommaJoin().TriangleBrackets();
            }

            if (type.DeclaringType == null)
            {
                var ns = GetOrCreateNamespace(type.Namespace);
                var existing = ns.Classes.FirstOrDefault(a => a.Name == name);
                if (existing == null)
                {
                    existing = new CsClass(name)
                    {
                        IsPartial = true,
                        Owner = this,
                        Visibility = Visibilities.InterfaceDefault,
                        Kind = type.GetNamespaceMemberKind()
                    };
                    ns.AddClass(existing);
                    isCreatedNew = true;
                }
                else
                {
                    isCreatedNew = false;
                }

                existing.DotNetType = type;
                _classesCache[typeP] = existing;
                return existing;
            }

            {
                var parent = GetOrCreateClass(TypeProvider.FromType(type.DeclaringType));
                var existing = parent.GetOrCreateNested(name, out isCreatedNew);
                existing.IsPartial = true;
                existing.DotNetType = type;
                existing.Kind = type.GetNamespaceMemberKind();
                if (isCreatedNew)
                    existing.Visibility = Visibilities.InterfaceDefault;
                return existing;
            }
        }

        {
            var (namespaceName, shortClassName) = typeP.TypeName.SpitNamespaceAndShortName();
            var ns = GetOrCreateNamespace(namespaceName);

            var name = new CsType(shortClassName);
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
                Owner = this,
                Visibility = Visibilities.InterfaceDefault,
                Kind = typeP.Kind
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

    public CsType GetTypeName(Type type)
    {
        return GeneratorsHelper.GetTypeName(this, type);
    }

    public bool IsKnownNamespace(string namespaceName)
    {
        if (_importNamespaces.IsKnownNamespace(namespaceName))
            return true;
        return GlobalUsings?.IsKnownNamespace(namespaceName) ?? false;
    }

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
        if (!string.IsNullOrEmpty(BeginContent))
            writer.WriteLine(BeginContent);
        if (!isEmbedded)
        {
            var nsList = _importNamespaces.GetNamespaces(GlobalUsings);
            foreach (var i in nsList)
                writer.WriteLine("using {0};", i);
            if (nsList.Count > 0)
                writer.EmptyLine();
        }

        var classByNamespace = Namespaces.ToDictionary(a => a.Name, a => a.Classes);
        var enumByNamespace = Namespaces.ToDictionary(a => a.Name, a => a.Enums);
        var directiveByNamespace = Namespaces.ToDictionary(a => a.Name, a => a.CompilerDirective);

        var fileNamespaces = classByNamespace.Keys.Union(enumByNamespace.Keys)
            .OrderBy(a => a).ToList();


        var config = new CodeEmitConfig
        {
            AllowReferenceNullable = Nullable.IsNullableReferenceEnabled()
        };

        var namespaceWriter = FileScopeNamespace.Check(fileNamespaces, out var comment);
        if (!string.IsNullOrEmpty(comment))
            writer.WriteLine("// suggestion: " + comment);

        foreach (var ns in fileNamespaces)
        {
            var addEmptyLine = false;
            if (string.IsNullOrEmpty(ns))
                continue; // should never occurs
            directiveByNamespace.TryGetValue(ns, out var compilerDirective);
            {
                writer.OpenCompilerIf(compilerDirective);
                namespaceWriter.OpenNamespace(ns, writer);
                // writer.Open("namespace {0}", ns);
                var ns1 = Namespaces.FirstOrDefault(a => a.Name == ns)?
                    .ImportNamespaces;
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
                        writer.DoWithKeepingIndent(() => i.MakeCode(writer, config));
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
            namespaceWriter.CloseNamespace(ns, writer);
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
        var x = Encoding.UTF8.GetBytes(GetCode());
        using var fs = new FileStream(filename, File.Exists(filename) ? FileMode.Create : FileMode.CreateNew);
        fs.Write(x, 0, x.Length);
#if !COREFX
        fs.Close();
#endif
    }

    public bool SaveIfDifferent(string filename, bool addBom = false)
    {
        return CodeFileUtils.SaveIfDifferent(GetCode(), filename, addBom);
    }

    public override string ToString()
    {
        return GetCode();
    }

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
            value = value?.Trim() ?? string.Empty;
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


    public FileScopeNamespaceConfiguration FileScopeNamespace
    {
        get => _fileScopeNamespace;
        set => _fileScopeNamespace = value ?? throw new ArgumentNullException(nameof(value));
    }

    [CanBeNull] public GlobalUsingsConfiguration GlobalUsings { get; set; }

    private readonly Dictionary<TypeProvider, CsClass> _classesCache = new();

    private List<CsEnum> _enums = new();

    private FileScopeNamespaceConfiguration _fileScopeNamespace = FileScopeNamespaceConfiguration.BlockScoped;

    /// <summary>
    ///     Przestrzenie nazw
    /// </summary>
    private readonly NamespacesHolder _importNamespaces = new();

    private string _suggestedFileName = string.Empty;
}