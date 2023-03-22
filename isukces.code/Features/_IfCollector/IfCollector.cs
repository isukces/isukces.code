using System.Linq;
using iSukces.Code.Interfaces;

namespace iSukces.Code
{
    public sealed class IfCollector
    {
        public IfCollector(string condition, string inversed = null)
            : this(new ConditionsPair(condition, inversed))
        {

        }

        public IfCollector(ConditionsPair condition)
        {
            Condition = condition;
        }

        private static string CloseCurly(bool required)
        {
            return required ? "} " : "";
        }

        private static string OpenCurly(bool required)
        {
            return required ? " {" : "";
        }

        private static void WriteLines(CsCodeWriter writer, string[] statementLines, bool intended = false)
        {
            if (intended)
                writer.IncIndent();
            foreach (var i in statementLines)
                writer.WriteLine(i);
            if (intended)
                writer.DecIndent();
        }


        private static void WriteOne(CsCodeWriter writer, string condition, string[] statementLines)
        {
            switch (statementLines.Length)
            {
                case 0:
                    return;
                case 1:
                    writer.SingleLineIf(condition, statementLines[0]);
                    return;
                default:
                    writer.Open($"if ({condition})");
                    WriteLines(writer, statementLines);
                    writer.Close();
                    return;
            }
        }

        public void WriteTo(CsCodeWriter writer)
        {
            var statementLines = SplitCode(Statement.Code);
            var elseLines      = SplitCode(Else.Code);

            if (Condition.IsAlwaysTrue)
            {
                WriteLines(writer, statementLines);
                return;
            }

            if (Condition.IsAlwaysFalse)
            {
                WriteLines(writer, elseLines);
                return;
            }

            if (elseLines.Length == 0)
            {
                WriteOne(writer, Condition.Condition, statementLines);
            }
            else if (statementLines.Length == 0)
            {
                WriteOne(writer, Condition.Inversed, elseLines);
            }
            else
            {
                var o1 = statementLines.Length > 1;
                writer.WriteLine($"if ({Condition.Condition}){OpenCurly(o1)}");
                WriteLines(writer, statementLines, true);

                var o2 = elseLines.Length > 1;
                writer.WriteLine(CloseCurly(o1) + "else" + OpenCurly(o2));
                WriteLines(writer, elseLines, true);
                if (o2)
                    writer.WriteLine("}");
            }
        }

        public static string[] SplitCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return new string[0];
            var statementLines = code.Split('\r', '\n')
                .Where(a => !string.IsNullOrWhiteSpace(a)).ToArray();
            return statementLines;
        }

        public ConditionsPair Condition { get; }
        public CsCodeWriter   Statement { get; } = new CsCodeWriter();
        public CsCodeWriter   Else      { get; } = new CsCodeWriter();
    }
}