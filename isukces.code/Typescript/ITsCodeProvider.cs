using System;
using isukces.code.interfaces;

namespace isukces.code.Typescript
{
    public interface ITsCodeProvider
    {
        void WriteCodeTo(ITsCodeWriter writer);
    }
  
}