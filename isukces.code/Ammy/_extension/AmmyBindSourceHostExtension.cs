using System;
using isukces.code.interfaces.Ammy;

namespace isukces.code.Ammy
{
    public static class AmmyBindSourceHostExtension
    {
        
        public static T WithBindFromAncestor<T>(this T src, Type type, int? level=null)
            where T : IAmmyBindSourceHost
        {
            src.SetBindingSource(new AncestorBindingSource(type, level));
            return src;
        }
    }
}