#nullable enable
using System;

namespace iSukces.Code;

public class AutoCodeSettings
{
    public StaticMethodInfo GetUrlStringEncodeOrThrow()
    {
        var tmp = UrlStringEncode;
        if (tmp.IsEmpty)
            throw new Exception(nameof(UrlStringEncode) + " is empty");
        return tmp;
    }

    public static AutoCodeSettings Default => InstanceHolder.DefaultInstance;

    /// <summary>
    /// Initialize this with
    /// AutoCodeSettings.Default.UrlStringEncode = new StaticMethodInfo(
    ///     typeof(System.Net.WebUtility),
    ///     nameof(System.Net.WebUtility.UrlEncode));
    /// </summary>
    public StaticMethodInfo? UrlStringEncode { get; set; }


    private class InstanceHolder
    {
        public static readonly AutoCodeSettings DefaultInstance = new AutoCodeSettings();
    }
}