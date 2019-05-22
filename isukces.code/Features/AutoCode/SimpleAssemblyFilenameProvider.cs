using System.IO;
using System.Reflection;

namespace isukces.code.AutoCode
{
    public class SimpleAssemblyFilenameProvider : IAssemblyFilenameProvider
    {
        public SimpleAssemblyFilenameProvider(IAssemblyBaseDirectoryProvider provider, string localFileName)
        {
            _provider      = provider;
            _localFileName = localFileName;
        }

        public FileInfo GetFilename(Assembly assembly)
        {
            var dir      = _provider.GetBaseDirectory(assembly);
            var fileName = Path.Combine(dir.FullName, _localFileName);
            return new FileInfo(fileName);
        }

        private readonly IAssemblyBaseDirectoryProvider _provider;
        private readonly string _localFileName;
    }
}