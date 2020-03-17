using System;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Parsing;

namespace Bitbrains.AmmyParser
{
    public partial class AstQualNameSegment
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            if (nodes.Count == 2)
            {
                var a = nodes[0].Token.Text;
                var b = nodes[1].Token.Text;
                AsString = a + b;
                NamePart = b;
                return;
            }

            throw new NotImplementedException();
        }


        protected override object DoEvaluate(ScriptThread thread)
        {
            return NamePart;
        }

        public string NamePart { get; private set; }
    }
}