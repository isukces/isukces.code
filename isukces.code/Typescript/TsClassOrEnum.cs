using System.Collections.Generic;

namespace isukces.code.Typescript
{
    public abstract class TsClassOrEnum
    {
        public TsDecorator AddDecorator<T>(string name)
        {
            var dec = new TsDecorator
            {
                Name = name
            };
            Decorators.Add(dec);
            return dec;
        }

        public string Name { get; set; }
        public bool IsExported { get; set; }
        public List<TsDecorator> Decorators { get; set; } = new List<TsDecorator>();
    }

    public static class TsClassOrEnumExtensions
    {
        public static T WithIsExported<T>(this T self, bool isExported = true) where T : TsClassOrEnum
        {
            self.IsExported = isExported;
            return self;
        }
    }
}