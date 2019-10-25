using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using isukces.code.FeatureImplementers;
using isukces.code.interfaces;
using JetBrains.Annotations;

namespace isukces.code.AutoCode
{
    public partial class Generators
    {
        /// <summary>
        ///     Generuje IEquatable i IComparable
        /// </summary>
        public class EqualityGenerator : IAutoCodeGenerator
        {
            public EqualityGenerator([NotNull] IMemberNullValueChecker nullChecker) =>
                NullChecker = nullChecker ?? throw new ArgumentNullException(nameof(nullChecker));

            private static CsExpression GetCoalesceExpressionForType(Type type)
            {
                if (type == typeof(int?) || type == typeof(int) ||
                    type == typeof(double?) || type == typeof(double))
                    return 0;
                return new CsExpression("default");
            }

            private static bool GetGetHashCodeForEnumType(Type type, CsExpression propNameExpression,
                out GetHashCodeExpressionData getHashCodeExpressionData)
            {
                var stripped = type.StripNullable();
                var ut       = Enum.GetUnderlyingType(stripped);
                if (ut == typeof(int))
                {
                    var values = Enum.GetValues(stripped).Cast<int>().ToArray();

                    var expr = CsExpression.TypeCast("int", propNameExpression);
                    if (type != stripped)
                    {
                        //    expr = propNameExpression.Is("null")
                        //       .Conditional(0, CsExpression.TypeCast("int", propNameExpression.CallProperty("Value")));
                        var c = 0;
                        if (values.Any())
                            c = values.Min();
                        expr = CsExpression.TypeCast("int?", propNameExpression).Coalesce(c);
                    }

                    // var qq = q is null ? 0 : (int)q.Value;

                    if (values.Length == 0)
                    {
                        getHashCodeExpressionData = expr;
                        return true;
                    }

                    var  min = values.Min();
                    int? max = values.Max();
                    if (min.Equals(max))
                        max = null;
                    {
                        getHashCodeExpressionData = new GetHashCodeExpressionData(expr, min, max);
                        return true;
                    }
                }

                getHashCodeExpressionData = null;
                return false;
            }

            private static bool IsFloatNumber(Type type) =>
                type == typeof(double) || type == typeof(decimal) || type == typeof(float);

            private static bool IsSimpleType(Type type)
            {
                if (type == typeof(int)) return true;
                if (type == typeof(uint)) return true;
                if (type == typeof(long)) return true;
                if (type == typeof(ulong)) return true;
                if (type == typeof(byte)) return true;
                if (type == typeof(sbyte)) return true;
                if (type == typeof(double)) return true;
                if (type == typeof(decimal)) return true;
                if (type == typeof(float)) return true;
                if (type == typeof(string)) return true;
                if (type == typeof(bool)) return true;
                if (type.GetTypeInfo().IsEnum) return true;
                return false;
            }

