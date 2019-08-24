using System;
using System.IO;
using System.Reflection;

namespace isukces.code
{
    public static class IsukcesCodeExtensions
    {
        public static DirectoryInfo FindFileHereOrInParentDirectories(this DirectoryInfo di, string fileName)
        {
            while (di != null)
            {
                if (!di.Exists)
                    return null;
                var fi = Path.Combine(di.FullName, fileName);
                if (File.Exists(fi))
                    return di;
                di = di.Parent;
            }

            return null;
        }

        public static DirectoryInfo FindFileHereOrInParentDirectories(this Type type, string fileName)
        {
            return type
#if COREFX
                .GetTypeInfo()
#endif
                .Assembly.FindFileHereOrInParentDirectories(fileName);
        }

        public static DirectoryInfo FindFileHereOrInParentDirectories(this Assembly a, string fileName)
        {
            var di = new FileInfo(a.Location).Directory;
            di = di.FindFileHereOrInParentDirectories(fileName);
            return di;
        }


    }
}