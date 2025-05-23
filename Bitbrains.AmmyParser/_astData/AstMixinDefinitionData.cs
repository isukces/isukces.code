#nullable disable
using Irony.Parsing;

namespace Bitbrains.AmmyParser
{
    public class AstMixinDefinitionData : IBaseData, IAstStatement
    {
        public AstMixinDefinitionData(SourceSpan span, string mixinName,
            AstObjectSettingsCollection objectSettings)
        {
            Span      = span;
            MixinName = mixinName;
            ObjectSettings = objectSettings;
        }

        public override string ToString()
        {
            return $"Mixin {MixinName}";
        }

        public SourceSpan Span      { get; }
        public string     MixinName { get; }
        public AstObjectSettingsCollection ObjectSettings { get; }
    }
}