            public void Generate(Type type, IAutoCodeGeneratorContext context)
            {
                var typeInfo = type.GetTypeInfo();
                _attEq   = typeInfo.GetCustomAttribute<Auto.EqualityGeneratorAttribute>();
                _attComp = typeInfo.GetCustomAttribute<Auto.ComparerGeneratorAttribute>();
                if (_attEq is null && _attComp is null)
                    return;
                _type = type;
                // context.AddNamespace("Pd.Interfaces");
                context.AddNamespace("System");
                _class = context.GetOrCreateClass(type);
                if (_attEq != null)
                {
                    var eqType = typeof(IAutoEquatable<>).MakeGenericType(type);
                    _class.ImplementedInterfaces.Add(_class.GetTypeName(eqType));
                }

                if (_attComp != null)
                {
                    var eqType = typeof(IAutoComparable<>).MakeGenericType(type);
                    _class.ImplementedInterfaces.Add(_class.GetTypeName(eqType));
                }

                _canBeNull = !NullChecker.TypeIsAlwaysNotNull(type);

                var properties = typeInfo.GetProperties(GeneratorsHelper.AllInstance);
                var fields = string.IsNullOrEmpty(_attEq?.UseOnlyPropertiesOrFields)
                    ? new FieldInfo[0]
                    : typeInfo.GetFields(GeneratorsHelper.AllInstance);
                _props = properties.Select(PropertyOrFieldInfo.FromProperty)
                    .Concat(fields.Select(PropertyOrFieldInfo.FromField))
                    .ToArray();

                _props = FilterProperties(_props, _attEq);

                var implementer = new EqualityFeatureImplementer(_class, type)
                {
                    IsEmptyObjectPropertyName = IsEmptyObjectPropertyName,
                    CachedGetHashCodeImplementation =
                        _attEq?.CachedGetHashCodeImplementation ?? GetHashCodeImplementationKind.Normal,
                    UseGetHashCodeInEqualityChecking = _attEq?.UseGetHashCodeInEqualityChecking ?? false,
                    CanBeNull                        = _canBeNull,
                    ImplementFeatures                = GetImplementFeatures()
                }.WithCompareToExpressions(GetCompareToExpressions());
                if (_attEq != null)
                {
                    var otherName = implementer.OtherArgName;
                    var equalityExpressions = _props
                        .Select(a =>
                        {
                            var template = FindEqualsCode(a);
                            var code     = template.Code.Format(a.Name, otherName);
                            return new EqualsExpressionData(code, template.Cost);
                        })
                        .OrderBy(a => a.Cost);
                    implementer = implementer
                        .WithEqualityExpressions(equalityExpressions)
                        .WithGetHashCodeExpressions(GetGetHashCodeExpressions());
                }

                implementer.CreateCode();
                _attEq = null;
                _class = null;
            }

            protected virtual IEqualityGeneratorPropertyInfo CreatePropertyInfo(PropertyOrFieldInfo prop)
            {
                // tworzę i wypełniam implementacje to ręcznie
                var info = new EqualityGeneratorPropertyInfo(prop.ValueType)
                    .WithMemberAttributes(prop.Member);
                info.PropertyValueIsNotNull = NullChecker.ReturnValueAlwaysNotNull(prop.Member);
                info.GetCoalesceExpression  = q => GetCoalesceExpressionForType(info.ResultType);
                if (info.ResultType.IsNullableType())
                    info.GetEqualsExpression = input =>
                    {
                        if (info.PropertyValueIsNotNull)
                            return GeneratorsHelper.CallMethod("Equals",
                                input.Left.CallProperty("Value"),
                                input.Right.CallProperty("Value"));
                        if (info.NullToEmpty)
                            input.Left.CallMethod("Equals", input.Right);
                        return (CsExpression)$"Equals({input.Left}, {input.Right})";
                    };
                else
                    info.GetEqualsExpression = input =>
                    {
                        if (info.PropertyValueIsNotNull)
                            return input.Left.CallMethod("Equals", input.Right);
                        return GeneratorsHelper.CallMethod("Equals", input.Left, input.Right);
                    };
                info.GetHashCodeExpression = input => (CsExpression)"{0}.GetHashCode()";
                return info;
            }

