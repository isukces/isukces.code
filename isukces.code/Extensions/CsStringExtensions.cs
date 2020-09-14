using System.Globalization;
using System.Runtime.CompilerServices;

namespace iSukces.Code
{
    public static class CsStringExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static string ToCsString(this int x) => x.ToString(CultureInfo.InvariantCulture);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static string ToCsString(this double x) => x.ToString(CultureInfo.InvariantCulture);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static string ToCsString(this long x) => x.ToString(CultureInfo.InvariantCulture);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static string ToCsString(this short x) => x.ToString(CultureInfo.InvariantCulture);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static string ToCsString(this ulong x) => x.ToString(CultureInfo.InvariantCulture);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static string ToCsString(this ushort x) => x.ToString(CultureInfo.InvariantCulture);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static string ToCsString(this byte x) => x.ToString(CultureInfo.InvariantCulture);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static string ToCsString(this uint x) => x.ToString(CultureInfo.InvariantCulture);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static string ToCsString(this sbyte x) => x.ToString(CultureInfo.InvariantCulture);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static string ToCsString(this float x) => x.ToString(CultureInfo.InvariantCulture);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static string ToCsString(this decimal x) => x.ToString(CultureInfo.InvariantCulture);
    }
}