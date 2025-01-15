#nullable disable
using System.Collections.Generic;
using iSukces.Code.AutoCode;
using iSukces.Code.Interfaces;

namespace iSukces.Code.Tests;

public class TestContext : FileSavedNotifierBase, IAutoCodeGeneratorContext
{
    public TestContext()
    {
        _file = new CsFile
        {
            Nullable = FileNullableOption.GlobalEnabled
        };
    }

    public void AddNamespace(string namepace)
    {
        _file.AddImportNamespace(namepace);
    }


    public CsClass GetOrCreateClass(TypeProvider type)
    {
        // if (_file == null) _file = new CsFile();
        return _file.GetOrCreateClass(type);
    }

    public CsNamespace GetOrCreateNamespace(string namespaceName)
    {
        return _file.GetOrCreateNamespace(namespaceName);
    }

    public string Code => _file?.GetCode();

    public IList<object> Tags { get; } = new List<object>();

    public ITypeNameResolver FileLevelResolver => (ITypeNameResolver)_file ?? FullNameTypeNameResolver.Instance;

    private readonly CsFile _file;
}