            protected virtual ExpressionWithObjectInstance FindCompareToCode(string fieldOrPropertyName)
            {
                var me1  = (CsExpression)fieldOrPropertyName;
                var oth1 = (CsExpression)("other." + fieldOrPropertyName);

                BinaryExpressionDelegateArgs CoalesceInput(IEqualityGeneratorPropertyInfo info,
                    BinaryExpressionDelegateArgs binaryExpressionDelegateArgs)
                {
                    return binaryExpressionDelegateArgs.Transform(a => info.Coalesce(a, _class));
                }

                PropertyOrFieldInfo FindPropertyOrFieldInfo()
                {
                    var p = _props.Where(a => a.Name == fieldOrPropertyName).ToArray();
                    if (p.Length == 1)
                        return p[0];

                    var pi = _type.GetTypeInfo().GetProperty(fieldOrPropertyName, GeneratorsHelper.AllInstance);
                    if (!(pi is null))
                        return PropertyOrFieldInfo.FromProperty(pi);
                    var fi = _type.GetTypeInfo().GetField(fieldOrPropertyName, GeneratorsHelper.AllInstance);
                    if (fi is null)
                        throw new Exception("Unable to find field nor property " + fieldOrPropertyName);
                    return PropertyOrFieldInfo.FromField(fi);
                }

                var property = FindPropertyOrFieldInfo();

                var type = property.ValueType;
                var input = new BinaryExpressionDelegateArgs(me1, oth1,
                    _class, property.ValueType);
                if (type == typeof(string))
                {
                    var info = EqualityGeneratorPropertyInfo.FindForString(property.Member, NullChecker);
                    // nulls are always equal for comparable for string 
                    if (info.NullToEmpty)
                        input = CoalesceInput(info, input);
                    return info.GetRelationalComparerExpression(input);
                }
                else
                {
                    var info = EqualityGeneratorPropertyInfo.Find(property.Member, NullChecker);
                    if (info != null) return info.GetRelationalComparerExpression(input);
                }

                if (!property.PropertyValueCanBeNull)
                    //return me1.CallMethod("CompareTo", oth1);
                    return new ExpressionWithObjectInstance(me1.CallMethod("CompareTo", oth1));
                //   $"{fieldOrPropertyName}.CompareTo(other.{fieldOrPropertyName})");

                {
                    var info = CreatePropertyInfo(property);
                    var m    = GeneratorsHelper.DefaultComparerMethodName(type, _class);
                    {
                        if (info.PropertyValueIsNotNull)
                        {
                            if (info.ResultType.IsNullableType())
                            {
                                // nullable but not null
                                m     = GeneratorsHelper.DefaultComparerMethodName(type.StripNullable(), _class);
                                input = input.Transform(a => a.CallProperty("Value"));
                            }
                        }
                        else
                        {
                            if (info.NullToEmpty)
                            {
                                input = CoalesceInput(info, input);
                                var sType  = type.StripNullable();
                                var interf = typeof(IComparable<>).MakeGenericType(sType);
                                if (interf.GetTypeInfo().IsAssignableFrom(sType))
                                    // $"({input.Left}).CompareTo({input.Right})"
                                    return new ExpressionWithObjectInstance(
                                        input.Left.CallMethod("CompareTo", input.Right));
                                m = GeneratorsHelper.DefaultComparerMethodName(sType, _class);
                            }
                        }
                    }
                    return new ExpressionWithObjectInstance(GeneratorsHelper.CallMethod(m.ExpressionTemplate, input),
                        m.Instance);
                }
            }

            protected virtual EqualsExpressionData FindEqualsCode(PropertyOrFieldInfo property)
            {
                const string twoArgs = "({0}, {1}.{0})";
                var          type    = property.ValueType;
                if (type == typeof(string))
                {
                    var info = EqualityGeneratorPropertyInfo.FindForString(property.Member, NullChecker);
                    var code = info.EqualsCode1(_class);
                    return code.WithCost(Auto.EqualityCostAttribute.String);
                }

                {
                    var info = EqualityGeneratorPropertyInfo.Find(property.Member, NullChecker);
                    if (info != null)
                    {
                        var code = info.EqualsCode1(_class);
                        return code.WithCost(Auto.EqualityCostAttribute.String);
                    }
                }
                if (!IsFloatNumber(type) && IsSimpleType(type))
                    return new EqualsExpressionData(
                        new CsExpression("{0} == {1}.{0}", CsOperatorPrecendence.Equality),
                        GetEqualsCost(type, false));

                if (property.IsCompareByReference)
                    return new EqualsExpressionData((CsExpression)(nameof(ReferenceEquals) + twoArgs), 1);
                var typeInfo = type.GetTypeInfo();
                if (typeInfo.IsGenericType)
                {
                    var gtt   = type.GetGenericTypeDefinition();
                    var types = typeInfo.GetGenericArguments();

                    if (gtt != typeof(Nullable<>))
                        throw new NotSupportedException(type.ToString());
                    {
                        // nullable jako Nazwa is null ? other.Nazwa is null : Nazwa.Value.Equals(other.Nazwa)
                        var arg  = types[0];
                        var cost = GetEqualsCost(arg, true);
                        var info = CreatePropertyInfo(property);
                        var code = info.EqualsCode1(_class);
                        return code.WithCost(cost);
                    }
                }

                {
                    var info = CreatePropertyInfo(property);
                    var code = info.EqualsCode1(_class);
                    var cost = GetEqualsCost(property.ValueType, false);
                    return code.WithCost(cost);
                }
            }

