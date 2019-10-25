using System;
using System.Collections.Generic;
using System.IO;
using isukces.code.AutoCode;
using isukces.code.CodeWrite;
using isukces.code.interfaces;

namespace isukces.code.Tests
{
    public class TestContext : IAutoCodeGeneratorContext
    {
        public void AddNamespace(string namepace)
        {
            _file.AddImportNamespace(namepace);
        }

        public void FileSaved(FileInfo fileInfo)
        {
        }


        public CsClass GetOrCreateClass(TypeProvider type)
        {
            if (_file == null)
                _file = new CsFile();
            return _file.GetOrCreateClass(type, new Dictionary<TypeProvider, CsClass>());
        }

        public IList<object> Tags { get; } = new List<object>();
        public string Code => _file.GetCode();

        private CsFile _file = new CsFile();
    }
}