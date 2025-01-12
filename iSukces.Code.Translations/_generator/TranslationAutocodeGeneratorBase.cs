using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using iSukces.Code.AutoCode;
using iSukces.Code.Interfaces;
using iSukces.Translation;
using JetBrains.Annotations;

namespace iSukces.Code.Translations;

public abstract class TranslationAutocodeGeneratorBase:FileSavedNotifierBase
{
    protected TranslationAutocodeGeneratorBase(IAssemblyFilenameProvider filenameProvider)
    {
        _filenameProvider = filenameProvider;
    }

    private static Assembly GetProxyTypeAssembly(ITranslationProxyCreateRequest a)
    {
        return a.ProxyType?.Assembly ?? TranslationAutocodeConfig.Instance.DefaultProxyType.Assembly;
    }

    private static Type GetRequestCreationType(object value)
    {
        switch (value)
        {
            case ITranslationProxyCreateRequest a1:
                return a1.ProxyType ?? TranslationAutocodeConfig.Instance.DefaultProxyType;
            case ICreateLiteLocalTextRequest a2:
                return a2.FieldHostingType;
            default:
                return null;
        }
    }

    /// <summary>
    ///     Dodaje ApplyNewTranslations
    /// </summary>
    /// <param name="c"></param>
    private void Create_SetupMethod(CsClass c)
    {
        var cw     = CsCodeWriter.Create<TranslationAutocodeGeneratorBase>();
        var config = TranslationAutocodeConfig.Instance;
        if (config.GetInstanceHolderInstanceCsExpression is null)
            throw new NullReferenceException("TranslationAutocodeConfig.Instance.GetInstanceHolderInstanceCsExpression is not set");
        var instance = config.GetInstanceHolderInstanceCsExpression(c);
        var declare  = string.Format("var tr = {0}.Translations;", instance);

        cw.WriteLine(declare);

        cw.WriteLine("// ITranslationUpdateTargetRequest processing");
        var byKey = config.Requests.OfType<ITranslationUpdateTargetRequest>()
            .GroupBy(a => a.Key)
            .OrderBy(a => a.Key);

        var needDeclareVariable = true;
        foreach (var singleKeyTranslations in byKey)
        {
            var translations = singleKeyTranslations
                .OrderBy(a => a.Key)
                .ToArray();
            var firstConsumer = translations[0];
            if (needDeclareVariable)
            {
                cw.WritelineNoIndent("#pragma warning disable 168");
                cw.WriteLine("string text;");
                cw.WritelineNoIndent("#pragma warning restore 168");
                needDeclareVariable = false;
            }

            const string variable = "out text";

            const string method                   = "tr.TryGetValue(";
            var          condition                = $"{method}{singleKeyTranslations.Key.CsEncode()}, {variable})";
            var          currentCompilerDirective = "";

            void Check(ITranslationRequest? key1, bool open)
            {
                if (!open)
                {
                    if (!string.IsNullOrEmpty(currentCompilerDirective))
                    {
                        cw.WriteLine("#endif");
                        currentCompilerDirective = "";
                    }

                    return;
                }

                var compilerDirective = GetCompilerDirectiveByTranslationKey(key1?.Key ?? "") ?? "";
                if (compilerDirective == currentCompilerDirective)
                    return;
                Check(null, false);

                if (string.IsNullOrEmpty(compilerDirective))
                    return;
                currentCompilerDirective = compilerDirective;
                cw.WriteLine("#if " + compilerDirective);
            }

            if (translations.Length == 1)
            {
                Check(translations[0], true);
                {
                    var statement = $"{firstConsumer.GetTarget(c)} = text;";
                    cw.SingleLineIf(condition, statement);
                }
                Check(null, false);
            }
            else
            {
                cw.Open("if (" + condition + ")");
                {
                    foreach (var info in translations)
                    {
                        Check(info, true);
                        cw.WriteLine($"{info.GetTarget(c)} = text;");
                    }

                    Check(null, false);
                }
                cw.Close();
            }
        }

        c.AddMethod("ApplyNewTranslations", CsType.Void)
            .WithVisibility(Visibilities.Private)
            .WithStatic()
            .WithBody(cw);
    }

