using Irony.Ast;
using Irony.Interpreter;
using Irony.Parsing;

namespace Bitbrains.AmmyParser
{
    public partial class AstMixinDefinition
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            AsString = GetType().Name;
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            return GetData(thread);
        }

        public IAstStatement GetData(ScriptThread thread)
        {
            var a = (object[])base.DoEvaluate(thread);
            return new AstMixinDefinitionData(Span,
                (string)a[0],
                (AstObjectSettingsCollection)a[3]
            );
        }
    }
}