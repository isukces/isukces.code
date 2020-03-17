using System;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Bitbrains.AmmyParser
{
    public class AstOptNode : AstNode
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var childNodes = treeNode.ChildNodes;
            if (childNodes.Count > 1)
                throw new Exception("Should have at most one child");
            if (childNodes.Count > 0)
            {
                var childNode = childNodes[0];
                AddChild(NodeUseType.Parameter, "wrapped", childNode);
                AsString = "Optional " + childNode;
            }
            else
            {
                AsString = "Empty optional";
            }
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            if (ChildNodes.Count == 0)
                return null;
            thread.CurrentNode = this;
            var result = ChildNodes[0].Evaluate(thread);
            thread.CurrentNode = Parent;
            return result;
        }
    }
}