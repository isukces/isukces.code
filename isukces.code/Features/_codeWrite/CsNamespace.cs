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

    public void AddImportNamespace(string ns) 
        => ImportNamespaces.Add(ns);

    public void AddImportNamespace<T>()
    {
        var item = typeof(T).Namespace;
        if (string.IsNullOrEmpty(item)) return;
        ImportNamespaces.Add(item);
    }

    [Obsolete("Use CsType instead of string", GlobalSettings.WarnObsolete)]
    public CsClass GetOrCreateClass(string csClassName) => GetOrCreateClass(new CsType(csClassName));

    public CsClass GetOrCreateClass(CsType csClassName)
    {
        csClassName.ThrowIfArray();
        var existing = Classes.FirstOrDefault(a => a.Name == csClassName);
        return existing ?? AddClass(new CsClass(csClassName));
    }

    public CsType GetTypeName(Type type) => GeneratorsHelper.GetTypeName(this, type);

    public bool IsKnownNamespace(string? namespaceName)
    {
        if (string.IsNullOrEmpty(namespaceName)) return false;
        if (Name == namespaceName) return true;
        if (ImportNamespaces.Contains(namespaceName)) return true;
        return Owner?.IsKnownNamespace(namespaceName) ?? false;
    }

    public ISet<string> ImportNamespaces { get; } = new HashSet<string>();

    public INamespaceOwner Owner { get; }

    public string Name { get; }

    public IReadOnlyList<CsClass> Classes { get; } = new List<CsClass>();

    public string CompilerDirective { get; set; }

    /// <summary>
    ///     Enums
    /// </summary>
    public IReadOnlyList<CsEnum> Enums { get; } = new List<CsEnum>();

}
