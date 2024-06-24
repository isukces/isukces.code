#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using iSukces.Code.Interfaces;

namespace iSukces.Code;

public sealed class FileScopeNamespaceConfiguration : IEquatable<FileScopeNamespaceConfiguration>
{
    private FileScopeNamespaceConfiguration(int number)
    {
        Number             = number;
        FileScopeNamespace = "";
    }

    private FileScopeNamespaceConfiguration(int number, string fileScopeNamespace)
    {
        Number             = number;
        FileScopeNamespace = fileScopeNamespace;
    }

    public static FileScopeNamespaceConfiguration AssumeDefined(string fileScopeNamespace)
        => new(XAssumeDefined, fileScopeNamespace);

    public static bool operator ==(FileScopeNamespaceConfiguration? left, FileScopeNamespaceConfiguration? right)
        => Equals(left, right);

    public static bool operator !=(FileScopeNamespaceConfiguration? left, FileScopeNamespaceConfiguration? right)
        => !Equals(left, right);

    public INamespaceWriter Check(List<string> fileNamespaces, out string comment)
    {
        var result = Number switch
        {
            XBlockScoped =>
                BlockScopedWriter.Instance,
            XAllowIfPossible when fileNamespaces.Count < 2 =>
                FileScopeWriter.Instance,
            XAllowIfPossible =>
                BlockScopedWriter.Instance,
            XAssumeDefined when fileNamespaces.Count != 1 =>
                new ErrorWriter("FileScopeNamespace is defined but there are more than one namespace in the file"),
            XAssumeDefined =>
                FileScopedAreadyDefinedWriter.Instance,
            _ => new ErrorWriter("Unknowne error")
        };
        comment = "";
        if (fileNamespaces.Count < 2 && result is BlockScopedWriter)
            comment = "File scope namespace is possible, use [AssumeDefinedNamespace]";
        return result;
    }

    public bool Equals(FileScopeNamespaceConfiguration? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Number == other.Number && FileScopeNamespace == other.FileScopeNamespace;
    }

    public override bool Equals(object? obj)
        => ReferenceEquals(this, obj)
           || obj is FileScopeNamespaceConfiguration other && Equals(other);

    public override int GetHashCode()
    {
#if NET48
        return Number * 397 ^ (FileScopeNamespace != null ? FileScopeNamespace.GetHashCode() : 0);
#else
        return HashCode.Combine(Number, FileScopeNamespace);
#endif
    }

    public override string ToString()
    {
        return Number switch
        {
            XAssumeDefined => $"Assume defined \"{FileScopeNamespace}\"",
            XBlockScoped => "Block scoped",
            XAllowIfPossible => "Allow if possible",
            _ => "Unknown"
        };
    }

    private int    Number             { get; }
    private string FileScopeNamespace { get; }

    private const int XBlockScoped = 0;
    private const int XAllowIfPossible = 1;
    private const int XAssumeDefined = 2;

    public static readonly FileScopeNamespaceConfiguration BlockScoped = new(XBlockScoped);
    public static readonly FileScopeNamespaceConfiguration AllowIfPossible = new(XAllowIfPossible);

    private sealed class ErrorWriter(string error) : INamespaceWriter
    {
        public void CloseNamespace(string ns, ICsCodeWriter writer)
        {
            throw new Exception(error);
        }

        public void OpenNamespace(string ns, ICsCodeWriter writer)
        {
            throw new Exception(error);
        }
    }

    private sealed class FileScopedAreadyDefinedWriter : INamespaceWriter
    {
        private FileScopedAreadyDefinedWriter()
        {
        }

        public void CloseNamespace(string ns, ICsCodeWriter writer)
        {
        }

        public void OpenNamespace(string ns, ICsCodeWriter writer)
        {
        }

        public static INamespaceWriter Instance { get; } = new FileScopedAreadyDefinedWriter();
    }


    public interface INamespaceWriter
    {
        void CloseNamespace(string ns, ICsCodeWriter writer);
        void OpenNamespace(string ns, ICsCodeWriter writer);
    }

    private class FileScopeWriter : INamespaceWriter
    {
        private FileScopeWriter()
        {
        }

        public void CloseNamespace(string ns, ICsCodeWriter writer)
        {
        }

        public void OpenNamespace(string? ns, ICsCodeWriter writer)
        {
            if (string.IsNullOrEmpty(ns))
                return;
            writer.WriteLine($"namespace {ns};");
            writer.WriteLine();
        }

        public static INamespaceWriter Instance { get; } = new FileScopeWriter();
    }

    private class BlockScopedWriter : INamespaceWriter
    {
        private BlockScopedWriter()
        {
        }

        public void CloseNamespace(string? ns, ICsCodeWriter writer)
        {
            if (!string.IsNullOrEmpty(ns))
                writer.Close();
        }

        public void OpenNamespace(string? ns, ICsCodeWriter writer)
        {
            if (!string.IsNullOrEmpty(ns))
                writer.Open($"namespace {ns}");
        }

        public static INamespaceWriter Instance { get; } = new BlockScopedWriter();
    }

    public static class CsScanner
    {
        public static FileScopeNamespaceConfiguration? SimpleScan(string? fileName)
        {
            if (fileName is null) return null;
            if (!File.Exists(fileName)) return null;
            var lines = File.ReadAllLines(fileName);
            foreach (var i in lines)
            {
                var m = Regex1.Match(i);
                if (!m.Success) continue;
                var tmp = m.Groups[1].Value;
                m = Regex2.Match(tmp);
                if (!m.Success) return null;

                if (m.Groups[2].Value != ";")
                    return null;
                return AssumeDefined(m.Groups[1].Value);
            }

            return null;
        }

        private const string Filter1 = @"^\s*namespace\s+(.*)$";

        private const string Filter2 = @"([a-z_][a-z0-9_]*(?:\.[a-z_][a-z0-9_]*)*)\s*(.*)$";

        private static readonly Regex Regex1 = new Regex(Filter1, RegexOptions.Compiled);

        private static readonly Regex Regex2 = new Regex(Filter2, RegexOptions.IgnoreCase | RegexOptions.Compiled);
    }
}
