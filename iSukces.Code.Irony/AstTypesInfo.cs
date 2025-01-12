using System;
using Irony.Interpreter.Ast;
using iSukces.Code.AutoCode;
using iSukces.Code.Interfaces;

#nullable disable
namespace iSukces.Code.Irony;

public sealed class AstTypesInfo : IAstTypesInfo
{
    public AstTypesInfo()
    {
    }

    public AstTypesInfo(CsType astType, CsType dataType, CsType nodeType)
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
            var nodeType = kind.GetNodeType();
            result.NodeType = resolver.GetTypeName(nodeType);

            // var items = GetItems().Select(a => a.Symbol).ToArray();
            result.GetEvaluateExpression = q =>
            {
                var cast = q.Variable;
                if (q.CastAst)
                    cast = CsExpression.TypeCast(result.NodeType, cast);
                var property = "Value";
                if (nodeType == typeof(IdentifierNode))
                    property = nameof(IdentifierNode.Symbol);
                else if (nodeType == typeof(LiteralValueNode))
                    property = nameof(LiteralValueNode.Value) + "?.ToString()";

                cast = cast.CallProperty(property);
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
                AstType = sh(nti.AstClass.Provider.GetTypeName(resolver, targetNamespace.AstNamespace))
            };
            result.NodeType = result.AstType;

            if (nti.DataClass.CreateAutoCode)
            {
                result.DataType = sh(nti.DataClass.Provider?.GetTypeName(resolver, targetNamespace.DataNamespace));

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

            CsType sh(FullTypeName x)
            {
                var xName = x?.Name ?? default;
                if (xName.IsVoid)
                    return default;
                var p = NamespaceAndName.Parse(xName.Declaration);
                if (string.IsNullOrEmpty(p.Namespace))
                    return xName;
                if (resolver is INamespaceContainer c)
                {
                    return c.GetTypeName(p.Namespace, p.Name);
                }

                return xName;
            }
        };
    }

    public CsType AstType  { get; set; }
    public CsType DataType { get; set; }
    public CsType NodeType { get; set; }

    public Func<GetEvaluateExpressionInput, GetEvaluateExpressionOutput> GetEvaluateExpression { get; set; }
}

