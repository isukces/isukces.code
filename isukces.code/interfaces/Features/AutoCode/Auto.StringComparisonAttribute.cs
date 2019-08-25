using System;
using isukces.code.AutoCode;
using isukces.code.FeatureImplementers;

namespace isukces.code.interfaces
{
    public partial class Auto
    {
        public enum GetHashCodeOptions
        {
            CoalesceArgumentIfNullable,
            NullValueGivesZero,
            MethodAcceptNulls
        }
        [AttributeUsage(AttributeTargets.Property)]
        public abstract class AbstractEqualityComparisonAttribute : Attribute
        {
            public abstract CsExpression GetCoalesceExpression(ITypeNameResolver resolver);
            public abstract CsExpression GetEqualsExpression(BinaryExpressionDelegateArgs input);
            public abstract CsExpression GetHashCodeExpression(UnaryExpressionDelegateArgs input);
            public abstract ExpressionWithObjectInstance GetRelationalComparerExpression(BinaryExpressionDelegateArgs input);

            public virtual GetHashCodeOptions GetGetHashCodeOption()
            {
                return GetHashCodeOptions.CoalesceArgumentIfNullable;
            }
        }

        [AttributeUsage(AttributeTargets.Property)]
        public class StringComparisonAttribute : AbstractEqualityComparisonAttribute
        {
            public StringComparisonAttribute(System.StringComparison comparison)
            {
                Comparison = comparison;
            }

            public override CsExpression GetCoalesceExpression(ITypeNameResolver resolver)
            {
                return (CsExpression)GeneratorsHelper.StringEmpty;
            }

            public override CsExpression GetEqualsExpression(BinaryExpressionDelegateArgs input)
            {
                return GetUniversal(input, nameof(Equals));
            }

            public override CsExpression GetHashCodeExpression(UnaryExpressionDelegateArgs input)
            {
                return GetUniversal(input, nameof(GetHashCode));
            }

            public override ExpressionWithObjectInstance GetRelationalComparerExpression(BinaryExpressionDelegateArgs input)
            {
                var comparerInstance = input.Resolver.GetMemeberName<StringComparer>(Comparison.ToString());
                var code             = GeneratorsHelper.CallMethod("{0}", "Compare", input);
                return new ExpressionWithObjectInstance(code, comparerInstance);
            }

            private CsExpression GetUniversal(IExpressionDelegateArgs input, string methodName)
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