#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace iSukces.Code.VsSolutions;

public partial class VsCoreProjectFile : VsProjectFile
{
    [Obsolete("Use XDocument+CsprojDocumentKind constructor instead", true)]
    public VsCoreProjectFile(XDocument document) 
        : this(document, CsprojDocumentKind.Project)
    {
    }

    public VsCoreProjectFile(XDocument document, CsprojDocumentKind kind) 
        : base(document)
    {
        Kind            = kind;
        TargetFramework                      = new XmlTargetFrameworkCache(Document);
        Authors                              = new XmlPropertyGroupValueCache(Document, Tags.Authors);
        Company                              = new XmlPropertyGroupValueCache(Document, Tags.Company);
        Copyright                            = new XmlPropertyGroupValueCache(Document, Tags.Copyright);
        LangVersion                          = new XmlCsLangVersionCache(Document);
        ProjectGuid                          = new XmlProjectGuidCache(Document);
        AssemblyFileVersion                  = new XmlPropertyGroupValueCache(Document, Tags.AssemblyFileVersion);
        AssemblyVersion                      = new XmlPropertyGroupValueCache(Document, Tags.AssemblyVersion);
        FileVersion                          = new XmlPropertyGroupValueCache(Document, Tags.FileVersion);
        Version                              = new XmlPropertyGroupValueCache(Document, Tags.Version);
        RootNamespace                        = new XmlPropertyGroupValueCache(Document, Tags.RootNamespace);
        Description                          = new XmlPropertyGroupValueCache(Document, Tags.Description);
        AssemblyOriginatorKeyFile            = new XmlPropertyGroupValueCache(Document, Tags.AssemblyOriginatorKeyFile);
        SignAssembly                         = new XmlPropertyGroupBoolValueCache(Document, Tags.SignAssembly);
        Product                              = new XmlPropertyGroupValueCache(Document, Tags.Product);
        PackageId                            = new XmlPropertyGroupValueCache(Document, Tags.PackageId);
        PackageDescription                   = new XmlPropertyGroupValueCache(Document, Tags.PackageDescription);
        DocumentationFile                    = new XmlPropertyGroupValueCache(Document, Tags.DocumentationFile);
        PackageVersion                       = new XmlPropertyGroupValueCache(Document, Tags.PackageVersion);
        PackageTags                          = new XmlPropertyGroupValueCache(Document, Tags.PackageTags);
        PackageLicenseExpression             = new XmlPropertyGroupValueCache(Document, Tags.PackageLicenseExpression);
        SymbolPackageFormat                  = new XmlPropertyGroupValueCache(Document, Tags.SymbolPackageFormat);
        PackageProjectUrl                    = new XmlPropertyGroupValueCache(Document, Tags.PackageProjectUrl);
        PublishRepositoryUrl                 = new XmlPropertyGroupValueCache(Document, Tags.PublishRepositoryUrl);
        RepositoryUrl                        = new XmlPropertyGroupValueCache(Document, Tags.RepositoryUrl);
        IncludeSource                        = new XmlPropertyGroupBoolValueCache(Document, Tags.IncludeSource);
        IncludeSymbols                       = new XmlPropertyGroupBoolValueCache(Document, Tags.IncludeSymbols);
        GeneratePackageOnBuild               = new XmlPropertyGroupBoolValueCache(Document, Tags.GeneratePackageOnBuild);
        ContinuousIntegrationBuild           = new XmlPropertyGroupBoolValueCache(Document, Tags.ContinuousIntegrationBuild);
        GenerateAssemblyDescriptionAttribute = new XmlPropertyGroupBoolValueCache(Document, Tags.GenerateAssemblyDescriptionAttribute);
        GenerateAssemblyTitleAttribute       = new XmlPropertyGroupBoolValueCache(Document, Tags.GenerateAssemblyTitleAttribute);
        EmbedUntrackedSources                = new XmlPropertyGroupBoolValueCache(Document, Tags.EmbedUntrackedSources);
        UseWpf                               = new XmlPropertyGroupBoolValueCache(Document, Tags.UseWpf);
        UseWindowsForms                      = new XmlPropertyGroupBoolValueCache(Document, Tags.UseWindowsForms);
        Nullable                             = new XmlPropertyGroupEnableDisableValueCache(Document, Tags.Nullable);
        ImplicitUsings                       = new XmlPropertyGroupEnableDisableValueCache(Document, Tags.ImplicitUsings);
    }    

    public CsprojDocumentKind              Kind            { get; }
    
    public IValueProvider<TargetFramework> TargetFramework                      { get; }
    public IValueProvider<string>          Authors                              { get; }
    public IValueProvider<string>          Company                              { get; }
    public IValueProvider<string>          Copyright                            { get; }
    public IValueProvider<CsLangVersion>   LangVersion                          { get; }
    public IValueProvider<Guid?>           ProjectGuid                          { get; }
    public IValueProvider<string>          AssemblyFileVersion                  { get; }
    public IValueProvider<string>          AssemblyVersion                      { get; }
    public IValueProvider<string>          FileVersion                          { get; }
    public IValueProvider<string>          Version                              { get; }
    public IValueProvider<string>          RootNamespace                        { get; }
    public IValueProvider<string>          Description                          { get; }
    public IValueProvider<string>          AssemblyOriginatorKeyFile            { get; }
    public IValueProvider<bool?>           SignAssembly                         { get; }
    public IValueProvider<string>          Product                              { get; }
    public IValueProvider<string>          PackageId                            { get; }
    public IValueProvider<string>          PackageDescription                   { get; }
    public IValueProvider<string>          DocumentationFile                    { get; }
    public IValueProvider<string>          PackageVersion                       { get; }
    public IValueProvider<string>          PackageTags                          { get; }
    public IValueProvider<string>          PackageLicenseExpression             { get; }
    public IValueProvider<string>          SymbolPackageFormat                  { get; }
    public IValueProvider<string>          PackageProjectUrl                    { get; }
    public IValueProvider<string>          PublishRepositoryUrl                 { get; }
    public IValueProvider<string>          RepositoryUrl                        { get; }
    public IValueProvider<bool?>           IncludeSource                        { get; }
    public IValueProvider<bool?>           IncludeSymbols                       { get; }
    public IValueProvider<bool?>           GeneratePackageOnBuild               { get; }
    public IValueProvider<bool?>           ContinuousIntegrationBuild           { get; }
    public IValueProvider<bool?>           GenerateAssemblyDescriptionAttribute { get; }
    public IValueProvider<bool?>           GenerateAssemblyTitleAttribute       { get; }
    public IValueProvider<bool?>           EmbedUntrackedSources                { get; }
    public IValueProvider<bool?>           UseWpf                               { get; }
    public IValueProvider<bool?>           UseWindowsForms                      { get; }
    public IValueProvider<bool?>           Nullable                             { get; }
    public IValueProvider<bool?>           ImplicitUsings                       { get; }

}

