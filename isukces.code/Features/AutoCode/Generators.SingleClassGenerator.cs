using System;
using System.Reflection;

namespace isukces.code.AutoCode
{
    public partial class Generators
    {
        public abstract class SingleClassGenerator
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
                    _class = Context.GetOrCreateClass(Type);          
                    return _class;
                }
            }

            protected Type Type { get; private set; }

            private CsClass _class;
            protected IAutoCodeGeneratorContext Context { get; private set; }
        }
    }
}