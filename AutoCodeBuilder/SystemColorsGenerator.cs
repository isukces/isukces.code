using System;
using System.Linq;
using iSukces.Code.Ammy;
using iSukces.Code.AutoCode;

namespace AutoCodeBuilder
{
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
}