    protected virtual void CreateInitCode(Assembly assembly, CsFile csFile)
    {
    }

    [UsedImplicitly]
    public void Generate(TranslationAutocodeGeneratorConfig cfg)
    {
        var configs = TranslationAutocodeConfig.Instance;

        if (configs.GetRequests is not null)
            foreach (var i in configs.GetRequests())
                configs.RequestsAdd(i);

        var f  = CsFileFactory.Instance.Create(typeof(TranslationAutocodeGeneratorBase));
        var ns = f.GetOrCreateNamespace(cfg.InitType.Namespace);
        var c  = ns.GetOrCreateClass((CsType)cfg.InitType.Name);
        c.IsPartial = true;
        Seal(c);
        Create_SetupMethod(c);

        var assemblies = configs.Requests
            .Select(a => GetRequestCreationType(a)?.Assembly)
            .Where(a => a != null)
            .Distinct()
            .ToHashSet();
        assemblies.Add(typeof(TranslationAutocodeGeneratorBase).Assembly);

        // GenerateCommonSources(dir, null, initTranslationRequests);
        var orderedAssemblies = assemblies
            .OrderBy(a => TranslationAutocodeConfig.Instance.IsMainAssembly(a) ? 999 : 1)
            .ToArray();
        foreach (var assembly in orderedAssemblies)
            GenerateCommonSources(assembly);

        const string filename  = "translations-forms.json";
        const string filename2 = "translations-wpf.json";

        SaveTranslations(cfg, filename, filename2, FileSaved);

        var csFile = Path.Combine(cfg.InitTypeDir.FullName, cfg.InitType.Name + ".Auto.cs");
        if (f.SaveIfDifferent(csFile))
            FileSaved(GetType(), filename);
    }
        
    protected virtual void TranslationFieldCreated(string fieldName, CsClass csClass, Type type,
        string sourceTextToTranslate)
    {
            
    }
    
