namespace iSukces.Code.Interfaces;

public interface ICsClassMember : IDescriptable, IConditional, IAttributable
{
    Visibilities Visibility { get; set; }
    bool         IsStatic   { get; set; }
}

public interface IClassMember2
{
    CsClass Owner { get; }
}

public static class CsClassMemberExtensions
{
    public static T WithStatic<T>(this T member, bool isStatic = true)
        where T : ICsClassMember
    {
        member.IsStatic = isStatic;
        return member;
    }

    public static T WithVisibility<T>(this T member, Visibilities visibility)
        where T : ICsClassMember
    {
        member.Visibility = visibility;
        return member;
    }
}