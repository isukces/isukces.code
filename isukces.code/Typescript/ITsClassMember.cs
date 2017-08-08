namespace isukces.code.Typescript
{
    public interface ITsClassMember : ITsCodeProvider
    {
        string Name { get; }
        bool IsStatic { get; }
        TsVisibility Visibility { get; }
    }
}