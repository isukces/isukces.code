using System.Text;

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
            var line = GetHeaderItems(cf.Flags.HasFlag(TsWriteContextFlags.HeadersOnly));
            cf.Formatter.Writeln(line + ";");
        }

        private string GetHeaderItems(bool headerOnly)
        {
            var sb = new StringBuilder();
            if (Visibility != TsVisibility.Default)
                sb.Append(Visibility.ToString().ToLower());
            if (IsStatic)
                sb.Append(" static");
            sb.Append(" " + Name);
            if (!string.IsNullOrEmpty(Type))
                sb.Append(": " + Type);
            if (!string.IsNullOrEmpty(Initializer) && !headerOnly)
            {
                sb.Append(" = " + Initializer);
            }
            return sb.ToString().Trim();
        }

        public bool IsStatic { get; set; }
        public TsVisibility Visibility { get; set; }
        public string Initializer { get; set; }
    }
}