using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using iSukces.Code.CodeWrite;
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
        
        public ICsOutputProvider ClassBased { get; set; }

        private Dictionary<string, ContextWrapper> _outputs = new Dictionary<string, ContextWrapper>(StringComparer.OrdinalIgnoreCase);


        public void Make(Assembly assembly)
        {
            ContextWrapper GetTarget(CsOutputFileInfo src)
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

                result = new ContextWrapper(CreateAutoCodeGeneratorContext, src);
                
                if (!src.IsEmbedded)
                {
                    foreach (var i in FileNamespaces)
                        result.File.AddImportNamespace(i);
                }

                _outputs[src.FileName] = result;
                return result;
            }
            
            
            
            
            var types = assembly.GetTypes();
            types = types.OrderBy(GetNamespace).ToArray();
            {
                var contextWrapper = GetTarget(null);
                var context        = contextWrapper.Context;
                foreach (var i in CodeGenerators.OfType<IAssemblyAutoCodeGenerator>())
                    i.AssemblyStart(assembly, context);
            }

            for (int index = 0, length = types.Length; index < length; index++)
            {
                var type = types[index];

                var file    = ClassBased?.GeOutputFileInfo(type);
                var contextWrapper = GetTarget(file);

                foreach (var i in CodeGenerators.OfType<IAutoCodeGenerator>())
                    i.Generate(type, contextWrapper.Context);
            }

            foreach (var i in CodeGenerators.OfType<IAssemblyAutoCodeGenerator>())
                i.AssemblyEnd(assembly, GetTarget(null).Context);

            var fileName     = _filenameProvider.GetFilename(assembly).FullName;
            var eventHandler = BeforeSave;
            foreach (var pair in _outputs)
            {
                // context = i.Context;
                var wrapper     = pair.Value;
                var info        = wrapper.SourceInfo;
                var csFile = wrapper.File;
                if (eventHandler != null)
                {
                    var args = new BeforeSaveEventArgs
                    {
                        File       = csFile,
                        FileName   = string.IsNullOrEmpty(pair.Key) ? fileName : pair.Key,
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
                    fin.Finalize();
                if (context.AnyFileSaved)
                    AnyFileSaved = true;
            }
        }

        protected virtual IFinalizableAutoCodeGeneratorContext CreateAutoCodeGeneratorContext(CsFile file)
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

        

        public event EventHandler<BeforeSaveEventArgs> BeforeSave;

        public class BeforeSaveEventArgs : EventArgs
        {
            public CsFile File       { get; set; }
            public string FileName   { get; set; }
            public bool   IsEmbedded { get; set; }
        }
    }

 
}