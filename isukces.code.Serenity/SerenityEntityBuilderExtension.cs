using isukces.code.interfaces;

namespace isukces.code.Serenity
{
    public static class SerenityEntityBuilderExtension
    {
        public static T WithAttribute<T>(this T self, string className, string value)
            where T : IAttributable
        {
            ICsAttribute at = new CsAttribute(className).WithArgument(value);
            return WithAttribute(self, at);
        }
        
        public static T WithAttribute<T>(this T self, string className)
            where T : IAttributable
        {
            ICsAttribute at = new CsAttribute(className);
            return WithAttribute(self, at);
        }
        
        public static T WithAttribute<T>(this T self, ICsAttribute attr)
            where T : IAttributable
        {
            self.RemoveAttribute(attr.Name);            
            self.Attributes.Add(attr);
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