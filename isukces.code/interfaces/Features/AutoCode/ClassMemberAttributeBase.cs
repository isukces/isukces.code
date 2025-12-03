using System;
using System.Diagnostics;

namespace iSukces.Code.Interfaces;

[Conditional("AUTOCODE_ANNOTATIONS")]
public abstract class ClassMemberAttributeBase : Attribute
{
    public string Name { get; set; }

    public string       Description { get; set; }
    public Visibilities Visibility  { get; set; }
}