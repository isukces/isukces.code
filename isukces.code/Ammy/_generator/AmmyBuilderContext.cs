using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using iSukces.Code.Interfaces;
using iSukces.Code.Interfaces.Ammy;

namespace iSukces.Code.Ammy
{
    public class AmmyBuilderContext : INamespaceCollection, IAmmyNamespaceProvider
    {
        public AmmyBuilderContext(string mixinNamePrefix) => MixinNamePrefix = mixinNamePrefix;

        public void AddImportNamespace(string ns)
        {
            ns = ns?.Trim();
            if (string.IsNullOrEmpty(ns))
                throw new ArgumentException(nameof(ns));
            _namespaces.Add(ns);
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

        public string GetFullMixinName(string shortName) => MixinNamePrefix + shortName;

        public MixinBuilder<T> RegisterMixin<T>(string name, bool globalName = false, bool overwrite = false)
        {
            if (!globalName)
                name = GetFullMixinName(name);
            if (!overwrite)
            {
                var existing = _mixins.FirstOrDefault(a => a.Name == name);
                if (existing != null)
                {
                    if (existing.MixinTargetType != typeof(T))
                        throw new Exception(
                            $"Invalid type for mixin {name}. Requested type is {typeof(T)}, but found {existing.MixinTargetType}.");
                    return new MixinBuilder<T>(existing);
                }
            }

            var mixinBuilder = new MixinBuilder<T>(name);
            _mixins.Add(mixinBuilder.WrappedMixin);
            AfterAddMixin(mixinBuilder);
            return mixinBuilder;
        }

        public AmmyBuilderContext RegisterVariable(string name, int value, bool globalName = false) =>
            RegisterVariable(name, value.ToCsString(), globalName);

        public AmmyBuilderContext RegisterVariable(string name, double value, bool globalName = false) =>
            RegisterVariable(name, value.ToCsString(), globalName);

        public AmmyBuilderContext RegisterVariable(string name, string value, bool globalName = false)
        {
            if (!globalName)
                name = GetFullMixinName(name);
            var v = new AmmyVariableDefinition(name, value);
            _variables.Add(v);
            return this;
        }

        public override string ToString() =>
            $"{nameof(AmmyBuilderContext)} {Variables.Count} variable(s), {Mixins.Count} mixin(s)";


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

        public ISet<string> Namespaces
        {
            get { return _namespaces; }
        }

        private readonly List<AmmyMixin> _mixins = new List<AmmyMixin>();
        private readonly List<AmmyVariableDefinition> _variables = new List<AmmyVariableDefinition>();
        private readonly HashSet<string> _namespaces = new HashSet<string>();
    }
}