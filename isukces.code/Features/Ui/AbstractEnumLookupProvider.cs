using System;
using System.Reflection;

namespace isukces.code.Ui
{
    public class AbstractEnumLookupProvider<T> : IEnumLookupProvider
    {
        protected static Item Make(T value, string label = null)
        {
            if (string.IsNullOrEmpty(label))
                label = value.ToString();
            return new Item(value, label);
        }

        public string GetDisplayMemberPath() => nameof(Item.TextLabel);

        public string GetSelectedValuePath() => nameof(Item.Value);

        public Tuple<Type, string> GetSourceStaticProperty()
        {
            const string enumvalues = "EnumValues";
            var          type       = GetType();
            var pi = type
#if COREFX
                .GetTypeInfo()
#endif
                .GetProperty(enumvalues, BindingFlags.Public | BindingFlags.Static);
            if (pi is null)
                throw new Exception($"Type {type} has no static property {enumvalues}.");
            return new Tuple<Type, string>(type, enumvalues);
        }

        public class Item
        {
            public Item(T value, string textLabel)
            {
                if (string.IsNullOrEmpty(textLabel))
                    textLabel = value.ToString();
                Value     = value;
                TextLabel = textLabel;
            }

            public T      Value     { get; }
            public string TextLabel { get; }
        }
    }
}