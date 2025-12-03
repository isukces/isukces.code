using System.IO;
using System.Linq;
using System.Text;
using iSukces.Code.Interfaces;

namespace iSukces.Code;

public abstract class CodeWriter : ICodeWriter
{
    protected CodeWriter(ILangInfo langInfo)
    {
        LangInfo = langInfo;
    }

    public void Append(string text) => _sb.Append(text);

    protected virtual string GetCodeForSave() => Code;

    public void Save(string filename)
    {
        var fi = new FileInfo(filename);
        fi.Directory?.Create();
        using var fs = new FileStream(filename, File.Exists(filename) ? FileMode.Create : FileMode.CreateNew);
        SaveToStream(fs);
    }

    public bool SaveIfDifferent(string filename)
    {
        if (!File.Exists(filename))
        {
            Save(filename);
            return true;
        }

        byte[] newa;
        var existing = File.ReadAllBytes(filename);
        using(var fs = new MemoryStream())
        {
            SaveToStream(fs);
            newa = fs.ToArray();
        }

        if (newa.SequenceEqual(existing)) 
            return false;
        File.WriteAllBytes(filename, newa);
        return true;
    }

    private void SaveToStream(Stream stream)
    {
#if BOM
        if (LangInfo.AddBOM)
            stream.Write(new byte[] {0xEF, 0xBB, 0xBF}, 0, 3);
#endif
        var x = Encoding.UTF8.GetBytes(GetCodeForSave());
        stream.Write(x, 0, x.Length);
    }
    
    public override string ToString() => Code;

    /// <summary>
    ///     opis języka
    /// </summary>
    public ILangInfo LangInfo { get; }

    public string[] Lines => Code.SplitToLines();

    /// <summary>
    ///     Czy trimować tekst
    /// </summary>
    public bool TrimText { get; set; }

    public void Replace(string search, string replaceBy)
    {
        var code = _sb.ToString();
        code = code.Replace(search, replaceBy);
        _sb.Clear();
        _sb.Append(code);
    }

    public string Code
    {
        get
        {
            var x = _sb.ToString();
            if (TrimText) 
                return x.Trim();
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
