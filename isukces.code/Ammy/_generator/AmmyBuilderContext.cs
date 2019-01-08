using System.Collections.Generic;

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

        public IEnumerable<AmmyMixin> GetMixins()
        {
            return _mixins;
        }

        public MixinBuilder<T> RegisterMixin<T>(string shortName)
        {
            var m = new MixinBuilder<T>(GetFullMixinName(shortName));
            _mixins.Add(m.WrappedMixin);
            return m;
        }

        public string MixinNamePrefix { get; }

        private readonly List<AmmyMixin> _mixins = new List<AmmyMixin>();
    }
}