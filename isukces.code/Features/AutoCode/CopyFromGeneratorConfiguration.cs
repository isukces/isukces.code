#region using

using System;
using System.Reflection;
using isukces.code.interfaces;

#endregion

namespace isukces.code.AutoCode
{
    public class CopyFromGeneratorConfiguration : IAutoCodeConfiguration
    {
        #region Properties

        public Type ListExtension { get; set; }
        public MethodInfo CustomCloneMethod { get; set; }

        #endregion
    }
}