#nullable enable
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using iSukces.Code.AutoCode;

namespace iSukces.Code.IO;

public static class CodeFileUtils
{
    public static bool AreEqual(byte[]? a, byte[]? b)
    {
        a ??= XArray.Empty<byte>();
        b ??= XArray.Empty<byte>();
        var al = a.Length;
        var bl = b.Length;
        if (al != bl) return false;
        if (al == 0)
            return true;
        for (var i = 0; i < al; i++)
        {
            if (a[i] != b[i])
                return false;
        }
        return true;
    }

    public static string? GetCallerFilePath([CallerFilePath] string? file = null)
    {
        return file;
    }
        
    public static byte[] Encode(string txt
#if BOM
        , bool addBom
#endif
        )
    {
        var bytes = Encoding.UTF8.GetBytes(txt);
#if BOM
        if (!addBom)
            return bytes;
        using var stream = new MemoryStream();
        stream.Write(Bom, 0, Bom.Length);
        stream.Write(bytes, 0, bytes.Length);
        return stream.ToArray();
#else
        return bytes;

#endif
    }

    public static bool SaveIfDifferent(string content, string fileName, object generator, FileSavedDelegate fileSaved)
    {
        var result = SaveIfDifferent(content, fileName);
        if (result)
            fileSaved?.Invoke(generator, fileName);
        return result;
    }

    public static bool SaveIfDifferent(string content, string fileName
#if BOM
        , bool addBom = false
#endif
        )
    {
        if (GlobalSettings.RejectFilenameWithSlashAppPrefix)
            GlobalSettings.CheckFilename(fileName);
        
        byte[]? existing = null;
        if (File.Exists(fileName))
            existing = File.ReadAllBytes(fileName);
#if BOM
        var newCodeBytes = Encode(content, addBom);
#else
        var newCodeBytes = Encode(content);
#endif
        if (AreEqual(existing, newCodeBytes))
            return false;
        new FileInfo(fileName).Directory?.Create();
        File.WriteAllBytes(fileName, newCodeBytes);
        return true;
    }

#if BOM
    private static readonly byte[] Bom = { 0xEF, 0xBB, 0xBF };
#endif
}