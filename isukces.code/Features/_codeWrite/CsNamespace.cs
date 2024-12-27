#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using iSukces.Code.AutoCode;
using iSukces.Code.Interfaces;

namespace iSukces.Code;

[DebuggerDisplay("CsNamespace {" + nameof(Name) + "}")]
public class CsNamespace : IClassOwner, INamespaceCollection, IConditional, IEnumOwner
{
    public CsNamespace(INamespaceOwner owner, string? name)
    {
        Usings            = new NamespacesHolder(ns =>
        {
            if (string.IsNullOrEmpty(ns))
                return new UsingInfo(NamespaceSearchResult.Empty);
            return new UsingInfo(Name == ns ? NamespaceSearchResult.Found : NamespaceSearchResult.NotFound);
        });
        Owner             = owner ?? throw new ArgumentNullException(nameof(owner));
        Name              = name?.Trim() ?? string.Empty;
        CompilerDirective = string.Empty;
    }

    public CsClass AddClass(CsClass csClass)
    {
        ((List<CsClass>)Classes).Add(csClass);
        csClass.Owner = this;
        return csClass;
    }

    public CsEnum AddEnum(CsEnum csEnum)
    {
        ((List<CsEnum>)Enums).Add(csEnum);
        csEnum.Owner = this;
        return csEnum;
    }

    public void AddImportNamespace(string? ns, string? alias)
    {
        Usings.Add(ns, alias);
    }

    public UsingInfo GetNamespaceInfo(string? namespaceName)
    {
        if (string.IsNullOrEmpty(namespaceName))
            return new UsingInfo(NamespaceSearchResult.Empty);
        if (Name == namespaceName)
            return new UsingInfo(NamespaceSearchResult.Found);
        var tmp = Usings.GetNamespaceInfo(namespaceName);
        if (tmp.SearchResult != NamespaceSearchResult.NotFound)
            return tmp;
        return Owner?.GetNamespaceInfo(namespaceName) ?? default;
    }


    // [Obsolete("Use CsType instead of string", GlobalSettings.WarnObsolete)]
    public CsClass GetOrCreateClass(string csClassName)
    {
        return GetOrCreateClass(new CsType(csClassName));
    }

    public CsClass GetOrCreateClass(CsType csClassName)
    {
        csClassName.ThrowIfArray();
        var existing = Classes.FirstOrDefault(a => a.Name == csClassName);
        return existing ?? AddClass(new CsClass(csClassName));
    }

    public CsType GetTypeName(Type type)
    {
        return GeneratorsHelper.GetTypeName(this, type);
    }

    public string? TryGetTypeAlias(TypeProvider type)
    {
        var alias = Usings.TryGetTypeAlias(type);
        if (!string.IsNullOrEmpty(alias))
            return alias;
        return Owner?.TryGetTypeAlias(type);
    }

    public NamespacesHolder Usings { get; }

    public INamespaceOwner? Owner { get; }

    public string Name { get; }

    public IReadOnlyList<CsClass> Classes { get; } = new List<CsClass>();

    public string? CompilerDirective { get; set; }

    /// <summary>
    ///     Enums
    /// </summary>
    public IReadOnlyList<CsEnum> Enums { get; } = new List<CsEnum>();
}