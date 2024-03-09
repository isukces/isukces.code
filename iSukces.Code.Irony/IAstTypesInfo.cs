using System;
using iSukces.Code.AutoCode;
using iSukces.Code.Interfaces;

namespace iSukces.Code.Irony
{
    public interface IAstTypesInfo
    {
        CsType AstType { get; }

        CsType DataType { get; }
        CsType NodeType { get; }

        Func<GetEvaluateExpressionInput, GetEvaluateExpressionOutput> GetEvaluateExpression { get; }
    }


    public struct GetEvaluateExpressionOutput
    {
        public GetEvaluateExpressionOutput(CsExpression expression, bool needScriptThread)
        {
            Expression       = expression;
            NeedScriptThread = needScriptThread;
        }

        public CsExpression Expression       { get; }
        public bool         NeedScriptThread { get; }
    }

    public struct GetEvaluateExpressionInput
    {
        public GetEvaluateExpressionInput(CsExpression variable, CsExpression scriptThread, bool castAst)
        {
            Variable     = variable;
            ScriptThread = scriptThread;
            CastAst      = castAst;
        }

        public CsExpression Variable     { get; }
        public CsExpression ScriptThread { get; }
        public bool         CastAst      { get; }
    }

    public delegate IAstTypesInfo AstTypesInfoDelegate(ITypeNameResolver resolver);
}