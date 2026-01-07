using System;
using System.Collections.Generic;
using System.Linq;
using iSukces.Code.Interfaces;

namespace iSukces.Code.FeatureImplementers;

public enum EqualityOperators
{
    Equal,
    NotEqual
}

public class KeyImplementer
{
    private const string HasValueFieldName = "_hasValue";

    public override string ToString()
    {
        return $"Maker {_cl.Name.Declaration} {_primitiveDeclaration}";
    }

    public KeyImplementer(CsClass cl, CsType primitive)
    {
        _cl                   = cl;
        if ((cl.Formatting.Flags & CodeFormattingFeatures.PropertyBackField) != 0)
            BackingField = PropertyBackingFieldRequest.UseIfPossible;
        _primitive            = cl.Reduce(primitive);
        Equality              = new BaseEqualityFeatureImplementer(cl);
        _primitiveDeclaration = primitive.Declaration;
        Special = _primitiveDeclaration switch
        {
            "string"                                                        => SpecialType.String,
            "int"                                                           => SpecialType.Int,
            "Guid" when cl.GetNamespaceInfo("System").IsKnownWithoutAlias() => SpecialType.Guid,
            "System.Guid"                                                   => SpecialType.Guid,
            "long"                                                          => SpecialType.Long,
            _                                                               => SpecialType.Other
        };

       
        switch (Special)
        {
            case SpecialType.String:
                EqualsExpressionFactory = EqualMethods.StringOrdinal;
                GetHashCodeExpressionFactory  = HashMethods.StringOrdinal;
                EmptyExpression       = "string.Empty";
                break;
            case SpecialType.Int:
                EqualsExpressionFactory = EqualMethods.EqualitySign;
                GetHashCodeExpressionFactory  = a => a;
                EmptyExpression       = "int.MinValue";
                break;
            case SpecialType.Long:
                EqualsExpressionFactory = EqualMethods.EqualitySign;
                GetHashCodeExpressionFactory  = HashMethods.Simple;
                EmptyExpression       = "long.MinValue";
                break;
            case SpecialType.Guid:
                var guid = cl.GetTypeName<Guid>();
                EmptyExpression       = guid.GetMemberCode(nameof(Guid.Empty));
                EqualsExpressionFactory = EqualMethods.Equals;
                GetHashCodeExpressionFactory  = HashMethods.Simple;
                break;
            case SpecialType.Other:
            default:
                EqualsExpressionFactory = EqualMethods.Equals;
                GetHashCodeExpressionFactory  = HashMethods.Simple;
                break;
        }
    }

    public static class CompareMethods
    {
        public static string StringOrdinalIgnoreCase(string left, string right) =>
            $"StringComparer.OrdinalIgnoreCase.Compare({left}, {right})";

        public static string Comparable(string left, string right) =>
            $"{left}.CompareTo({right})";
    }
    
    public static class HashMethods
    {
        public static string StringOrdinalIgnoreCase(string value)
        {
            return $"StringComparer.OrdinalIgnoreCase.GetHashCode({value})";
        }
        public static string StringOrdinal(string value)
        {
            return $"StringComparer.Ordinal.GetHashCode({value})";
        }
        public static string Simple(string value)
        {
            return $"{value}.GetHashCode()";
        }
    }
    
    public static class EqualMethods
    {
        private static string AddNegate(string expression, bool isNegate)
        {
            return isNegate ? $"!{expression}" : expression;
        }

        public static string EqualitySign(string myValue, string otherValue, EqualityOperators oper)
        {
            var isNegate = oper == EqualityOperators.NotEqual;
            return isNegate ? $"{myValue} != {otherValue}" : $"{myValue} == {otherValue}";
        }

        public static string Equals(string myValue, string otherValue, EqualityOperators oper)
        {
            var isNegate = oper == EqualityOperators.NotEqual;
            return AddNegate($"{myValue}.Equals({otherValue})", isNegate);
        }

        public static string StringOrdinal(string myValue, string otherValue, EqualityOperators oper)
        {
            var isNegate = oper == EqualityOperators.NotEqual;
            return AddNegate($"StringComparer.Ordinal.Equals({myValue}, {otherValue})", isNegate);
        }
        
