using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using iSukces.Code;
using iSukces.Code.Interfaces;

namespace AutoCodeBuilder;

public class BaseGenerator
{
    protected static bool CheckIfAnonymousType(Type type)
    {
        if (type == null)
            throw new ArgumentNullException(nameof(type));

        // HACK: The only way to detect anonymous types right now.
        return Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
               && type.IsGenericType && type.Name.Contains("AnonymousType")
               && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$"))
               && (type.Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic;
    }

    protected static CsCodeWriter CreateCodeWriter(string generatorClassName,
        [CallerFilePath] string? callerFilePath = null,
        [CallerLineNumber] int lineNumber = 0,
        [CallerMemberName] string? memberName = null)
    {
        var info = new SourceCodeLocation(lineNumber, memberName, callerFilePath)
            .WithGeneratorClassName(generatorClassName);
        return CsCodeWriter.Create(info);
    }


    protected static bool NotImplements<TInterface>(Type type)
    {
        if (type.GetTypeInfo().IsGenericType)
        {
            if (!typeof(TInterface).IsAssignableFrom(type)) return true;
        }
        else
        {
            if (!typeof(TInterface).IsAssignableFrom(type)) return true;
        }

        return false;
    }

    protected CsCodeWriter CreateCodeWriter([CallerFilePath] string? filePath = null,
        [CallerLineNumber] int lineNumber = 0,
        [CallerMemberName] string? memberName = null) =>
        CsCodeWriter.Create(new SourceCodeLocation(lineNumber, memberName, filePath)
            .WithGeneratorClass(GetType()));


    protected static CsMethod CreateMethod(string name, Type type, CsClass cl, CodeWriter cf,
        [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string? memberName = null)
    {
/*
            var info = new SourceCodeInfo(lineNumber, memberName)
                .WithGeneratorClass(GetType());
*/

        var m = cl.AddMethod(name, cl.GetTypeName(type))
            .WithBody(cf);
        /*
        if (cf is CsCodeWriter ex)
            ex.AddAutocodeGeneratedAttribute(m, cl);
        else
        */
        // m.WithAutocodeGeneratedAttribute(cl, info.GetGenerator1());
        m.WithAutocodeGeneratedAttribute(cl, "");
        return m;
    }

    protected bool IgnoreType(Type type)
    {
        if (Skip.Contains(type))
            return true;
        if (type.IsInterface)
            return true;
        if (type.IsAbstract)
            return true;
        if (CheckIfAnonymousType(type))
            return true;
        return false;
    }

    public HashSet<Type> Skip { get; } = new HashSet<Type>();
}
