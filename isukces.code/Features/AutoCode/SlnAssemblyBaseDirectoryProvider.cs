#nullable enable
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace iSukces.Code.AutoCode
{
    public class SlnAssemblyBaseDirectoryProvider : IAssemblyBaseDirectoryProvider
    {
        public SlnAssemblyBaseDirectoryProvider(DirectoryInfo solutionDir, string? optionalSubDirectory = null)
        {
            SolutionDir          = solutionDir;
            OptionalSubDirectory = optionalSubDirectory;
        }

        /// <summary>
        ///     Takes directory from assembly containing 'T' location,
        ///     then travels up till find 'slnShortFilename'.
        ///     Found directory is taken as solution root directory
        /// </summary>
        /// <param name="slnShortFilename"></param>
        /// <param name="optionalSubDirectory"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static SlnAssemblyBaseDirectoryProvider Make<T>(string slnShortFilename,
            string? optionalSubDirectory = null)
        {
            var myAssembly = typeof(T)
#if COREFX
                .GetTypeInfo()
#endif
                .Assembly;
            var solutionDir = CodeUtils.SearchFoldersUntilFileExists(myAssembly, slnShortFilename);
            if (solutionDir == null)
                throw new Exception($"Unable to find {slnShortFilename}.");
            return new SlnAssemblyBaseDirectoryProvider(solutionDir, optionalSubDirectory);
        }

        public DirectoryInfo GetBaseDirectory(Assembly assembly)
        {
            var csProjs = SolutionDir.FullName;
            if (!string.IsNullOrEmpty(OptionalSubDirectory))
                csProjs = Path.Combine(csProjs, OptionalSubDirectory);
            var args = new GetBaseDirectoryEventArgs
            {
                Assembly      = assembly,
                RootDirectory = new DirectoryInfo(csProjs),
                ProjectSubDir = assembly.GetName().Name
            };
            OnGetBaseDirectory?.Invoke(this, args);
            var items = new[] {args.RootDirectory.FullName, args.ProjectSubDir};
            items = items.Where(item => !string.IsNullOrEmpty(item)).ToArray();

            DirectoryInfo di;
            switch (items.Length)
            {
                case 0:
                    throw new Exception("Emptyfile name");
                case 1:
                    di = new DirectoryInfo(items[0]);
                    break;
                default:
                    di = new DirectoryInfo(Path.Combine(items));
                    break;
            }

            di = new DirectoryInfo(di.FullName);
            return di;
        }

        public DirectoryInfo                                 SolutionDir          { get; }
        public string                                        OptionalSubDirectory { get; set; }
        public event EventHandler<GetBaseDirectoryEventArgs> OnGetBaseDirectory;
    }

    public class GetBaseDirectoryEventArgs
    {
        public Assembly      Assembly      { get; set; }
        public DirectoryInfo RootDirectory { get; set; }
        public string        ProjectSubDir { get; set; }
    }
}