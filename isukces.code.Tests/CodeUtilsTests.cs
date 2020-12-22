using System;
using System.Linq.Expressions;
using Xunit;

namespace iSukces.Code.Tests
{
    public class CodeUtilsTests
    {
        [Fact]
        public void T01_Should_get_path()
        {
            var r = CodeUtils.GetMemberPath<MainClass, object>(a => a.SomeProperty.Name);
            Assert.Equal("SomeProperty.Name", r);
        }
        
        [Fact]
        public void T02_Should_get_property_type()
        {
            var r = CodeUtils.GetMemberPath<MainClass, object>(a => ((NestedDerived)a.SomeProperty).Number);
            Assert.Equal("SomeProperty.Number", r);
        }

        [Fact]
        public void T03_Should_get_path_with_cast()
        {
            Expression<Func<MainClass, object>> func = a => ((NestedDerived)a.SomeProperty).Number;
            var r = CodeUtils.GetMemberInfo(func);
            Assert.Equal(typeof(int), r.Type);
        }
        class MainClass
        {
            public Nested SomeProperty { get; set; }
        }

        class Nested
        {
            public string Name { get; set; }
        }

        class NestedDerived :Nested
        {
            public int Number { get; set; }
        }
    }
}