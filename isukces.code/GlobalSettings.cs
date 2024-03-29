using System;
using System.Reflection;
using JetBrains.Annotations;

namespace iSukces.Code;

public sealed class GlobalSettings
{
    public const bool WarnObsolete = false;
    
    public static string CommaSeparator = ", ";
    public static string AssignEqual = " = ";
    public static bool DefaultReSharperDisableAll = true;
    public static InvalidOperationNotification DoNotAllowCsTypeToString = InvalidOperationNotification.ThrowException;
    
    
    public static Action<string>  EmergencyLog = Console.WriteLine;
 
}

public enum InvalidOperationNotification
{
    Ignore,
    ThrowException,
    ReturnWarningText,
    EmergencyLog
}