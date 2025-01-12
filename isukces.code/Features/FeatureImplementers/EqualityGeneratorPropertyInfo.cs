using System;
using System.Reflection;
using iSukces.Code.AutoCode;
using iSukces.Code.Interfaces;

namespace iSukces.Code.FeatureImplementers
{
    public interface IEqualityGeneratorPropertyInfo
    {
        CsExpression Coalesce(CsExpression expression, ITypeNameResolver resolver);
        EqualsExpressionData EqualsCode(CsExpression left, CsExpression right, ITypeNameResolver resolver);

        bool PropertyValueIsNotNull { get; }
        bool NullToEmpty            { get; }
        Type ResultType             { get; }
    }

    public static class EqualityGeneratorPropertyInfoExt
    {
        public static EqualsExpressionData EqualsCode1(this IEqualityGeneratorPropertyInfo info, ITypeNameResolver resolver)
            => info.EqualsCode((CsExpression)"{0}", (CsExpression)"{1}.{0}", resolver);
    }

    public class EqualityGeneratorPropertyInfo : IEqualityGeneratorPropertyInfo
    {
        public EqualityGeneratorPropertyInfo(Type resultType) => ResultType = resultType;

        public static EqualityGeneratorPropertyInfo? Find(MemberInfo member,
            IMemberNullValueChecker checker)
        {
            var sca = member.GetCustomAttribute<Auto.AbstractEqualityComparisonAttribute>();
            if (sca == null)
                return null;
            var info = new EqualityGeneratorPropertyInfo(GeneratorsHelper.GetMemberResultType(member))
                .WithMemberAttributes(member);
            info.PropertyValueIsNotNull = checker.ReturnValueAlwaysNotNull(member);
            return info;
        }

        public static EqualityGeneratorPropertyInfo FindForString(MemberInfo member,
            IMemberNullValueChecker checker)
        {
            CsExpression Call(string name, IExpressionDelegateArgs args)
            {
                var instance = args.Resolver.GetMemeberName<StringComparer>(nameof(StringComparer.Ordinal));
                return GeneratorsHelper.CallMethod(instance, name, args);
            }

            CsExpression DefaultEquals(BinaryExpressionDelegateArgs input) => Call(nameof(Equals), input);

            CsExpression DefaultGetHashCode(UnaryExpressionDelegateArgs input) => Call(nameof(GetHashCode), input);

            ExpressionWithObjectInstance DefaultGetCompareTo(BinaryExpressionDelegateArgs input) => new(Call("Compare", input));

            var a = new EqualityGeneratorPropertyInfo(typeof(string))
                {
                    GetEqualsExpression             = DefaultEquals,
                    GetRelationalComparerExpression = DefaultGetCompareTo,
                    GetHashCodeExpression           = DefaultGetHashCode,
                    GetCoalesceExpression           = _ => (CsExpression)GeneratorsHelper.StringEmpty
                }
                .WithMemberAttributes(member);
            a.PropertyValueIsNotNull = checker.ReturnValueAlwaysNotNull(member);
            return a;
        }

        public static bool Is1<T>(int a, int b) => a >> b is T;

        public string CheckNull(string expr, bool orEmpty)
        {
            if (orEmpty && ResultType == typeof(string))
                return $"string.IsNullOrEmpty({expr})";
            return $"ReferenceEquals({expr}, null)";
        }

        public CsExpression Coalesce(CsExpression expression, ITypeNameResolver resolver)
        {
            var co = GetCoalesceExpression(resolver);
            return expression.Coalesce(co);
        }

        public EqualsExpressionData EqualsCode(CsExpression left, CsExpression right, ITypeNameResolver resolver)
        {
            if (NullToEmpty && !PropertyValueIsNotNull)
            {
                left  = Coalesce(left, resolver);
                right = Coalesce(right, resolver);
            }

            if (GetEqualsExpression is null)
                throw new NullReferenceException(nameof(GetEqualsExpression));
            var expr = GetEqualsExpression(new BinaryExpressionDelegateArgs(left, right, resolver, ResultType));
            return new EqualsExpressionData(expr);
        }


