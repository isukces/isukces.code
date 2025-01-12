using System;
using System.Collections.Generic;

namespace iSukces.Code;

public sealed class CsPrimaryConstructor
{
    public CsPrimaryConstructor(IReadOnlyList<CsMethodParameter>? arguments)
    {
        Arguments = arguments ?? Array.Empty<CsMethodParameter>();
    }

    public CsPrimaryConstructor(params CsMethodParameter[]? arguments)
    {
        Arguments = arguments ?? Array.Empty<CsMethodParameter>();
    }

    public IReadOnlyList<CsMethodParameter> Arguments { get; }
}


/*
 public sealed class CsImplicitConstructor(IReadOnlyList<CsMethodParameter> arguments)
{
    public IReadOnlyList<CsMethodParameter> Arguments { get; } = arguments;
}

 */
