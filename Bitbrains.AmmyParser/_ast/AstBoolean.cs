#nullable disable
using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Parsing;

namespace Bitbrains.AmmyParser
{
    public partial class AstBoolean
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var text = treeNode.ChildNodes.Single().Token.Text;
            Value = text == "true";
            AsString = text;
        }

        public bool Value { get; set; }

        protected override object DoEvaluate(ScriptThread thread)
        {
            return Value;
        }
    }
}
