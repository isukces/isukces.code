using System.Collections.Generic;
using System.Linq;

namespace isukces.code.Typescript
{
    public class TsMethod : ITsClassMember
    {
        public TsMethod(string name = null)
        {
            Name = name;
        }

        public TsMethod WithArgument(string name, string type = null)
        {
            var arg = new TsMethodArgument
            {
                Name = name,
                Type = type
            };
            Arguments.Add(arg);
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

        public void WriteCodeTo(TsCodeFormatter formatter)
        {            
            var header = string.Join(" ", GetHeaderItems());
            if (formatter.HeadersOnly)
            {
                formatter.Writeln(header + ";");
                return;
            }
            formatter.Open(header);
            if (!string.IsNullOrEmpty(Body))
            {
                var b = Body.Replace("\r\n", "\n").Split('\n');
                foreach (var line in b)
                    formatter.Writeln(line);
            }
            formatter.Close();
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

        public string Name { get; set; }
        public bool IsStatic { get; set; }
        public TsVisibility Visibility { get; set; }
        public List<TsMethodArgument> Arguments { get; set; } = new List<TsMethodArgument>();
        public string ResultType { get; set; }
        public string Body { get; set; }
    }
}