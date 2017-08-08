using System.Collections.Generic;

namespace isukces.code.Typescript
{
    public class TsField : TsMethodArgument, ITsClassMember
    {
        public TsField()
        {
        }

        public TsField(string name)
        {
            Name = name;
        }

        public void WriteCodeTo(TsWriteContext cf)
        {
            var line = string.Join(" ", GetHeaderItems());
            cf.Formatter.Writeln(line + ";");
        }

        private IEnumerable<string> GetHeaderItems()
        {
            if (Visibility != TsVisibility.Default)
                yield return Visibility.ToString().ToLower();
            if (IsStatic)
                yield return "static";
            yield return Name;
            if (!string.IsNullOrEmpty(Initializer))
            {
                yield return "=";
                yield return Initializer;
            }
        }

        public bool IsStatic { get; set; }
        public TsVisibility Visibility { get; set; }
        public string Initializer { get; set; }
    }
}