        public static string StringOrdinalIgnoreCase(string myValue, string otherValue, EqualityOperators oper)
        {
            var isNegate = oper == EqualityOperators.NotEqual;
            return AddNegate($"StringComparer.OrdinalIgnoreCase.Equals({myValue}, {otherValue})", isNegate);
        }
    }

    public void AddIEquatable(CsType? genericArgument = null)
    {
        AddGenericInterface(genericArgument, "System", "IEquatable");
    }

    public void AddIComparable(CsType? genericArgument = null)
    {
        AddGenericInterface(genericArgument, "System", "IComparable");
    }

    private void AddGenericInterface(CsType? genericArgument, string ns, string interfaceType)
    {
        var tt = _cl.GetTypeName(ns, interfaceType);
        tt.GenericParamaters = new[] { genericArgument ?? _cl.Name };
        _cl.ImplementedInterfaces.Add(tt);
    }

    public CsMethod GuidNew(string name = "NewId")
    {
        return _cl.AddMethod(name, _cl.Name)
            .WithStatic()
            .WithBodyAsExpression(_cl.Name.New("Guid.NewGuid()"));
    }

    public CsMethod GuidParse(string name = "Parse")
    {
        var m = _cl.AddMethod(name, _cl.Name)
            .WithStatic()
            .WithBodyAsExpression(_cl.Name.New("Guid.Parse(guid)"));
        m.Parameters.Add(new CsMethodParameter("guid", CsType.String));
        return m;
    }

    public CsMethod OperatorBool(CsType left, CsType right, string op)
    {
        return Operator(left, right, CsType.Bool, op);
    }

    public CsMethod Operator(CsType left, CsType right, CsType result, string op)
    {
        var m = _cl.AddMethod(op, result, $"{op} operator");
        m.Parameters.Add(new CsMethodParameter("left", left));
        m.Parameters.Add(new CsMethodParameter("right", right));
        return m;
    }

    public CsMethod Operator(CsType value, CsType result, string op)
    {
        var m = _cl.AddMethod(op, result, $"{op} operator");
        m.Parameters.Add(new CsMethodParameter("value", value));
        return m;
    }

    public CsProperty IsEmptyProperty(string? expression = null, bool addAttribute = false)
    {
        if (string.IsNullOrEmpty(expression))
        {
            if (Special == SpecialType.String)
                expression = "string.IsNullOrEmpty(Value)";
            else
            {
                if (string.IsNullOrWhiteSpace(EmptyExpression))
                    throw new Exception("EmptyExpression is empty");
                if (Special == SpecialType.Int)
                    expression = $"Value == {EmptyExpression}";
                else
                    expression = $"{EmptyExpression}.Equals(Value)";
            }
        }

        if (AddHasValueField)
            expression = $"!{HasValueFieldName} || {expression}";
        var p = _cl.AddProperty("IsEmpty", CsType.Bool)
            .WithOwnGetterAsExpression(expression)
            .WithNoEmitField()
            .WithIsPropertyReadOnly();
        if (addAttribute)
        {
            var at = CsAttribute
                .Make<Auto.ShouldSerializeInfoAttribute>(_cl)
                .WithArgument("!{0}.IsEmpty");
            _cl.Attributes.Add(at);
        }

        return p;
    }

    public void ComparableOperators()
    {
        var skipEquals = Special is SpecialType.Int or SpecialType.Guid;

        const string eq      = "left.Equals(right)";
    
        foreach (var op in ">,<,>=,<=".Split(','))
        {
            var expr = $"left.CompareTo(right) {op} 0";
            //var expr = $"{compare} {op} 0";
            var m    = Operator(_cl.Name, _cl.Name, CsType.Bool, op);
            if (!skipEquals)
            {
                expr = op is "<" or ">" 
                    ? $"!{eq} && {expr}"
                    : $"{eq} || {expr}";
            }

            m.WithBodyAsExpression(expr);
        }
    }

