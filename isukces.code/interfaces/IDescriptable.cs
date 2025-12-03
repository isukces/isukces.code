using System.Runtime.CompilerServices;

namespace iSukces.Code.Interfaces;

public interface IDescriptable
{
    string? Description { get; set; }
}

public interface ICommentable
{
    void   AddComment(string? x);
    string GetComments();
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

        public void AddCommentLocation(SourceCodeLocation x)
        {
            self.AddComment(x.ToString());
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
