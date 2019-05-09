using System;
using System.Reflection;
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
            var type = GetMemberResultType(mi);
            return TypeIsAlwaysNotNull(type) || HasMemberNotNullAttribute(mi);
        }

        protected Type GetMemberResultType([NotNull] MemberInfo mi)
        {
            if (mi == null) throw new ArgumentNullException(nameof(mi));
            if (mi is PropertyInfo pi)
                return pi.PropertyType;
            if (mi is FieldInfo fi)
                return fi.FieldType;
            if (mi is MethodInfo mb)
                return mb.ReturnType;
            throw new NotSupportedException(mi.GetType().ToString());
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