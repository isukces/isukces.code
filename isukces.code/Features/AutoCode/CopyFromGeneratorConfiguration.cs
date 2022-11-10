using System;
using System.Reflection;

namespace iSukces.Code.AutoCode
{
    public class CopyFromGeneratorConfiguration 
    {
        public Type ListExtension { get; set; }
        public MethodInfo CustomCloneMethod { get; set; }
    }
}