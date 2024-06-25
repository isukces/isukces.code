using System.Diagnostics;
using iSukces.Build;

namespace Build.iSukces.Code;

internal class Script
{
    public Script()
    {
        _slnFile = FindSolutionFile();
        CsProj   = Path.Combine(_slnFile.Directory!.FullName, "iSukces.Code", "iSukces.Code.csproj");
        BinDir   = Path.Combine(_slnFile.Directory!.FullName, "iSukces.Code", "bin", "Release");
    }

    private static FileInfo FindSolutionFile()
    {
        var slnSearchAssembly = typeof(Program).Assembly;
        var solutionDir       = slnSearchAssembly.ScanSolutionDir(SolutionName);
        return new FileInfo(Path.Combine(solutionDir.FullName, SolutionName));
    }

    private void Build()
    {
        ExeRunner.WorkingDir = SolutionDir.FullName;
        ExeRunner.Execute("dotnet", "build", "-c", "RELEASE", _slnFile.Name);
    }

    private void ClearBinObj()
    {
        var myDir = Path.Combine(SolutionDir.FullName, "Build.iSukces.Code");
        var skipClearBinObj = new HashSet<string>(StringComparer.CurrentCultureIgnoreCase)
        {
            myDir
        };
        BuildUtils.ClearBinObj(SolutionDir, skipClearBinObj);
    }

    private void CopyToLocalNugetFeed()
    {
        var localNuget = Environment.GetEnvironmentVariable("LOCALNUGET");
        if (string.IsNullOrEmpty(localNuget))
        {
            Console.WriteLine("LOCALNUGET not set");
            Process.Start("explorer.exe", BinDir);
            return;
        }

        foreach (var ext in new[] { "nupkg", "snupkg" })
        {
            var fn  = $"{isukcesCode}.{Version}.{ext}";
            var src = Path.Combine(BinDir, fn);
            File.Copy(src, Path.Combine(localNuget, fn), true);
        }

        Process.Start("explorer.exe", localNuget);
    }

    public void Run()
    {
        var csProjFile = CsProjFile.Load(CsProj);
        Version = csProjFile.GetVersion("Version");
        ClearBinObj();
        // UpdateVersion();
        Build();
        CopyToLocalNugetFeed();
    }

    public void UpdateVersion()
    {
        var csProjFile = CsProjFile.Load(CsProj);
        Version = csProjFile.GetVersion("Version");
        Version = CsProjFile.UpdateVersion(Version);
        csProjFile.SetAllVersions(Version);
        csProjFile.Save(CsProj);
    }

    public string? Version { get; set; }

    private string        CsProj      { get; }
    private DirectoryInfo SolutionDir => _slnFile.Directory!;

    #region Fields

    private const string isukcesCode = "iSukces.Code";

    private const    string   SolutionName = "isukces.code.nuget.sln";
    private readonly FileInfo _slnFile;

    public string BinDir;

    #endregion
}