            protected virtual GetHashCodeExpressionData FindGetHashCode(PropertyOrFieldInfo prop)
            {
                var type               = prop.ValueType;
                var typeStripped       = type.StripNullable();
                var propName           = prop.Name;
                var propNameExpression = (CsExpression)propName;
                if (type == typeof(string))
                {
                    var info = EqualityGeneratorPropertyInfo.FindForString(prop.Member, NullChecker);
                    return info.GetHash(propNameExpression, _class);
                }

                {
                    var info = EqualityGeneratorPropertyInfo.Find(prop.Member, NullChecker);
                    if (info != null)
                        return info.GetHash(propNameExpression, _class);
                }
                if (typeStripped.GetTypeInfo().IsEnum)
                    if (GetGetHashCodeForEnumType(type, propNameExpression, out var getHashCodeExpressionData))
                        return getHashCodeExpressionData;

                if (type == typeof(int)) return propNameExpression;
                if (type == typeof(int?)) return propNameExpression.Coalesce((CsExpression)"0");
                if (type == typeof(bool) || type == typeof(bool?))
                {
                    var e = propNameExpression;
                    if (type == typeof(bool?))
                        e = new CsExpression.Binary(e, true, CsOperatorPrecendence.Equality, "==");
                    e = e.Conditional(1, 0);
                    return new GetHashCodeExpressionData(e, 0, 1);
                }

                {
                    var info = CreatePropertyInfo(prop);
                    if (info.NullToEmpty && !info.PropertyValueIsNotNull)
                    {
                        var result = info.Coalesce(propNameExpression, _class).CallMethod("GetHashCode");
                        return result;
                    }

                    var hc = info.PropertyValueIsNotNull
                        ? propNameExpression.CallMethod("GetHashCode")
                        : propNameExpression.OptionalNull().CallMethod("GetHashCode").Coalesce(0);
                    return new GetHashCodeExpressionData(hc);
                }
            }

            protected virtual int GetEqualsCost(Type type, bool nullable)
            {
                var offset = nullable ? 5 : 0;

                if (type == typeof(bool)) return Auto.EqualityCostAttribute.Bool + offset;
                if (type == typeof(int)) return Auto.EqualityCostAttribute.Int + offset;
                if (type == typeof(uint)) return Auto.EqualityCostAttribute.Int + offset;
                if (type == typeof(long)) return Auto.EqualityCostAttribute.Int + offset;
                if (type == typeof(ulong)) return Auto.EqualityCostAttribute.Int + offset;
                if (type == typeof(byte)) return Auto.EqualityCostAttribute.Int + offset;
                if (type == typeof(sbyte)) return Auto.EqualityCostAttribute.Int + offset;
                if (type == typeof(double)) return Auto.EqualityCostAttribute.Float + offset;
                if (type == typeof(decimal)) return Auto.EqualityCostAttribute.Decimal + offset;
                if (type == typeof(float)) return Auto.EqualityCostAttribute.Float + offset;
                var typeInfo = type.GetTypeInfo();
                if (typeInfo.IsEnum)
                    return Auto.EqualityCostAttribute.Int + offset;
                var at = typeInfo.GetCustomAttribute<Auto.EqualityCostAttribute>();
                if (at != null)
                    return at.Cost;
                return 100;
            }

            protected virtual bool IsToHeavyToGetHashCode(Type type)
            {
                bool CheckInterface(Type gt) => gt == typeof(IReadOnlyList<>) || gt == typeof(IEnumerable<>);

                type = type.StripNullable();
                if (IsSimpleType(type))
                    return false;
                if (type.IsArray)
                    return true;

                var typeInfo = type.GetTypeInfo();

                if (typeInfo.IsGenericType)

                    type = type.GetGenericTypeDefinition();
                if (CheckInterface(type))
                    return true;
                foreach (var i in typeInfo.GetInterfaces())
                    if (CheckInterface(i))
                        return true;
                return false;
            }

