using System;

namespace isukces.code.AutoCode
{
    public interface IAutoCodeGenerator
    {
        void Generate(Type type, IAutoCodeGeneratorContext context);
    }
}