using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using iSukces.Code.CodeWrite;
using iSukces.Code.Interfaces;

namespace iSukces.Code.AutoCode
{
    public partial class AutoCodeGenerator  
    {
        public AutoCodeGenerator(IAssemblyFilenameProvider filenameProvider)
        {
            _filenameProvider = filenameProvider;
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

        public void Make<T>()
        {
            var assembly = typeof(T)
#if COREFX
                .GetTypeInfo()
#endif
                .Assembly;
            Make(assembly);
        }

        public void Make(Assembly assembly)
        {
            _csFile = new CsFile();
            foreach (var i in FileNamespaces)
                _csFile.AddImportNamespace(i);
            _classes = new Dictionary<TypeProvider, CsClass>();
            var types = assembly.GetTypes();
            types = types.OrderBy(GetNamespace).ToArray();
            var context = CreateAutoCodeGeneratorContext(_csFile);
            foreach (var i in CodeGenerators.OfType<IAssemblyAutoCodeGenerator>())
                i.AssemblyStart(assembly, context);

            for (int index = 0, length = types.Length; index < length; index++)
            {
                var type = types[index];
                foreach (var i in CodeGenerators.OfType<IAutoCodeGenerator>()) 
                    i.Generate(type, context);
            }

            foreach (var i in CodeGenerators.OfType<IAssemblyAutoCodeGenerator>())
                i.AssemblyEnd(assembly, context);

            var fileName     = _filenameProvider.GetFilename(assembly).FullName;
            var eventHandler = BeforeSave;
            if (eventHandler != null)
            {
                var args = new BeforeSaveEventArgs
                {
                    File     = _csFile,
                    FileName = fileName
                };
                eventHandler(this, args);
                fileName = args.FileName;
            }

            if (_csFile.SaveIfDifferent(fileName))
                AnyFileSaved = true;
            
            context.Finalize();
            
            if (context.AnyFileSaved)
                AnyFileSaved = true;
        }

        protected virtual IFinalizableAutoCodeGeneratorContext CreateAutoCodeGeneratorContext(CsFile file)
        {
            var context = new SimpleAutoCodeGeneratorContext(file, GetOrCreateClass);
            return context;
        }

        /*public TConfig ResolveConfig<TConfig>() where TConfig : class, IAutoCodeConfiguration, new()
        {
            return (TConfig)ResolveConfigInternal(typeof(TConfig));
        }*/

        public AutoCodeGenerator WithGenerator(IAutoCodeGeneratorBase generator)
        {
            CodeGenerators.Add(generator);
            return this;
        }

        public AutoCodeGenerator WithGenerator<T>(Action<T> configure = null)
            where T : IAutoCodeGeneratorBase, new()
        {
            var generator = new T();
            configure?.Invoke(generator);
            return WithGenerator(generator);
        }

        protected CsClass GetOrCreateClass(TypeProvider type)
        {
            // have to be protected due to CreateAutoCodeGeneratorContext method 
            return _csFile.GetOrCreateClass(type, _classes);
        }

        private object ResolveConfigInternal(Type type)
        {
            if (_configs.TryGetValue(type, out var value))
                return value;
            value          = Activator.CreateInstance(type);
            _configs[type] = value;
            return value;
        }

        public bool AnyFileSaved { get; set; }

        public List<IAutoCodeGeneratorBase> CodeGenerators { get; } = new List<IAutoCodeGeneratorBase>();

        public ISet<string> FileNamespaces { get; } = new HashSet<string>();
        private readonly IAssemblyFilenameProvider _filenameProvider;

        private readonly Dictionary<Type, object> _configs = new Dictionary<Type, object>();

        private Dictionary<TypeProvider, CsClass> _classes;
        private CsFile _csFile;

        public event EventHandler<BeforeSaveEventArgs> BeforeSave;

        public class BeforeSaveEventArgs : EventArgs
        {
            public CsFile File     { get; set; }
            public string FileName { get; set; }
        }
    }
}