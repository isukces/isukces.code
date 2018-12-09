using isukces.code.interfaces;

namespace isukces.code.Serenity
{
    public static class SerenityEntityBuilderExtension
    {
        public static T WithAttribute<T>(this T self, string className, string value)
            where T : IAttributable
        {
            self.RemoveAttribute(className);
            ICsAttribute at = new CsAttribute(className).WithArgument(value);
            self.Attributes.Add(at);
            return self;
        }
        
        public static T WithAttribute<T>(this T self, string className)
            where T : IAttributable
        {
            self.RemoveAttribute(className);
            ICsAttribute at = new CsAttribute(className);
            self.Attributes.Add(at);
            return self;
        }

        public static void RemoveAttribute<T>(this T self, string className) where T : IAttributable
        {
            for (var index = self.Attributes.Count - 1; index >= 0; index--)
            {
                if (!(self.Attributes[index] is CsAttribute csAttribute)) continue;
                if (csAttribute.Name == className)
                    self.Attributes.RemoveAt(index);
            }
        }
    }
}