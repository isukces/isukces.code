using System;
using System.Reflection;
using iSukces.Code.Interfaces;

namespace iSukces.Code.AutoCode
{
    public class CopyFromGeneratorConfiguration 
    {
        public Type ListExtension { get; set; }
        public MethodInfo CustomCloneMethod { get; set; }
    }
}