using System;
using System.Reflection;
using isukces.code.interfaces;
using JetBrains.Annotations;

namespace isukces.code.AutoCode
{
    public interface IEqualityGeneratorPropertyInfo
    {
        bool PropertyValueIsNotNull { get; }
        bool NullToEmpty            { get; }
        Type ResultType             { get; }
        EqualsExpressionData EqualsCode(string left, string right, ITypeNameResolver resolver);
        string Coalesce(string expr, ITypeNameResolver resolver, bool addBracketsOutside = false);
    }

    public class EqualityGeneratorPropertyInfo:IEqualityGeneratorPropertyInfo
    {
        public EqualityGeneratorPropertyInfo(Type resultType)
        {
            ResultType = resultType;
        }

        [CanBeNull]
        public static EqualityGeneratorPropertyInfo Find(MemberInfo member,
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

        [NotNull]
        public static EqualityGeneratorPropertyInfo FindForString(MemberInfo member,
            IMemberNullValueChecker checker)
        {
            string Call(string name, IExpressionDelegateArgs args)
            {
                var instance = args.Resolver.GetMemeberName<StringComparer>(nameof(StringComparer.Ordinal));
                return GeneratorsHelper.CallMethod(instance, name, args);
            }

            string DefaultEquals(BinaryExpressionDelegateArgs input)
            {
                return Call(nameof(Equals), input);
            }

            string DefaultGetHashCode(UnaryExpressionDelegateArgs input)
            {
                return Call(nameof(GetHashCode), input);
            }

            ExpressionWithObjectInstance DefaultGetCompareTo(BinaryExpressionDelegateArgs input)
            {
                return new ExpressionWithObjectInstance(Call("Compare", input));
            }

            var a = new EqualityGeneratorPropertyInfo(typeof(string))
                {
                    GetEqualsExpression             = DefaultEquals,
                    GetRelationalComparerExpression = DefaultGetCompareTo,
                    GetHashCodeExpression           = DefaultGetHashCode,
                    GetCoalesceExpression           = _ => GeneratorsHelper.StringEmpty
                }
                .WithMemberAttributes(member);
            a.PropertyValueIsNotNull = checker.ReturnValueAlwaysNotNull(member);
            return a;
        }

        public string CheckNull(string expr, bool orEmpty)
        {
            if (orEmpty && ResultType == typeof(string))
                return $"string.IsNullOrEmpty({expr})";
            return $"ReferenceEquals({expr}, null)";
        }

        public string Coalesce(string expr, ITypeNameResolver resolver, bool addBracketsOutside = false)
        {
            var co = GetCoalesceExpression(resolver);
            if (string.IsNullOrEmpty(co))
                throw new Exception("CoalesceExpression is empty");
            var tmp = expr + " ?? " + GetCoalesceExpression(resolver);
            if (addBracketsOutside)
                tmp = "(" + tmp + ")";
            return tmp;
        }

        public EqualsExpressionData EqualsCode(string left, string right, ITypeNameResolver resolver)
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


        public GetHashCodeExpressionData GetHash(string propertyName, ITypeNameResolver resolver)
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
                                argumentExpression += ".Value";
                            break;
                        case Auto.GetHashCodeOptions.MethodAcceptNulls:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
            }

            var result =
                GetHashCodeExpression(new UnaryExpressionDelegateArgs(argumentExpression, resolver, ResultType));
            if (PropertyValueIsNotNull)
                return new GetHashCodeExpressionData(result);
            if (GetHashCodeOption == Auto.GetHashCodeOptions.NullValueGivesZero)
            {
                result = $"{propertyName} is null ? 0 : {result}";
                return new GetHashCodeExpressionData(result, true);
            }

            return new GetHashCodeExpressionData(result);
        }

        public EqualityGeneratorPropertyInfo With(Auto.AbstractEqualityComparisonAttribute sca)
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
        {
            return With(member.GetCustomAttribute<Auto.AbstractEqualityComparisonAttribute>())
                .WithNullToEmpty(member.GetCustomAttribute<Auto.NullIsEmptyAttribute>() != null);
        }


        public EqualityGeneratorPropertyInfo WithNullToEmpty(bool nullToEmpty)
        {
            NullToEmpty = nullToEmpty;
            return this;
        }


        public Func<ITypeNameResolver, string>  GetCoalesceExpression           { get; set; }
        public bool                             PropertyValueIsNotNull          { get; set; }
        public bool                             NullToEmpty                     { get; protected set; }
        public BinaryExpressionDelegate<string> GetEqualsExpression             { get; set; }
        public BinaryExpressionDelegate<ExpressionWithObjectInstance>      GetRelationalComparerExpression { get; protected set; }
        public UnaryExpressionDelegate          GetHashCodeExpression           { get; set; }
        public Auto.GetHashCodeOptions          GetHashCodeOption               { get; set; }
        public Type                             ResultType                      { get; }
    }

    public struct GetHashCodeExpressionData
    {
        public GetHashCodeExpressionData(string code, bool needBracketsWhenInExpression = false)
        {
            Code                         = code;
            NeedBracketsWhenInExpression = needBracketsWhenInExpression;
        }

        public string GetCode(bool addBrackets)
        {
            return addBrackets ? BracketsCode : Code;
        }

        public string BracketsCode => NeedBracketsWhenInExpression ? $"({Code})" : Code;

        public string Code                         { get; }
        public bool   NeedBracketsWhenInExpression { get; }
    }

    public delegate T BinaryExpressionDelegate<out T>(BinaryExpressionDelegateArgs input);

    public delegate string UnaryExpressionDelegate(UnaryExpressionDelegateArgs input);

    public struct BinaryExpressionDelegateArgs : IExpressionDelegateArgs
    {
        public BinaryExpressionDelegateArgs(string left, string right, ITypeNameResolver resolver, Type dataType)
        {
            Left     = left;
            Right    = right;
            Resolver = resolver;
            DataType = dataType;
        }

        public string[] GetArguments()
        {
            return new[] {Left, Right};
        }

        public BinaryExpressionDelegateArgs Transform(Func<string, string> map)
        {
            return new BinaryExpressionDelegateArgs(map(Left), map(Right), Resolver, DataType);
        }

        public BinaryExpressionDelegateArgs WithLeftRight(string newLeft, string newRight)
        {
            return new BinaryExpressionDelegateArgs(newLeft, newRight, Resolver, DataType);
        }

        public string            Left     { get; }
        public string            Right    { get; }
        public ITypeNameResolver Resolver { get; }
        public Type              DataType { get; }
    }

    public struct UnaryExpressionDelegateArgs : IExpressionDelegateArgs
    {
        public UnaryExpressionDelegateArgs(string argument, ITypeNameResolver resolver, Type dataType)
        {
            Argument = argument;
            Resolver = resolver;
            DataType = dataType;
        }

        public string[] GetArguments()
        {
            return new[] {Argument};
        }

        public string            Argument { get; }
        public ITypeNameResolver Resolver { get; }
        public Type              DataType { get; }
    }
}