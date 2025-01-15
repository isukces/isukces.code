#nullable disable
using System;
using System.IO;
using iSukces.Code.Tests.EqualityGenerator;
using Xunit;

namespace iSukces.Code.Tests;

public class TestUtils
{
    public static void CompareWithResource(string actual, string resourcePrefix,
        string method, string file, string ext)
    {
        method = method
            .Replace("_Should_create_", "_")
            .Replace("_Should_", "_");

        var name     = resourcePrefix + method + ext;
        var assembly = typeof(EqualityGeneratorTests).Assembly;
        var s        = assembly.GetManifestResourceStream(name);
        if (s is null)
        {
            var allNames = assembly.GetManifestResourceNames();
            Save(false);
            throw new Exception("Resource not found, please recompile");
        }

        string expected;
        try
        {
            using var reader = new StreamReader(s);
            expected = reader.ReadToEnd();
        }
        finally
        {
            s.Dispose();
        }

        var isNullOrEmpty = string.IsNullOrEmpty(expected.Trim());
        if (expected != actual || isNullOrEmpty)
            Save(!isNullOrEmpty);
        Assert.Equal(expected, actual);
        return;

        void Save(bool addSubfolder)
        {
            var dir = new FileInfo(file).Directory.FullName;
            if (addSubfolder)
                dir = Path.Combine(dir, "new");
            var fn = Path.Combine(dir, method + ext);
            new FileInfo(fn).Directory.Create();
            File.WriteAllText(fn, actual);
        }
    }
}