using System;
using System.Diagnostics;

namespace iSukces.Code.Interfaces;

[AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
[Conditional("AUTOCODE_ANNOTATIONS")]
public class AddInterfaceAttribute : Attribute
{
    public AddInterfaceAttribute(Type _interface)
    {
        Interface = _interface;
    }

    /// <summary>
    /// Interfejs implementowany przez klasę
    /// </summary>
    public Type Interface { get; set; }
}