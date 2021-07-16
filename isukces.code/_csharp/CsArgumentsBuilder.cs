using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using iSukces.Code.AutoCode;
using iSukces.Code.Interfaces;

namespace iSukces.Code
{
    public sealed class CsArgumentsBuilder
    {
        public void Add(CsExpression expression)
        {
            if (expression is null)
                AddCode("null");
            else
                AddCode(expression.Code);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CsArgumentsBuilder AddCode(string ammyCode)
        {
            Items.Add(ammyCode);
            return this;
        }

        public CsArgumentsBuilder AddValue(string text) => AddCode(text.CsEncode());

        public bool Any() => Items.Count > 0;

        public string CallMethod(string methodName, bool addSemicolon)
        {
            if (addSemicolon)
                return methodName + CodeEx + ";";
            return methodName + CodeEx;
        }

        public override string ToString() => Code;

        public string Code
        {
            get { return string.Join(", ", Items); }
        }

        public string CodeEx
        {
            get { return "(" + Code + ")"; }
        }

        public List<string> Items { get; } = new List<string>();
    }
    
    internal sealed class IfCollector
    {
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
      public sealed class ConditionsPair
      {
          public ConditionsPair(string condition, string inversed = null)
          {
              Condition = condition;
              if (inversed is null)
                  inversed = $"!({condition})";
              Inversed = inversed;
          }

          public static ConditionsPair FromInversed(string inversed)
          {
              return new ConditionsPair($"!({inversed})", inversed);
          }

          public bool IsAlwaysTrue
          {
              get
              {
                  return Condition == "true";
              }
          }
          public bool IsAlwaysFalse
          {
              get
              {
                  return Condition == "false";
              }
          }

          public static ConditionsPair FromIsNull(string variable)
          {
              return new ConditionsPair($"{variable} is null");
          }

          public static implicit operator ConditionsPair(bool x)
          {
              return x
                  ? new ConditionsPair("true", "false")
                  : new ConditionsPair("false", "true");
          }

          public override string ToString()
          {
              return $"Condition={Condition}, Inversed={Inversed}";
          }

          public string Condition { get; }

          public string Inversed { get; }
      }
}