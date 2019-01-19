using System;
using System.Collections.Generic;
using isukces.code;
using isukces.code.Ammy;
using isukces.code.AutoCode;
using isukces.code.interfaces;
using isukces.code.interfaces.Ammy;
using JetBrains.Annotations;

namespace AutoCodeBuilder
{
    internal class AmmyBindConverterHostGenerator : BaseGenerator, IAutoCodeGenerator
    {
        public void Generate(Type type, IAutoCodeGeneratorContext context)
        {
            if (IgnoreType(type)) return;
            if (FluentBindGenerator.IgnoreWithConverterStatic(type)) return;
            if (NotImplements<IAmmyBindConverterHost>(type)) return;
            context.AddNamespace<IAmmyBindConverterHost>();

            var cl = context.GetOrCreateClass(type);
            {
                var cf = CreateCode(nameof(AmmyBindConverterHostGenerator), "G1")
                        .WriteLine("return this.WithConverterStatic(typeof(T), propertyName);");

                var m = CreateMethod("WithConverterStatic<T>", type, cl, cf);
                m.AddParam("propertyName", "string");
            }
        }
    }
}