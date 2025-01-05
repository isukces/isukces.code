using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace iSukces.Code.Translations
{
    internal static class iSukces_Code_Translations_Extensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SureNotNull<T>(this T value, string name)
            where T : class
        {
            if (value is null)
                throw new NullReferenceException(name);
            return value;
        }
    }
}
