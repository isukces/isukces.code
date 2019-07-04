using isukces.code.Ammy;
using Xunit;

namespace isukces.code.Tests.Ammy
{
    public class AmmyBuilderContextTests
    {
        [Fact]
        public void T01_Should_create_EmbedFileName()
        {
            var a = new AmmyBuilderContext("Bla");
            a.EmbedInRelativeFile(null, @"c:\bla\some.file.cs");
            Assert.Equal(@"c:\bla\some.ammy", a.EmbedFileName);
        }
        
        [Fact]
        public void T02_Should_create_EmbedFileName()
        {
            var a = new AmmyBuilderContext("Bla");
            a.EmbedInRelativeFile("MyFile", @"c:\bla\some.file.cs");
            Assert.Equal(@"c:\bla\MyFile.ammy", a.EmbedFileName);
        }
    }
}