#nullable disable
using System.Reflection;
using iSukces.Code.Interfaces;
using JetBrains.Annotations;

namespace Sample.AppWithCSharpCodeEmbedding.AutoCode
{
    internal class JetbrainsAttributeNullValueChecker : AbstractMemberNullValueChecker
    {
        protected override bool HasMemberNotNullAttribute(MemberInfo mi)
        {
            return mi.GetCustomAttribute<NotNullAttribute>() != null;
        }
    }
}

