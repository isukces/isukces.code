#nullable enable
using System;

namespace iSukces.Code;

public sealed class GlobalSettings
{
    // public const bool WarnObsolete = true;
    // ReSharper disable ConvertToConstant.Global
    public static string                       CommaSeparator             = ", ";
    public static string                       AssignEqual                = " = ";
    public static bool                         DefaultReSharperDisableAll = true;
    public static InvalidOperationNotification DoNotAllowCsTypeToString   = InvalidOperationNotification.ThrowException;
    public static Action<string>               EmergencyLog               = Console.WriteLine;
    // ReSharper restore ConvertToConstant.Global

}

public enum InvalidOperationNotification
{
    Ignore,
    ThrowException,
    ReturnWarningText,
    EmergencyLog
}