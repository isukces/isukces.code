namespace iSukces.Code.Typescript
{
    public interface ITsClassMember : ITsCodeProvider
    {
        string Name { get; }
        bool IsStatic { get; }
        TsVisibility Visibility { get; }
    }

    public interface ITsIntroducedItem
    {
        ITsCodeProvider Introduction { get; }
    }
}
