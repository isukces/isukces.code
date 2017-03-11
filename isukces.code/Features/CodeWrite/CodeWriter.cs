#region using

using System.IO;
using System.Text;
using isukces.code.interfaces;

#endregion

namespace isukces.code.CodeWrite
{
    public class CodeWriter : ICodeWriter
    {
        public void AppendText(string text)
        {
            _sb.Append(text);
        }

        public void Save(string filename)
        {
            var code = ToString();
            if (File.Exists(filename))
            {
                var currentCode = Encoding.UTF8.GetString(File.ReadAllBytes(filename));
                if (currentCode == code) return;
            }
            File.WriteAllBytes(filename, Encoding.UTF8.GetBytes(code));
        }

        public override string ToString()
        {
            return _sb.ToString();
        }


        private void AddIndent()
        {
            if (Indent > 0)
                _sb.Append(new string(' ', Indent * 4));
        }

        public int Indent { get; set; }
        public string Code => _sb.ToString();


        private readonly StringBuilder _sb = new StringBuilder();
    }
}