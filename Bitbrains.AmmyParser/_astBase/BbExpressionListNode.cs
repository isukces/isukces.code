using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Bitbrains.AmmyParser
{
    public class BbExpressionListNode : AstNode
    {
        private static object GetValue(ScriptThread thread, AstNode childNode)
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
            var map = GetMap();
            if (map is null)
            {
                var objArray = new object[ChildNodes.Count];
                for (var index = 0; index < objArray.Length; index++)
                {
                    var childNode = ChildNodes[index];
                    objArray[index] = GetValue(thread, childNode);
                }

                return objArray;
            }

            if (map.Length == 1)
            {
                var index     = map[0];
                var childNode = ChildNodes[index];
                var value     = GetValue(thread, childNode);
                return value;
            }

            {
                var objArray = new object[map.Length];
                for (var outIdx = 0; outIdx < map.Length; outIdx++)
                {
                    var index     = map[outIdx];
                    var childNode = ChildNodes[index];
                    objArray[outIdx] = GetValue(thread, childNode);
                }

                return objArray;
            }
        }

        protected virtual int[] GetMap() => null;
    }
}