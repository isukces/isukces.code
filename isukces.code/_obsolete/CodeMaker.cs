#if OBSOLETE
using System;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace isukces.code
{
    [Obsolete]
    public class CodeMaker
    {

        /// <summary>
        /// obiekt formatujący
        /// </summary>
        public CodeFormatter Code { get; set; }

        /// <summary>
        /// Typ badanej klasy
        /// </summary>
        public Type SourceType { get; set; }

        public static string AddBrackets(string x)
        {
            return AddBrackets1(x, false);
        }
        public static string AddBracketsForFunctionArgument(string x)
        {
            return AddBrackets1(x, true);
        }

        public static string StripBrackets(string x)
        {
            if (string.IsNullOrEmpty(x))
                throw new ArgumentNullException(nameof(x));
            x = x.Trim();

            if (x.StartsWith("(", StringComparison.Ordinal) && x.EndsWith(")", StringComparison.Ordinal))
            {
                if (x.Contains("\""))
                    throw new NotImplementedException();
                var pos = AAA(x);
                if (pos)
                    return x.Substring(1, x.Length - 2).Trim();
            }
            return x;


        }

        public static bool IsIdentifier(string x)
        {
            return Regex.IsMatch(x, "^[a-z_][a-z_0-9]*$", RegexOptions.IgnoreCase);
        }

        public static string AddBrackets1(string x, bool backetAlwaysRequered)
        {
            if (string.IsNullOrEmpty(x))
                throw new ArgumentNullException("x");
            x = x.Trim();
            if (!backetAlwaysRequered)
            {
                if (Regex.IsMatch(x, "^\\d+$", RegexOptions.IgnoreCase)) // integer
                    return x;
                if (IsIdentifier(x)) return x;
            }
            if (x.StartsWith("(") && x.EndsWith(")"))
            {
                if (x.Contains("\""))
                    throw new NotImplementedException();
                var pos = AAA(x);
                if (pos)
                    return x;
            }
            return string.Format("({0})", x);
        }

        private static bool AAA(string x)
        {
            var l = 1;
            var pos = true;
            for (int i = 1, max = x.Length - 2; i <= max && pos; i++)
            {
                var c = x[i];
                switch (c)
                {
                    case '(':
                        l++;
                        break;
                    case ')':
                        l--;
                        if (l == 0)
                            pos = false;
                        break;
                }
            }
            return pos;
        }

        private static bool AllPropertiesValue(PropertyInfo[] pis)
        {
            foreach (var pi in pis)
                if (!IsValueOrString(pi))
                    return false;
            return true;
        }

        private static bool IsValueOrString(PropertyInfo pi)
        {
            #if COREFX
            return pi.PropertyType.GetTypeInfo().IsValueType || pi.PropertyType == typeof(string);
            #else
            return pi.PropertyType.IsValueType || pi.PropertyType.Equals(typeof(string));
            #endif
        }

        private void _Clone(PropertyInfo[] pis)
        {
            Code.Writeln("/// <summary>");
            Code.Writeln("/// Wykonuje głęboką kopię obiektu ");
            Code.Writeln("/// </summary>");
            Code.Writeln("/// <returns>kopia obiektu</returns>");
            Code.Open("public object Clone()");
            Code.Writeln("{0} result = new {0}();", SourceType.Name);
            Code.Writeln("result.CopyFrom(this);");
            Code.Writeln("return result;");
            Code.Close();
        }

        private void _CopyFrom(PropertyInfo[] pis)
        {
            Code.Writeln("/// <summary>");
            Code.Writeln("/// Kopiuje własności z obiektu źródłowego");
            Code.Writeln("/// </summary>");
            Code.Writeln("/// <param name=\"source\">obiekt źródłowy kopiowania</param>");
            Code.Open("public void CopyFrom({0} source)", SourceType.Name);
            Code.Writeln("if (source == (object)null) throw new ArgumentNullException(\"Source\");");
            foreach (var pi in pis)
            {
                if (IsValueOrString(pi))
                {
                    Code.Writeln("this.{0} = source.{0};", pi.Name);
                    continue;
                }
                Code.Writeln("this.{0} = ({1})source.{0}.Clone();", pi.Name, pi.PropertyType.Name);
            }
            Code.Close();
        }

        private void _Equals()
        {
            Code.Writeln("/// <summary>");
            Code.Writeln("/// Sprawdza, czy wskazany obiekt jest równy bieżącemu");
            Code.Writeln("/// </summary>");
            Code.Writeln("/// <param name=\"obj\">obiekt do porównania z obiektem bieżącym</param>");
            Code.Writeln("/// <returns><c>true</c> jeśli wskazany obiekt jest równy bieżącemu; w przeciwnym wypadku<c>false</c></returns>");

            Code.Open("public override bool Equals(object obj)");
            Code.Writeln("if (obj is {0}) return ({0})obj == this;", SourceType.Name);
            Code.Writeln("return false;");
            Code.Close();
        }

        private void _GetHashCode(PropertyInfo[] pis)
        {
            Code.Writeln("/// <summary>");
            Code.Writeln("/// Zwraca kod HASH obiektu");
            Code.Writeln("/// </summary>");
            Code.Writeln("/// <returns>kod HASH obiektu</returns>");

            Code.Open("public override int GetHashCode()");
            if (AllPropertiesValue(pis))
            {
                var b = new StringBuilder();
                b.Append("return ");
                var i = 0;
                foreach (var pi in pis)
                {
                    var x = string.Format("{0}.GetHashCode()", pi.Name);
                    if (i++ == 0)
                    {
                        Code.Writeln("{0}.GetHashCode()", pi.Name);
                    }
                    else
                    {
                    }
                    if (i++ > 0) b.Append(" ^ ");
                    b.Append(string.Format("{0}.GetHashCode()", pi.Name));
                }
                if (i > 0) b.Append(";");
                else
                    b.AppendLine("0;");
                Code.Writeln(b.ToString());
            }
            else
            {
                var b = new StringBuilder();
                b.Append("int result = ");
                var i = 0;
                foreach (var pi in pis)
                {
                    if (!IsValueOrString(pi)) continue;
                    if (i++ > 0) b.Append(" ^ ");
                    b.Append(string.Format("{0}.GetHashCode()", pi.Name));
                }
                if (i > 0)
                    b.Append(";");
                else
                    b.Append("0;");
                Code.Writeln(b.ToString());
                // a teraz nie nullowe
                foreach (var pi in pis)
                {
                    if (IsValueOrString(pi)) continue;
                    Code.Writeln("if ({0} != (object)null) result ^= {0}.GetHashCode();", pi.Name);
                }
                Code.Writeln("return result;");
            }
            Code.Close();
        }

        private string _Operator(PropertyInfo[] pis, bool equal)
        {
            var r = new StringBuilder();
            var rowne = equal ? "==" : "!=";
            Code.Writeln("/// <summary>");
            Code.Writeln("/// Realizuje operator " + (equal ? "==" : "!="));
            Code.Writeln("/// </summary>");
            Code.Writeln("/// <param name=\"left\">lewa strona porównania</param>");
            Code.Writeln("/// <param name=\"right\">prawa strona porównania</param>");
            Code.Writeln("/// <returns><c>true</c> jeśli obiekty są " + (equal ? "równe" : "różne") + "</returns>");

            Code.Open("public static bool operator " + (equal ? "==" : "!=") + "(" + SourceType.Name + " left, " + SourceType.Name + " right)");
            Code.Writeln("if (left " + rowne + " (object)null && right " + rowne + " (object)null) return " + (equal ? "true" : "false") + ";");
            Code.Writeln("if (left " + rowne + " (object)null || right " + rowne + " (object)null) return " + (equal ? "false" : "true") + ";");

            var i = 0;
            r.Append("return ");
            foreach (var pi in pis)
            {
                if (i++ > 0)
                    r.Append(equal ? " && " : " || ");
                r.Append(string.Format("left.{0} == right.{0}", pi.Name, equal ? " == " : " != "));
            }
            r.Append(i > 0 ? ";" : "true;");
            Code.Writeln(r.ToString());
            Code.Close();
            return r.ToString();
        }

        /// <summary>
        /// Zwraca proponowany kod dla metod GetHashCode, Equals, Clone, CopyFrom oraz operatorów == i !=
        /// </summary>
        /// <returns>kod c#</returns>
        public string StandardMethods()
        {
            #if COREFX
            var pis = SourceType.GetTypeInfo().GetProperties();
            #else
            var pis = SourceType.GetProperties();
            #endif
            Code.Writeln("#region Operators");
            _Operator(pis, true);
            _Operator(pis, false);
            Code.Writeln("#endregion");
            Code.Writeln();

            Code.Writeln("#region Methods");
            _Equals();
            _GetHashCode(pis);
            _CopyFrom(pis);
            _Clone(pis);
            Code.Writeln("#endregion");
            Code.Writeln();
            return Code.Text;
        }



        /// <summary>
        /// Zwraca proponowany kod dla metod GetHashCode, Equals, Clone, CopyFrom oraz operatorów == i !=
        /// </summary>
        /// <param name="type">Klasa, do zbadnia</param>
        /// <returns>kod c#</returns>
        public static string StandardMethods(Type type)
        {
            var cm = new CodeMaker();
            cm.Code = new CsCodeFormatter();
            cm.SourceType = type;
            return cm.StandardMethods();
        }


    }
}
#endif