    private void GenerateCommonSources(Assembly assembly)
    {
        if (assembly == null) throw new ArgumentNullException(nameof(assembly));
        var translationProxyCreateRequestsForAssembly
            = TranslationAutocodeConfig.Instance.Requests
                .OfType<ITranslationProxyCreateRequest>()
                .Where(a => assembly == GetProxyTypeAssembly(a))
                .ToList();

        var createLiteLocalTextRequestForAssembly
            = TranslationAutocodeConfig.Instance.Requests
                .OfType<ICreateLiteLocalTextRequest>()
                .Where(a => a.FieldHostingType.Assembly == assembly)
                .ToList();

        var csFile = CsFileFactory.Instance.Create(typeof(TranslationAutocodeGeneratorBase));
        csFile.BeginContent = ("#pragma warning disable CA1416\r\n"+csFile.BeginContent).Trim();
        {
            if (assembly == typeof(TranslationAutocodeGeneratorBase).Assembly)
                csFile.AddImportNamespace(
                    TranslationAutocodeConfig.Instance.CommonTranslations,
                    typeof(StaticPropertyReference)
                );
            csFile.AddImportNamespace(
                typeof(CreateLiteLocalTextSourcesRequest),
                typeof(IEnumerable<object>),
                typeof(LiteLocalTextSource)
            );
        }

        var qqq1 = translationProxyCreateRequestsForAssembly
            .GroupBy(a => a.ProxyType ?? TranslationAutocodeConfig.Instance.DefaultProxyType)
            .ToDictionary(a => a.Key, a => a.ToList());

        var qqq2 = createLiteLocalTextRequestForAssembly.GroupBy(a => a.FieldHostingType)
            .ToDictionary(a => a.Key, a => a.ToList());

        var types = qqq1.Keys.Concat(qqq2.Keys).Distinct();
        foreach (var type in types)
        {
            if (type is null)
                continue;
            var shouldProcess = TranslationAutocodeConfig.Instance.AutocodeAssemblies?.ShouldProcessType(type) ?? false;
            if (!shouldProcess)
                continue;
            var csClass = csFile.GetOrCreateClass(TypeProvider.FromType(type));
            csClass.IsPartial = true;
            // AutocodeTools.Seal(csClass);
            csClass.Visibility = Visibilities.Public;

            if (qqq1.TryGetValue(type, out var list1))
                TranslationProxyGenerator.GenerateProxyProperties(csClass, list1,
                    type != TranslationAutocodeConfig.Instance.TranslationHolder, type);

            if (qqq2.TryGetValue(type, out var list2))
                LiteLocalTextSourceGenerator.Create(csClass, list2, (f, w) =>
                {
                    TranslationFieldCreated(f, csClass, type, w);
                });
        }

        CreateInitCode(assembly, csFile);

        var assemblyFilenameProvider = _filenameProvider.SureNotNull(nameof(_filenameProvider));
        var dir                      = assemblyFilenameProvider.GetFilename(assembly).Directory.SureNotNull("Directory");
        var filename                 = Path.Combine(dir.FullName, "+TranslationCode.cs");

        if (TranslationAutocodeConfig.Instance.IsMainAssembly(assembly))
        {
            var actions = TranslationAutocodeConfig.Instance.InitTranslationRequests;
            if (actions.Any())
            {
                var t  = TranslationAutocodeConfig.Instance.TranslationsInit;
                var ns = csFile.GetOrCreateNamespace(t.Namespace);
                var cs = ns.GetOrCreateClass((CsType)t.Name);
                cs.IsPartial = true;

                var writer = new CsMethodCodeWriter(cs);
                writer.Location = SourceCodeLocation.Make<TranslationAutocodeGeneratorBase>();
                writer.WriteLine("// generator : " + writer.Location);

                writer.WritelineNoIndent("#if true");

                foreach (var action in actions)
                    action.Invoke(writer);

                writer.WritelineNoIndent("#endif");
                const string name = "InitTranslationClients";
                cs.AddMethod(name, CsType.Void)
                    .WithBody(writer)
                    .WithStatic();
            }
        }

        if (csFile.SaveIfDifferent(filename))
            FileSaved(GetType(), filename);
    }

    protected virtual string? GetCompilerDirectiveByTranslationKey(string key)
    {
        return null;
    }

    protected abstract IEnumerable<IAutoCodeGenerator> GetScanners();

    protected virtual void SaveTranslations(TranslationAutocodeGeneratorConfig cfg, string filename, string filename2,
        FileSavedDelegate saved)
    {
    }


    public void ScanForTranslation(IEnumerable<Assembly> assemblies)
    {
        var codeGenerators = GetScanners().ToArray();
        foreach (var assembly in assemblies.Distinct())
        {
            foreach (var generator in codeGenerators.OfType<IAssemblyAutoCodeGenerator>())
                generator.AssemblyStart(assembly, null);
            foreach (var type in assembly.GetTypes())
            foreach (var generator in codeGenerators)
                generator.Generate(type, null!);

            foreach (var generator in codeGenerators.OfType<IAssemblyAutoCodeGenerator>())
                generator.AssemblyEnd(assembly, null!);
        }
    }

    protected virtual void Seal(CsClass csClass)
    {
        if (csClass.IsStatic)
            throw new InvalidOperationException();
        csClass.IsSealed = true;
    }

    public TranslationAutocodeGeneratorBase WithLang(string regionName)
    {
        var region = new CultureInfo(regionName);
        TargetLanguages.Add(region);
        return this;
    }

    private readonly IAssemblyFilenameProvider _filenameProvider;
    protected readonly HashSet<CultureInfo> TargetLanguages = new HashSet<CultureInfo>();
}