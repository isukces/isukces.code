using System;
using System.Runtime.CompilerServices;

namespace iSukces.Code
{
    public struct SourceCodeLocation
    {
        public SourceCodeLocation(int lineNumber, string memberName, string filePath = null)
        {
            FilePath           = filePath;
            MemberName         = memberName;
            LineNumber         = lineNumber;
            GeneratorClassName = null;
        }

        public static SourceCodeLocation Make(
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string filePath = null,
            [CallerLineNumber] int lineNumber = 0
        ) =>
            new SourceCodeLocation(lineNumber, memberName, filePath);

        public static SourceCodeLocation Make<T>(
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string filePath = null,
            [CallerLineNumber] int lineNumber = 0) =>
            Make(memberName, filePath, lineNumber).WithGeneratorClass(typeof(T));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string Join(string sep, string x, string y)
        {
            x = x?.Trim();
            y = y?.Trim();
            if (string.IsNullOrEmpty(x))
                return y;
            if (string.IsNullOrEmpty(y))
                return x;
            return x + sep + y;
        }

        public override string ToString()
        {
            var tmp = Join(":", MemberName, LineNumber == 0 ? "" : LineNumber.ToCsString());
            return Join(".", GeneratorClassName, tmp);
        }

        public SourceCodeLocation WithGeneratorClass(Type getType)
        {
            GeneratorClassName = getType.Name;
            return this;
        }

        public SourceCodeLocation WithGeneratorClassName(string generatorClassName)
        {
            GeneratorClassName = generatorClassName;
            return this;
        }

        public string FilePath           { get; }
        public string MemberName         { get; }
        public int    LineNumber         { get; }
        public string GeneratorClassName { get; private set; }
    }
}