    public CsMethod CompareTo()
    {
        var comparer = CompareExpressionFactory ?? CompareMethods.Comparable;
        var expre    = comparer("Value", "other.Value");
        var m = _cl.AddMethod("CompareTo", CsType.Int32)
            .WithBodyAsExpression(expre);
        var pt = _cl.Name.ToReferenceNullableIfPossible(_cl.Kind);
        m.AddParam("other", pt);
        return m;
    }

    private const string ValuePropertyField = "_value";

    public CsProperty ValueProperty()
    {
        var propertyField = ValuePropertyField;
        if (BackingField != PropertyBackingFieldRequest.DoNotUse)
            propertyField = "field";
        var p = _cl.AddProperty("Value", _primitive)
            .WithIsPropertyReadOnly();
        p.BackingField = BackingField; 
        if (AddHasValueField)
        {
            if (string.IsNullOrEmpty(EmptyExpression))
                throw new Exception("EmptyExpression is empty");
            if (Special == SpecialType.String)
                p.WithOwnGetterAsExpression(
                    $"{HasValueFieldName} ? ({propertyField} ?? {EmptyExpression}) : {EmptyExpression}");
            else
                p.WithOwnGetterAsExpression($"{HasValueFieldName} ? {propertyField} : {EmptyExpression}");
            p.IsReadOnly = true;
            _cl.AddField(HasValueFieldName, CsType.Bool).WithIsReadOnly();
        }
        else
        {
            if (Special == SpecialType.String)
                p.WithOwnGetterAsExpression($"{propertyField} ?? {EmptyExpression}");
            else
                p.WithMakeAutoImplementIfPossible();
        }

        p.EmitField = EmitValuePropertyField;    
        if (Special == SpecialType.String)
            p.FieldTypeOverride = CsType.StringNullable;
        return p;
    }

    public PropertyBackingFieldRequest BackingField { get; set; }

    private bool EmitValuePropertyField =>
        Special == SpecialType.String
        || BackingField != PropertyBackingFieldRequest.DoNotUse;

    public CsMethod IsZero()
    {
        return _cl.AddMethod("IsZero", CsType.Bool).WithBodyAsExpression("Value == 0");
    }

    public CsProperty ZeroProperty()
    {
        var p = _cl.AddProperty("Zero", _cl.Name);
        p.IsStatic              = true;
        p.EmitField             = false;
        p.SetterType            = PropertySetter.None;
        p.OwnGetter             = _cl.Name.New("0");
        p.OwnGetterIsExpression = true;
        return p;
    }

    public CsProperty HasValueProperty(string expression)
    {
        var p = _cl.AddProperty("HasValue", CsType.Bool)
            .WithOwnGetterAsExpression(expression)
            .WithNoEmitField()
            .WithIsPropertyReadOnly();
        return p;
    }

    public CsProperty EmptyProperty(string? expression = null)
    {
        if (string.IsNullOrWhiteSpace(expression))
        {
            if (string.IsNullOrWhiteSpace(EmptyExpression))
                throw new Exception("EmptyExpression is empty");
            expression = _cl.Name.New(EmptyExpression);
        }

        var p = _cl.AddProperty("Empty", _cl.Name)
            .WithStatic()
            .WithNoEmitField()
            .WithIsPropertyReadOnly()
            .WithOwnGetterAsExpression(expression);
        return p;
    }

    public void NumberOperators(bool extended)
    {
        if (extended)
        {
            NumberOperator(false, false, "+");
            NumberOperator(false, false, "-");
            NumberOperator(true, false, "+");
            NumberOperator(true, false, "-");
        }

        NumberOperator(false, true, "+");
        NumberOperator(false, true, "-");

        Add1("+");
        Add1("-");
        return;

        void Add1(string op)
        {
            var t    = _cl.Name;
            var body = t.New($"value.Value {op} 1");
            if (AddIsEmptyProperty)
                body = $"value.IsEmpty ? value : {body}";
            Operator(t, t, op + op).WithBodyAsExpression(body);
        }
    }

