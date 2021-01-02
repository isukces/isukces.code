using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Samples.Irony.AmmyGrammar.Ast
{
    public class BaseAstNode : AstNode
    {
        protected static object GetValue(ScriptThread thread, AstNode childNode)
        {
            if (childNode is NullNode)
                return null;
            if (childNode is IdentifierNode iNode)
                return iNode.Symbol;
            var res = childNode.Evaluate(thread);
            return res;
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            foreach (var childNode in treeNode.ChildNodes)
                AddChild(NodeUseType.Parameter, "expr", childNode);
            AsString = "Expression list";
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            var map      = GetMap();
            var objArray = new object[map?.Length ?? ChildNodes.Count];
            if (map is null)
            {
                for (var index = 0; index < objArray.Length; index++)
                {
                    var childNode = ChildNodes[index];
                    objArray[index] = GetValue(thread, childNode);
                }
            }
            else
            {
                for (var outIdx = 0; outIdx < map.Length; outIdx++)
                {
                    var index     = map[outIdx];
                    var childNode = ChildNodes[index];
                    objArray[outIdx] = GetValue(thread, childNode);
                }

                if (map.Length == 1)
                    return objArray[0];
            }

            return objArray;
        }

        protected virtual int[] GetMap() => null;
    }
}