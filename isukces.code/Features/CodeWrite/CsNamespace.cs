using System;
using System.Collections.Generic;
using System.Linq;
using iSukces.Code.AutoCode;
using iSukces.Code.Interfaces;

namespace iSukces.Code
{
    public class CsNamespace : IClassOwner, INamespaceCollection, IConditional
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
        
        public CsEnum AddEnum(CsEnum csEnum)
        {
            ((List<CsEnum>)Enums).Add(csEnum);
            csEnum.Owner = this;
            return csEnum;
        }

        public void AddImportNamespace(string ns)
        {
            ImportNamespaces.Add(ns);
        }

        public CsClass GetOrCreateClass(string csClassName)
        {
            var existing = Classes.FirstOrDefault(a => a.Name == csClassName);
            return existing ?? AddClass(new CsClass(csClassName));
        }

        public bool IsKnownNamespace(string namespaceName)
        {
            if (string.IsNullOrEmpty(namespaceName)) return false;
            if (Name == namespaceName) return true;
            if (ImportNamespaces.Contains(namespaceName)) return true;
            return Owner?.IsKnownNamespace(namespaceName) ?? false;
        }

        public string GetTypeName(Type type)
        {
            return GeneratorsHelper.GetTypeName(this, type);
        }

        public ISet<string> ImportNamespaces { get; } = new HashSet<string>();

        public INamespaceOwner        Owner             { get; set; }
        public string                 Name              { get; }
        public IReadOnlyList<CsClass> Classes           { get; } = new List<CsClass>();
        public string                 CompilerDirective { get; set; }
        
        /// <summary>
        ///     Przestrzenie nazw
        /// </summary>
        public IReadOnlyList<CsEnum> Enums { get;  } = new List<CsEnum>();
    }
}