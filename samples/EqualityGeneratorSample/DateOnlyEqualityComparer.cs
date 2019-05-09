using System;
using System.Collections.Generic;
using isukces.code.interfaces;

namespace EqualityGeneratorSample
{
    /// <summary>
    ///     Sample attribute for date only datetime comparision
    /// </summary>
    public class DateOnlyEqualityAttribute : Auto.AbstractEqualityComparisonAttribute
    {
        public override string GetCoalesceExpression(ITypeToNameResolver resolver)
        {
            return resolver.GetMemeberName<DateTime>(nameof(DateTime.MinValue));
        }

        public override string GetEqualityComparerExpression(ITypeToNameResolver resolver)
        {
            return resolver.GetMemeberName<DateOnlyEqualityComparer>(nameof(DateOnlyEqualityComparer.Instance));
        }

        public override string GetRelationalComparerExpression(ITypeToNameResolver resolver)
        {
            return null;
        }
    }

    public class DateOnlyEqualityComparer : IEqualityComparer<DateTime>
    {
        private DateOnlyEqualityComparer()
        {
        }

        public bool Equals(DateTime x, DateTime y)
        {
            return x.Date.Equals(y.Date);
        }

        public int GetHashCode(DateTime obj)
        {
            return obj.Date.GetHashCode();
        }

        public static DateOnlyEqualityComparer Instance => InstanceHolder.SingleInstance;

        private class InstanceHolder
        {
            public static readonly DateOnlyEqualityComparer SingleInstance = new DateOnlyEqualityComparer();
        }
    }
}