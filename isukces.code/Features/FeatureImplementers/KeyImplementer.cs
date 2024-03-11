#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using iSukces.Code.Interfaces;

namespace iSukces.Code.FeatureImplementers;

public class KeyImplementer
{
    private const string HasValue = "_hasValue";

    public override string ToString()
        => $"Maker {_cl.Name.Declaration} {_primitiveDeclaration}";

    public KeyImplementer(CsClass cl, CsType primitive)
    {
        _cl                   = cl;
        _primitive            = cl.Reduce(primitive);
        Equality              = new BaseEqualityFeatureImplementer(cl);
        _primitiveDeclaration = primitive.Declaration;
        Special = _primitiveDeclaration switch
        {
            "string" => SpecialType.String,
            "int" => SpecialType.Int,
            "Guid" when cl.IsKnownNamespace("System") => SpecialType.Guid,
            "System.Guid" => SpecialType.Guid,
            "long" => SpecialType.Long,
            _ => SpecialType.Other
        };

        const string valueGethashcode = "Value.GetHashCode()";
        switch (Special)
        {
            case SpecialType.String:
                var stringComparer = _cl.GetTypeNameD<StringComparer>();
                ValueEqualsExpression = StringEqualsExpression;
                GetHasCodeExpression  = $"{stringComparer}.Ordinal.GetHashCode(Value ?? string.Empty)";
                EmptyExpression       = "string.Empty";
                break;
            case SpecialType.Int:
                ValueEqualsExpression = SimpleEqualsExpression;
                GetHasCodeExpression  = "Value";
                EmptyExpression       = "int.MinValue";
                break;
            case SpecialType.Long:
                ValueEqualsExpression = SimpleEqualsExpression;
                GetHasCodeExpression  = valueGethashcode;
                EmptyExpression       = "long.MinValue";
                break;
            case SpecialType.Guid:
                var guid = cl.GetTypeName<Guid>();
                EmptyExpression       = guid.GetMemberCode(nameof(Guid.Empty));
                ValueEqualsExpression = x => $"{x}.Equals(Value)";
                GetHasCodeExpression  = valueGethashcode;
                break;
            case SpecialType.Other:
            default:
                ValueEqualsExpression = x => $"Value.Equals({x})";
                GetHasCodeExpression  = valueGethashcode;
                break;
        }
    }

    private static string SimpleEqualsExpression(string x) => $"Value == {x}";

