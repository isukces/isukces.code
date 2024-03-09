using System;
using JetBrains.Annotations;

namespace iSukces.Code.Irony;

public class FullTypeName
{
    public FullTypeName([NotNull] CsType name)
    {
        name.ThrowIfVoid();
        Name = name;
    }

    public override string ToString() => Name.Declaration;

    public CsType Name { get; }
}