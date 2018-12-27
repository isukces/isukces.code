using System.Collections.Generic;

namespace isukces.code.interfaces
{
    public interface ICsClassMember : IDescriptable, IConditional
    {
        Visibilities        Visibility { get; set; }
        bool                IsStatic   { get; set; }
        IList<ICsAttribute> Attributes { get; set; }
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
}