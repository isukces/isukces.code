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

            protected SingleClassGenerator(Type type, Func<Type, CsClass> classFactory)
            {
                _type = type;
                _classFactory = classFactory;
            }

            #endregion

            #region Properties

            protected CsClass Class
            {
                get
                {
                    if (_class != null)
                        return _class;
                    return _class = _classFactory(_type);
                }
            }

            protected Type Type => _type;

            #endregion

            #region Fields

            private readonly Type _type;
            private readonly Func<Type, CsClass> _classFactory;
            private CsClass _class;

            #endregion
        }

        #endregion
    }
}