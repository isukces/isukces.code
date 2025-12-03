using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using iSukces.Code.AutoCode;

namespace iSukces.Code.Interfaces;

public interface IAttributable
{
    IList<ICsAttribute> Attributes { get; }
}

public static class AttributableExt
{
    public static string CutAttributeSuffix(string name)
    {
        if (name.EndsWith(AttributeSuffix, StringComparison.Ordinal))
            name = name[..^AttributeSuffixLength];
        return name;
    }


    public static T WithAttribute<T>(this T self, Type t)
        where T : IAttributable, INamespaceContainer
    {
        var tn = self.GetTypeName(t);
        return self.WithAttribute(new CsAttribute(tn));
    }

    private const string AttributeSuffix = "Attribute";

    private static readonly int AttributeSuffixLength = AttributeSuffix.Length;

    extension<T>(T self)
        where T : IAttributable
    {
        public void RemoveAttribute(string className)
        {
            for (var index = self.Attributes.Count - 1; index >= 0; index--)
            {
                if (!(self.Attributes[index] is CsAttribute csAttribute)) continue;
                if (csAttribute.Name == className)
                    self.Attributes.RemoveAt(index);
            }
        }

        public T WithAttribute(INamespaceContainer nsProvider, Type attributeType)
        {
            var tn = nsProvider.GetTypeName(attributeType);
            return self.WithAttribute(new CsAttribute(tn));
        }

        public T WithAttribute(ICsAttribute attribute)
        {
            self.Attributes.Add(attribute);
            return self;
        }

        public T WithAttribute(string className, string value)
        {
            ICsAttribute at = new CsAttribute(className).WithArgument(value);
            return WithAttribute(self, at);
        }

        public FluentAttributesBuilder<T> WithAttributeBuilder(INamespaceContainer nsProvider)
        {
            return new FluentAttributesBuilder<T>(self, nsProvider);
        }

        public T WithAttributeFromName(string className)
        {
            ICsAttribute at = new CsAttribute(className);
            return WithAttribute(self, at);
        }

        public T WithAttributeFromName(CsType className)
        {
            ICsAttribute at = new CsAttribute(className);
            return WithAttribute(self, at);
        }

        public T WithAttributeOnce(ICsAttribute attribute)
        {
            self.RemoveAttribute(attribute.Name);
            self.Attributes.Add(attribute);
            return self;
        }

        public T WithAutocodeGeneratedAttribute(ITypeNameResolver resolver,
            SourceCodeLocation location)
        {
            return WithAutocodeGeneratedAttribute(self, resolver, location.ToString());
        }

        public T WithAutocodeGeneratedAttribute(ITypeNameResolver resolver,
            string generatorInfo = "")
        {
            var name = resolver.GetTypeName(typeof(AutocodeGeneratedAttribute))
                .Declaration;
            var at = new CsAttribute(name);
            if (!string.IsNullOrEmpty(generatorInfo))
                at.WithArgumentCode(generatorInfo.CsEncode());
            return WithAttribute(self, at);
        }

        public T WithAutocodeGeneratedAttributeAuto(ITypeNameResolver resolver,
            [CallerMemberName] string? callerMemberName = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string? path = null)
        {
            var gen = new SourceCodeLocation(callerLineNumber, callerMemberName, path);
            return WithAutocodeGeneratedAttribute(self, resolver, gen.ToString());
        }
    }
}
