using System;
using System.Collections.Generic;
using System.Linq;

namespace iSukces.Code.Interfaces;

public interface ICsCodeWriter : ICodeWriter
{
}

public static class CsCodeWriterExtensions
{
    extension<T>(T self)
        where T : ICsCodeWriter
    {
        public void AddNamespaces(string namespaceName)
        {
            self.WriteLine("using {0};", namespaceName);
        }

        public void AddNamespaces(IEnumerable<string> namespaceName)
        {
            foreach (var x in namespaceName)
                self.AddNamespaces(x);
        }

        public void CloseCompilerIf(bool close)
        {
            if (close)
                self.WritelineNoIndent("#endif");
        }

        public void CloseCompilerIf(string? directive)
        {
            if (!string.IsNullOrEmpty(directive))
                self.WritelineNoIndent("#endif");
        }

        public void CloseCompilerIf(IConditional? conditional)
        {
            self.CloseCompilerIf(conditional?.CompilerDirective);
        }

        public void ClosePragmaWarnings(IList<CsPragmaWarning> list)
        {
            self.PragmaWarnings(list, false);
        }

        public bool OpenCompilerIf(string? directive)
        {
            if (string.IsNullOrEmpty(directive)) return false;
            self.WritelineNoIndent(CompilerDirectives.If + " " + directive);
            return true;
        }

        public bool OpenCompilerIf(IConditional? conditional)
        {
            return self.OpenCompilerIf(conditional?.CompilerDirective);
        }

        public T OpenIf(string condition)
        {
            self.Open($"if ({condition})");
            return self;
        }

        public void OpenPragmaWarnings(IList<CsPragmaWarning> list)
        {
            self.PragmaWarnings(list, true);
        }

        public T OpenSwitch(string expression)
        {
            self.Open($"switch ({expression})");
            return self;
        }

        private void PragmaWarnings(IList<CsPragmaWarning>? list, bool start)
        {
            if (list is null || list.Count == 0)
                return;
            foreach (var gr in GetActions()
                         .GroupBy(a => a.Item1)
                         .OrderBy(a => a.Key))
            {
                var items = gr.Select(b => b.Item2)
                    .Distinct()
                    .OrderBy(u => u)
                    .ToArray();

                var list1 = string.Join(", ", items);
                self.WritelineNoIndent($"#pragma warning {gr.Key} {list1}");
            }

            return;

            IEnumerable<(string, string)> GetActions()
            {
                foreach (var el in list)
                {
                    var name = el.Name?.Trim();
                    if (string.IsNullOrEmpty(name))
                        continue;
                    var action = el.Enabling == Enabling.Enable ? "enable" : "disable";
                    if (!start)
                        action = "restore";
                    yield return (action, name);
                }
            }
        }

        public T SingleLineIf(string condition, string statement, string? elseStatement = null)
        {
            self.WriteLine("if (" + condition + ")");
            self.IncIndent();
            self.WriteLine(statement);
            self.DecIndent();
            if (string.IsNullOrEmpty(elseStatement))
                return self;
            self.WriteLine("else");
            self.IncIndent();
            self.WriteLine(elseStatement);
            self.DecIndent();
            return self;
        }

        public T SingleLineIfThrow(string condition, CsType type, string? arg = null)
        {
            var statement = type.ThrowNew(arg);
            return self.SingleLineIf(condition, statement + ";");
        }

        public void WriteComment(ICommentable? c)
        {
            var comment = c?.GetComments()?.Trim();
            if (string.IsNullOrEmpty(comment))
                return;
            var lines = comment.Replace("\r\n", "\n").Split('\n');
            if (lines.Length == 1)
            {
                self.WriteLine("// " + lines[0]);
            }
            else
            {
                self.WriteLine("/*");
                foreach (var line in lines)
                    self.WriteLine(line);
                self.WriteLine("*/");
            }
        }

        public void WriteLambda(string header, string expression, int maxLineLength,
            bool addSemiColon)
        {
            var emptyHeader = string.IsNullOrEmpty(header);
            if (emptyHeader)
            {
                self.WriteLine($"=> {expression};");
                return;
            }

            if (addSemiColon)
                expression += ";";

            header = $"{header} => ";
            var lineLength = header.Length + expression.Length + self.Indent * CodeFormatting.IndentSpaces;
            if (lineLength <= maxLineLength)
                self.WriteLine(header + expression);
            else
                self.WriteLine(header).IncIndent().WriteLine(expression).DecIndent();
        }

        public void WriteLambda(string header, IReadOnlyList<string> expression,
            int maxLineLength)
        {
            switch (expression.Count)
            {
                case 0:
                    throw new InvalidOperationException("No expression lines");
                case 1:
                    WriteLambda(self, header, expression.Single(), maxLineLength, true);
                    return;
            }

            WriteLambda(self, header, expression[0], maxLineLength, false);
            self.IncIndent();
            for (var index = 1; index < expression.Count; index++)
            {
                var line = expression[index];
                if (index == expression.Count - 1)
                    line += ";";
                self.WriteLine(line);
            }

            self.DecIndent();
        }

        public T WriteMultiLineSummary(IReadOnlyList<string>? lines, bool skipIfEmpty = false)

        {
            lines ??= [];
            if (lines.Count == 0 && skipIfEmpty)
                return self;
            self.WriteLine("/// <summary>");
            foreach (var line in lines)
                self.WriteSummary(line);
            self.WriteLine("/// </summary>");
            return self;
        }


        public T WriteSingleLineSummary(string? x, bool skipIfEmpty = false)

        {
            if (x is null)
            {
                if (skipIfEmpty)
                    return self;
                return WriteMultiLineSummary(self, XArray.Empty<string>());
            }

            var lines = x.Split('\r', '\n').Where(q => !string.IsNullOrEmpty(q?.Trim()))
                .ToArray();
            return WriteMultiLineSummary(self, lines, skipIfEmpty);
        }

        public T WriteSummary(string x)
        {
            // System.Xml.Linq.XObject el = new XElement("param", new XAttribute("name", p.Name), p.Description);
            // cs.Writeln("/// {0}", el);
            self.WriteLine("/// {0}", x);
            return self;
        }
    }


    extension<T>(T self)
        where T : ICodeWriter
    {
        public T EmptyLine(bool skip = false)
        {
            if (!skip)
                self.Append("\r\n");
            return self;
        }

        public string GetIndent()
        {
            return self.Indent > 0 ? new string(' ', self.Indent * 4) : "";
        }

        public ICodeWriter OpenConstructor(string x, string? baseOrThis = null)
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

        public T WriteIndent()
        {
            if (self.Indent > 0)
                self.Append(new string(' ', self.Indent * CodeFormatting.IndentSpaces));
            return self;
        }

        public T WriteNewLineAndIndent()
        {
            self.WriteLine();
            if (self.Indent > 0)
                self.Append(new string(' ', self.Indent * CodeFormatting.IndentSpaces));
            return self;
        }
    }
}
