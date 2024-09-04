#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using iSukces.Code.Interfaces;

namespace iSukces.Code.AutoCode;

public partial class AutoCodeGenerator
{
    public class SimpleAutoCodeGeneratorContext : IFinalizableAutoCodeGeneratorContext
    {
        public SimpleAutoCodeGeneratorContext(Func<TypeProvider, CsClass> getOrCreateClassFunc,
            Action<string> addNamespaceAction, Func<string, CsNamespace> getOrCreateNamespaceFunc)
        {
            GetOrCreateClassFunc     = getOrCreateClassFunc;
            AddNamespaceAction       = addNamespaceAction;
            GetOrCreateNamespaceFunc = getOrCreateNamespaceFunc;
        }

        public SimpleAutoCodeGeneratorContext(CsFile file, Func<TypeProvider, CsClass> getOrCreateClassFunc)
        {
            GetOrCreateClassFunc     = getOrCreateClassFunc;
            AddNamespaceAction       = ns => file.AddImportNamespace(ns);
            GetOrCreateNamespaceFunc = file.GetOrCreateNamespace;
            FileLevelResolver        = file;
        }

        public void AddNamespace(string namepace)
        {
            AddNamespaceAction(namepace);
        }

        public void FileSaved(FileInfo fileInfo)
        {
            SavedFiles.Add(fileInfo);
        }

        public virtual void FinalizeContext(Assembly assembly)
        {
        }

        public CsClass GetOrCreateClass(TypeProvider type)
        {
            return GetOrCreateClassFunc(type);
        }

        public CsNamespace GetOrCreateNamespace(string namespaceName)
        {
            return GetOrCreateNamespaceFunc(namespaceName);
        }

        #region Properties

        public List<FileInfo>              SavedFiles               { get; } = new();
        public Func<string, CsNamespace>   GetOrCreateNamespaceFunc { get; }
        public Func<TypeProvider, CsClass> GetOrCreateClassFunc     { get; }
        public Action<string>              AddNamespaceAction       { get; }

        #endregion

        public IList<object> Tags         { get; } = new List<object>();
        public bool          AnyFileSaved => SavedFiles.Count > 0;

        public ITypeNameResolver FileLevelResolver { get; set; }
    }
}