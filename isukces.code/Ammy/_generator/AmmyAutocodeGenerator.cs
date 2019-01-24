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

        public void AddMixin(AmmyMixin mixin)
        {
            CodeParts["b MIXIN:" + mixin.Name] = mixin;
        }

        public void AddVariable(string name, string value)
        {
            AddVariable(new AmmyVariableDefinition(name, value));
        }

        public void AssemblyEnd(Assembly assembly, IAutoCodeGeneratorContext context)
        {
            if (CodeParts.Count == 0)
            {
                CodeParts = null;
                return;
            }

            var fi = _getFileLocation(assembly);

            var ctx = new ConversionCtx(_writer);
            {
                var h = ResolveSeparateLines;
                if (h != null)
                    ctx.OnResolveSeparateLines += (a, b) => h.Invoke(this, b);
            }
            foreach (var i in CodeParts.OrderBy(a => a.Key))
                i.Value.WriteLineTo(_writer, ctx);

            var txt = _writer.FullCode;
            if (CodeFileUtils.SaveIfDifferent(txt, fi.FullName, false))
                AfterSave?.Invoke(this, EventArgs.Empty);
            CodeParts = null;
            _writer   = null;
        }

        public void AssemblyStart(Assembly assembly, IAutoCodeGeneratorContext context)
        {
            CodeParts = new Dictionary<string, IAmmyCodePieceConvertible>();
            _writer   = new AmmyCodeWriter();
            StartAssembly?.Invoke(this, new StartAssemblyEventArgs
            {
                Assembly   = assembly,
                CodeWriter = _writer
            });
        }

        public virtual void Generate(Type type, IAutoCodeGeneratorContext context)
        {
            var methods = type.GetTypeInfo().GetMethods(BindingFlags.Static | BindingFlags.Public);
            foreach (var method in methods)
                UseAmmyBuilderAttribute(type, method);
        }

        private void AddVariable(AmmyVariableDefinition variableDefinition)
        {
            CodeParts["a VARIABLE " + variableDefinition.Name] = variableDefinition;
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
            foreach (var mixin in ctx.Mixins)
                AddMixin(mixin);
            foreach (var variableDefinition in ctx.Variables)
                AddVariable(variableDefinition);
        }


        protected Dictionary<string, IAmmyCodePieceConvertible> CodeParts { get; private set; }

        private AmmyCodeWriter _writer;

        private readonly Func<Assembly, FileInfo> _getFileLocation;

        public event EventHandler AfterSave;

        public event EventHandler<ConversionCtx.ResolveSeparateLinesEventArgs> ResolveSeparateLines;
        public event EventHandler<CreateMixinPrefixEventArgs>                  CreateMixinPrefix;
        public event EventHandler<StartAssemblyEventArgs>                      StartAssembly;

        public class StartAssemblyEventArgs
        {
            public Assembly       Assembly   { get; set; }
            public AmmyCodeWriter CodeWriter { get; set; }
        }

        public class CreateMixinPrefixEventArgs : EventArgs
        {
            public string Prefix { get; set; }
        }
    }
}