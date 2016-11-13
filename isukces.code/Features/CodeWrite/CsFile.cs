#region using

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using isukces.code.interfaces;

#endregion

namespace isukces.code.CodeWrite
{
    public class CsFile : ICsCodeMaker
    {
        #region Instance Methods

        public void Append(CsFile x)
        {
            _classes.AddRange(x._classes);
            _importNamespaces = _importNamespaces.Union(x._importNamespaces).Distinct().ToList();
        }


        // Public Methods 

        public string GetCode()
        {
            var w = new CodeWriter();
            MakeCode(w);
            return w.ToString();
        }

        public void MakeCode(ICodeWriter writer)
        {
            foreach (var i in _importNamespaces.Distinct().OrderBy(i => i))
                writer.WriteLine("using {0};", i);
            if (_importNamespaces.Any())
                writer.EmptyLine();
            writer.Open("namespace {0}", _nameSpace);
            {
                var addEmptyLine = false;
                foreach (var i in _classes)
                {
                    if (addEmptyLine)
                        writer.EmptyLine();
                    addEmptyLine = true;
                    i.MakeCode(writer);
                }
                if (_enums != null)
                    foreach (var i in _enums)
                    {
                        if (addEmptyLine)
                            writer.EmptyLine();
                        addEmptyLine = true;
                        i.MakeCode(writer);
                    }
            }
            writer.Close();
        }

        public void Save(string filename)
        {
            var fi = new FileInfo(filename);
            if (fi.Directory == null)
                throw new NullReferenceException("fi.Directory");
            fi.Directory.Create();
            var x = Encoding.UTF8.GetBytes(GetCode());
            using(var fs = new FileStream(filename, File.Exists(filename) ? FileMode.Create : FileMode.CreateNew))
            {
                fs.Write(x, 0, x.Length);
                fs.Close();
            }
        }

        public void SaveIfDifferent(string filename)
        {
            if (!File.Exists(filename))
            {
                Save(filename);
                return;
            }
            var text = GetCode();
            var existing = File.ReadAllBytes(filename);
            var newa = Encoding.UTF8.GetBytes(text);
            if (newa.SequenceEqual(existing)) return;
            File.WriteAllBytes(filename, newa);
        }

        public override string ToString()
        {
            return GetCode();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     przestrzeń nazw pliku
        /// </summary>
        public string NameSpace
        {
            get { return _nameSpace; }
            set
            {
                value = value?.Trim() ?? string.Empty;
                _nameSpace = value;
            }
        }

        /// <summary>
        ///     Przestrzenie nazw
        /// </summary>
        public List<string> ImportNamespaces
        {
            get { return _importNamespaces; }
            set
            {
                if (value == null) value = new List<string>();
                _importNamespaces = value;
            }
        }

        /// <summary>
        ///     Przestrzenie nazw
        /// </summary>
        public List<CsClass> Classes
        {
            get { return _classes; }
            set
            {
                if (value == null) value = new List<CsClass>();
                _classes = value;
            }
        }

        /// <summary>
        ///     Przestrzenie nazw
        /// </summary>
        public List<CsEnum> Enums
        {
            get { return _enums; }
            set
            {
                if (value == null) value = new List<CsEnum>();
                _enums = value;
            }
        }

        /// <summary>
        /// </summary>
        public string SuggestedFileName
        {
            get { return _suggestedFileName; }
            set
            {
                value = value?.Trim() ?? string.Empty;
                _suggestedFileName = value;
            }
        }

        #endregion

        #region Fields

        private string _nameSpace = "isukces.nonamespace";
        private List<string> _importNamespaces = new List<string>();
        private List<CsClass> _classes = new List<CsClass>();
        private List<CsEnum> _enums = new List<CsEnum>();
        private string _suggestedFileName = string.Empty;

        #endregion
    }
}