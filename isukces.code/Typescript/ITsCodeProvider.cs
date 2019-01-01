using System;

namespace isukces.code.Typescript
{
    public interface ITsCodeProvider
    {
        void WriteCodeTo(TsCodeFormatter formatter);
    }
  
}