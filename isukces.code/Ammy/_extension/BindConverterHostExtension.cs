using System;
using isukces.code.interfaces.Ammy;

namespace isukces.code.Ammy
{
    public static class BindConverterHostExtension
    {
        public static T WithConverter<T>(this T src, object converter)
            where T : IAmmyBindConverterHost
        {
            src.SetBindConverter(converter);
            return src;
        }

        public static T WithConverterStatic<T>(this T src, Type type, string property)
            where T : IAmmyBindConverterHost
        {
            src.SetBindConverter(new StaticBindingSource(type, property));
            return src;
        }

        public static T WithConverterStaticResource<T>(this T src, string resourceName)
            where T : IAmmyBindConverterHost
        {
            src.SetBindConverter(new AmmyStaticResource(resourceName));
            return src;
        }
    }
}