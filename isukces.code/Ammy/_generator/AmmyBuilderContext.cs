using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;

namespace isukces.code.Ammy
{
    public class AmmyBuilderContext
    {
        public AmmyBuilderContext(string mixinNamePrefix)
        {
            MixinNamePrefix = mixinNamePrefix;
        }


        public void EmbedInRelativeFile(string shortFileName, [CallerFilePath] string csFile = null)
        {
            if (csFile is null)
                throw new ArgumentNullException(nameof(csFile));
            if (string.IsNullOrEmpty(csFile))
                throw new ArgumentException(nameof(csFile));
            var fi = new FileInfo(csFile);
            if (string.IsNullOrEmpty(shortFileName))
            {
                var shortName = fi.Name;
                shortFileName = shortName.Substring(0, shortName.Length - fi.Extension.Length);
                shortFileName = shortFileName.Split('.')[0];
            }

            var fi2 = new FileInfo(Path.Combine(fi.Directory.FullName, shortFileName));
            EmbedFileName = fi2.FullName;
            if (string.IsNullOrEmpty(fi2.Extension))
                EmbedFileName += ".ammy";
        }

        public void EmbedInRelativeFile([CallerFilePath] string csFile = null)
        {
            EmbedInRelativeFile(null, csFile);
        }

        public string GetFullMixinName(string shortName)
        {
            return MixinNamePrefix + shortName;
        }

        public MixinBuilder<T> RegisterMixin<T>(string name, bool globalName = false)
        {
            if (!globalName)
                name = GetFullMixinName(name);
            var mixinBuilder = new MixinBuilder<T>(name);
            _mixins.Add(mixinBuilder.WrappedMixin);
            AfterAddMixin(mixinBuilder);
            return mixinBuilder;
        }

        public AmmyBuilderContext RegisterVariable(string name, int value, bool globalName = false)
        {
            return RegisterVariable(name, value.ToCsString(), globalName);
        }

        public AmmyBuilderContext RegisterVariable(string name, double value, bool globalName = false)
        {
            return RegisterVariable(name, value.ToCsString(), globalName);
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

        protected virtual void AfterAddMixin<T>(MixinBuilder<T> mixinBuilder)
        {
        }

        public IReadOnlyList<AmmyMixin> Mixins
        {
            get { return _mixins; }
        }

        public IReadOnlyList<AmmyVariableDefinition> Variables
        {
            get { return _variables; }
        }

        public string MixinNamePrefix { get; }

        public string EmbedFileName { get; set; }

        private readonly List<AmmyMixin> _mixins = new List<AmmyMixin>();
        private readonly List<AmmyVariableDefinition> _variables = new List<AmmyVariableDefinition>();
    }
}