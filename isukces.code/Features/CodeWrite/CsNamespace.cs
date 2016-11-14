using System;
using System.Collections.Generic;
using isukces.code.AutoCode;
using isukces.code.interfaces;

namespace isukces.code.CodeWrite
{
    public class CsNamespace : IClassOwner, INamespaceCollection
    {
        public CsNamespace(INamespaceOwner owner, string name)
        {
            Owner = owner;
            Name = name;
        }

        public ISet<string> ImportNamespaces { get; } = new HashSet<string>();

        public ISet<string> GetNamespaces(bool withParent)
        {
            var pNs = Owner?.GetNamespaces(true);

            return GeneratorsHelper.MakeCopy(
                ImportNamespaces,
                withParent ? pNs : null,
                withParent ? null : pNs);
        }

        public string TypeName(Type type)
        {
            return GeneratorsHelper.TypeName(type, this);
        }

        public void AddImportNamespace(string nameSpace)
        {
            ImportNamespaces.Add(nameSpace);
        }

        public INamespaceOwner Owner { get; set; }
        public string Name { get; private set; }
        public IReadOnlyList<CsClass> Classes { get; private set; } = new List<CsClass>();

        public void AddClass(CsClass csClass)
        {
            ((List<CsClass>)Classes).Add(csClass);
            csClass.ClassOwner = this;
        }
    }
}