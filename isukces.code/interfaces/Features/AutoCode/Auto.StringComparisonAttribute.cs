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
            public abstract string GetEqualsExpression(BinaryExpressionDelegateArgs input);
            public abstract string GetHashCodeExpression(UnaryExpressionDelegateArgs input);
            public abstract string GetRelationalComparerExpression(BinaryExpressionDelegateArgs input);
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

            public override string GetEqualsExpression(BinaryExpressionDelegateArgs input)
            {
                return GetUniversal(input, nameof(Equals));
            }

            public override string GetHashCodeExpression(UnaryExpressionDelegateArgs input)
            {
                return GetUniversal(input, nameof(GetHashCode));
            }

            public override string GetRelationalComparerExpression(BinaryExpressionDelegateArgs input)
            {
                return GetUniversal(input, "Compare");
            }

            private string GetUniversal(IExpressionDelegateArgs input, string methodName)
            {
                var comparerInstance = input.Resolver.GetMemeberName<StringComparer>(Comparison.ToString());
                return GeneratorsHelper.CallMethod(comparerInstance, methodName, input);
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