#nullable disable
using System;
using System.Reflection;
using iSukces.Code.Interfaces;
using Xunit;

namespace iSukces.Code.Tests
{
    public class MemberNullValueCheckerTests
    {
        [Theory]
        [InlineData(typeof(int))]
        [InlineData(typeof(uint))]
        [InlineData(typeof(long))]
        [InlineData(typeof(ulong))]
        [InlineData(typeof(byte))]
        [InlineData(typeof(sbyte))]
        [InlineData(typeof(double))]
        [InlineData(typeof(decimal))]
        [InlineData(typeof(float))]
        [InlineData(typeof(Guid))]
        [InlineData(typeof(DateTime))]
        [InlineData(typeof(DateTimeOffset))]
        [InlineData(typeof(TimeSpan))]
        [InlineData(typeof(bool))]
        [InlineData(typeof(char))]

        public void T01_ShouldFindNotNull(Type type)
        {
            var a = new MyChecker();
            Assert.True(a.TypeIsAlwaysNotNull(type));
        }

        [Theory]
        [InlineData(typeof(int?))]
        [InlineData(typeof(uint?))]
        [InlineData(typeof(long?))]
        [InlineData(typeof(ulong?))]
        [InlineData(typeof(byte?))]
        [InlineData(typeof(sbyte?))]
        [InlineData(typeof(double?))]
        [InlineData(typeof(decimal?))]
        [InlineData(typeof(float?))]
        [InlineData(typeof(Guid?))]
        [InlineData(typeof(string))]
        [InlineData(typeof(DateTime?))]
        [InlineData(typeof(DateTimeOffset?))]
        [InlineData(typeof(TimeSpan?))]
        [InlineData(typeof(bool?))]
        [InlineData(typeof(char?))]
        [InlineData(typeof(byte[]))]
        [InlineData(typeof(Uri))]
        public void T02_ShouldFindNull(Type type)
        {
            var a = new MyChecker();
            Assert.False(a.TypeIsAlwaysNotNull(type));
        }

        private class MyChecker : AbstractMemberNullValueChecker
        {
            protected override bool HasMemberNotNullAttribute(MemberInfo mi)
            {
                return false;
            }
        }
    }
}
