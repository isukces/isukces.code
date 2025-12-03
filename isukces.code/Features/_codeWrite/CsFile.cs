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
    public CsFile()
    {
        Usings = new(null);
    }

    public void AddImportNamespace(string? ns, string? alias = null)
    {
        Usings.Add(ns, alias);
    }

    public string GetCode(bool isEmbedded = false)
    {
        var writer = new CsCodeWriter();
        MakeCode(writer, isEmbedded);
        return writer.Code;
    }

    public UsingInfo GetNamespaceInfo(string? namespaceName)
    {
        if (string.IsNullOrEmpty(namespaceName))
            return new(NamespaceSearchResult.Empty);
        var a = Usings.GetNamespaceInfo(namespaceName);
        if (a.SearchResult != NamespaceSearchResult.NotFound)
            return a;
        if (GlobalUsings is not null)
        {
            var info = GlobalUsings.GetNamespaceInfo(namespaceName);
            if (info.SearchResult != NamespaceSearchResult.NotFound)
                return info;
        }

        return new UsingInfo(NamespaceSearchResult.NotFound);
    }

    public CsClass GetOrCreateClass(string namespaceName, CsType className)
    {
        var ns = GetOrCreateNamespace(namespaceName);
        return ns.GetOrCreateClass(className);
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
            if (c.DotNetType is null && typeP.Type is not null)
                c.DotNetType = typeP.Type;
            isCreatedNew = false;
            return c;
        }

        if (typeP.Type is not null)
        {
            var type = typeP.Type;
            var name = new CsType(type.Name);
            var ti   = type.GetTypeInfo();
            if (ti.IsGenericType)
            {
                if (!ti.IsGenericTypeDefinition)
                    throw new NotSupportedException();
                name = new CsType(type.Name.Split('`')[0]);
                var genericArguments = ti.GetGenericArguments();
                var nn               = genericArguments.Select(a => a.Name);
                if (type.DeclaringType is not null)
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

            if (type.DeclaringType is null)
            {
                var ns       = GetOrCreateNamespace(type.Namespace);
                var existing = ns.Classes.FirstOrDefault(a => a.Name == name);
                if (existing is null)
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
                {
                    isCreatedNew = false;
                }

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
            var (namespaceName, shortClassName) = typeP.TypeName.SpitNamespaceAndShortName();
            var ns = GetOrCreateNamespace(namespaceName);

            var name   = new CsType(shortClassName);
            var result = ns.Classes.FirstOrDefault(aa => aa.Name == name);
            if (result is not null)
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
        if (result is not null)
            return result;
        result = new CsNamespace(this, name);
        ((List<CsNamespace>)Namespaces).Add(result);
        return result;
    }

    public CsType GetTypeName(Type type)
    {
        return GeneratorsHelper.GetTypeName(this, type);
    }

    public void MakeCode(ICsCodeWriter writer, bool isEmbedded = false)
    {
        if (Namespaces.Count == 0)
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
            if (Usings.Emit(writer, GlobalUsings))
                writer.EmptyLine();

        var classByNamespace     = Namespaces.ToDictionary(a => a.Name, a => a.Classes);
        var enumByNamespace      = Namespaces.ToDictionary(a => a.Name, a => a.Enums);
        var directiveByNamespace = Namespaces.ToDictionary(a => a.Name, a => a.CompilerDirective);

        var fileNamespaces = classByNamespace.Keys
            .Union(enumByNamespace.Keys)
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
                if (!isEmbedded)
                    EmitUsings(ns, ref addEmptyLine);
            }

            if (classByNamespace.TryGetValue(ns, out var classList))
                EmitClasses(classList, ref addEmptyLine);

            if (enumByNamespace.TryGetValue(ns, out var enumList))
                EmitEnums(enumList, ref addEmptyLine);
            namespaceWriter.CloseNamespace(ns, writer);
            writer.CloseCompilerIf(compilerDirective);
        }

        if (!string.IsNullOrEmpty(EndContent))
            writer.WriteLine(EndContent);
        return;

        void EmitClasses(IReadOnlyList<CsClass> classes, ref bool addEmptyLine)
        {
            foreach (var c in classes.OrderBy(a => a.Name))
            {
                if (addEmptyLine)
                    writer.EmptyLine();
                addEmptyLine = true;
                writer.DoWithKeepingIndent(() => c.MakeCode(writer, config));
            }
        }

        void EmitEnums(IReadOnlyList<CsEnum> enums, ref bool addEmptyLine)
        {
            foreach (var e in enums)
            {
                if (addEmptyLine)
                    writer.EmptyLine();
                addEmptyLine = true;
                writer.DoWithKeepingIndent(() => e.MakeCode(writer));
            }
        }

        void EmitUsings(string ns, ref bool addEmptyLine)
        {
            var ns1 = Namespaces.FirstOrDefault(a => a.Name == ns)?.Usings;
            if (ns1 is not null)
                if (ns1.Emit(writer, GlobalUsings))
                    addEmptyLine = true;
        }
    }

    private void Save(string filename)
    {
        var fi = new FileInfo(filename);
        if (fi.Directory is null)
            throw new NullReferenceException("fi.Directory");
        fi.Directory.Create();
        var       x  = Encoding.UTF8.GetBytes(GetCode());
        using var fs = new FileStream(filename, File.Exists(filename) ? FileMode.Create : FileMode.CreateNew);
        fs.Write(x, 0, x.Length);
#if !COREFX
        fs.Close();
#endif
    }

    public bool SaveIfDifferent(string filename
#if BOM
        , bool addBom = false
#endif
        )
    {
#if BOM
        return CodeFileUtils.SaveIfDifferent(GetCode(), filename, addBom);
#else
        return CodeFileUtils.SaveIfDifferent(GetCode(), filename);
#endif
    }

    public override string ToString() => GetCode();

    public string? TryGetTypeAlias(TypeProvider type) => Usings.TryGetTypeAlias(type);

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

    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public bool ReSharperDisableAll { get; set; } = GlobalSettings.DefaultReSharperDisableAll;


    public FileScopeNamespaceConfiguration FileScopeNamespace
    {
        get => _fileScopeNamespace;
        set => _fileScopeNamespace = value ?? throw new ArgumentNullException(nameof(value));
    }

    public GlobalUsingsConfiguration? GlobalUsings { get; set; }

    public NamespacesHolder Usings { get; }

    private readonly Dictionary<TypeProvider, CsClass> _classesCache = new();

    private List<CsEnum> _enums = new();

    private FileScopeNamespaceConfiguration _fileScopeNamespace = FileScopeNamespaceConfiguration.BlockScoped;

    private string _suggestedFileName = string.Empty;
}

