using System.IO;
using System.Runtime.CompilerServices;

namespace iSukces.Code.Interfaces;

public interface IDescriptable
{
    string? Description { get; set; }
}

public interface ICommentable
{
    void   AddComment(string? x);
    string? GetComments();
}

public static class CommentableEx
{
    extension(ICommentable self)
    {
        public void AddCommentLocation(string? prefix,
            SourceCodeLocation x)
        {
            prefix += " " + x;
            prefix =  prefix.Trim();
            self.AddComment(prefix);
        }

        public void AddComment(SourceCodeLocation location, bool skipLineNumbers = false, string prefix = "created: ")
        {
            if (skipLineNumbers)
                location = location.WithNoLineNumber();
            var txt = GetText(location);
            self.AddComment("created: " + txt);
            return;

            static string GetText(SourceCodeLocation location)
            {
                var txt = location.ToString();
                if (string.IsNullOrEmpty(location.FilePath)) return txt;
                var fi = new FileInfo(location.FilePath);
                var fn = fi.Name;
                fn  = fn[..^fi.Extension.Length];
                txt = fn + "." + txt;
                return txt;
            }
        }

        public void AddCommentLocation<T>(string? prefix = null,
            [CallerMemberName] string? memberName = null,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0)
        {
            var m = SourceCodeLocation.Make<T>(
                memberName,
                filePath,
                lineNumber);
            AddCommentLocation(self, prefix, m);
        }
    }
}
