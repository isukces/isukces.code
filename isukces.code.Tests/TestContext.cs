using System;
using System.Collections.Generic;
using System.IO;
using iSukces.Code.AutoCode;
using iSukces.Code.CodeWrite;
using iSukces.Code.Interfaces;

namespace iSukces.Code.Tests
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

        public CsNamespace GetOrCreateNamespace(string namespaceName)
        {
            return _file.GetOrCreateNamespace(namespaceName);
        }

        public IList<object> Tags         { get; } = new List<object>();
        public bool          AnyFileSaved { get; }
        public string        Code         => _file.GetCode();

        private CsFile _file = new CsFile();
    }
}