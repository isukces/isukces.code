using System;
using System.Reflection;

namespace iSukces.Code.AutoCode;

public class SimpleCsOutputProvider : ICsOutputProvider
{
    public CsOutputFileInfo GetOutputFileInfo(Type type)
    {
        var name = DefaultMethodName;
        var at   = type.GetCustomAttribute<AutocodeCustomOutputMethodAttribute>(false);
        if (at is not null)
            name = at.MethodName;
        var method = type.GetMethod(name, GeneratorsHelper.AllStatic);
        if (method is null)
            return null;
        if (method.Invoke(null, null) is not string fileName)
            return null;
        fileName = CallerFilePathFixer.Fix(fileName);
        var configuration = AssumeDefinedNamespaceAttribute.Get(type);
        var info = new CsOutputFileInfo(fileName, true)
        {
            FileScopeNamespace = configuration
        };
        return info;
    }

    public static string DefaultMethodName = "GetCodeFilePath";
}