        public GetHashCodeExpressionData GetHash(CsExpression propertyName, ITypeNameResolver resolver)
        {
            var argumentExpression = propertyName;
            if (!PropertyValueIsNotNull)
            {
                if (NullToEmpty)
                    argumentExpression = Coalesce(argumentExpression, resolver);
                else
                    switch (GetHashCodeOption)
                    {
                        case Auto.GetHashCodeOptions.CoalesceArgumentIfNullable:
                            argumentExpression = Coalesce(argumentExpression, resolver);
                            break;

                        case Auto.GetHashCodeOptions.NullValueGivesZero:
                            if (ResultType.IsNullableType())
                                argumentExpression = argumentExpression.CallProperty("Value");
                            break;
                        case Auto.GetHashCodeOptions.MethodAcceptNulls:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
            }

            CsExpression result =
                GetHashCodeExpression(new UnaryExpressionDelegateArgs(argumentExpression, resolver, ResultType));
            if (PropertyValueIsNotNull)
                return new GetHashCodeExpressionData(result);
            if (GetHashCodeOption == Auto.GetHashCodeOptions.NullValueGivesZero)
            {
                result = propertyName.Is("null").Conditional(0, result);
                return result;
            }

            return new GetHashCodeExpressionData(result);
        }

        public EqualityGeneratorPropertyInfo With(Auto.AbstractEqualityComparisonAttribute? sca)
        {
            if (sca == null)
                return this;
            GetCoalesceExpression           = sca.GetCoalesceExpression;
            GetEqualsExpression             = sca.GetEqualsExpression;
            GetHashCodeExpression           = sca.GetHashCodeExpression;
            GetRelationalComparerExpression = sca.GetRelationalComparerExpression;
            GetHashCodeOption               = sca.GetGetHashCodeOption();
            return this;
        }

        public EqualityGeneratorPropertyInfo WithMemberAttributes(MemberInfo member)
            => With(member.GetCustomAttribute<Auto.AbstractEqualityComparisonAttribute>())
                .WithNullToEmpty(member.GetCustomAttribute<Auto.NullIsEmptyAttribute>() != null);


        public EqualityGeneratorPropertyInfo WithNullToEmpty(bool nullToEmpty)
        {
            NullToEmpty = nullToEmpty;
            return this;
        }

        public Func<ITypeNameResolver, CsExpression>  GetCoalesceExpression { get; set; }
        public BinaryExpressionDelegate<CsExpression> GetEqualsExpression   { get; set; }

        public BinaryExpressionDelegate<ExpressionWithObjectInstance> GetRelationalComparerExpression { get; protected set; }

        public UnaryExpressionDelegate GetHashCodeExpression { get; set; }
        public Auto.GetHashCodeOptions GetHashCodeOption     { get; set; }

        public bool PropertyValueIsNotNull { get; set; }
        public bool NullToEmpty            { get; protected set; }
        public Type ResultType             { get; }
    }

    public delegate T BinaryExpressionDelegate<out T>(BinaryExpressionDelegateArgs input);

    public delegate CsExpression UnaryExpressionDelegate(UnaryExpressionDelegateArgs input);

    public struct BinaryExpressionDelegateArgs : IExpressionDelegateArgs
    {
        public BinaryExpressionDelegateArgs(CsExpression left, CsExpression right, ITypeNameResolver resolver, Type dataType)
        {
            Left     = left;
            Right    = right;
            Resolver = resolver;
            DataType = dataType;
        }

        public string[] GetArguments()
        {
            return new[] { Left.Code, Right.Code };
        }

        public BinaryExpressionDelegateArgs Transform(Func<CsExpression, CsExpression> map)
            => new(map(Left), map(Right), Resolver, DataType);

        public BinaryExpressionDelegateArgs WithLeftRight(CsExpression newLeft, CsExpression newRight)
            => new(newLeft, newRight, Resolver, DataType);

        public CsExpression Left     { get; }
        public CsExpression Right    { get; }
        public Type         DataType { get; }

        public ITypeNameResolver Resolver { get; }
    }

    public struct UnaryExpressionDelegateArgs : IExpressionDelegateArgs
    {
        public UnaryExpressionDelegateArgs(CsExpression argument, ITypeNameResolver resolver, Type dataType)
        {
            Argument = argument;
            Resolver = resolver;
            DataType = dataType;
        }

        public string[] GetArguments()
        {
            return new[] { Argument.Code };
        }

        public CsExpression Argument { get; }
        public Type         DataType { get; }

        public ITypeNameResolver Resolver { get; }
    }
}

