using System.Collections.Generic;
using iSukces.Code.AutoCode;
using iSukces.Code.CodeWrite;
using Xunit;

namespace iSukces.Code.Tests
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
namespace iSukces.Code.Tests
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

        [Fact]
        public void T02_Should_add_comment()
        {
            var c     = new CsFile();
            var type  = TypeProvider.FromType(typeof(ParentGeneric<>.Nested));
            var cache = new Dictionary<TypeProvider, CsClass>();
            var myClass = c.GetOrCreateClass(type, cache);
            myClass.AddComment("Line");
            {
                var p = myClass.AddProperty("Count", "int");
                p.AddComment("line1");
                p.AddComment("line2");
            }
            var code = c.GetCode();
            Assert.Equal(@"// ReSharper disable All
namespace iSukces.Code.Tests
{
    partial class ParentGeneric<T>
    {
        // Line
        partial class Nested
        {
            /*
            line1
            line2
            */
            public int Count
            {
                get { return _count; }
                set { _count = value; }
            }

            private int _count;

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