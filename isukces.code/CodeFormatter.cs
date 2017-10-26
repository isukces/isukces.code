using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using isukces.code.Typescript;

namespace isukces.code
{
    public delegate void CodeFormatterDelegate();

    public class CodeFormatter
    {
        public void Block(string open, string close, CodeFormatterDelegate method)
        {
            Writeln(open);
            IncIndent();
            method();
            Close(close);
        }

        public void Block(string open, CodeFormatterDelegate method)
        {
            Writeln(open);
            IncIndent();
            method();
            Close();
        }


        public void Clear()
        {
            _sb = new StringBuilder();
            _indent = 0;
            opening = new Dictionary<int, string>();
        }

        public void Close()
        {
            Close(false);
        }

        public void Close(string customCloseText)
        {
            DecIndent();
            Writeln(customCloseText);
        }

        public void Close(bool addEmptyLine)
        {
            DecIndent();
            if (AutoCloseComment)
                Writeln(LangInfo.CloseText + " " + LangInfo.OneLineComment + " " + opening[_indent]);
            else
                Writeln(LangInfo.CloseText);
            if (addEmptyLine)
                Writeln();
        }


        public void CloseNl()
        {
            Close(false);
            Writeln();
        }

        public void DecIndent()
        {
            if (_indent == 0) return;
            _indent--;
            _indentStr = _indentStr.Substring(INDENT.Length);
        }

        public void IncIndent()
        {
            _indent++;
            _indentStr += INDENT;
        }

        public void Open(string x)
        {
            opening[_indent] = x;
            // Writeln(x + " " + LangInfo.OpenText);
            Writeln(x);
            Writeln(LangInfo.OpenText);
            IncIndent();
        }

        public void Open(string format, params object[] args)
        {
            Open(string.Format(format, args));
        }

        public void Save(string filename)
        {
            var fi = new FileInfo(filename);
            fi.Directory.Create();
            var x = Encoding.UTF8.GetBytes(Text);
            using (var fs = new FileStream(filename, File.Exists(filename) ? FileMode.Create : FileMode.CreateNew))
            {
                if (LangInfo.AddBOM)
                    fs.Write(new byte[] { 0xEF, 0xBB, 0xBF }, 0, 3);
                fs.Write(x, 0, x.Length);
                // fs.Close();
            }
        }

        public void SaveIfDifferent(string filename)
        {
            if (!File.Exists(filename))
            {
                Save(filename);
                return;
            }
            byte[] existing, newa;
            existing = File.ReadAllBytes(filename);
            using (var fs = new MemoryStream())
            {
                {
                    if (LangInfo.AddBOM)
                        fs.Write(new byte[] { 0xEF, 0xBB, 0xBF }, 0, 3);
                    var x = Encoding.UTF8.GetBytes(Text);
                    fs.Write(x, 0, x.Length);
                }
                newa = fs.ToArray();
            }
            if (newa.SequenceEqual(existing)) return;
            File.WriteAllBytes(filename, newa);
        }

        public void Writeln(TsEnumItem format)
        {
            _sb.AppendLine();
        }

        public virtual void Writeln(string x = null)
        {
            if (x == (object)null)
            {
                _sb.AppendLine();
                return;
            }
            //x = x.Trim();
            if (string.IsNullOrEmpty(x))
                _sb.AppendLine();
            else
                _sb.AppendLine(_indentStr + x);
        }

        public void Writeln(string format, params object[] args)
        {
            Writeln(string.Format(format, args));
        }

        /// <summary>
        ///     opis języka
        /// </summary>
        public ILangInfo LangInfo { get; set; }

        public string[] Lines
        {
            get
            {
                var tmp = Text;
                var l = tmp.Split('\n');
                for (var i = 0; i < l.Length; i++)
                {
                    l[i] = l[i].Replace("\r", "");
                }
                return l;
            }
        }

        /// <summary>
        ///     Czy trimować tekst
        /// </summary>
        public bool TrimText { get; set; }


        public string Text
        {
            get
            {
                var x = _sb.ToString();
                if (TrimText) return x.Trim();
                return x;
            }
        }


        public bool AutoCloseComment { get; set; }

        private int _indent;
        private string _indentStr = "";
        private Dictionary<int, string> opening = new Dictionary<int, string>();
        private StringBuilder _sb = new StringBuilder();

        private const string INDENT = "    ";
    }
}