using iSukces.Code.Interfaces;

namespace iSukces.Code.Irony
{
    public interface ICsExpression
    {
        string GetCode(ITypeNameResolver resolver);
    }
}