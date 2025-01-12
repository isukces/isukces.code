using System;

namespace iSukces.Code;

public static class GlobalSettings
{
    public static void CheckFilename(string? fileName)
    {
        if (string.IsNullOrEmpty(fileName))
            throw new ArgumentException("File name can't be empty");
        if (fileName.StartsWith(SlashAppPrefix, StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("File starting with " + SlashAppPrefix + " is not allowed");
        
    }

    public const string SlashAppPrefix = "/_/app/";


    // public const bool WarnObsolete = true;
    // ReSharper disable ConvertToConstant.Global
    // ReSharper disable FieldCanBeMadeReadOnly.Global
    public static string CommaSeparator = ", ";
    public static string AssignEqual = " = ";
    public static bool DefaultReSharperDisableAll = true;
    public static InvalidOperationNotification DoNotAllowCsTypeToString = InvalidOperationNotification.ThrowException;
    public static bool RejectFilenameWithSlashAppPrefix = true;
    public static Action<string> EmergencyLog = Console.WriteLine;
    // ReSharper restore ConvertToConstant.Global
    // ReSharper restore FieldCanBeMadeReadOnly.Global
}

public enum InvalidOperationNotification
{
    Ignore,
    ThrowException,
    ReturnWarningText,
    EmergencyLog
}

