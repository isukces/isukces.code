using System;
using isukces.code;
using isukces.code.AutoCode;
using isukces.code.interfaces;
using isukces.code.interfaces.Ammy;

namespace AutoCodeBuilder
{
    internal class AmmyBindSourceHostGenerator : BaseGenerator, IAutoCodeGenerator
    {
        public void Generate(Type type, IAutoCodeGeneratorContext context)
        {
            if (IgnoreType(type)) return;
            if (NotImplements<IAmmyBindSourceHost>(type)) return;
            context.AddNamespace<IAmmyBindSourceHost>();

            var cl = context.GetOrCreateClass(type);
            {
                var cf = new CsCodeWriter();
                cf.WriteLine("return this.WithBindFromAncestor(typeof(T), level);");

                var m = CreateMethod("WithBindFromAncestor<T>", type, cl, cf);
                m.AddParam("level", "int?").WithConstValueNull();
            }
        }
    }
}