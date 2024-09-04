#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using iSukces.Code.Interfaces;

namespace iSukces.Code.Typescript
{
    public class TsMethod : ITsClassMember
    {
        public TsMethod(string? name = null)
        {
            Name = name;
        }

        public TsMethod WithArgument(string name, string? type = null)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            var isOptional = name.EndsWith("?", StringComparison.Ordinal);
            if (isOptional)
                name = name.TrimEnd('?', ' ');
            var arg = new TsMethodArgument
            {
                Name       = name,
                Type       = type,
                IsOptional = isOptional
            };
            Arguments.Add(arg);
            return this;
        }

        public TsMethod WithBody(string body)
        {
            Body = body;
            return this;
        }

        public TsMethod WithBody(ICodeWriter body)
        {
            Body = body.Code;
            return this;
        }

        public TsMethod WithIsStatic(bool isStatic)
        {
            IsStatic = isStatic;
            return this;
        }

        public TsMethod WithResultType(string resultType)
        {
            ResultType = resultType;
            return this;
        }

        public TsMethod WithVisibility(TsVisibility visibility)
        {
            Visibility = visibility;
            return this;
        }

        public void WriteCodeTo(ITsCodeWriter writer)
        {
            var header = string.Join(" ", GetHeaderItems());
            if (writer.HeadersOnly)
            {
                writer.WriteLine(header + ";");
                return;
            }

            writer.Open(header);
            if (!string.IsNullOrEmpty(Body))
            {
                var b = Body.Replace("\r\n", "\n").Split('\n');
                foreach (var line in b)
                    writer.WriteLine(line);
            }

            writer.Close();
        }

        private IEnumerable<string> GetHeaderItems()
        {
            if (Visibility != TsVisibility.Default)
                yield return Visibility.ToString().ToLower();
            if (IsStatic)
                yield return "static";
            yield return GetNameWithArguments();
            if (!string.IsNullOrEmpty(ResultType))
                yield return ": " + ResultType;
        }

        private string GetNameWithArguments()
        {
            if (Arguments == null || Arguments.Count == 0)
                return $"{Name}()";
            return Name + "(" + string.Join(",", Arguments.Select(a => a.GetTsCode())) + ")";
        }

        public string                 Name       { get; set; }
        public bool                   IsStatic   { get; set; }
        public TsVisibility           Visibility { get; set; }
        public List<TsMethodArgument> Arguments  { get; set; } = new List<TsMethodArgument>();
        public string                 ResultType { get; set; }
        public string                 Body       { get; set; }
    }
}