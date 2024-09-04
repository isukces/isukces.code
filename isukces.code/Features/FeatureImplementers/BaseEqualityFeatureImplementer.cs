#nullable enable
#nullable enable
#nullable enable
#nullable enable
namespace iSukces.Code.FeatureImplementers;

public class BaseEqualityFeatureImplementer
{
    public BaseEqualityFeatureImplementer(CsClass cl)
    {
        this.cl  = cl;
        TypeName = cl.Name.Declaration;
    }

    public CsMethod EqualityOperator(bool equal) 
        => cl.AddBinaryOperator(equal ? "==" : "!=", CsType.Bool);


    public CsMethod EqualityOperator(bool equal, string expression) 
        => EqualityOperator(equal).WithBodyAsExpression(expression);

    public CsMethod EqualsMyType(bool nullableValue)
    {
        var argType = cl.Name;
        if (nullableValue)
            argType.Nullable = NullableKind.ValueNullable;
        var m = EqualsAny("other", argType);
        return m;
    }

    public CsMethod EqualsOverideObject()
    {
        var m = EqualsAny("obj", CsType.ObjectNullable);
        m.Overriding = OverridingType.Override;
        return m;
    }

    public CsMethod EqualsAny(string argName, CsType argType)
    {
        var m = cl.AddMethod(nameof(Equals), CsType.Bool);
        m.Parameters.Add(new CsMethodParameter(argName, argType));
        return m;
    }

    public CsMethod HashCode()
    {
        var m = cl.AddMethod(nameof(GetHashCode), CsType.Int32);
        m.Overriding = OverridingType.Override;
        return m;
    }

    public CsClass cl       { get; }
    public string  TypeName { get; }
}
