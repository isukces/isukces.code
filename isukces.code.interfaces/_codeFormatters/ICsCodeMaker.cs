﻿using System;
using JetBrains.Annotations;

namespace isukces.code.interfaces
{
    /*
   

    public interface ICodeWriter
    {
        void AppendText(string text);

        [NotNull]
        string GetCode();

        int Indent { get; set; }

 
    }

    public static class CodeWriterExtensions
    {
        [NotNull]
        public static T AppendTextFormat<T>([NotNull] this T writer, string format, params object[] args)
            where T : ICodeWriter
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));
            var text = string.Format(format, args);
            writer.AppendText(text);
            return writer;
        }

      
    }
    
    
    public interface ICsCodeMaker
    {
        void MakeCode(ICsCodeWritter writer);
    }
    */
}