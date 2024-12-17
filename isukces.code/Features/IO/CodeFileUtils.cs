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

    public static bool SaveIfDifferent(string content, string filename, object generator, FileSavedDelegate fileSaved)
    {
        var result = SaveIfDifferent(content, filename);
        if (result)
            fileSaved?.Invoke(generator, filename);
        return result;
    }

    public static bool SaveIfDifferent(string content, string filename
#if BOM
        , bool addBom = false
#endif
        )
    {
        byte[]? existing = null;
        if (File.Exists(filename))
            existing = File.ReadAllBytes(filename);
#if BOM
        var newCodeBytes = Encode(content, addBom);
#else
        var newCodeBytes = Encode(content);
#endif
        if (AreEqual(existing, newCodeBytes))
            return false;
        new FileInfo(filename).Directory?.Create();
        File.WriteAllBytes(filename, newCodeBytes);
        return true;
    }

#if BOM
    private static readonly byte[] Bom = { 0xEF, 0xBB, 0xBF };
#endif
}