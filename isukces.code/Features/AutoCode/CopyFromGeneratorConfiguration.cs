﻿using System;
using System.Reflection;
using isukces.code.interfaces;

namespace isukces.code.AutoCode
{
    public class CopyFromGeneratorConfiguration 
    {
        public Type ListExtension { get; set; }
        public MethodInfo CustomCloneMethod { get; set; }
    }
}