using System;
using System.Runtime.CompilerServices;
using isukces.code.interfaces;
using JetBrains.Annotations;

namespace isukces.code
{
    public class CsCodeWriter : CodeWriter, ICsCodeWriter
    {
        public CsCodeWriter([CallerMemberName] string memberName = null,
            [CallerFilePath] string filePath = null,
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
            code.WriteLine("// generator : " + location.ToString());
            return code;
        }

        protected static CsCodeWriter Create<T>([CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null
        ) =>
            Create(new SourceCodeLocation(lineNumber, memberName, filePath)
                .WithGeneratorClass(typeof(T)));

        public void AddAutocodeGeneratedAttribute(IAttributable attributable, [NotNull] ITypeNameResolver resolver)
        {
            attributable.WithAutocodeGeneratedAttribute(resolver, Location);
        }

        public SourceCodeLocation Location { get; set; }
    }
}