using iSukces.Code.AutoCode;

namespace iSukces.Code.FeatureImplementers
{
    public interface IExpressionWithObjectInstance
    {
        CsExpression ExpressionTemplate { get; }
        string       Instance           { get; }
    }

    public struct CompareToExpressionData : IExpressionWithObjectInstance
    {
        public CompareToExpressionData(string fieldName, CsExpression expressionTemplate, string instance = null)
        {
            FieldName          = fieldName;
            ExpressionTemplate = expressionTemplate;
            Instance           = instance;
        }

        public string       FieldName          { get; }
        public CsExpression ExpressionTemplate { get; }
        public string       Instance           { get; }
    }

    public struct ExpressionWithObjectInstance : IExpressionWithObjectInstance
    {
        public ExpressionWithObjectInstance(CsExpression expressionTemplate, string instance = null)
        {
            ExpressionTemplate = expressionTemplate;
            Instance           = instance;
        }

        public CsExpression ExpressionTemplate { get; }
        public string       Instance           { get; }
    }
}