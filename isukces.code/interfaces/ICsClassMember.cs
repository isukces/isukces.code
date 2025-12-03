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
    extension<T>(T member)
        where T : ICsClassMember
    {
        public T WithStatic(bool isStatic = true)
        {
            member.IsStatic = isStatic;
            return member;
        }

        public T WithVisibility(Visibilities visibility)
        {
            member.Visibility = visibility;
            return member;
        }
    }
}
