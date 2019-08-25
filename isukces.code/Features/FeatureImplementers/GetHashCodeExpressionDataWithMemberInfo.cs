using System;
using System.Reflection;

namespace isukces.code.FeatureImplementers
{
    public struct GetHashCodeExpressionDataWithMemberInfo : IComparable<GetHashCodeExpressionDataWithMemberInfo>,
        IComparable
    {
        public GetHashCodeExpressionDataWithMemberInfo(MemberInfo member, GetHashCodeExpressionData code)
        {
            Member = member;
            Code   = code;
        }

        public static bool operator >(GetHashCodeExpressionDataWithMemberInfo left,
            GetHashCodeExpressionDataWithMemberInfo right) => left.CompareTo(right) > 0;

        public static bool operator >=(GetHashCodeExpressionDataWithMemberInfo left,
            GetHashCodeExpressionDataWithMemberInfo right) => left.CompareTo(right) >= 0;

        public static bool operator <(GetHashCodeExpressionDataWithMemberInfo left,
            GetHashCodeExpressionDataWithMemberInfo right) => left.CompareTo(right) < 0;

        public static bool operator <=(GetHashCodeExpressionDataWithMemberInfo left,
            GetHashCodeExpressionDataWithMemberInfo right) => left.CompareTo(right) <= 0;

        public int CompareTo(GetHashCodeExpressionDataWithMemberInfo other)
        {
            var enum1 = GetMemberType().GetTypeInfo().IsEnum;
            var enum2 = other.GetMemberType().GetTypeInfo().IsEnum;
            return enum1.CompareTo(enum2);
        }

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            return obj is GetHashCodeExpressionDataWithMemberInfo other
                ? CompareTo(other)
                : throw new ArgumentException(
                    $"Object must be of type {nameof(GetHashCodeExpressionDataWithMemberInfo)}");
        }

        private Type GetMemberType()
        {
            if (Member is PropertyInfo pi)
                return pi.PropertyType;
            if (Member is FieldInfo fi)
                return fi.FieldType;
            throw new NotImplementedException();
        }

        public MemberInfo                Member { get; }
        public GetHashCodeExpressionData Code   { get; }

        public override string ToString() => Code.ToString();
    }
}