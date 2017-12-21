using isukces.code.interfaces;
using Xunit;

namespace isukces.code.Tests
{
    public class CsTests
    {
        [Fact]
        public void ShouldCsCite()
        {
            const string quote = "\"";
            const string backslash = "\\";
            
            const string specialR = "\r";
            Assert.Equal(1, specialR.Length);
            Assert.Equal(quote + backslash + "r" + quote, specialR.CsCite());
            var specialN = "\n";
            Assert.Equal(1, specialN.Length);
            Assert.Equal(quote + backslash + "n" + quote, specialN.CsCite());
            var specialT = "\t";
            Assert.Equal(1, specialT.Length);
            Assert.Equal(quote + backslash + "t" + quote, specialT.CsCite());
        }
    }
}