using System;
using System.Linq;
using System.Reflection;

namespace isukces.code.AutoCode
{
    public partial class Generators
    {
        public abstract class SingleClassGenerator<TAttribute> : SingleClassGenerator
            where TAttribute : Attribute
        {
            public override void Generate(Type type, IAutoCodeGeneratorContext context)
            {
                Attribute = type.GetTypeInfo().GetCustomAttribute<TAttribute>();
                if (Attribute == null)
                    return;
                base.Generate(type, context);
            }

            protected TAttribute Attribute { get; private set; }
        }

        public abstract class SingleClassGeneratorMultiple<TAttribute>
            : SingleClassGenerator
            where TAttribute : Attribute
        {
            public override void Generate(Type type, IAutoCodeGeneratorContext context)
            {
                Attributes = type
#if COREFX
                    .GetTypeInfo()
#endif
                    .GetCustomAttributes<TAttribute>(false).ToArray();
                if (Attributes == null || Attributes.Length < 1) return;
                base.Generate(type, context);
            }


            protected TAttribute[] Attributes { get; private set; }
        }


        public abstract class SingleClassGenerator : IAutoCodeGenerator
        {
            protected void Setup(Type type, IAutoCodeGeneratorContext context)
            {
                Type    = type;
                _class  = null;
                Context = context;
            }
            
            public virtual void Generate(Type type, IAutoCodeGeneratorContext context)
            {
                Setup(type, context);
                GenerateInternal();
            }

            protected abstract void GenerateInternal();

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

            protected Type                      Type    { get; private set; }
            protected IAutoCodeGeneratorContext Context { get; private set; }

            protected T GetCustomAttribute<T>(bool inherit= true)
                where T : Attribute
            {
                return Type
#if COREFX
                    .GetTypeInfo()
#endif
                    .GetCustomAttribute<T>(inherit);
            }

            protected T[] GetCustomAttributes<T>(bool inherit= true)
                where T : Attribute
            {
                return Type
#if COREFX
                    .GetTypeInfo()
#endif
                    .GetCustomAttributes<T>(inherit).ToArray();
            }

            private CsClass _class;
        }
 
    }
}