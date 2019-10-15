using Xunit;

namespace isukces.code.Tests
{
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData(" ", "")]
        [InlineData(" a", "a")]
        [InlineData(" HelloWorld", "Hello world")]
        public void T01_Should_decamelize(string src, string expected)
        {
            Assert.Equal(src.Decamelize(), expected);
        }
        
    }
}