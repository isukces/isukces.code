using System.Collections.Generic;
using System.Globalization;

namespace isukces.code.Ammy
{
    public class AmmyBuilderContext
    {
        public AmmyBuilderContext(string mixinNamePrefix)
        {
            MixinNamePrefix = mixinNamePrefix;
        }

        public string GetFullMixinName(string shortName)
        {
            return MixinNamePrefix + shortName;
        }

        public MixinBuilder<T> RegisterMixin<T>(string name, bool globalName = false)
        {
            if (!globalName)
                name = GetFullMixinName(name);
            var m = new MixinBuilder<T>(name);
            _mixins.Add(m.WrappedMixin);
            return m;
        }

        public AmmyBuilderContext RegisterVariable(string name, int value, bool globalName = false)
        {
            return RegisterVariable(name, value.ToString(CultureInfo.InvariantCulture), globalName);
        }

        public AmmyBuilderContext RegisterVariable(string name, double value, bool globalName = false)
        {
            return RegisterVariable(name, value.ToString(CultureInfo.InvariantCulture), globalName);
        }

        public AmmyBuilderContext RegisterVariable(string name, string value, bool globalName = false)
        {
            if (!globalName)
                name = GetFullMixinName(name);
            var v = new AmmyVariableDefinition(name, value);
            _variables.Add(v);
            return this;
        }

        public override string ToString()
        {
            return $"{nameof(AmmyBuilderContext)} {Variables.Count} variable(s), {Mixins.Count} mixin(s)";
        }

        public IReadOnlyList<AmmyMixin>              Mixins          => _mixins;
        public IReadOnlyList<AmmyVariableDefinition> Variables       => _variables;
        public string                                MixinNamePrefix { get; }

        private readonly List<AmmyMixin> _mixins = new List<AmmyMixin>();
        private readonly List<AmmyVariableDefinition> _variables = new List<AmmyVariableDefinition>();
    }
}