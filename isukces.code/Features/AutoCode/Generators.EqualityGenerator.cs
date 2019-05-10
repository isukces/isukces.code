using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            public EqualityGenerator([NotNull] IMemberNullValueChecker nullChecker)
            {
                NullChecker = nullChecker ?? throw new ArgumentNullException(nameof(nullChecker));
            }
            /*
            private static bool CanBeNull(PropertyOrFieldInfo prop)
            {
#if DEBUGx
            if (prop.Member is PropertyInfo pi)
                if (pi.PropertyType == typeof(Vector?))
                    Debug.Write("");
#endif
                if (prop.Member.GetCustomAttribute<NotNullAttribute>() != null)
                    return false;
                return prop.CanBeNull;
            }
            */


            private static PropertyOrFieldInfo[] FilterProperties(PropertyOrFieldInfo[] props,
                [CanBeNull] Auto.EqualityGeneratorAttribute att)
            {
                if (att is null)
                    return new PropertyOrFieldInfo[0];

                var useOnly = !string.IsNullOrEmpty(att.UseOnlyPropertiesOrFields)
                    ? att.UseOnlyPropertiesOrFields.Split(',').ToHashSet()
                    : null;

                return props.Where(a =>
                {
                    if (!string.IsNullOrEmpty(att.IsEmptyProperty))
                        if (att.IsEmptyProperty == a.Name)
                            return false;
                    if (useOnly != null)
                        return useOnly.Contains(a.Name);
                    return !att.SkipProperties.Contains(a.Name);
                }).ToArray();
            }

            private static bool IsFloatNumber(Type type)
            {
                return type == typeof(double) || type == typeof(decimal) || type == typeof(float);
            }

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
                    IsEmptyObjectPropertyName       = IsEmptyObjectPropertyName,
                    CachedGetHashCodeImplementation = _attEq?.CachedGetHashCodeImplementation ?? false,
                    CanBeNull                       = _canBeNull,
                    ImplementFeatures               = GetImplementFeatures()
                }.WithCompareToExpressions(GetCompareToExpressions());
                if (_attEq != null)
                {
                    var otherName = implementer.OtherArgName;
                    var equalityExpressions = _props
                        .Select(a =>
                        {
                            var template = GetEqualityCodeForMember(a);
                            var code     = string.Format(template.Code, a.Name, otherName);
                            return new CodePieceWithComputeCost(code, template.Cost, template.Brackets);
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

            protected virtual bool CanBeCompared(Type type)
            {
                type = type.StripNullable();
                if (IsSimpleType(type)) return true;
                //if (type == typeof(Point)) return true;
                //if (type == typeof(Vector)) return true;
                //if (type == typeof(IPerson)) return true;
                //if (type == typeof(ICompany)) return true;

                bool IsIequatable(Type t)
                {
                    return t.GetTypeInfo().IsGenericType && t.GetGenericTypeDefinition() == typeof(IEquatable<>);
                }

                if (IsIequatable(type))
                    return true;
                foreach (var i in type.GetTypeInfo().GetInterfaces())
                    if (IsIequatable(i))
                        return true;
                return false;
            }

            protected virtual int GetCost(Type type, bool nullable)
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


            protected virtual CodePieceWithComputeCost GetEqualityCodeForMember(PropertyOrFieldInfo property)
            {
                const string twoArgs = "({0}, {1}.{0})";

                string CallMethod(string mn, string otherArgs = null)
                {
                    return $"{{0}}.{mn}({{1}}.{{0}}{otherArgs})";
                }

                var type = property.ValueType;
                if (type == typeof(string))
                {
                    var info = EqualityGeneratorPropertyInfo.FindForString(property.Member, NullChecker);
                    var code = info.EqualsCode("{0}", "{1}.{0}", _class);
                    return new CodePieceWithComputeCost(code, Auto.EqualityCostAttribute.String);
                }

                {
                    var info = EqualityGeneratorPropertyInfo.Find(property.Member, NullChecker);
                    if (info != null)
                    {
                        var code = info.EqualsCode("{0}", "{1}.{0}", _class);
                        return new CodePieceWithComputeCost(code, Auto.EqualityCostAttribute.String);
                    }
                }
                if (!IsFloatNumber(type) && IsSimpleType(type))
                    return new CodePieceWithComputeCost("{0} == {1}.{0}", GetCost(type, false));

                if (property.CanBeNull && property.IsCompareByReference)
                    return new CodePieceWithComputeCost(nameof(ReferenceEquals) + twoArgs, 1);
                var typeInfo = type.GetTypeInfo();
                if (typeInfo.IsGenericType)
                {
                    var gtt   = type.GetGenericTypeDefinition();
                    var types = typeInfo.GetGenericArguments();

                    if (gtt != typeof(Nullable<>))
                        throw new NotSupportedException(type.ToString());
                    {
                        // nullable jako Nazwa is null ? other.Nazwa is null : Nazwa.Value.Equals(other.Nazwa)
                        var          arg  = types[0];
                        var          cost = GetCost(arg, true);
                        const string code = "{0} is null ? {1}.{0} is null : {0}.Value.Equals({1}.{0})";
                        return new CodePieceWithComputeCost(code, cost, true);
                    }
                }

                if (!CanBeCompared(type))
                    throw new Exception(string.Format(
                        "Unable to create equality code for type {0}. Override method {1} or {2}",
                        type, nameof(CanBeCompared), nameof(GetEqualityCodeForMember)));

                return new CodePieceWithComputeCost(CallMethod("Equals"), GetCost(type, false));
            }

            protected virtual bool ToHeavyToGetHashCode(Type type)
            {
                bool CheckInterface(Type gt)
                {
                    return gt == typeof(IReadOnlyList<>) || gt == typeof(IEnumerable<>);
                }

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


            private GetHashCodeExpressionData FindHashCode(PropertyOrFieldInfo prop)
            {
                var type     = prop.ValueType;
                var propName = prop.Name;
                if (type == typeof(string))
                {
                    var info = EqualityGeneratorPropertyInfo.FindForString(prop.Member, NullChecker);
                    return info.GetHash(propName, _class);
                }

                {
                    var info = EqualityGeneratorPropertyInfo.Find(prop.Member, NullChecker);
                    if (info != null)
                        return info.GetHash(propName, _class);
                }
                if (type.GetTypeInfo().IsEnum)
                {
                    var ut = Enum.GetUnderlyingType(type);
                    if (ut == typeof(int))
                        return new GetHashCodeExpressionData("(int)" + propName);
                }

                if (type == typeof(int)) return new GetHashCodeExpressionData(propName);
                if (type == typeof(bool)) return new GetHashCodeExpressionData( propName + " ? 1 : 0", true);
                if (type == typeof(bool?)) return new GetHashCodeExpressionData(propName + " == true ? 1 : 0", true);
                

                var hc = NullChecker.ReturnValueAlwaysNotNull(prop.Member)
                    ? $"{propName}.GetHashCode()"
                    : $"{propName}?.GetHashCode()  ?? 0";
                return new GetHashCodeExpressionData(hc);
            }

            private string GetCompareCode(string name)
            {
                var                 p = _props.Where(a => a.Name == name).ToArray();
                PropertyOrFieldInfo property;
                if (p.Length == 1)
                    property = p[0];
                else
                {
                    var pi = _type.GetTypeInfo().GetProperty(name, GeneratorsHelper.AllInstance);
                    if (pi is null)
                    {
                        var fi = _type.GetTypeInfo().GetField(name, GeneratorsHelper.AllInstance);
                        if (fi is null)
                            throw new Exception("Unable to find field nor property " + name);
                        property = PropertyOrFieldInfo.FromField(fi);
                    }
                    else
                    {
                        property = PropertyOrFieldInfo.FromProperty(pi);
                    }
                }

                var type = property.ValueType;
                var input = new BinaryExpressionDelegateArgs(name, "other." + name, _class, property.ValueType);
                if (type == typeof(string))
                {
                    var info = EqualityGeneratorPropertyInfo.FindForString(property.Member, NullChecker);
                    // name, "other." + name, _class,property.ValueType
                    
                    return info.GetRelationalComparerExpression(input);
                }
                else
                {
                    var info = EqualityGeneratorPropertyInfo.Find(property.Member, NullChecker);
                    if (info != null)
                    {
                        return info.GetRelationalComparerExpression(input);
                    }
                }
 
                if (!property.CanBeNull)
                    return $"{name}.CompareTo(other.{name})";

                var m = GeneratorsHelper.DefaultComparerMethodName(type, _class);
                return GeneratorsHelper.CallMethod(m, input);// $"{m}({input.Left}, other.{input.Right})";
            }

            private IEnumerable<EqualityFeatureImplementer.C1> GetCompareToExpressions()
            {
                var fields2 = _attComp?.Fields;
                if (fields2 is null || fields2.Length == 0) yield break;

                for (var index = 0; index < fields2.Length; index++)
                {
                    var fieldName         = fields2[index];
                    var compareExpression = GetCompareCode(fieldName);
                    yield return new EqualityFeatureImplementer.C1(fieldName, compareExpression);
                }
            }

            private IEnumerable<GetHashCodeExpressionData> GetGetHashCodeExpressions()
            {
                HashSet<string> filter = null;
                if (!string.IsNullOrEmpty(_attEq?.GetHashCodeProperties))
                    filter = _attEq.GetHashCodeProperties.Split(',').ToHashSet();
                for (var i = 0; i < _props.Length; i++)
                {
                    var prop = _props[i];
                    if (filter != null && !filter.Contains(prop.Name))
                        continue;
                    if (ToHeavyToGetHashCode(prop.ValueType))
                        continue;
                    var hc = FindHashCode(prop);
                    yield return hc;
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

            private string IsEmptyObjectPropertyName => _attEq?.IsEmptyProperty;


            [CanBeNull] private Auto.ComparerGeneratorAttribute _attComp;

            [CanBeNull] private Auto.EqualityGeneratorAttribute _attEq;

            private bool _canBeNull;
            private CsClass _class;
            private PropertyOrFieldInfo[] _props;
            private Type _type;
        }
    }


    public interface IAutoEquatable<T> : IEquatable<T>
    {
    }

    public interface IAutoComparable<in T> : IComparable<T>, IComparable
    {
    }
}