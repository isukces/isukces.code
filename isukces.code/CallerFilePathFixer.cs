using System;
using System.IO;
// ReSharper disable UnusedMember.Global

namespace iSukces.Code;

/// <summary>
/// Try to fix file path i.e. staring with /_/app/ prefix
/// </summary>
public static class CallerFilePathFixer
{
    public static string Fix(string? path)
    {
        if (string.IsNullOrEmpty(path))
            return "";
        var h = ProcessSaveFileName;
        if (h is null)
            return path;
        var e = new ProcessSaveFileNameEventArgs
        {
            Path = path
        };
        h(null, e);
        return e.Path;
    }
    
    /// <summary>
    /// Naprawia ścieżkę zapisu pliku dla prefixu /_/app/
    /// </summary>
    /// <param name="args"></param>
    /// <param name="solutionDir"></param>
    /// <exception cref="Exception"></exception>
    public static void FixSlashApp(ProcessSaveFileNameEventArgs args, DirectoryInfo solutionDir)
    {
        if (args.Path.StartsWith(GlobalSettings.SlashAppPrefix, StringComparison.OrdinalIgnoreCase))
        {
            var path = args.Path[7..].Replace('/', '\\');
            args.Path = Path.Combine(solutionDir.FullName, path);
        }
    }

    public static event EventHandler<ProcessSaveFileNameEventArgs>? ProcessSaveFileName;

    public sealed class ProcessSaveFileNameEventArgs : EventArgs
    {
        public required string Path { get; set; }
    }
}

