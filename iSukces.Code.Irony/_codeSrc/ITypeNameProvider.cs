using iSukces.Code.Interfaces;

#nullable disable
namespace iSukces.Code.Irony
{
    public interface ITypeNameProvider
    {
        FullTypeName GetTypeName(ITypeNameResolver resolver, string baseNamespace);
    }
}

