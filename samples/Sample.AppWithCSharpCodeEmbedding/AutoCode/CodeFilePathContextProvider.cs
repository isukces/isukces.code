using System;
using iSukces.Code.AutoCode;

namespace Sample.AppWithCSharpCodeEmbedding.AutoCode;

internal class CodeFilePathContextProvider : ICsOutputProvider
{
    public CsOutputFileInfo GetOutputFileInfo(Type type)
    {
        var method = type.GetMethod("GetCodeFilePath", GeneratorsHelper.AllStatic);
        if (method is null)
            return null;
        if (method.Invoke(null, null) is string fileName)
            return new CsOutputFileInfo(fileName, true);
        return null;
    }
}