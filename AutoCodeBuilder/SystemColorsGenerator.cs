using System;
using System.Linq;
using iSukces.Code.Ammy;
using iSukces.Code.AutoCode;
using System.Windows;

namespace AutoCodeBuilder
{
#if AMMY
    public class SystemColorsGenerator : BaseGenerator, IAutoCodeGenerator
    {
        public void Generate(Type type, IAutoCodeGeneratorContext context)
        {
            var values = Enum
                .GetValues(typeof(SystemColorsKeys))
                .OfType<SystemColorsKeys>().ToArray();
            foreach (var i in values)
            {
                
            }
        }
    }
#endif
}