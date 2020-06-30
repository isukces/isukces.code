using System;
using System.IO;
using System.Reflection;
using isukces.code.AutoCode;

namespace AmmySamples
{
    internal class AssemblyBaseDirectoryProvider : IAssemblyBaseDirectoryProvider
    {
        public AssemblyBaseDirectoryProvider()
        {
            _provider = SlnAssemblyBaseDirectoryProvider.Make<Program>("isukces.code.sln");
            _provider.OnGetBaseDirectory += (a, b) =>
            {
                var name = b.Assembly.GetName().Name;
                /*
                if (string.Equals(name, "TelerikStyles", StringComparison.OrdinalIgnoreCase))
                    b.ProjectSubDir = "!Small\\" + name;
                */
            };
        }


        public DirectoryInfo GetBaseDirectory(Assembly assembly) => _provider.GetBaseDirectory(assembly);

        private readonly SlnAssemblyBaseDirectoryProvider _provider;
    }
}