            private bool AcceptProperty(Auto.EqualityGeneratorAttribute att, PropertyOrFieldInfo prop,
                HashSet<string> useOnly)
            {
                bool Internal()
                {
                    if (!string.IsNullOrEmpty(att.IsEmptyProperty))
                        if (att.IsEmptyProperty == prop.Name)
                            return false;
                    if (useOnly != null)
                        return useOnly.Contains(prop.Name);
                    if (SkipPropertiesDependsOnPropertyAttribute)
                    {
                        if (prop.Member.GetCustomAttribute<DependsOnPropertyAttribute>() != null)
                            return false;
                    }
                    return !prop.IsEqualityGeneratorSkip;
                }

                var result = Internal();
                var h      = OnAcceptProperty;
                if (h is null)
                    return result;
                var args = new AcceptPropertyEventArgs(att, prop, useOnly, result);
                h(this, args);
                return args.Accept;
            }

            private PropertyOrFieldInfo[] FilterProperties(PropertyOrFieldInfo[] props,
                [CanBeNull] Auto.EqualityGeneratorAttribute att)
            {
                if (att is null)
                    return new PropertyOrFieldInfo[0];

                var useOnly = !string.IsNullOrEmpty(att.UseOnlyPropertiesOrFields)
                    ? att.UseOnlyPropertiesOrFields.Split(',').ToHashSet()
                    : null;

                return props.Where(a => AcceptProperty(att, a, useOnly)).ToArray();
            }

            private IEnumerable<CompareToExpressionData> GetCompareToExpressions()
            {
                var fields2 = _attComp?.Fields;
                if (fields2 is null || fields2.Length == 0) yield break;

                for (var index = 0; index < fields2.Length; index++)
                {
                    var fieldName         = fields2[index];
                    var compareExpression = FindCompareToCode(fieldName);
                    yield return new CompareToExpressionData(fieldName, compareExpression.ExpressionTemplate,
                        compareExpression.Instance);
                }
            }

            private IEnumerable<GetHashCodeExpressionDataWithMemberInfo> GetGetHashCodeExpressions()
            {
                var filter = _attEq?.GetHashCodeProperties.ToHashSet();
                for (var i = 0; i < _props.Length; i++)
                {
                    var prop = _props[i];
                    if (filter != null && filter.Any() && !filter.Contains(prop.Name))
                        continue;
                    if (IsToHeavyToGetHashCode(prop.ValueType))
                        continue;
                    var hc = FindGetHashCode(prop);
                    yield return new GetHashCodeExpressionDataWithMemberInfo(prop.Member, hc);
                }
            }

            private EqualityFeatureImplementer.Features GetImplementFeatures()
            {
                var result = _attEq != null
                    ? EqualityFeatureImplementer.Features.Equality
                    : EqualityFeatureImplementer.Features.None;
                return _attComp is null
                    ? result
                    : result | EqualityFeatureImplementer.Features.CompareTo |
                      EqualityFeatureImplementer.Features.CompareOperators;
            }

            protected IMemberNullValueChecker NullChecker { get; }

            private string IsEmptyObjectPropertyName
            {
                get { return _attEq?.IsEmptyProperty; }
            }

            [CanBeNull] private Auto.ComparerGeneratorAttribute _attComp;

            [CanBeNull] private Auto.EqualityGeneratorAttribute _attEq;

            private bool _canBeNull;
            private CsClass _class;
            private PropertyOrFieldInfo[] _props;
            private Type _type;
            
            
            /// <summary>
            /// Do not check equality by properties marked with DependsOnPropertyAttribute
            /// </summary>
            public bool SkipPropertiesDependsOnPropertyAttribute { get; set; }
            

            public event EventHandler<AcceptPropertyEventArgs> OnAcceptProperty;

            public class AcceptPropertyEventArgs:EventArgs
            {
                public AcceptPropertyEventArgs(Auto.EqualityGeneratorAttribute attribute, PropertyOrFieldInfo property,
                    HashSet<string> useOnly, bool defaultAnswer)
                {
                    Attribute     = attribute;
                    Property      = property;
                    UseOnly       = useOnly;
                    DefaultAnswer = defaultAnswer;
                    Accept        = defaultAnswer;
                }


                public Auto.EqualityGeneratorAttribute Attribute     { get; }
                public PropertyOrFieldInfo             Property      { get; }
                public HashSet<string>                 UseOnly       { get; }
                public bool                            DefaultAnswer { get; }

                public bool Accept { get; set; }
            }
        }
    }


    public interface IAutoEquatable<T> : IEquatable<T>
    {
    }

    public interface IAutoComparable<in T> : IComparable<T>, IComparable
    {
    }
}