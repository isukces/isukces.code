using System.IO;

namespace isukces.code.vssolutions
{
    public static class VsSolutionsExtensions
    {
        public static FileName GetAppConfigFile(this FileName projectFile) => projectFile.GetRelativeFile("app.config");

        public static FileName GetPackagesConfigFile(this FileName projectFile) =>
            projectFile.GetRelativeFile("packages.config");

        private static FileName GetRelativeFile(this FileName projectFile, string name)
        {
            // ReSharper disable once PossibleNullReferenceException
            var fi             = new FileInfo(projectFile.FullName);
            var configFileInfo = new FileInfo(Path.Combine(fi.Directory.FullName, name));
            return new FileName(configFileInfo);
        }
    }
}