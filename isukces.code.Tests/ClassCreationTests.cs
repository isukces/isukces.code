using System.Collections.Generic;
using isukces.code.AutoCode;
using isukces.code.CodeWrite;
using Xunit;

namespace isukces.code.Tests
{
    public class ClassCreationTests
    {
        [Fact]
        public void T01_Should_create_class()
        {
            var c     = new CsFile();
            var type  = TypeProvider.FromType(typeof(ParentGeneric<>.Nested));
            var cache = new Dictionary<TypeProvider, CsClass>();
            c.GetOrCreateClass(type, cache);
            var code = c.GetCode();
            Assert.Equal(@"// ReSharper disable All
namespace isukces.code.Tests
{
    partial class ParentGeneric<T>
    {
        partial class Nested
        {
        }

    }
}
", code, ignoreLineEndingDifferences: true, ignoreWhiteSpaceDifferences: true);
        }
    }

    internal class ParentGeneric<T>
    {
        public class Nested
        {
        }
    }
}