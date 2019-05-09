using System;
using System.Reflection;
using isukces.code.interfaces;
using JetBrains.Annotations;

namespace isukces.code.AutoCode
{
    public class EqualityGeneratorPropertyInfo
    {
        [CanBeNull]
        public static EqualityGeneratorPropertyInfo Find(MemberInfo member,
            IMemberNullValueChecker checker)
        {
            var sca = member.GetCustomAttribute<Auto.AbstractEqualityComparisonAttribute>();
            if (sca == null)
                return null;
            var info = new EqualityGeneratorPropertyInfo()
                .With(sca)
                .WithNullToEmpty(member.GetCustomAttribute<Auto.NullIsEmptyAttribute>() != null);
            info.PropertyValueIsNotNull = checker.ReturnValueAlwaysNotNull(member);
            return info;
        }

        [NotNull]
        public static EqualityGeneratorPropertyInfo FindForString(MemberInfo member,
            IMemberNullValueChecker checker)
        {
            const string defaultStringComparer = "System.StringComparer.Ordinal";
            var a = new EqualityGeneratorPropertyInfo
                {
                    GetEqualityComparerExpression   = _=> defaultStringComparer,
                    GetRelationalComparerExpression = _=> defaultStringComparer,
                    GetCoalesceExpression           = _=> GeneratorsHelper.StringEmpty
                }
                .With(member.GetCustomAttribute<Auto.AbstractEqualityComparisonAttribute>())
                .WithNullToEmpty(member.GetCustomAttribute<Auto.NullIsEmptyAttribute>() != null);
            a.PropertyValueIsNotNull = checker.ReturnValueAlwaysNotNull(member);
            return a;
        }

        public string Coalesce(string expr, ITypeNameResolver resolver, bool addBracketsOutside = false)
        {
            var co = GetCoalesceExpression(resolver);
            if (string.IsNullOrEmpty(co))
                throw new Exception("CoalesceExpression is empty");
            var tmp = expr + " ?? " + GetCoalesceExpression(resolver);
            if (addBracketsOutside)
                tmp = "(" + expr + ")";
            return tmp;
        }

        public string EqualsCode(string left, string right, ITypeNameResolver resolver)
        {
            if (NullToEmpty && !PropertyValueIsNotNull)
            {
                left  = Coalesce(left, resolver);
                right = Coalesce(right, resolver);
            }

            return $"{GetEqualityComparerExpression(resolver)}.Equals({left}, {right})";
        }


        public string GetHash(string propName, ITypeNameResolver resolver)
        {
            if (!PropertyValueIsNotNull)
                propName = Coalesce(propName, resolver);
            return $"{GetEqualityComparerExpression(resolver)}.GetHashCode({propName})";
        }

        private EqualityGeneratorPropertyInfo With(Auto.AbstractEqualityComparisonAttribute sca)
        {
            if (sca == null)
                return this;
            GetCoalesceExpression           = sca.GetCoalesceExpression;
            GetEqualityComparerExpression   = sca.GetEqualityComparerExpression;
            GetRelationalComparerExpression = sca.GetRelationalComparerExpression;
            return this;
        }

        private EqualityGeneratorPropertyInfo WithNullToEmpty(bool nullToEmpty)
        {
            NullToEmpty = nullToEmpty;
            return this;
        }
 

        public Func<ITypeNameResolver, string> GetCoalesceExpression           { get; set; }
        public bool                              PropertyValueIsNotNull          { get; set; }
        public bool                              NullToEmpty                     { get; protected set; }
        public Func<ITypeNameResolver, string> GetEqualityComparerExpression   { get; protected set; }
        public Func<ITypeNameResolver, string> GetRelationalComparerExpression { get; protected set; }
    }
}