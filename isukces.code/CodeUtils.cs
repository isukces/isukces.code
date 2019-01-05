using System.IO;
using System.Reflection;

namespace isukces.code
{
    public static  class CodeUtils
    {
        public static DirectoryInfo SearchFoldersUntilFileExists(Assembly a, string fileName)
        {
            var di = new FileInfo(a.Location).Directory;
            di = SearchFoldersUntilFileExists(di,fileName);
            return di;
        }

        public static DirectoryInfo SearchFoldersUntilFileExists(DirectoryInfo di, string fileName)
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

    }
}