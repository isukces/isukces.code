using System.Collections.Generic;
using System.Linq;
using iSukces.Code.Interfaces;

namespace iSukces.Code.Typescript
{
    public abstract class TsNamespaceMember : ITsCodeProvider, ITsIntroducedItem
    {
        public TsDecorator AddDecorator(string name)
        {
            var dec = new TsDecorator(name);
            Decorators.Add(dec);
            return dec;
        }

        public abstract void WriteCodeTo(ITsCodeWriter writer);


        public string Name { get; set; }
        public bool IsExported { get; set; }
        public List<TsDecorator> Decorators { get; } = new List<TsDecorator>();
        public ITsCodeProvider Introduction { get; set; }
        public string CommentAbove { get; set; }

        protected void WriteCommonHeaderCode(ITsCodeWriter writer)
        {
            if (!string.IsNullOrEmpty(CommentAbove))
            {
                var lines = CommentAbove.Replace("\r\n", "\n").Split('\n');
                if (lines.Length == 1)
                    writer.WriteLine("// " + lines[0]);
                else
                {
                    writer.WriteLine("/*");
                    foreach (var i in lines)
                        writer.WriteLine(i);
                    writer.WriteLine("*/");
                }
                    
            } 
                
            Introduction?.WriteCodeTo(writer);
            if (Decorators != null && Decorators.Any())
                foreach (var i in Decorators)
                    i.WriteCodeTo(writer);
        }
    }

    public static class TsClassOrEnumExtensions
    {
        public static T WithIsExported<T>(this T self, bool isExported = true) where T : TsNamespaceMember
        {
            self.IsExported = isExported;
            return self;
        }
    }
}