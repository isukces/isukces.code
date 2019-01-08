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
        public AmmyAutocodeGenerator(Func<Assembly, FileInfo> getFileLocation)
        {
            _getFileLocation = getFileLocation;
        }

        public void AssemblyEnd(Assembly assembly, IAutoCodeGeneratorContext context)
        {
            if (CodeParts.Count == 0)
            {
                CodeParts = null;
                return;
            }

            var fi   = _getFileLocation(assembly);
            var writ = new AmmyCodeWriter();
            InitCodeWriter?.Invoke(this, new InitCodeWriterEventArgs
            {
                CodeWriter = writ
            });

            var ctx = new ConversionCtx(writ);
            {
                var h = OnResolveSeparateLines;
                if (h != null)
                    ctx.OnResolveSeparateLines += (a, b) => h.Invoke(this, b);
            }
            foreach (var i in CodeParts.OrderBy(a => a.Key))
                i.Value.WriteLineTo(writ, ctx);

            var txt = writ.FullCode;
            if (CodeFileUtils.SaveIfDifferent(txt, fi.FullName, false)) 
                AfterSave?.Invoke(this, EventArgs.Empty);
            CodeParts = null;
        }

        public event EventHandler AfterSave;
        
        public void AssemblyStart(Assembly assembly, IAutoCodeGeneratorContext context)
        {
            CodeParts = new Dictionary<string, IAmmyCodePieceConvertible>();
        }

        public virtual void Generate(Type type, IAutoCodeGeneratorContext context)
        {
            var methods = type.GetTypeInfo().GetMethods(BindingFlags.Static | BindingFlags.Public);
            foreach (var method in methods)
                UseAmmyBuilderAttribute(type, method);
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
            foreach (var mixin in ctx.GetMixins())
                CodeParts["b MIXIN:" + mixin.Name] = mixin;
        }

        protected Dictionary<string, IAmmyCodePieceConvertible> CodeParts { get; private set; }

        private readonly Func<Assembly, FileInfo> _getFileLocation;

        public event EventHandler<ConversionCtx.ResolveSeparateLinesEventArgs> OnResolveSeparateLines;

        public event EventHandler<InitCodeWriterEventArgs>    InitCodeWriter;
        public event EventHandler<CreateMixinPrefixEventArgs> CreateMixinPrefix;

        public class CreateMixinPrefixEventArgs : EventArgs
        {
            public string Prefix { get; set; }
        }

        public class InitCodeWriterEventArgs : EventArgs
        {
            public AmmyCodeWriter CodeWriter { get; set; }
        }
    }
}