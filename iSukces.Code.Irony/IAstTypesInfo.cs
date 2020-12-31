using System;
using iSukces.Code.AutoCode;
using iSukces.Code.Interfaces;

namespace iSukces.Code.Irony
{
    public interface IAstTypesInfo
    {
        string AstType { get; }

        string DataType { get; }
        string NodeType { get; }

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
        public GetEvaluateExpressionInput(CsExpression variable, CsExpression scriptThread)
        {
            Variable     = variable;
            ScriptThread = scriptThread;
        }

        public CsExpression Variable     { get; }
        public CsExpression ScriptThread { get; }
    }

    public delegate IAstTypesInfo AstTypesInfoDelegate(ITypeNameResolver resolver);
}