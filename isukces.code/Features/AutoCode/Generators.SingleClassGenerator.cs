#region using

using System;

#endregion

namespace isukces.code.AutoCode
{
    internal partial class Generators
    {
        #region Nested

        internal abstract class SingleClassGenerator
        {
            #region Constructors

            #endregion

            #region Instance Methods

            protected void Setup(Type type, IAutoCodeGeneratorContext context)
            {
                Type = type;
                _class = null;
                Context = context;
            }

            #endregion

            #region Properties

            protected CsClass Class
            {
                get
                {
                    if (_class != null)
                        return _class;
                    return _class = Context.GetOrCreateClass(Type);
                }
            }

            protected Type Type { get; private set; }

            #endregion

            #region Fields

            private CsClass _class;
            protected IAutoCodeGeneratorContext Context { get; private set; }

            #endregion
        }

        #endregion
    }
}