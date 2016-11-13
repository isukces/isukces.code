#region using

using System.IO;
using System.Text;
using isukces.code.interfaces;

#endregion

namespace isukces.code.CodeWrite
{
    public class CodeWriter : ICodeWriter
    {
        #region Instance Methods

        public void AppendText(string text)
        {
            _sb.Append(text);
        }

        #endregion

        #region Properties

        public int Indent { get; set; }

        #endregion

        #region Fields

        private readonly StringBuilder _sb = new StringBuilder();

        #endregion

        #region Methods

        // Public Methods 


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


        // Private Methods 

        private void AddIndent()
        {
            if (Indent > 0)
                _sb.Append(new string(' ', Indent * 4));
        }

        #endregion Methods
    }
}