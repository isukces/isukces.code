using System;
using iSukces.Code;
using iSukces.Code.AutoCode;
using iSukces.Code.Interfaces;
using iSukces.Code.Interfaces.Ammy;

/*
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
                var cf = CreateCode(nameof(AmmyBindSourceHostGenerator),"G1")
                    .WriteLine("return this.WithBindFromAncestor(typeof(T), level);");

                var m = CreateMethod("WithBindFromAncestor<T>", type, cl, cf);
                m.AddParam("level", "int?").WithConstValueNull();
            }
        }
    }
}
*/