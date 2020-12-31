using iSukces.Code.Interfaces;

namespace iSukces.Code.Irony
{
    public interface ITypeNameProvider
    {
        FullTypeName GetTypeName(ITypeNameResolver resolver, string baseNamespace);
    }
}