#nullable disable
using System;
using iSukces.Code.AutoCode;
using iSukces.Code.FeatureImplementers;
using iSukces.Code.Interfaces;

namespace EqualityGeneratorSample
{
    /// <summary>
    ///     Sample attribute for date only datetime comparision
    /// </summary>
    public class DateOnlyEqualityAttribute : Auto.AbstractEqualityComparisonAttribute
    {
        private static CsExpression GetUniversal(IExpressionDelegateArgs input, string methodName)
        {
            var comparerInstance = input.Resolver
                .GetTypeName(typeof(DateOnlyComparer))
                .Declaration;
            return GeneratorsHelper.CallMethod(comparerInstance, methodName, input);
        }

        public override CsExpression GetCoalesceExpression(ITypeNameResolver resolver)
        {
            var expression= resolver.GetMemeberName<DateTime>(nameof(DateTime.MinValue));
            return new CsExpression(expression, CsOperatorPrecendence.Expression);
        }


        public override CsExpression GetEqualsExpression(BinaryExpressionDelegateArgs input)
        {
            var method = input.DataType == typeof(DateTime)
                ? nameof(DateOnlyComparer.Equals)
                : nameof(DateOnlyComparer.EqualsNullable);
            return GetUniversal(input, method);
        }

        public override CsExpression GetHashCodeExpression(UnaryExpressionDelegateArgs input)
        {
            return GetUniversal(input, nameof(GetHashCode));
        }

        public override ExpressionWithObjectInstance GetRelationalComparerExpression(BinaryExpressionDelegateArgs input)
        {
            var method = input.DataType == typeof(DateTime)
                ? nameof(DateOnlyComparer.Compare)
                : nameof(DateOnlyComparer.CompareNullable);
            return new ExpressionWithObjectInstance(GetUniversal(input, method));
        }

        public override Auto.GetHashCodeOptions GetGetHashCodeOption()
        {
            return Auto.GetHashCodeOptions.NullValueGivesZero;
        }
    }

    public static class DateOnlyComparer

    {
        public static int Compare(DateTime x, DateTime y)
        {
            return x.Date.CompareTo(y.Date);
        }

        public static int CompareNullable(DateTime? x, DateTime? y)
        {
            if ((object)x == (object)y)
                return 0;
            if (x == null)
                return -1;
            if (y == null)
                return 1;
            return x.Value.Date.CompareTo(y.Value.Date);
        }

        public static bool Equals(DateTime x, DateTime y)
        {
            return x.Date.Equals(y.Date);
        }

        public static bool EqualsNullable(DateTime? x, DateTime? y)
        {
            if ((object)x == (object)y)
                return true;
            if (x == null || y == null)
                return false;
            return x.Value.Date.Equals(y.Value.Date);
        }

        public static int GetHashCode(DateTime obj)
        {
            return obj.Date.GetHashCode();
        }
        
        public static int GetHashCode(DateTime? obj)
        {
            if (obj == null)
                return 0;
            return obj.Value.Date.GetHashCode();
        }
    }
}
