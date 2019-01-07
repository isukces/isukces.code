using System.Collections.Generic;

namespace isukces.code.Ammy
{
    public class AmmyBuilderContext
    {
        public AmmyBuilderContext(string prefix)
        {
            _prefix = prefix;
        }

        public IEnumerable<AmmyMixin> GetMixins()
        {
            return _mixins;
        }

        public MixinBuilder<T> RegisterMixin<T>(string name)
        {
            var m = new MixinBuilder<T>(_prefix + name);
            _mixins.Add(m.WrappedMixin);
            return m;
        }


        private readonly List<AmmyMixin> _mixins = new List<AmmyMixin>();
        private readonly string _prefix;
    }
}