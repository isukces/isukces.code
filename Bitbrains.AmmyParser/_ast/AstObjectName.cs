using Irony.Ast;
using Irony.Interpreter;
using Irony.Parsing;

namespace Bitbrains.AmmyParser
{
    public partial class AstObjectName
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            return base.DoEvaluate(thread);
        }
    }
}