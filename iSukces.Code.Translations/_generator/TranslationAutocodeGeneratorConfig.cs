using System;
using System.IO;
using System.Reflection;
// ReSharper disable ClassNeverInstantiated.Global

namespace iSukces.Code.Translations;

public sealed class TranslationAutocodeGeneratorConfig
{
    public DirectoryInfo? ResourcesTarget      { get; set; }
    public DirectoryInfo? TranslationSourceDir { get; set; }
    public Type?          InitType             { get; set; }
    public DirectoryInfo? InitTypeDir          { get; set; }

    public string InitTypeClassFile
    {
        get
        {
            var allStatic = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;
            var m         = InitType.GetMethod("GetClassFile", allStatic);
            if (m is null)
            {
                throw new Exception($"Method {InitType.FullName}.GetClassFile not found");
            }

            var f = (string)m.Invoke(null, null);
            return f;
        }
    }
}
