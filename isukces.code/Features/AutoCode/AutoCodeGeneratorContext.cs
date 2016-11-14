using System;
using System.Reflection;

namespace isukces.code.AutoCode
{
    public class AutoCodeGeneratorContext : IAutoCodeGeneratorContext
    {
        public Type ListExtension { get; set; }
        public MethodInfo CustomCloneMethod { get; set; }
    }
}