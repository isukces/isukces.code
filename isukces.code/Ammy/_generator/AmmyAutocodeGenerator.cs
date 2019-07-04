using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using isukces.code.AutoCode;
using isukces.code.interfaces.Ammy;
using isukces.code.IO;

namespace isukces.code.Ammy
{
    public class AmmyAutocodeGenerator : IAutoCodeGenerator, IAssemblyAutoCodeGenerator
    {
        public AmmyAutocodeGenerator(IAssemblyFilenameProvider filenameProvider)
        {
            _filenameProvider = filenameProvider;
        }

        public void AssemblyEnd(Assembly assembly, IAutoCodeGeneratorContext context)
        {
            var fi   = _filenameProvider.GetFilename(assembly);
            var code = EmitCode(CodeParts, _writer);
            if (CodeFileUtils.SaveIfDifferent(code, fi.FullName, false))
                context.FileSaved(fi);

            foreach (var pair in _otherFiles) Embed(pair.Value.CodeParts, pair.Key);

            CodeParts = null;
            _writer   = null;
            _context = null;
        }

        public void AssemblyStart(Assembly assembly, IAutoCodeGeneratorContext context)
        {
            _context = context;
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

        private void Embed(Dictionary<AmmyCodePartsKey, IAmmyCodePieceConvertible> cp, string ctxEmbedFileName)
        {
            var readedFromFile = File.Exists(ctxEmbedFileName)
                ? File.ReadAllText(ctxEmbedFileName)
                : string.Empty;

            var code = EmitCode(cp, new AmmyCodeWriter());

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

            var ctx = new AmmyBuilderContext(prefix);
            method.Invoke(null, new object[] {ctx});
            var cp = CodeParts;
            if (!string.IsNullOrEmpty(ctx.EmbedFileName))
            {
                if (!_otherFiles.TryGetValue(ctx.EmbedFileName, out var info))
                    _otherFiles[ctx.EmbedFileName] = info = new EmbeddedInfo();
                cp = info.CodeParts;
            }

            foreach (var mixin in ctx.Mixins)
                cp.AddMixin(mixin);
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

        private class EmbeddedInfo
        {
            public EmbeddedInfo()
            {
                CodeParts = new Dictionary<AmmyCodePartsKey, IAmmyCodePieceConvertible>();
            }

            public Dictionary<AmmyCodePartsKey, IAmmyCodePieceConvertible> CodeParts { get; }
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