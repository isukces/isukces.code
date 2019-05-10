namespace isukces.code.AutoCode
{
    public interface IExpressionWithObjectInstance
    {
        string ExpressionTemplate { get; }
        string Instance           { get; }
    }

    public struct CompareToExpressionData : IExpressionWithObjectInstance
    {
        public CompareToExpressionData(string fieldName, string expressionTemplate, string instance = null)
        {
            FieldName          = fieldName;
            ExpressionTemplate = expressionTemplate;
            Instance           = instance;
        }

        public string FieldName          { get; }
        public string ExpressionTemplate { get; }
        public string Instance           { get; }
    }

    public struct ExpressionWithObjectInstance : IExpressionWithObjectInstance
    {
        public ExpressionWithObjectInstance(string expressionTemplate, string instance = null)
        {
            ExpressionTemplate = expressionTemplate;
            Instance           = instance;
        }

        public string ExpressionTemplate { get; }
        public string Instance           { get; }
    }
}