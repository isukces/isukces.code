using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using isukces.code.AutoCode;
using isukces.code.interfaces;
using isukces.code.IO;

namespace isukces.code.CodeWrite
{
    public class CsFile : ICsCodeMaker, IClassOwner, INamespaceCollection, INamespaceOwner
    {
        public void AddImportNamespace(string aNamespace)
        {
            _importNamespaces.Add(aNamespace);
        }

        public ISet<string> GetNamespaces(bool withParent)
        {
            var s = new HashSet<string>();
            foreach (var i in _importNamespaces)
                s.Add(i);
            return s;
        }

        public CsClass GetOrCreateClass(string namespaceName, string className)
        {
            var ns = GetOrCreateNamespace(namespaceName);
            return ns.GetOrCreateClass(className);
        }

        public CsClass GetOrCreateClass(Type type, Dictionary<Type, CsClass> classesCache)
        {
            if (classesCache.TryGetValue(type, out var c))
                return c;
            if (type.DeclaringType == null)
            {
                var a = classesCache[type] = new CsClass(type.Name)
                {
                    IsPartial  = true,
                    DotNetType = type,
                    Owner      = this,
                    Visibility = Visibilities.InterfaceDefault,
                    Kind = type.GetNamespaceMemberKind()
                };
                var ns = GetOrCreateNamespace(type.Namespace);
                ns.AddClass(a);
                return a;
            }

            var parent   = GetOrCreateClass(type.DeclaringType, classesCache);
            var existing = parent.GetOrCreateNested(type.Name);
            existing.IsPartial  = true;
            existing.DotNetType = type;
            existing.Kind = type.GetNamespaceMemberKind();
            existing.Visibility = Visibilities.InterfaceDefault;
            return existing;
        }

        public CsNamespace GetOrCreateNamespace(string name)
        {
            var result = Namespaces.FirstOrDefault(ns => ns.Name == name);
            if (result != null)
                return result;
            result = new CsNamespace(this, name);
            ((List<CsNamespace>)Namespaces).Add(result);
            return result;
        }

        public void MakeCode(ICodeWriter writer)
        {
            if (Namespaces == null || Namespaces.Count == 0)
                return;
            const string emptyNamespace = "";
            foreach (var i in _importNamespaces.OrderBy(i => i))
                writer.WriteLine("using {0};", i);
            if (_importNamespaces.Any())
                writer.EmptyLine();
            var classByNamespace     = Namespaces.ToDictionary(a => a.Name, a => a.Classes);
            var directiveByNamespace = Namespaces.ToDictionary(a => a.Name, a => a.CompilerDirective);
            var enumByNamespace = (_enums ?? new List<CsEnum>())
                .GroupBy(c => c.DotNetType == null ? emptyNamespace : c.DotNetType.Namespace)
                .ToDictionary(a => a.Key, a => a.ToList());
            var fileNamespaces = classByNamespace.Keys.Union(enumByNamespace.Keys)
                .OrderBy(a => a).ToList();

            foreach (var ns in fileNamespaces)
            {
                var addEmptyLine = false;
                if (string.IsNullOrEmpty(ns))
                    continue; // should never occurs
                directiveByNamespace.TryGetValue(ns, out var compilerDirective);
                {
                    writer.WriteLine("// ReSharper disable once CheckNamespace");
                    writer.OpenCompilerIf(compilerDirective);
                    writer.Open("namespace {0}", ns);
                    var ns1 = Namespaces.FirstOrDefault(a => a.Name == ns)?.ImportNamespaces;
                    if (ns1 != null && ns1.Any())
                    {
                        foreach (var i in ns1.OrderBy(a => a))
                            writer.WriteLine($"using {i};");
                        addEmptyLine = true;
                    }
                }

                {
                    if (classByNamespace.TryGetValue(ns, out var classList))
                        foreach (var i in classList)
                        {
                            if (addEmptyLine)
                                writer.EmptyLine();
                            addEmptyLine = true;
                            writer.DoWithKeepingIndent(() => i.MakeCode(writer));
                        }

                    if (enumByNamespace.TryGetValue(ns, out var enumList))
                        foreach (var i in enumList)
                        {
                            if (addEmptyLine)
                                writer.EmptyLine();
                            addEmptyLine = true;
                            writer.DoWithKeepingIndent(() => i.MakeCode(writer));
                        }
                }
                if (!string.IsNullOrEmpty(ns))
                    writer.Close();
                writer.CloseCompilerIf(compilerDirective);
            }
        }

        public bool SaveIfDifferent(string filename, bool addBom = false)
        {
            return CodeFileUtils.SaveIfDifferent(GetCode(), filename, addBom);
        }

        public override string ToString()
        {
            return GetCode();
        }

        public string TypeName(Type type)
        {
            return GeneratorsHelper.TypeName(type, this);
        }


        // Public Methods 

        private string GetCode()
        {
            var writer = new CodeWriter();
            MakeCode(writer);
            return writer.Code;
        }

        private void Save(string filename)
        {
            var fi = new FileInfo(filename);
            if (fi.Directory == null)
                throw new NullReferenceException("fi.Directory");
            fi.Directory.Create();
            var x = Encoding.UTF8.GetBytes(GetCode());
            using(var fs = new FileStream(filename, File.Exists(filename) ? FileMode.Create : FileMode.CreateNew))
            {
                fs.Write(x, 0, x.Length);
#if !COREFX
                fs.Close();
#endif
            }
        }

        /// <summary>
        ///     Przestrzenie nazw
        /// </summary>
        public IReadOnlyList<CsNamespace> Namespaces { get; } = new List<CsNamespace>();

        /// <summary>
        ///     Przestrzenie nazw
        /// </summary>
        public List<CsEnum> Enums
        {
            get => _enums;
            set => _enums = value ?? new List<CsEnum>();
        }

        /// <summary>
        /// </summary>
        public string SuggestedFileName
        {
            get => _suggestedFileName;
            set
            {
                value              = value?.Trim() ?? string.Empty;
                _suggestedFileName = value;
            }
        }

        /// <summary>
        ///     Przestrzenie nazw
        /// </summary>
        private readonly ISet<string> _importNamespaces = new HashSet<string>();

        private List<CsEnum> _enums = new List<CsEnum>();
        private string _suggestedFileName = string.Empty;
    }
}