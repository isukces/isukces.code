#region using

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using isukces.code.CodeWrite;
using isukces.code.interfaces;

#endregion

namespace isukces.code.AutoCode
{
    public class AutoCodeGenerator
    {
        public AutoCodeGeneratorContext Context { get; set; } = new AutoCodeGeneratorContext();
        #region Instance Methods

        public void Make(Assembly assembly, string outFileName, ref bool saved)
        {
            _csFile = new CsFile();
            foreach (var i in FileNamespaces)
                _csFile.AddImportNamespace(i);
            _classes = new Dictionary<Type, CsClass>();
            var types = assembly.GetTypes();
            foreach (var type in types.OrderBy(aa => aa.Namespace ?? ""))
            {
                Generators.LazyGenerator.Generate(type, GetOrCreateClass);
                Generators.DependencyPropertyGenerator.Generate(type, GetOrCreateClass);
                Generators.CopyFromGenerator.Generate(type, GetOrCreateClass, Context);
                Generators.ShouldSerializeGenerator.Generate(type, GetOrCreateClass);
                Generators.ReactivePropertyGenerator.Generate(type, GetOrCreateClass, ns => _csFile.AddImportNamespace(ns));
                Generators.ReactiveCommandGenerator.Generate(type, GetOrCreateClass, ns => _csFile.AddImportNamespace(ns));
            }

            // _csFile.Classes.AddRange(_classes.Values);

            var fileName = Path.Combine(BaseDir.FullName, outFileName);
            if (_csFile.SaveIfDifferent(fileName))
                saved = true;
        }


        private CsClass GetOrCreateClass(Type type)
        {
            {
                CsClass c;
                if (_classes.TryGetValue(type, out c))
                    return c;
            }
            if (type.DeclaringType == null)
            {
                var a = _classes[type] = new CsClass(type.Name)
                {
                    IsPartial = true,
                    DotNetType = type,
                    ClassOwner = _csFile,
                    Visibility = Visibilities.InterfaceDefault

                };
                var ns = _csFile.GetOrCreateNamespace(type.Namespace);
                ns.AddClass(a);
                return a;
            }
            var parent = GetOrCreateClass(type.DeclaringType);
            var existing = parent.GetOrCreateNested(type.Name);
            existing.IsPartial = true;
            existing.DotNetType = type;
            existing.Visibility = Visibilities.InterfaceDefault;
            return existing;
        }

        #endregion

        #region Properties

        public DirectoryInfo BaseDir { get; set; }


        // Public Methods 

        public ISet<string> FileNamespaces { get; } = new HashSet<string>();

        #endregion

        #region Fields

        private Dictionary<Type, CsClass> _classes;
        private CsFile _csFile;

        #endregion
    }
}