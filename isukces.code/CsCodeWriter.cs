using System;
using System.Runtime.CompilerServices;
using iSukces.Code.Interfaces;

namespace iSukces.Code;

public class CsCodeWriter : CodeWriter, ICsCodeWriter
{
    public CsCodeWriter([CallerMemberName] string? memberName = null,
        [CallerFilePath] string? filePath = null,
        [CallerLineNumber] int lineNumber = 0)
        : base(CsLangInfo.Instance)
    {
        Location = new SourceCodeLocation(lineNumber, memberName, filePath);
    }

    public static CsCodeWriter Create(SourceCodeLocation location)
    {
        var code = new CsCodeWriter
        {
            Location = location
        };
        code.WriteLine($"// generator : {location}");
        return code;
    }

    public static CsCodeWriter Create<T>(
        bool skipLineNumber = false,
        [CallerLineNumber] int lineNumber = 0,
        [CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null
    )
    {
        if (skipLineNumber)
            lineNumber = 0;
        var sourceCodeLocation = new SourceCodeLocation(lineNumber, memberName, filePath)
            .WithGeneratorClass(typeof(T));
        return Create(sourceCodeLocation);
    }

    public static CsCodeWriter Create(Type generatorClass, [CallerLineNumber] int lineNumber = 0,
        [CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null
    ) =>
        Create(new SourceCodeLocation(lineNumber, memberName, filePath)
            .WithGeneratorClass(generatorClass));

    public void AddAutocodeGeneratedAttribute(IAttributable attributable, ITypeNameResolver resolver)
    {
        attributable.WithAutocodeGeneratedAttribute(resolver, Location);
    }

    public SourceCodeLocation Location { get; set; }
}
