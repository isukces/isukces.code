namespace isukces.code.interfaces
{
    public interface ICsAttribute: IDirectCode
    {
        string Name { get; set; }
    }

    public interface IDirectCode
    {
        string Code { get; }
    }
}