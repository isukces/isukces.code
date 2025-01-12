#nullable disable
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace iSukces.Code.VsSolutions;

public class Solution
{
    public Solution(FileInfo solutionFile)
    {
        Projects     = new List<SolutionProject>();
        SolutionFile = new FileName(solutionFile);
        var lines = File.ReadAllLines(SolutionFile.FullName);

        var inProject = false;
        foreach (var i in lines)
        {
            var ii = i.Trim();
            if (inProject)
            {
                if (ii == "EndProject") inProject = false;
                continue;
            }

            var a = TryParseProject(ii);
            if (a == null)
                continue;
            Projects.Add(a);
            inProject = true;
        }
    }

         

    public override string ToString() => string.Format("Solution {0}", SolutionFile.Name);
         

    private SolutionProject TryParseProject(string line)
    {
        var match = ProjectRegex.Match(line);
        if (!match.Success)
            return null;
        var fi = new FileInfo(Path.Combine(SolutionFile.Directory.FullName, match.Groups[3].Value));
        var project = new SolutionProject
        {
            // LocationUid = Guid.Parse(match.Groups[1].Value),
            // Name = match.Groups[2].Value,
            // ReSharper disable once PossibleNullReferenceException
            Location = new FileName(fi)
            // ProjectUid = Guid.Parse(match.Groups[4].Value)
        };
        return project.Location.Exists
            ? project
            : null;
    }

    public FileName SolutionFile { get; }

    public List<SolutionProject> Projects { get; }

    private static readonly Regex ProjectRegex = new Regex(ProjectRegexFilter, RegexOptions.Compiled);

    private const string ProjectRegexFilter =
        @"^Project\(\s*\""*{([^}]+)\}\""\s*\)\s*=\s*\""([^\""]+)\""\s*,\s*\""([^\""]+)\""\s*,\s*\s*\""*{([^}]+)\}\""\s*(.*)$";
}
