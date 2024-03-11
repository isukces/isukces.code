using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using iSukces.Code.AutoCode;
using JetBrains.Annotations;

namespace iSukces.Code.Interfaces;

public interface IAttributable
{
    IList<ICsAttribute> Attributes { get; }
}

public static class AttributableExt
{
    public static string CutAttributeSuffix(string name)
    {
        if (name.EndsWith(AttributeSuffix))
            name = name.Substring(0, name.Length - AttributeSuffixLength);
        return name;
    }

    public static void RemoveAttribute<T>(this T self, string className)
        where T : IAttributable
    {
        for (var index = self.Attributes.Count - 1; index >= 0; index--)
        {
            if (!(self.Attributes[index] is CsAttribute csAttribute)) continue;
            if (csAttribute.Name == className)
                self.Attributes.RemoveAt(index);
        }
    }

    public static T WithAttribute<T>(this T element, INamespaceContainer nsProvider, Type attributeType)
        where T : IAttributable
    {
        var tn = nsProvider.GetTypeName(attributeType);
        return element.WithAttribute(new CsAttribute(tn));
    }

    public static T WithAttribute<T>(this T attributable, ICsAttribute attribute)
        where
        T : IAttributable
    {
        attributable.Attributes.Add(attribute);
        return attributable;
    }

    public static T WithAttribute<T>(this T element, Type t)
        where T : IAttributable, INamespaceContainer
    {
        var tn = element.GetTypeName(t);
        return element.WithAttribute(new CsAttribute(tn));
    }

    public static T WithAttribute<T>(this T self, string className, string value)
        where T : IAttributable
    {
        ICsAttribute at = new CsAttribute(className).WithArgument(value);
        return WithAttribute(self, at);
    }

    public static FluentAttributesBuilder<T> WithAttributeBuilder<T>(this T element, INamespaceContainer nsProvider)
        where T : IAttributable
    {
        return new FluentAttributesBuilder<T>(element, nsProvider);
    }

    public static T WithAttributeFromName<T>(this T self, string className)
        where T : IAttributable
    {
        ICsAttribute at = new CsAttribute(className);
        return WithAttribute(self, at);
    }

    public static T WithAttributeFromName<T>(this T self, CsType className)
        where T : IAttributable
    {
        ICsAttribute at = new CsAttribute(className);
        return WithAttribute(self, at);
    }

    public static T WithAttributeOnce<T>(this T self, ICsAttribute attribute)
        where T : IAttributable
    {
        self.RemoveAttribute(attribute.Name);
        self.Attributes.Add(attribute);
        return self;
    }

    public static T WithAutocodeGeneratedAttribute<T>(this T attributable,
        [NotNull] ITypeNameResolver resolver,
        SourceCodeLocation location)
        where T : IAttributable
    {
        return WithAutocodeGeneratedAttribute(attributable, resolver, location.ToString());
    }

    public static T WithAutocodeGeneratedAttribute<T>(this T attributable,
        ITypeNameResolver resolver,
        string generatorInfo = "")
        where T : IAttributable
    {
        var name = resolver.GetTypeName(typeof(AutocodeGeneratedAttribute))
            .Declaration;
        var at = new CsAttribute(name);
        if (!string.IsNullOrEmpty(generatorInfo))
            at.WithArgumentCode(generatorInfo.CsEncode());
        return WithAttribute(attributable, at);
    }

    public static T WithAutocodeGeneratedAttributeAuto<T>(this T attributable,
        ITypeNameResolver resolver,
        [CallerMemberName] string callerMemberName = null,
        [CallerLineNumber] int callerLineNumber = 0,
        [CallerFilePath] string path = null)
        where T : IAttributable
    {
        var gen = new SourceCodeLocation(callerLineNumber, callerMemberName, path);
        return WithAutocodeGeneratedAttribute(attributable, resolver, gen.ToString());
    }

    private static readonly int AttributeSuffixLength = AttributeSuffix.Length;

    private const string AttributeSuffix = "Attribute";
}