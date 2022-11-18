using System;
using System.Collections.Generic;
using System.Linq;
using Irony.Parsing;

namespace Bitbrains.AmmyParser
{
    public class FullQualifiedNameData : IBaseData
    {
        public FullQualifiedNameData(SourceSpan span, IEnumerable<string> result)
        {
            Span = span;
            if (result is null)
                Parts = Array.Empty<string>();
            else
                Parts = result as IReadOnlyList<string> ?? result.ToArray();
        }

        public IReadOnlyList<string> Parts { get; }

        public SourceSpan Span { get; }

        public override string ToString()
        {
            return string.Join(".", Parts);
        }

        public IReadOnlyList<string> GetPartsWithPrefix(string prefix)
        {
            var result = new List<string>(Parts.Count + 1) {prefix};
            result.AddRange(Parts);
            return result.ToArray();

        }
    }
}