using System.IO;
using System.Reflection;

namespace iSukces.Code.AutoCode
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
            return new FileInfo(_provider.GetFileName(assembly, _localFileName));
        }

        private readonly IAssemblyBaseDirectoryProvider _provider;
        private readonly string _localFileName;
    }
}
