#if OBSOLETE
using System;
using iSukces.Code.Interfaces;

namespace iSukces.Code.CodeWrite
{
    [Obsolete]
    public static class CSharpCodeWriterExtension
    {
       
     
        [Obsolete]
        public static ICodeWriter OpenBrackets(this ICodeWriter _this)
        {
            _this.WriteLine("{");
            _this.Indent++;
            return _this;
        }
        
        [Obsolete]
        public static ICodeWriter CloseBrackets(this ICodeWriter _this)
        {
            _this.Indent--;
            _this.WriteLine("}");
            return _this;
        }


        [Obsolete]
        public static ICodeWriter WriteLine(this ICodeWriter _this, string format, params object[] parameters)
        {
            // _this.Indent++;
            if (_this.Indent > 0)
                _this.AppendText(GetIndent(_this));
            _this.AppendText(string.Format(format + "\r\n", parameters));
            return _this;
        }

        public static ICodeWriter Open(this ICodeWriter _this, string text)
        {
            _this.WriteLine(text);
            _this.WriteLine("{");
            _this.Indent++;
            return _this;
        }

        public static ICodeWriter Open(this ICodeWriter _this, string format, params object[] parameters)
        {
            return _this.Open(string.Format(format, parameters));
        }


        public static ICodeWriter Close(this ICodeWriter _this)
        {
            _this.Indent--;
            _this.WriteLine("}");
            return _this;
        }


        

 

      
 
      
    }
}
#endif
