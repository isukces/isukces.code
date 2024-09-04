#nullable enable
using System;
using System.Reflection;

namespace iSukces.Code.FeatureImplementers
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
            int GetGroup(GetHashCodeExpressionDataWithMemberInfo x)
            {
                var type = x.GetMemberType().StripNullable();
                if (type == typeof(bool))
                    return 3;

#if COREFX
                if (type.GetTypeInfo().IsEnum) return 2;
#else
                if (type.IsEnum) return 2;
#endif
                return 1;
            }

            var g1 = GetGroup(this);
            var g2 = GetGroup(other);
            return g1.CompareTo(g2);
        }

        public int CompareTo(object? obj)
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

        public override string ToString() => Code.ToString();

        public MemberInfo                Member { get; }
        public GetHashCodeExpressionData Code   { get; }
    }
}
