#nullable enable
#nullable enable
#nullable enable
#nullable enable
namespace iSukces.Code.Interfaces;

public interface ICsAttribute: IDirectCode, IConditional
{
    string Name { get; set; }
        
}

public interface IDirectCode
{
    string Code { get; }
}