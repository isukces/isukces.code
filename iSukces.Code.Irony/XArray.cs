using System.Runtime.CompilerServices;

#nullable disable
namespace iSukces.Code.Irony
{
#if NET451
    static class XArray
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] Empty<T>()
        {
            return X<T>.E;
        }

        class X<T>
        {
            #region Fields

            public static readonly T[] E = new T[0];

            #endregion
        }
    }
#else
    static class XArray
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] Empty<T>() => System.Array.Empty<T>();
    }
#endif
}


