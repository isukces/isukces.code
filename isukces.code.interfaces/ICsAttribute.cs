namespace isukces.code.interfaces
{
    public interface ICsAttribute: IDirectCode
    {        
    }

    public interface IDirectCode
    {
        string Code { get; }
    }
}