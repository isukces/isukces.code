using System.IO;
using isukces.code.vssolutions;
using Xunit;

namespace isukces.code.Tests.VsSolutions
{
    public class NuspecCacheTests
    {
        private const string dir = @"c:\programs\conexx\conexx.total\app\packages";
        [Fact]
        public void T01_should_read_cache()
        {
            var ddir = new DirectoryInfo(dir);
            var c = NuspecCache.GetForDirectory(ddir);
            //NuspecCache.Save(ddir, c);
            //var c = 
        }
        
    }
}