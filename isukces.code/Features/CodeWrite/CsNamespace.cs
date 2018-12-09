using System;
using System.Collections.Generic;
using System.Linq;
using isukces.code.AutoCode;
using isukces.code.interfaces;

namespace isukces.code.CodeWrite
{
    public class CsNamespace : IClassOwner, INamespaceCollection
    {
        public CsNamespace(INamespaceOwner owner, string name)
        {
            Owner = owner;
            Name  = name;
        }

        public CsClass AddClass(CsClass csClass)
        {
            ((List<CsClass>)Classes).Add(csClass);
            csClass.Owner = this;
            return csClass;
        }

        public void AddImportNamespace(string aNamespace)
        {
            ImportNamespaces.Add(aNamespace);
        }
       
        public ISet<string> GetNamespaces(bool withParent)
        {
            var pNs = Owner?.GetNamespaces(true);

            return GeneratorsHelper.MakeCopy(
                ImportNamespaces,
                withParent ? pNs : null,
                withParent ? null : pNs);
        }

        public CsClass GetOrCreateClass(string csClassName)
        {
            var existing = Classes.FirstOrDefault(a => a.Name == csClassName);
            return existing ?? AddClass(new CsClass(csClassName));
        }

        public string TypeName(Type type)
        {
            return GeneratorsHelper.TypeName(type, this);
        }

        public ISet<string> ImportNamespaces { get; } = new HashSet<string>();

        public INamespaceOwner        Owner   { get; set; }
        public string                 Name    { get; private set; }
        public IReadOnlyList<CsClass> Classes { get; private set; } = new List<CsClass>();
    }
}