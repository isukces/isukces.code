using iSukces.Code.Interfaces;

#nullable disable
namespace iSukces.Code.Irony
{
    public interface ICsExpression
    {
        string GetCode(ITypeNameResolver resolver);
    }
}

