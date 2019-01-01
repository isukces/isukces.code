using System.IO;
using System.Linq;
using System.Text;
using isukces.code.interfaces;

namespace isukces.code
{
    public abstract class CodeWriter : ICodeWriter
    {
        protected CodeWriter(ILangInfo langInfo)
        {
            LangInfo = langInfo;
        }

        public void Append(string text)
        {
            _sb.Append(text);
        }
/*
        public void Block(string open, string customCloseText, Action method)
        {
            Writeln(open);
            this.IncIndent();
            method();
            this.Close(customCloseText);
        }

        public void Block(string open, Action method)
        {
            Writeln(open);
            this.IncIndent();
            method();
            this.Close();
        }


        public void Clear()
        {
            _sb     = new StringBuilder();
            Indent  = 0;
            // opening = new Dictionary<int, string>();
        }
*/

        public void Save(string filename)
        {
            var fi = new FileInfo(filename);
            fi.Directory.Create();
            var x = Encoding.UTF8.GetBytes(Code);
            using(var fs = new FileStream(filename, File.Exists(filename) ? FileMode.Create : FileMode.CreateNew))
            {
                if (LangInfo.AddBOM)
                    fs.Write(new byte[] {0xEF, 0xBB, 0xBF}, 0, 3);
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
            using(var fs = new MemoryStream())
            {
                {
                    if (LangInfo.AddBOM)
                        fs.Write(new byte[] {0xEF, 0xBB, 0xBF}, 0, 3);
                    var x = Encoding.UTF8.GetBytes(Code);
                    fs.Write(x, 0, x.Length);
                }
                newa = fs.ToArray();
            }

            if (newa.SequenceEqual(existing)) return;
            File.WriteAllBytes(filename, newa);
        }


        // public bool AutoCloseComment { get; set; }

        public override string ToString()
        {
            return Code;
        }

        /*
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
        */

        /// <summary>
        ///     opis języka
        /// </summary>
        public ILangInfo LangInfo { get; }

        public string[] Lines
        {
            get
            {
                var tmp = Code;
                var l   = tmp.Split('\n');
                for (var i = 0; i < l.Length; i++)
                    l[i] = l[i].Replace("\r", "");
                return l;
            }
        }

        /// <summary>
        ///     Czy trimować tekst
        /// </summary>
        public bool TrimText { get; set; }

        public string Code
        {
            get
            {
                var x = _sb.ToString();
                if (TrimText) return x.Trim();
                return x;
            }
        }


        public int Indent
        {
            get => _indent;
            set
            {
                if (value < 0)
                    value = 0;
                if (value == _indent)
                    return;

                _indent = value;
                _indentStr = value > 0
                    ? new string(' ', IndentSize * value)
                    : string.Empty;
            }
        }


        private string _indentStr = "";

        // private Dictionary<int, string> opening = new Dictionary<int, string>();
        private readonly StringBuilder _sb = new StringBuilder();
        private int _indent;
        private const int IndentSize = 4;
    }
}