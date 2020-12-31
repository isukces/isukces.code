using System;
using iSukces.Code.AutoCode;

namespace iSukces.Code.Irony
{
    public sealed class AstTypesInfo : IAstTypesInfo
    {
        public AstTypesInfo()
        {
        }

        public AstTypesInfo(string astType, string dataType, string nodeType)
        {
            AstType  = astType;
            DataType = dataType;
            NodeType = nodeType;
        }


        public static AstTypesInfoDelegate BuildFrom(SpecialTerminalKind kind)
        {
            return resolver =>
            {
                var result = new AstTypesInfo();
                var t      = kind.GetAstClass();
                result.AstType = resolver.GetTypeName(t);

                t               = kind.GetDataClass();
                result.DataType = resolver.GetTypeName(t);

                result.NodeType = resolver.GetTypeName(kind.GetNodeType());

                // var items = GetItems().Select(a => a.Symbol).ToArray();
                result.GetEvaluateExpression = q =>
                {
                    var cast = CsExpression.TypeCast(result.NodeType, q.Variable);
                    cast = cast.CallProperty("Symbol");
                    return new GetEvaluateExpressionOutput(cast, false);
                };

                return result;
            };
        }

        public static AstTypesInfoDelegate BuildFrom(NonTerminalInfo nti, IGrammarNamespaces targetNamespace)
        {
            return resolver =>
            {
                var result = new AstTypesInfo
                {
                    AstType = nti.AstClassTypeName.GetTypeName(resolver, targetNamespace.AstNamespace)?.Name
                };
                result.NodeType = result.AstType;

                if (nti.CreateDataClass)
                {
                    result.DataType = nti.DataClassName?.GetTypeName(resolver, targetNamespace.DataNamespace)?.Name;

                    result.GetEvaluateExpression = expression =>
                    {
                        /*
                         * var childNode = ChildNodes[i];
                var q         = childNode.Evaluate(scriptThread);
                // result[i] = (Samples.Irony.AmmyGrammar.Data.AmmyUsingStatement)childNode;
                         */
                        var cast = expression.Variable.CallMethod("Evaluate", expression.ScriptThread);
                        cast = CsExpression.TypeCast(result.DataType, cast);
                        return new GetEvaluateExpressionOutput(cast, true);
                    };
                }

                return result;
            };
        }

        public string AstType { get; set; }

        public string DataType { get; set; }
        public string NodeType { get; set; }

        public Func<GetEvaluateExpressionInput, GetEvaluateExpressionOutput> GetEvaluateExpression { get; set; }
    }
}