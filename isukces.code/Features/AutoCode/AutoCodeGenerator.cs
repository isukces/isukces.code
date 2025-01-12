#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using iSukces.Code.IO;

// ReSharper disable UnusedMember.Global

namespace iSukces.Code.AutoCode;

public class FileSavedNotifierBase : IFileSavedNotifier
{
    public void FileSaved(object generator, string fileName)
    {
        OnFileSaved?.Invoke(this, new FileSavedEventArgs(generator, fileName));
        AnyFileSaved = true;
    }

    #region Properties

    public bool AnyFileSaved { get; private set; }

    #endregion

    public event EventHandler<FileSavedEventArgs>? OnFileSaved;
}

public partial class AutoCodeGenerator : FileSavedNotifierBase
{
    public AutoCodeGenerator(IAssemblyFilenameProvider filenameProvider)
    {
        _filenameProvider = filenameProvider;
        CodeGenerators.AddRange(GetStandardGenerators());
        TypeBasedOutputProvider = null;
    }

    private static string GetNamespace(Type? type)
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

    protected virtual void AfterCreateFile(CsFile file)
    {
    }

    protected virtual IFinalizableAutoCodeGeneratorContext CreateAutoCodeGeneratorContext(CsFile file,
        Assembly assembly)
    {
        var context = new SimpleAutoCodeGeneratorContext(file, file.GetOrCreateClass);
        return context;
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
            var csFileInfo     = TypeBasedOutputProvider?.GetOutputFileInfo(type);
            var contextWrapper = GetContextWrapper(csFileInfo);
            var context        = contextWrapper.Context;
            context.OnFileSaved += HandleNestedSave;
            foreach (var i in CodeGenerators.OfType<IAutoCodeGenerator>())
            {
                i.Generate(type, context);
            }
            context.OnFileSaved -= HandleNestedSave;
        }

        foreach (var i in CodeGenerators.OfType<IAssemblyAutoCodeGenerator>())
        {
            var contextWrapper = GetContextWrapper(null);
            var context        = contextWrapper.Context;
            context.OnFileSaved += HandleNestedSave;
            i.AssemblyEnd(assembly, context);
            context.OnFileSaved -= HandleNestedSave;
        }

        var fileNameAssembly = _filenameProvider.GetFilename(assembly).FullName;
        var eventHandler     = BeforeSave;
        foreach (var kv in _outputs)
        {
            var key      = kv.Key;
            var wrapper  = kv.Value;
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
                var saved = CodeFileUtils.SaveIfDifferent(allContent, sourceInfoFileName);
                if (saved)
                    FileSaved(GetType(), sourceInfoFileName);
            }
            else
            {
                if (csFile.SaveIfDifferent(fileName))
                    FileSaved(GetType(), fileName);
            }

            {
                var context = wrapper.Context;
                context.OnFileSaved += HandleNestedSave;
                if (context is IFinalizableAutoCodeGeneratorContext fin)
                    fin.FinalizeContext(assembly);

                context.OnFileSaved -= HandleNestedSave;

              
            }
        }

        _outputs.Clear();
        return;

        ContextWrapper GetContextWrapper(CsOutputFileInfo? src)
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

    private object ResolveConfigInternal(Type type)
    {
        if (_configs.TryGetValue(type, out var value))
            return value;
        value          = Activator.CreateInstance(type);
        _configs[type] = value ?? throw new Exception("Cannot create instance of " + type);
        return value;
    }
    
    
    void HandleNestedSave(object? sender, FileSavedEventArgs e)
    {
        FileSaved(e.Generator, e.FileName);
    }

    public AutoCodeGenerator WithGenerator(IAutoCodeGeneratorBase generator)
    {
        CodeGenerators.Add(generator);
        return this;
    }

    public AutoCodeGenerator WithGenerator<T>(Action<T>? configure = null)
        where T : IAutoCodeGeneratorBase, new()
    {
        var generator = new T();
        configure?.Invoke(generator);
        return WithGenerator(generator);
    }

    #region Properties

    public List<IAutoCodeGeneratorBase> CodeGenerators { get; } = new();

    
    [Obsolete("Use CsFileFactory.Instance.CreateCsFile event instead")]
    public ISet<string> FileNamespaces { get; } = new HashSet<string>();

    /// <summary>
    ///     Allows to specify separate output cs file for some types
    /// </summary>
    public ICsOutputProvider? TypeBasedOutputProvider { get; set; }

    #endregion

    public event EventHandler<BeforeSaveEventArgs>? BeforeSave;

    #region Fields

    private readonly IAssemblyFilenameProvider _filenameProvider;

    private readonly Dictionary<Type, object> _configs = new();

    private readonly Dictionary<string, ContextWrapper> _outputs = new(StringComparer.OrdinalIgnoreCase);

    #endregion


    public class BeforeSaveEventArgs : EventArgs
    {
#if NET8_0_OR_GREATER
        public required CsFile File       { get; init; }
        public required string FileName   { get; set; }
        public required bool   IsEmbedded { get; init; }
#else
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public CsFile File       { get; init; }
        public string FileName   { get; set; }
        public bool   IsEmbedded { get; init; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#endif
    }
}
