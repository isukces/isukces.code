#if AMMY
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using iSukces.Code.AutoCode;
using iSukces.Code.Interfaces;
using iSukces.Code.Interfaces.Ammy;
using iSukces.Code.IO;
using JetBrains.Annotations;

namespace iSukces.Code.Ammy
{
    public class AmmyAutocodeGenerator : IAutoCodeGenerator, IAssemblyAutoCodeGenerator
    {
        public AmmyAutocodeGenerator(IAssemblyFilenameProvider filenameProvider) =>
            _filenameProvider = filenameProvider;

        public void AssemblyEnd(Assembly assembly, IAutoCodeGeneratorContext context)
        {
            var fi   = _filenameProvider.GetFilename(assembly);
            var code = EmitCode(CodeParts, _writer);
            if (CodeFileUtils.SaveIfDifferent(code, fi.FullName, false))
                context.FileSaved(fi);

            foreach (var pair in _otherFiles)
                Embed(pair.Value.CodeParts, pair.Key, pair.Value);

            CodeParts = null;
            _writer   = null;
            _context  = null;
        }

        public void AssemblyStart(Assembly assembly, IAutoCodeGeneratorContext context)
        {
            _context  = context;
            CodeParts = new Dictionary<AmmyCodePartsKey, IAmmyCodePieceConvertible>();
            _writer   = new AmmyCodeWriter();
            AfterStartAssembly(assembly, _writer, context);
        }

        public virtual void Generate(Type type, IAutoCodeGeneratorContext context)
        {
            var methods = type.GetTypeInfo().GetMethods(BindingFlags.Static | BindingFlags.Public);
            foreach (var method in methods)
                UseAmmyBuilderAttribute(type, method);
        }

        protected virtual void AfterStartAssembly(Assembly assembly, AmmyCodeWriter writer,
            IAutoCodeGeneratorContext context)
        {
        }

        /// <summary>
        ///     Creates context for Ammy builders
        /// </summary>
        /// <param name="prefix">mixin name prefix</param>
        /// <param name="type">type that contains building method</param>
        /// <returns></returns>
        protected virtual AmmyBuilderContext CreateAmmyBuilderContext(string prefix, [CanBeNull] Type type) =>
            new AmmyBuilderContext(prefix);

        private void Embed(Dictionary<AmmyCodePartsKey, IAmmyCodePieceConvertible> cp, string ctxEmbedFileName,
            [CanBeNull] IAmmyNamespaceProvider provider)
        {
            var readedFromFile = File.Exists(ctxEmbedFileName)
                ? File.ReadAllText(ctxEmbedFileName)
                : string.Empty;

            var writer = new AmmyCodeWriter();
            if (provider?.Namespaces != null)
                foreach (var i in provider.Namespaces)
                    writer.Namespaces.Add(i);
            var code = EmitCode(cp, writer);

            var result = CodeEmbeder.Embed(readedFromFile, code);

            var fi = new FileInfo(ctxEmbedFileName);
            if (CodeFileUtils.SaveIfDifferent(result, fi.FullName, false))
                _context.FileSaved(fi);
        }

        private string EmitCode(Dictionary<AmmyCodePartsKey, IAmmyCodePieceConvertible> cp, AmmyCodeWriter writer)
        {
            if (cp == null || cp.Count == 0)
                return string.Empty;
            var ctx = new ConversionCtx(writer);
            {
                var h = ResolveSeparateLines;
                if (h != null)
                    ctx.OnResolveSeparateLines += (a, b) => h.Invoke(this, b);
            }
            foreach (var i in cp.OrderBy(a => a.Key))
                i.Value.WriteLineTo(writer, ctx);

            return writer.FullCode;
        }

        private void UseAmmyBuilderAttribute(Type type, MethodInfo method)
        {
            var at = method.GetCustomAttribute<AmmyBuilderAttribute>();
            if (at == null) return;
            var prefix       = type.Name + "_";
            var eventHandler = CreateMixinPrefix;
            if (eventHandler != null)
            {
                var args = new CreateMixinPrefixEventArgs {Prefix = prefix};
                eventHandler.Invoke(this, args);
                prefix = args.Prefix;
            }

            var ctx = CreateAmmyBuilderContext(prefix, type);
            method.Invoke(null, new object[] {ctx});
            var cp = CodeParts;
            if (!string.IsNullOrEmpty(ctx.EmbedFileName))
            {
                if (!_otherFiles.TryGetValue(ctx.EmbedFileName, out var info))
                    _otherFiles[ctx.EmbedFileName] = info = new EmbeddedInfo();
                cp = info.CodeParts;
                foreach (var i in ctx.Namespaces)
                    info.AddImportNamespace(i);
            }

#if AMMY
            foreach (var mixin in ctx.Mixins)
                cp.AddMixin(mixin);
#endif
            foreach (var variableDefinition in ctx.Variables)
                cp.AddVariable(variableDefinition);
        }

        protected Dictionary<AmmyCodePartsKey, IAmmyCodePieceConvertible> CodeParts { get; private set; }

        private readonly Dictionary<string, EmbeddedInfo> _otherFiles =
            new Dictionary<string, EmbeddedInfo>(StringComparer.OrdinalIgnoreCase);

        private AmmyCodeWriter _writer;
        private readonly IAssemblyFilenameProvider _filenameProvider;
        private IAutoCodeGeneratorContext _context;
        public event EventHandler<ConversionCtx.ResolveSeparateLinesEventArgs> ResolveSeparateLines;
        public event EventHandler<CreateMixinPrefixEventArgs>                  CreateMixinPrefix;

        private class EmbeddedInfo : INamespaceCollection, IAmmyNamespaceProvider
        {
            public EmbeddedInfo()
            {
                CodeParts  = new Dictionary<AmmyCodePartsKey, IAmmyCodePieceConvertible>();
                Namespaces = new HashSet<string>();
            }

            public void AddImportNamespace(string ns)
            {
                ns = ns?.Trim();
                if (string.IsNullOrEmpty(ns))
                    throw new ArgumentException(nameof(ns));
                Namespaces.Add(ns);
            }

            [NotNull]
            public Dictionary<AmmyCodePartsKey, IAmmyCodePieceConvertible> CodeParts { get; }


            public ISet<string> Namespaces { get; } = new HashSet<string>();
        }

        public class CreateMixinPrefixEventArgs : EventArgs
        {
            public string Prefix { get; set; }
        }
    }


    public enum AmmyCodePartsKeyKind
    {
        Variable,
        Mixin,
        Alias
    }
}
#endif