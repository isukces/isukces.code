using Irony.Parsing;

namespace Bitbrains.AmmyParser
{
    public class AstObjectPropertySettingData : IBaseData, IAstObjectSetting
    {
        public AstObjectPropertySettingData(string name, object value, SourceSpan span)
        {
            Name  = name;
            Value = value;
            Span  = span;
        }

        public override string ToString()
        {
            return $"{Span.Location} {Name} = {Value}";
        }

        public string     Name  { get; }
        public object     Value { get; }
        public SourceSpan Span  { get; }
    }
}