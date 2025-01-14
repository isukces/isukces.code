#nullable disable
using System;
using System.Collections.Generic;
using System.IO;
using iSukces.Code;
using iSukces.Code.AutoCode;
using iSukces.Code.Interfaces;

namespace iSukces.Code.Tests;

public class TestContext : FileSavedNotifierBase, IAutoCodeGeneratorContext
{
    public void AddNamespace(string namepace)
    {
        _file.AddImportNamespace(namepace);
    }


    public CsClass GetOrCreateClass(TypeProvider type)
    {
        if (_file == null)
            _file = new CsFile();
        return _file.GetOrCreateClass(type);
    }

    public CsNamespace GetOrCreateNamespace(string namespaceName) => _file.GetOrCreateNamespace(namespaceName);

    public IList<object> Tags         { get; } = new List<object>();

    public ITypeNameResolver FileLevelResolver
    {
        get { return (ITypeNameResolver)_file ?? FullNameTypeNameResolver.Instance; }
    }

    public string Code
    {
        get { return _file?.GetCode(); }
    }

    private CsFile _file = new CsFile();
}
