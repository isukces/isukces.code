// ReSharper disable MemberCanBePrivate.Global

using System.Collections.Generic;
using System.Linq;

namespace iSukces.Code.Interfaces
{
    public interface ICsCodeWriter : ICodeWriter
    {
    }

    public static class CsCodeWriterExtensions
    {
        public static void AddNamespaces(this ICsCodeWriter src, string namespaceName)
        {
            src.WriteLine("using {0};", namespaceName);
        }

        public static void AddNamespaces(this ICsCodeWriter src, IEnumerable<string> namespaceName)
        {
            foreach (var x in namespaceName)
                src.AddNamespaces(x);
        }


        public static void CloseCompilerIf(this ICsCodeWriter _this, string directive)
        {
            if (!string.IsNullOrEmpty(directive))
                _this.WritelineNoIndent("#endif");
        }

        public static void CloseCompilerIf(this ICsCodeWriter _this, IConditional conditional)
        {
            _this.CloseCompilerIf(conditional?.CompilerDirective);
        }

        public static T EmptyLine<T>(this T _this, bool skip = false)
            where T : ICodeWriter
        {
            if (!skip)
                _this.Append("\r\n");
            return _this;
        }

        public static string GetIndent(this ICodeWriter _this) =>
            _this.Indent > 0 ? new string(' ', _this.Indent * 4) : "";


        public static void OpenCompilerIf(this ICsCodeWriter _this, string directive)
        {
            if (!string.IsNullOrEmpty(directive))
                _this.WritelineNoIndent("#if " + directive);
        }

        public static void OpenCompilerIf(this ICsCodeWriter _this, IConditional conditional)
        {
            _this.OpenCompilerIf(conditional?.CompilerDirective);
        }


        public static ICodeWriter OpenConstructor(this ICodeWriter self, string x, string baseOrThis = null)
        {
            self.WriteLine(x);
            if (!string.IsNullOrEmpty(baseOrThis))
            {
                self.Indent++;
                self.WriteLine(": " + baseOrThis);
                self.Indent--;
            }

            self.WriteLine("{");
            self.Indent++;
            return self;
        }

        public static T SingleLineIf<T>(this T src, string condition, string statement, string elseStatement = null)
            where T : ICsCodeWriter
        {
            src.WriteLine("if (" + condition + ")");
            src.IncIndent();
            src.WriteLine(statement);
            src.DecIndent();
            if (string.IsNullOrEmpty(elseStatement))
                return src;
            src.WriteLine("else");
            src.IncIndent();
            src.WriteLine(elseStatement);
            src.DecIndent();
            return src;
        }

        public static void WriteComment(this ICsCodeWriter writer, ICommentable c)
        {
            var comment = c?.GetComments()?.Trim();
            if (string.IsNullOrEmpty(comment))
                return;
            var lines = comment.Replace("\r\n", "\n").Split('\n');
            if (lines.Length == 1)
            {
                writer.WriteLine("// " + lines[0]);
            }
            else
            {
                writer.WriteLine("/*");
                foreach (var line in lines)
                    writer.WriteLine(line);
                writer.WriteLine("*/");
            }
        }


        public static T WriteIndent<T>(this T _this)
            where T : ICodeWriter
        {
            if (_this.Indent > 0)
                _this.Append(new string(' ', _this.Indent * 4));
            return _this;
        }

        public static T WriteNewLineAndIndent<T>(this T _this)
            where T : ICodeWriter
        {
            _this.WriteLine();
            if (_this.Indent > 0)
                _this.Append(new string(' ', _this.Indent * 4));
            return _this;
        }

        public static T WriteSingleLineSummary<T>(this T src, string x, bool skipIfEmpty = false)
            where T : ICsCodeWriter

        {
            if (x == null)
            {
                if (skipIfEmpty)
                    return src;
                return WriteMultiLineSummary(src, new string[0]);
            }
            var lines = x.Split('\r', '\n').Where(q => !string.IsNullOrEmpty(q?.Trim()))
                .ToArray();
            return WriteMultiLineSummary(src, lines, skipIfEmpty);
        }
        
        public static T WriteMultiLineSummary<T>(this T src, IReadOnlyList<string> lines, bool skipIfEmpty = false)
            where T : ICsCodeWriter

        {
            lines = lines ?? new string[0];
            if (lines.Count == 0 && skipIfEmpty)
                return src;
            src.WriteLine("/// <summary>");
            foreach (var line in lines)
                src.WriteSummary(line);
            src.WriteLine("/// </summary>");
            return src;
        }


        public static T WriteSummary<T>(this T src, string x)
            where T : ICsCodeWriter
        {
            // System.Xml.Linq.XObject el = new XElement("param", new XAttribute("name", p.Name), p.Description);
            // cs.Writeln("/// {0}", el);
            src.WriteLine("/// {0}", x);
            return src;
        }
    }
}