    public static string StringEqualsExpression(string otherValue)
        => $"StringComparer.Ordinal.Equals(Value, {otherValue})";

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
        var typeName = _cl.IsKnownNamespace(ns) ? interfaceType : $"{ns}.{interfaceType}"; //
        var tt       = CsType.Generic(typeName, genericArgument ?? _cl.Name);
        _cl.ImplementedInterfaces.Add(tt);
    }

    public CsMethod GuidNew(string name = "NewId") => _cl.AddMethod(name, _cl.Name)
        .WithStatic()
        .WithBodyAsExpression(_cl.Name.New("Guid.NewGuid()"));

    public CsMethod GuidParse(string name = "Parse")
    {
        var m = _cl.AddMethod(name, _cl.Name)
            .WithStatic()
            .WithBodyAsExpression(_cl.Name.New("Guid.Parse(guid)"));
        m.Parameters.Add(new CsMethodParameter("guid", CsType.String));
        return m;
    }

    public CsMethod OperatorBool(CsType left, CsType right, string op) => Operator(left, right, CsType.Bool, op);

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

        if (SupportsSetValue)
            expression = $"!{HasValue} || {expression}";
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

        const string l1 = "if (Equals(left, right)) return false;";
        foreach (var op in ">,<,>=,<=".Split(','))
        {
            var l2 = $"left.CompareTo(right) {op} 0";
            var m  = Operator(_cl.Name, _cl.Name, CsType.Bool, op);
            if (skipEquals)
                m.WithBodyAsExpression(l2);
            else
                m.Body = $"{l1}\r\nreturn {l2};";
        }
    }

    public CsMethod CompareTo()
    {
        const string expre = "Value.CompareTo(other.Value)";
        var m = _cl.AddMethod("CompareTo", CsType.Int32)
            .WithBodyAsExpression(expre);
        m.AddParam("other", _cl.Name);
        return m;
    }

    private const string ValuePropertyField = "_value";

    public CsProperty ValueProperty()
    {
        var p = _cl.AddProperty("Value", _primitive)
            .WithIsPropertyReadOnly();
        if (SupportsSetValue)
        {
            if (string.IsNullOrEmpty(EmptyExpression))
                throw new Exception("EmptyExpression is empty");
            if (Special == SpecialType.String)
                p.WithOwnGetterAsExpression($"{HasValue} ? ({ValuePropertyField} ?? {EmptyExpression}) : {EmptyExpression}");
            else
                p.WithOwnGetterAsExpression($"{HasValue} ? {ValuePropertyField} : {EmptyExpression}");
            p.IsReadOnly = true;
            _cl.AddField(HasValue, CsType.Bool).WithIsReadOnly();
        }
        else
        {
            if (Special == SpecialType.String)
                p.WithOwnGetterAsExpression($"{ValuePropertyField} ?? {EmptyExpression}");
            else
                p.WithMakeAutoImplementIfPossible();
        }

        p.EmitField = HasValuePropertyField;
        return p;
    }

    private bool HasValuePropertyField => SupportsSetValue || Special == SpecialType.String;

    public CsMethod IsZero() => _cl.AddMethod("IsZero", CsType.Bool).WithBodyAsExpression("Value == 0");

    public CsProperty ZeroProperty()
    {
        var p = _cl.AddProperty("Zero", _cl.Name);
        p.IsStatic              = true;
        p.EmitField             = false;
        p.IsPropertyReadOnly    = true;
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
            if (SupportsEmpty)
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
        var target = HasValuePropertyField ? ValuePropertyField : "Value";
        if (Special == SpecialType.String)
        {
            w.WriteLine("value = value?.Trim();");
            w.WriteLine($"{target} = string.IsNullOrEmpty(value) ? null : value;");
        }
        else
            w.WriteLine($"{target} = value;");

        if (SupportsSetValue)
            w.WriteLine($"{HasValue} = true;");

        var m = _cl.AddConstructor().WithBody(w);
        m.AddParam("value", _primitive);
        return m;
    }

    public CsMethod EqualsMyType(bool nullableValue)
    {
        string expr;
        if (nullableValue)
            expr = $"{IsNotNull("other")} && {ValueEqualsExpression("other.Value.Value")}";
        else
            expr = ValueEqualsExpression("other.Value");

        return Equality.EqualsMyType(nullableValue).WithBodyAsExpression(expr);
    }
    
    public CsMethod EqualsPrimitive(bool nullableValue)
    {
        var    type = _primitive;
        if (nullableValue)
            type = type with { Nullable = NullableKind.ValueNullable };
        var    m = Equality.EqualsAny("other", type);
        string expr;
        if (nullableValue)
            expr = $"{IsNotNull("other")} && {ValueEqualsExpression("other.Value")}";
        else
            expr = ValueEqualsExpression("other");

        return m.WithBodyAsExpression(expr);
        
        
        /*
        var m = _cl.AddMethod("Equals", CsType.Bool)
            .WithBodyAsExpression($"{PropertyName}.Equals(other)");
        m.AddParam("other", primitive);*/
    }

    public CsMethod EqualsOverideObject() => Equality.EqualsOverideObject()
        .WithBodyAsExpression($"obj is {Equality.TypeName} s && Value.Equals(s.Value)");

    public CsMethod HashCode() => Equality.HashCode()
        .WithBodyAsExpression(GetHasCodeExpression);

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
            expression = $"{(toStringPrefix.Trim() + " ").CsEncode()} + Value";
        m.WithBodyAsExpression(expression);
        return m;
    }

    public CsMethod ToStringOverride()
    {
        var m = _cl.AddMethod(nameof(ToString), CsType.String);
        m.Overriding = OverridingType.Override;

        return m;
    }

    public CsMethod EqualityOperator(bool equal)
    {
        string expression;
        if (Special == SpecialType.String)
        {
            var a = "left.Value.Equals(right.Value)";
            if (equal)
                expression = a;
            else
                expression = $"!{a}";
        }
        else

            expression = equal ? "left.Value == right.Value" : "left.Value != right.Value";

        var m = Equality.EqualityOperator(equal, expression);
        return m;
    }

    public Func<string, string> ValueEqualsExpression { get; init; }
    public string               GetHasCodeExpression  { get; init; }

    public BaseEqualityFeatureImplementer Equality { get; }

    private readonly CsType _primitive;
    private readonly CsClass _cl;
    private readonly string _primitiveDeclaration;
    public SpecialType Special { get; }

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
    public required bool SupportsEmpty    { get; init; }
    public required bool SupportsSetValue { get; init; }
#else
    public bool SupportsEmpty    { get; init; }
    public bool SupportsSetValue { get; init; }
#endif


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
            cl.IsStatic  = true;
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

        public int CompareTo(AsMethodCreationInfo other) => string.Compare(StructType, other.StructType, StringComparison.Ordinal);

        public int CompareTo(object? obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            if (obj is not AsMethodCreationInfo info)
                throw new ArgumentException($"Object must be of type {nameof(AsMethodCreationInfo)}");
            return CompareTo(info);
        }

        #region Properties

        public string StructType  { get; }
        public CsType WrappedType { get; }

        #endregion
    }
}