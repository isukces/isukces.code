using Irony.Interpreter;

namespace Bitbrains.AmmyParser
{
    public partial class AstObjectPropertySetting
    {
        public IAstObjectSetting GetData(ScriptThread thread)
        {
            var result = (object[])base.DoEvaluate(thread);
            return new AstObjectPropertySettingData(
                (string)result[0],
                result[1],
                Span
            );
        }

        protected override object DoEvaluate(ScriptThread thread) => GetData(thread);
    }
}