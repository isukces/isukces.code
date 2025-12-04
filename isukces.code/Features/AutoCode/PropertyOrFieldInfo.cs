using System;
using System.Reflection;
using iSukces.Code.Interfaces;
using JetBrains.Annotations;

namespace iSukces.Code.AutoCode;

public struct PropertyOrFieldInfo
{
    public PropertyOrFieldInfo(string name, Type valueType, MemberInfo member)
    {
        Name      = name;
        ValueType = valueType;
        Member    = member;
    }

    public static PropertyOrFieldInfo FromField(FieldInfo field)
    {
        return new PropertyOrFieldInfo(field.Name, field.FieldType, field);
    }

    public static PropertyOrFieldInfo FromProperty(PropertyInfo prop)
    {
        return new PropertyOrFieldInfo(prop.Name, prop.PropertyType, prop);
    }

    public override string ToString()
    {
        return $"{ValueType} {Name} from {Member.DeclaringType}";
    }

    public string     Name      { get; }
    public Type       ValueType { get; }
    public MemberInfo Member    { get; }

    public bool IsCompareByReference
    {
        get
        {
            var typeInfo = ValueType.GetTypeInfo();
            if (Member.GetCustomAttribute<CompareByReferenceAttribute>() is null) 
                return false;
            if (typeInfo.IsClass || typeInfo.IsInterface)
                return true;
            throw new Exception($"Use {nameof(CompareByReferenceAttribute)} only for classes and interfaces");

        }
    }

    public bool IsNullIsEmpty => Member.GetCustomAttribute<Auto.NullIsEmptyAttribute>() is not null;

    public bool IsEqualityGeneratorSkip => Member.GetCustomAttribute<Auto.EqualityGeneratorSkipAttribute>() is not null;

        
        
    public bool PropertyValueCanBeNull
    {
        get
        {
            var typeInfo         = ValueType.GetTypeInfo();
            var nullableByNature = typeInfo.IsClass || typeInfo.IsInterface;
            if (!nullableByNature)
                if (typeInfo.IsGenericType)
                {
                    var gtt = ValueType.GetGenericTypeDefinition();
                    if (gtt == typeof(Nullable<>))
                        nullableByNature = true;
                }

            if (!nullableByNature) return false;
            var ats = Member.GetCustomAttributes();
            var at  = Member.GetCustomAttribute<NotNullAttribute>();
            if (at is null)
                return true;
            return false;
        }
    }
}