using System;

namespace isukces.code.AutoCode
{
    internal partial class Generators
    {
        internal abstract class SingleClassGenerator
        {
            protected void Setup(Type type, IAutoCodeGeneratorContext context)
            {
                Type = type;
                _class = null;
                Context = context;
            }

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

            private CsClass _class;
            protected IAutoCodeGeneratorContext Context { get; private set; }
        }
    }
}