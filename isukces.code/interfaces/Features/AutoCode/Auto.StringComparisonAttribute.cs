using System;
using isukces.code.AutoCode;

namespace isukces.code.interfaces
{
    public partial class Auto
    {
        [AttributeUsage(AttributeTargets.Property)]
        public abstract class AbstractEqualityComparisonAttribute : Attribute
        {
            public abstract string GetCoalesceExpression(ITypeNameResolver resolver);
            public abstract string GetEqualityComparerExpression(ITypeNameResolver resolver);
            public abstract string GetRelationalComparerExpression(ITypeNameResolver resolver);
        }

        [AttributeUsage(AttributeTargets.Property)]
        public class StringComparisonAttribute : AbstractEqualityComparisonAttribute
        {
            public StringComparisonAttribute(System.StringComparison comparison)
            {
                Comparison = comparison;
            }

            public override string GetCoalesceExpression(ITypeNameResolver resolver)
            {
                return GeneratorsHelper.StringEmpty;
            }

            public override string GetEqualityComparerExpression(ITypeNameResolver resolver)
            {
                return GetRelationalComparerExpression(resolver);
            }

            public override string GetRelationalComparerExpression(ITypeNameResolver resolver)
            {
                return resolver.GetTypeName(typeof(StringComparer))+ "." + Comparison;
            }

            public System.StringComparison Comparison { get; }
        }

        public class StringComparison
        {
            public class OrdinalIgnoreCaseAttribute : StringComparisonAttribute
            {
                public OrdinalIgnoreCaseAttribute() : base(System.StringComparison.OrdinalIgnoreCase)
                {
                }
            }

            public class CurrentCultureIgnoreCaseAttribute : StringComparisonAttribute
            {
                public CurrentCultureIgnoreCaseAttribute() : base(System.StringComparison.CurrentCultureIgnoreCase)
                {
                }
            }

            public class OrdinalAttribute : StringComparisonAttribute
            {
                public OrdinalAttribute() : base(System.StringComparison.Ordinal)
                {
                }
            }

            public class CurrentCulture : StringComparisonAttribute
            {
                public CurrentCulture() : base(System.StringComparison.CurrentCulture)
                {
                }
            }
        }
    }
}