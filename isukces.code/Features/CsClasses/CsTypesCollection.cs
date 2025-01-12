using System.Collections.Generic;

namespace iSukces.Code;

public sealed class CsTypesCollection : List<CsType>
{
    public void Add(string name) => Add(new CsType(name));
}

