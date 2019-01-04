using System.Collections.Generic;
using JetBrains.Annotations;

namespace isukces.code.interfaces.Ammy
{
    public interface IAmmyContainer : IAmmyPropertyContainer, IAmmyContentItemsContainer
    {
    }

    public interface IAmmyPropertyContainer
    {
        [NotNull]
        IDictionary<string, object> Properties { get; }
    }

    public interface IAmmyContentItemsContainer
    {
        [NotNull]
        IList<object> ContentItems { get; }
    }

    public static class AmmyContainerExt
    {
        public static T WithProperty<T>(this T self, string name, object value)
            where T : IAmmyPropertyContainer
        {
            self.Properties[name] = value;
            return self;
        }

        public static T WithPropertyNotNull<T>(this T self, string name, object value)
            where T : IAmmyPropertyContainer
        {
            if (value != null)
                self.Properties[name] = value;
            else
                self.Properties.Remove(name);
            return self;
        }

        public static T WithContent<T>(this T self, object value)
            where T : IAmmyContentItemsContainer
        {
            self.ContentItems.Add(value);
            return self;
        }
        
        
        

    }
}