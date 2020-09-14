using System;
using System.Collections.Generic;
using System.IO;

namespace iSukces.Code.vssolutions
{
    public class NuspecCache
    {
        #region Static Methods

        public static Dictionary<string, Nuspec> GetForDirectory(DirectoryInfo directory)
        {
            var result = new Dictionary<string, Nuspec>(StringComparer.OrdinalIgnoreCase);
            try
            {
                var fn     = GetFileName(directory);
                if (!fn.Exists)
                    return result;
                var fromFile = JsonHelper.Load<Dictionary<string, Nuspec>>(fn);
                if (fromFile != null)
                    foreach (var i in fromFile)
                        result[i.Key] = i.Value;
                return result;
            }
            catch
            {
                return result;
            }
        }

        public static void Save(DirectoryInfo directory, Dictionary<string, Nuspec> cache1)
        {
            var fn = GetFileName(directory);
            JsonHelper.Save(fn, cache1);
        }

        private static FileInfo GetFileName(DirectoryInfo directory) =>
            new FileInfo(Path.Combine(directory.FullName, "$packageScannerCache.cache"));

        #endregion
    }
}