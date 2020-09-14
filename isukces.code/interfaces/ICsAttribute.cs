namespace iSukces.Code.Interfaces
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