using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Bitbrains.AmmyParser
{
    public class EnumValueNode : AstNode
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            AsString = Text = treeNode.ChildNodes.Single().Token.Text;
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            return Text;
        }

        public string Text { get; private set; }
    }
}