    public CsMethod NumberOperator(bool leftPrimitive, bool rightPrimitive, string op)
    {
        var x  = _cl.Name;
        var lt = leftPrimitive ? _primitive : x;
        var rt = rightPrimitive ? _primitive : x;

        var le = leftPrimitive ? "" : ".Value";
        var re = rightPrimitive ? "" : ".Value";
        var e  = $"left{le} {op} right{re}";
        e = x.New(e);
        var m = Operator(lt, rt, x, op).WithBodyAsExpression(e);
        return m;
    }

    public CsMethod StringPlusOperator(bool leftPrimitive, bool rightPrimitive)
    {
        if (Special != SpecialType.String)
            throw new InvalidOperationException("Not a string");
        var x  = _cl.Name;
        var lt = leftPrimitive ? _primitive : x;
        var rt = rightPrimitive ? _primitive : x;

        var    le = leftPrimitive ? "" : ".Value";
        var    re = rightPrimitive ? "" : ".Value";
        string e;

        e = $"left{le} + right{re}";
        e = x.New(e);

        if (leftPrimitive)
        {
            if (rightPrimitive)
                throw new Exception("Both left and right are primitive");
            e = $"string.IsNullOrEmpty(left) ? right : {e}";
        }
        else
        {
            if (rightPrimitive)
                e = $"string.IsNullOrEmpty(right) ? left : {e}";
        }

        // e = x.New(e);
        var m = Operator(lt, rt, x, "+").WithBodyAsExpression(e);
        return m;
    }

    public CsMethod Constructor()
    {
        var w      = new CsCodeWriter();
        //var target = EmitValuePropertyField ? ValuePropertyField : "Value";
        //var target = "Value";
        if (Special == SpecialType.String)
        {
            w.WriteLine("value = value?.Trim();");
            w.WriteLine("_value = string.IsNullOrEmpty(value) ? null : value;");
        }
        else
        {
            w.WriteLine("Value = value;");
        }

        if (AddHasValueField)
            w.WriteLine($"{HasValueFieldName} = true;");

        var m = _cl.AddConstructor().WithBody(w);
        if (Special == SpecialType.String)
            m.AddParam("value", CsType.StringNullable);
        else
            m.AddParam("value", _primitive);
        return m;
    }

    public CsMethod EqualsMyType(bool nullableValue)
    {
        string expr;
        if (nullableValue)
            expr = $"{IsNotNull("other")} && {EqualsExpressionFactory("Value", "other.Value.Value", EqualityOperators.Equal)}";
        else
            expr = EqualsExpressionFactory("Value", "other.Value", EqualityOperators.Equal);

        return Equality.EqualsMyType(nullableValue).WithBodyAsExpression(expr);
    }

    public CsMethod EqualsPrimitive(bool nullableValue)
    {
        var type = _primitive;
        if (nullableValue)
            type = type with { Nullable = NullableKind.ValueNullable };
        var    m = Equality.EqualsAny("other", type);
        string expr;
        if (nullableValue)
            expr = $"{IsNotNull("other")} && {EqualsExpressionFactory("Value", "other.Value", EqualityOperators.Equal)}";
        else
            expr = EqualsExpressionFactory("Value", "other", EqualityOperators.Equal);

        return m.WithBodyAsExpression(expr);
    }

    public CsMethod EqualsOverideObject()
    {
        return Equality.EqualsOverideObject()
            .WithBodyAsExpression($"obj is {Equality.TypeName} s && Value.Equals(s.Value)");
    }

    public CsMethod HashCode()
    {
        return Equality.HashCode()
            .WithBodyAsExpression(GetHashCodeExpressionFactory("Value"));
    }

    public string IsNotNull(string x)
    {
        var supportsModern = (_cl.Formatting.Flags & CodeFormattingFeatures.IsNotNull) != 0;
        return supportsModern
            ? $"{x} is not null"
            : $"!({x} is null)";
    }

    public CsMethod ToStringOverride(string? toStringPrefix)
    {
        var m = ToStringOverride();

        string expression;
        if (string.IsNullOrEmpty(toStringPrefix))
            expression = Special == SpecialType.String ? "Value" : "Value.ToString()";
        else
            expression = $"{(toStringPrefix?.Trim() + " ").CsEncode()} + Value";
        m.WithBodyAsExpression(expression);
        return m;
    }

