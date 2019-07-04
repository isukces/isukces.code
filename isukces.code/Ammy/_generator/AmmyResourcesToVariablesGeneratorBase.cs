using System;
using System.Linq;
using System.Reflection;
using System.Text;
using isukces.code.AutoCode;
using JetBrains.Annotations;

namespace isukces.code.Ammy
{
    /// <summary>
    /// Use commented 'GetResourceNames' method
    /// from  
    /// https://github.com/isukces/isukces.code/blob/master/isukces.code/Ammy/_generator/AmmyResourcesToVariablesGeneratorBase.cs
    /// in derived class.
    /// It requires reference to PresentationCore.
    /// </summary>
    public abstract class AmmyResourcesToVariablesGeneratorBase : AmmyAutocodeGenerator
    {
        protected AmmyResourcesToVariablesGeneratorBase(IAssemblyFilenameProvider filenameProvider) : base(filenameProvider)
        {
        }

        protected virtual string FindSourceFileFromCompiledUri(string compiledResourceUriPath)
        {
            if (compiledResourceUriPath.EndsWith(".baml", StringComparison.Ordinal))
                compiledResourceUriPath =
                    compiledResourceUriPath.Substring(0, compiledResourceUriPath.Length - 5) + ".xaml";
            return compiledResourceUriPath;
        }

        [CanBeNull]
        protected virtual string GetUriVariableName(string localUriPath)
        {
            var lastIndexOfDot = localUriPath.LastIndexOf('.');
            var ext            = lastIndexOfDot > 0 ? localUriPath.Substring(lastIndexOfDot) : "";
            if (IgnoreResourceByExtension(ext))
                return null;
            if (lastIndexOfDot > 0)
                localUriPath = localUriPath.Substring(0, lastIndexOfDot);
            var shortName = localUriPath.Split('/', '\\').Last();
            if (shortName.EndsWith(".g", StringComparison.OrdinalIgnoreCase))
                shortName = shortName.Substring(0, shortName.Length - 2);
            shortName = shortName.Replace("%20", " ");
            shortName = UriToVariableName(shortName);
            var varPrefix = GetVariablePrefixByExtension(ext);
            return varPrefix + shortName;
        }

        /*
        public static IReadOnlyList<string> GetContentNames(Assembly assembly)
        {
            var resourceUris = assembly
                .GetCustomAttributes(typeof(AssemblyAssociatedContentFileAttribute), true)
                .Cast<AssemblyAssociatedContentFileAttribute>()
                .Select(attr => attr.RelativeContentFilePath)
                .ToList();
            return resourceUris;
        }
       
        public static string[] GetResourceNames(Assembly assembly)
        {
            try
            {
                var resName = assembly.GetName().Name + ".g.resources";
                using(var stream = assembly.GetManifestResourceStream(resName))
                {
                    if (stream == null)
                        return new string[0];
                    using(var reader = new ResourceReader(stream))
                    {
                        return reader.Cast<DictionaryEntry>().Select(entry => (string)entry.Key).ToArray();
                    }
                }
            }
            catch
            {
                return new string[0];
            }
        }
         */

        protected virtual string GetVariablePrefixByExtension(string ext)
        {
            if (string.IsNullOrEmpty(ext))
                return string.Empty;
            ext = ext.ToLower();
            switch (ext)
            {
                case ".jpg":
                case ".png":
                case ".gif":
                    return "Img";
                case ".xaml": return "Res";
                case ".cur": return "Cursor";
                case ".ico": return "Icon";
                default:
                    return string.Empty;
            }
        }

        protected virtual bool IgnoreResourceByExtension(string ext)
        {
            return false;
        }

        protected virtual string UriToVariableName(string x)
        {
            var sb    = new StringBuilder();
            var upper = true;
            foreach (var i in x)
                if (i == '_' || char.IsWhiteSpace(i) || i == '-' || i == '.')
                {
                    upper = true;
                }
                else
                {
                    sb.Append(upper ? char.ToUpperInvariant(i) : i);
                    upper = false;
                }

            return sb.ToString();
        }

        protected void WriteAmmyVariables(Assembly assembly, params string[] localResourceNames)
        {
            var assemblyName = assembly.GetName().Name;
            var prefix       = $"pack://application:,,,/{assemblyName};component/";
            foreach (var compiledResourceUriPath in localResourceNames)
            {
                var localUriPath = FindSourceFileFromCompiledUri(compiledResourceUriPath);
                var varName      = GetUriVariableName(localUriPath);
                if (varName == null)
                    continue;
                CodeParts.AddVariable(varName, prefix + localUriPath);
            }
        }
    }
}