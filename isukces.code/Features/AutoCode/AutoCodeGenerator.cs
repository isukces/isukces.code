using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using iSukces.Code.IO;

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

        protected virtual void AfterCreateFile(CsFile file)
        {
            
        }

        public void Make(Assembly assembly)
        {
            var types = assembly.GetTypes();
            types = types.OrderBy(GetNamespace).ToArray();
            {
                var contextWrapper = GetContextWrapper(null);
                var context        = contextWrapper.Context;
                foreach (var i in CodeGenerators.OfType<IAssemblyAutoCodeGenerator>())
                    i.AssemblyStart(assembly, context);
            }

            for (int index = 0, length = types.Length; index < length; index++)
            {
                var type           = types[index];
                var file           = TypeBasedOutputProvider?.GetOutputFileInfo(type);
                var contextWrapper = GetContextWrapper(file);
                var context        = contextWrapper.Context;
                foreach (var i in CodeGenerators.OfType<IAutoCodeGenerator>())
                {
                    i.Generate(type, context);
                }
                if (context.AnyFileSaved)
                    AnyFileSaved = true;
            }

            foreach (var i in CodeGenerators.OfType<IAssemblyAutoCodeGenerator>())
            {
                var contextWrapper = GetContextWrapper(null);
                var context        = contextWrapper.Context;
                i.AssemblyEnd(assembly, context);
                if (context.AnyFileSaved)
                    AnyFileSaved = true;
            }

            var fileNameAssembly     = _filenameProvider.GetFilename(assembly).FullName;
            var eventHandler = BeforeSave;
            foreach (var (key, wrapper) in _outputs)
            {
                var info     = wrapper.SourceInfo;
                var csFile   = wrapper.File;
                var fileName = string.IsNullOrEmpty(key) ? fileNameAssembly : key;
                if (eventHandler != null)
                {
                    var args = new BeforeSaveEventArgs
                    {
                        File       = csFile,
                        FileName   = string.IsNullOrEmpty(key) ? fileName : key,
                        IsEmbedded = info.IsEmbedded
                    };
                    eventHandler(this, args);
                    fileName = args.FileName;
                }

                if (info.IsEmbedded)
                {
                    var sourceInfoFileName = info.FileName;
                    var c = File.Exists(sourceInfoFileName)
                        ? File.ReadAllText(sourceInfoFileName)
                        : "";
                    var allContent = EndCodeEmbedder.Append(c, csFile.GetCode(true),
                        info.EmbeddedFileDelimiter);
                    var saved = CodeFileUtils.SaveIfDifferent(allContent, sourceInfoFileName, false);
                    if (saved)
                        AnyFileSaved = true;
                }
                else
                {
                    if (csFile.SaveIfDifferent(fileName))
                        AnyFileSaved = true;
                }

                var context = wrapper.Context;
                if (context is IFinalizableAutoCodeGeneratorContext fin)
                    fin.FinalizeContext(assembly);
                if (context.AnyFileSaved)
                    AnyFileSaved = true;
            }
            _outputs.Clear();
            return;

            ContextWrapper GetContextWrapper(CsOutputFileInfo src)
            {
                if (src is null)
                {
                    var fn = _filenameProvider.GetFilename(assembly).FullName;
                    src = new CsOutputFileInfo(fn, false);
                }

                if (_outputs.TryGetValue(src.FileName, out var result))
                {
                    if (result.SourceInfo.IsEmbedded != src.IsEmbedded)
                        throw new Exception("File " + src.FileName + " is both embedded and not embedded");
                    return result;
                }

                result = new ContextWrapper(CreateAutoCodeGeneratorContext, src, assembly);
                foreach (var i in FileNamespaces)
                    result.File.AddImportNamespace(i);
                AfterCreateFile(result.File);
                _outputs[src.FileName] = result;
                return result;
            }
        }

        protected virtual IFinalizableAutoCodeGeneratorContext CreateAutoCodeGeneratorContext(CsFile file, Assembly assembly)
        {
            var context = new SimpleAutoCodeGeneratorContext(file, file.GetOrCreateClass);
            return context;
        }

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

        
        /// <summary>
        /// Allows to specify separate output cs file for some types
        /// </summary>
        public ICsOutputProvider TypeBasedOutputProvider { get; set; }

        private readonly Dictionary<string, ContextWrapper> _outputs = new Dictionary<string, ContextWrapper>(StringComparer.OrdinalIgnoreCase);
        
        public event EventHandler<BeforeSaveEventArgs> BeforeSave;

        public class BeforeSaveEventArgs : EventArgs
        {
            public CsFile File       { get; set; }
            public string FileName   { get; set; }
            public bool   IsEmbedded { get; set; }
        }
    }

 
}