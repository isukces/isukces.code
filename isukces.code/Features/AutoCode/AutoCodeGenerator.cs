using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using isukces.code.CodeWrite;
using isukces.code.interfaces;

namespace isukces.code.AutoCode
{
    public partial class AutoCodeGenerator : IConfigResolver
    {
        public AutoCodeGenerator()
        {
            CodeGenerators.AddRange(GetStandardGenerators());
        }

        private static string GetNamespace(Type type)
        {
            try
            {
                return type?.Namespace ?? "";
            }
            catch
            {
                return "";
            }
        }

        private static IEnumerable<IAutoCodeGenerator> GetStandardGenerators()
        {
            yield return new Generators.LazyGenerator();
            yield return new Generators.DependencyPropertyGenerator();
            yield return new CopyFromGenerator();

            yield return new Generators.ShouldSerializeGenerator();
            yield return new Generators.BuilderGenerator();

            yield return new Generators.ReactivePropertyGenerator();
            yield return new Generators.ReactiveCommandGenerator();
        }

        public void Make(Assembly assembly, string outFileName, ref bool saved)
        {
            if (BaseDir == null)
                throw new NullReferenceException(nameof(BaseDir));
            _csFile = new CsFile();
            foreach (var i in FileNamespaces)
                _csFile.AddImportNamespace(i);
            _classes = new Dictionary<Type, CsClass>();
            var types = assembly.GetTypes();
            types = types.OrderBy(GetNamespace).ToArray();
            for (int index = 0, length = types.Length; index < length; index++)
            {
                var type = types[index];
                IAutoCodeGeneratorContext context = new SimpleAutoCodeGeneratorContext(
                    GetOrCreateClass,
                    ns => _csFile.AddImportNamespace(ns),
                    ResolveConfigInternal
                );
                foreach (var i in CodeGenerators)
                    i.Generate(type, context);
            }
            var fileName = Path.Combine(BaseDir.FullName, outFileName);
            if (_csFile.SaveIfDifferent(fileName, false))
                saved = true;
        }

        public TConfig ResolveConfig<TConfig>() where TConfig : class, IAutoCodeConfiguration, new()
        {
            return (TConfig)ResolveConfigInternal(typeof(TConfig));
        }


        private CsClass GetOrCreateClass(Type type)
        {
            {
                CsClass c;
                if (_classes.TryGetValue(type, out c))
                    return c;
            }
            if (type.DeclaringType == null)
            {
                var a = _classes[type] = new CsClass(type.Name)
                {
                    IsPartial = true,
                    DotNetType = type,
                    Owner = _csFile,
                    Visibility = Visibilities.InterfaceDefault
                };
                var ns = _csFile.GetOrCreateNamespace(type.Namespace);
                ns.AddClass(a);
                return a;
            }
            var parent = GetOrCreateClass(type.DeclaringType);
            var existing = parent.GetOrCreateNested(type.Name);
            existing.IsPartial = true;
            existing.DotNetType = type;
            existing.Visibility = Visibilities.InterfaceDefault;
            return existing;
        }

        private object ResolveConfigInternal(Type type)
        {
            object value;
            if (_configs.TryGetValue(type, out value))
                return value;
            value = Activator.CreateInstance(type);
            _configs[type] = value;
            return value;
        }

        public List<IAutoCodeGenerator> CodeGenerators { get; } = new List<IAutoCodeGenerator>();

        public DirectoryInfo BaseDir { get; set; }


        public ISet<string> FileNamespaces { get; } = new HashSet<string>();

        private readonly Dictionary<Type, object> _configs = new Dictionary<Type, object>();

        private Dictionary<Type, CsClass> _classes;
        private CsFile _csFile;
    }
}