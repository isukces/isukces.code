﻿using System.Collections.Generic;

namespace isukces.code.Typescript
{
    public abstract class TsNamespaceMember : ITsCodeProvider
    {
        public TsDecorator AddDecorator(string name)
        {
            var dec = new TsDecorator
            {
                Name = name
            };
            Decorators.Add(dec);
            return dec;
        }

        public abstract void WriteCodeTo(TsWriteContext cf);


        public string Name { get; set; }
        public bool IsExported { get; set; }
        public List<TsDecorator> Decorators { get; set; } = new List<TsDecorator>();
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