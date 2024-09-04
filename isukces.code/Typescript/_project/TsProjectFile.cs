#nullable enable
using System;
using System.IO;

namespace iSukces.Code.Typescript
{
    /// <summary>
    /// Helps to manage relations between ts files
    /// </summary>
    public class TsProjectFile
    {
        public TsProjectFile(TsFile file, DirectoryInfo webProjectRoot, DirectoryInfo resultFileDir)
        {
            File           = file;
            ResultFileDir  = resultFileDir;
            WebProjectRoot = webProjectRoot;
        }


        private static string GetRelativePath(FileInfo fileInfo, DirectoryInfo baseDir)
        {
            var pathUri = new Uri(fileInfo.FullName);
            // Folders must end in a slash
            var folder = baseDir.FullName;
            if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
                folder += Path.DirectorySeparatorChar;
            var folderUri = new Uri(folder);
            var tmp       = folderUri.MakeRelativeUri(pathUri).ToString();
            return Uri.UnescapeDataString(tmp.Replace('/', Path.DirectorySeparatorChar));
        }


        /// <summary>
        ///     Add reference to typescript file.
        /// </summary>
        /// <param name="relFilePath">File path related to <see cref="WebProjectRoot">ProjectRoot</see></param>
        public void AddRelativeReference(string relFilePath)
        {
            var fi  = new FileInfo(Path.Combine(WebProjectRoot.FullName, relFilePath));
            var rel = GetRelativePath(fi, ResultFileDir);
            File.References.Add(new TsReference(rel));
        }

        public TsFile File { get; }

        /// <summary>
        ///     Directory where typescript file will be saved
        /// </summary>
        public DirectoryInfo ResultFileDir { get; }

        /// <summary>
        ///     Root directory of web project
        /// </summary>
        public DirectoryInfo WebProjectRoot { get; }
    }
}