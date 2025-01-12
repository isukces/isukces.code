#nullable disable
using iSukces.Code.FeatureImplementers;
using Xunit;

namespace iSukces.Code.Tests;

public class KeyImplementerTests
{
    [Fact]
    public void T01_Should_create()
    {
        var file = new CsFile
        {
            FileScopeNamespace = FileScopeNamespaceConfiguration.AllowIfPossible,
            Nullable = FileNullableOption.GlobalEnabled
        };
        var cl = file.GetOrCreateClass("MyTest", (CsType)"SomeKey");
        cl.Formatting = new CodeFormatting(CodeFormattingFeatures.Cs12, 120);
        var main = new KeyImplementer(cl, CsType.String)
        {
            SupportsEmpty    = false,
            SupportsSetValue = false
        }.WithStringOrdinalIgnoreCase();
        
        main.Constructor();
        main.ValueProperty();
        main.EqualsOverideObject();
        main.EqualsMyType(true);
        main.EqualsMyType(false);
        main.AddIEquatable(cl.Name);
        main.HashCode();
        main.EqualityOperator(EqualityOperators.Equal);
        main.EqualityOperator(EqualityOperators.NotEqual);
        var          code     = file.GetCode();
        const string expected = @"
// ReSharper disable All
namespace MyTest;

public class SomeKey : System.IEquatable<SomeKey>
{
    public SomeKey(string? value)
    {
        value = value?.Trim();
        _value = string.IsNullOrEmpty(value) ? null : value;
    }

    public override bool Equals(object? obj) => obj is SomeKey s && Value.Equals(s.Value);

    public bool Equals(SomeKey? other) => 
        other is not null && StringComparer.OrdinalIgnoreCase.Equals(Value, other.Value.Value);

    public bool Equals(SomeKey other) => StringComparer.OrdinalIgnoreCase.Equals(Value, other.Value);

    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);

    public static bool operator ==(SomeKey left, SomeKey right) => 
        StringComparer.OrdinalIgnoreCase.Equals(left.Value, right.Value);

    public static bool operator !=(SomeKey left, SomeKey right) => 
        !StringComparer.OrdinalIgnoreCase.Equals(left.Value, right.Value);

    public string Value => _value ?? string.Empty;

    private string? _value;

}
";
        Assert.Equal(expected.Trim(), code.Trim());
    }
}
