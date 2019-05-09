using System;
using System.Reflection;
using isukces.code.interfaces;
using JetBrains.Annotations;

namespace isukces.code.AutoCode
{
    public class EqualityGeneratorPropertyInfo
    {
        private EqualityGeneratorPropertyInfo(Type resultType)
        {
            _resultType = resultType;
        }

        [CanBeNull]
        public static EqualityGeneratorPropertyInfo Find(MemberInfo member,
            IMemberNullValueChecker checker)
        {
            var sca = member.GetCustomAttribute<Auto.AbstractEqualityComparisonAttribute>();
            if (sca == null)
                return null;
            var info = new EqualityGeneratorPropertyInfo(GeneratorsHelper.GetMemberResultType(member))
                .With(sca)
                .WithNullToEmpty(member.GetCustomAttribute<Auto.NullIsEmptyAttribute>() != null);
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

            string DefaultGetCompareTo(BinaryExpressionDelegateArgs input)
            {
                return Call("Compare", input);
            }

            var a = new EqualityGeneratorPropertyInfo(typeof(string))
                {
                    GetEqualsExpression             = DefaultEquals,
                    GetRelationalComparerExpression = DefaultGetCompareTo,
                    GetHashCodeExpression           = DefaultGetHashCode,
                    GetCoalesceExpression           = _ => GeneratorsHelper.StringEmpty
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

            return GetEqualsExpression(new BinaryExpressionDelegateArgs(left, right, resolver, _resultType));
        }


        public string GetHash(string propName, ITypeNameResolver resolver)
        {
            if (!PropertyValueIsNotNull)
                propName = Coalesce(propName, resolver);
            return GetHashCodeExpression(new UnaryExpressionDelegateArgs(propName, resolver, _resultType));
        }

        private EqualityGeneratorPropertyInfo With(Auto.AbstractEqualityComparisonAttribute sca)
        {
            if (sca == null)
                return this;
            GetCoalesceExpression           = sca.GetCoalesceExpression;
            GetEqualsExpression             = sca.GetEqualsExpression;
            GetHashCodeExpression           = sca.GetHashCodeExpression;
            GetRelationalComparerExpression = sca.GetRelationalComparerExpression;
            return this;
        }

        private EqualityGeneratorPropertyInfo WithNullToEmpty(bool nullToEmpty)
        {
            NullToEmpty = nullToEmpty;
            return this;
        }


        public Func<ITypeNameResolver, string> GetCoalesceExpression           { get; set; }
        public bool                            PropertyValueIsNotNull          { get; set; }
        public bool                            NullToEmpty                     { get; protected set; }
        public BinaryExpressionDelegate        GetEqualsExpression             { get; protected set; }
        public BinaryExpressionDelegate        GetRelationalComparerExpression { get; protected set; }
        public UnaryExpressionDelegate         GetHashCodeExpression           { get; protected set; }
        private readonly Type _resultType;
    }

    public delegate string BinaryExpressionDelegate(BinaryExpressionDelegateArgs input);

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