    public CsMethod ToStringOverride()
    {
        var m = _cl.AddMethod(nameof(ToString), CsType.String);
        m.Overriding = OverridingType.Override;

        return m;
    }

    public CsMethod EqualityOperator(EqualityOperators oper)
    {
        string expression =GetExpression();
        var m = Equality.EqualityOperator(oper, expression);
        return m;

        string GetExpression()
        {
            ValueEqualsExpressionDelegate? factory = EqualsExpressionFactory;
            if (factory is null)
            {
                factory = Special switch
                {
                    SpecialType.String => EqualMethods.StringOrdinal,
                    SpecialType.Int => EqualMethods.EqualitySign,
                    _                  => EqualMethods.Equals
                };
            }
            return factory("left.Value", "right.Value", oper);
        }
    }

    public delegate string CompareExpressionDelegate(string myValue, string otherValue);
    public delegate string ValueEqualsExpressionDelegate(string myValue, string otherValue, EqualityOperators oper);
    public delegate string GetHashCodeExpressionDelegate(string value);

    public ValueEqualsExpressionDelegate? EqualsExpressionFactory      { get; set; }
    public GetHashCodeExpressionDelegate? GetHashCodeExpressionFactory { get; set; }
    public CompareExpressionDelegate?     CompareExpressionFactory     { get; set; }

    public BaseEqualityFeatureImplementer Equality { get; }

    private readonly CsType      _primitive;
    private readonly CsClass     _cl;
    private readonly string      _primitiveDeclaration;
    public           SpecialType Special { get; }

    public enum SpecialType
    {
        Other,
        String,
        Int,
        Long,
        Guid
    }

    public string? EmptyExpression { get; init; }
#if NET8_0_OR_GREATER
    public required bool AddIsEmptyProperty { get; init; }
    public required bool AddHasValueField   { get; init; }
#else
    public bool AddIsEmptyProperty { get; init; }
    public bool AddHasValueField   { get; init; }
#endif

    /*
    [Obsolete("Use " + nameof(AddHasValueField) + " instead", true)]
    public bool SupportsSetValue { get; set; }

    [Obsolete("Use " + nameof(AddIsEmptyProperty) + " instead", true)]
    public bool SupportsEmpty { get; init; }
    */


    public readonly struct AsMethodCreationInfo : IComparable<AsMethodCreationInfo>, IComparable
    {
        public AsMethodCreationInfo(string structType, CsType wrappedType)
        {
            StructType  = structType;
            WrappedType = wrappedType;
        }

        public static void Emit(CsClass cl, IEnumerable<AsMethodCreationInfo> _allClasses,
            Func<string, string> mapName)
        {
            cl.IsStatic = true;
            var list = _allClasses.ToList();
            list.Sort();
            foreach (var i in list)
            {
                //var name = $"As{i.StructType.TrimStart('X')}";
                var name = mapName(i.StructType);
                var m = cl.AddMethod(name, (CsType)i.StructType)
                    .WithStatic()
                    .WithBodyAsExpression($"new {i.StructType}(x)");
                var g = cl.Reduce(i.WrappedType);
                var p = m.AddParam("x", g);
                p.UseThis = true;
            }
        }

        public int CompareTo(AsMethodCreationInfo other)
        {
            return string.Compare(StructType, other.StructType, StringComparison.Ordinal);
        }

        public int CompareTo(object? obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            if (obj is not AsMethodCreationInfo info)
                throw new ArgumentException($"Object must be of type {nameof(AsMethodCreationInfo)}");
            return CompareTo(info);
        }

        public string StructType  { get; }
        public CsType WrappedType { get; }
    }

    public KeyImplementer WithStringOrdinalIgnoreCase()
    {
        EqualsExpressionFactory      = EqualMethods.StringOrdinalIgnoreCase;
        GetHashCodeExpressionFactory = HashMethods.StringOrdinalIgnoreCase;
        CompareExpressionFactory     = CompareMethods.StringOrdinalIgnoreCase;
        return this;
    }
}
