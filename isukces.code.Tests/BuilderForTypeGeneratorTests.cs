using iSukces.Code.AutoCode;
using iSukces.Code.Interfaces;
using Xunit;

namespace iSukces.Code.Tests;

public class BuilderForTypeGeneratorTests
{
    [Fact]
    public void T01_ShouldBuild()
    {
        var gen                      = new Generators.BuilderForTypeGenerator();
        var context = new TestContext();
        CsClass.DefaultCodeFormatting = new CodeFormatting(CodeFormattingFeatures.Cs12, 120);
        context.AddNamespace("System.Runtime.CompilerServices");
        gen.Generate(typeof(B), context);
        var expected = @"
// ReSharper disable All
using System.Runtime.CompilerServices;

// suggestion: File scope namespace is possible, use [AssumeDefinedNamespace]
namespace iSukces.Code.Tests
{
    partial class BuilderForTypeGeneratorTests
    {
        partial class B
        {
#pragma warning disable CS8618
            public B()
            {
            }
#pragma warning restore CS8618

#pragma warning disable CS8618
            public B(A source)
            {
                if (source is null) return;
                Name = source.Name;
                Surname = source.Surname;
            }
#pragma warning restore CS8618

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public A Build() => new A(Name, Surname);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public B WithName(string? newName)
            {
                Name = newName;
                return this;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public B WithSurname(string newSurname)
            {
                Surname = newSurname;
                return this;
            }

            public string? Name { get; set; }

            public string Surname { get; set; }

        }

    }
}
";
        Assert.Equal(expected.Trim(), context.Code.Trim());
    }

    public sealed class A
    {
        public A(string? name, string surname)
        {
            Name = name;
            Surname = surname;
        }

        public string Surname { get; set; }
        public string? Name { get; set; }
    }

    [Auto.BuilderForType(typeof(A))]
    public sealed class B
    {
        
    }
    
}
