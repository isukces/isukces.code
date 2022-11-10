#if AMMY
namespace iSukces.Code.Interfaces.Ammy
{
    public interface IAmmyObjectBuilder<TPropertyBrowser>: 
        IAmmyCodePieceConvertible, 
        IAmmyContainer,
        IAmmyPropertyContainer<TPropertyBrowser>
    
    {
        
    }
}
#endif