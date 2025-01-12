#nullable disable
using System;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Bitbrains.AmmyParser
{
    public partial class AstIdentifierOrBuiltin
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            var a     = nodes.Get<IdentifierNode>();
            if (a != null)
                Identifier = a.Item1.Symbol;
            else
                throw new NotSupportedException();
            AsString = Term.Name + ":" + Identifier;
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            return Identifier;
        }

        public string Identifier { get; set; }
    }
}
