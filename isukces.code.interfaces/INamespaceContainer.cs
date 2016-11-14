using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isukces.code.interfaces
{
    public interface INamespaceContainer
    {
        ISet<string> GetNamespaces(bool withParent);

    }
    public interface INamespaceCollection
    {
        void AddImportNamespace(string nameSpace);

    }

    public interface ITypeNameResolver
    {
        string TypeName(Type type);
    }

    public interface IClassOwner : INamespaceContainer, ITypeNameResolver
    {

    }

    public interface INamespaceOwner : INamespaceContainer, ITypeNameResolver
    {
        
    }
}
