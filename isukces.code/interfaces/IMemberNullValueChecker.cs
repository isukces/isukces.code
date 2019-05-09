using System;
using System.Reflection;
using isukces.code.AutoCode;
using JetBrains.Annotations;

namespace isukces.code.interfaces
{
    public interface IMemberNullValueChecker
    {
        bool ReturnValueAlwaysNotNull(MemberInfo mi);
        bool TypeIsAlwaysNotNull(Type type);
    }

    public abstract class AbstractMemberNullValueChecker : IMemberNullValueChecker
    {
        public bool ReturnValueAlwaysNotNull(MemberInfo mi)
        {
            var type = GeneratorsHelper.GetMemberResultType(mi);
            return TypeIsAlwaysNotNull(type) || HasMemberNotNullAttribute(mi);
        }

        protected abstract bool HasMemberNotNullAttribute(MemberInfo mi);

        public virtual bool TypeIsAlwaysNotNull(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsClass || typeInfo.IsInterface)
                return false;
            if (!typeInfo.IsGenericType) return true;
            var gtt = type.GetGenericTypeDefinition();
            return gtt != typeof(Nullable<>);
        }
    }
}