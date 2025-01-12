#nullable disable
using Irony.Ast;
using Irony.Interpreter;
using Irony.Parsing;

namespace Bitbrains.AmmyParser
{
    public partial class AstQualNameSegmentsOpt2
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            var baseResult                                         = base.DoEvaluate(thread);
            if (baseResult is object[] xx)
            {
                var result                                             = new string[xx.Length];
                for (var i = result.Length - 1; i >= 0; i--) result[i] = (string)xx[i];
                return new FullQualifiedNameData(Span, result);
            }
            return (FullQualifiedNameData)baseResult;
        }
    }
}
