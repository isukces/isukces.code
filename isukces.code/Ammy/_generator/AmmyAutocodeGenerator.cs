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
            CodeParts[AmmyCodePartsKey.Mixin(mixin.Name)] = mixin;
        }

        public void AddVariable(string name, string value)
        {
            AddVariable(new AmmyVariableDefinition(name, value));
        }

        public void AddVariable(AmmyVariableDefinition variableDefinition)
        {
            CodeParts[new AmmyCodePartsKey(AmmyCodePartsKeyKind.Variable, variableDefinition.Name)] =
                variableDefinition;
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
            {
                context.FileSaved(fi);
                AfterSave?.Invoke(this, EventArgs.Empty);
            }

            CodeParts = null;
            _writer   = null;
        }

        public void AssemblyStart(Assembly assembly, IAutoCodeGeneratorContext context)
        {
            CodeParts = new Dictionary<AmmyCodePartsKey, IAmmyCodePieceConvertible>();
            _writer   = new AmmyCodeWriter();
            AfterStartAssembly(assembly, _writer);
        }

        public virtual void Generate(Type type, IAutoCodeGeneratorContext context)
        {
            var methods = type.GetTypeInfo().GetMethods(BindingFlags.Static | BindingFlags.Public);
            foreach (var method in methods)
                UseAmmyBuilderAttribute(type, method);
        }

        protected virtual void AfterStartAssembly(Assembly assembly, AmmyCodeWriter writer)
        {
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


        protected Dictionary<AmmyCodePartsKey, IAmmyCodePieceConvertible> CodeParts { get; private set; }

        private AmmyCodeWriter _writer;

        private readonly Func<Assembly, FileInfo> _getFileLocation;

        public event EventHandler AfterSave;

        public event EventHandler<ConversionCtx.ResolveSeparateLinesEventArgs> ResolveSeparateLines;
        public event EventHandler<CreateMixinPrefixEventArgs>                  CreateMixinPrefix;


        public class CreateMixinPrefixEventArgs : EventArgs
        {
            public string Prefix { get; set; }
        }
    }

    public struct AmmyCodePartsKey : IEquatable<AmmyCodePartsKey>, IComparable<AmmyCodePartsKey>, IComparable
    {
        public AmmyCodePartsKey(AmmyCodePartsKeyKind kind, string name)
        {
            Kind = kind;
            Name = name;
        }

        public static AmmyCodePartsKey Mixin(string mixinName)
        {
            return new AmmyCodePartsKey(AmmyCodePartsKeyKind.Mixin, mixinName);
        }

        public static bool operator ==(AmmyCodePartsKey left, AmmyCodePartsKey right)
        {
            return left.Equals(right);
        }

        public static bool operator >(AmmyCodePartsKey left, AmmyCodePartsKey right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(AmmyCodePartsKey left, AmmyCodePartsKey right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static bool operator !=(AmmyCodePartsKey left, AmmyCodePartsKey right)
        {
            return !left.Equals(right);
        }

        public static bool operator <(AmmyCodePartsKey left, AmmyCodePartsKey right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(AmmyCodePartsKey left, AmmyCodePartsKey right)
        {
            return left.CompareTo(right) <= 0;
        }

        public int CompareTo(AmmyCodePartsKey other)
        {
            var kindComparison = Kind.CompareTo(other.Kind);
            if (kindComparison != 0) return kindComparison;
            return string.Compare(Name, other.Name, StringComparison.Ordinal);
        }

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            return obj is AmmyCodePartsKey other
                ? CompareTo(other)
                : throw new ArgumentException($"Object must be of type {nameof(AmmyCodePartsKey)}");
        }

        public bool Equals(AmmyCodePartsKey other)
        {
            return Kind == other.Kind && string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            return obj is AmmyCodePartsKey other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int)Kind * 397) ^ (Name != null ? Name.GetHashCode() : 0);
            }
        }

        public AmmyCodePartsKeyKind Kind { get; }
        public string               Name { get; }
    }

    public enum AmmyCodePartsKeyKind
    {
        Variable,
        Mixin,
        Alias
    }
}