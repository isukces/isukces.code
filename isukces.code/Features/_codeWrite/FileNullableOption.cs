using System.Runtime.CompilerServices;

namespace iSukces.Code;

public enum FileNullableOption
{
    Unknown,
    GlobalEnabled,
    GlobalDisabled,
    LocalEnabled,
    LocalDisabled
}

public static class FileNullableOptionExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNullableReferenceEnabled(this FileNullableOption option)
    {
        return option is FileNullableOption.GlobalEnabled or FileNullableOption.LocalEnabled;
    }
}
