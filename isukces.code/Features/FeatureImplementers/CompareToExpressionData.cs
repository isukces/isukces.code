namespace isukces.code.AutoCode
{
    public struct CompareToExpressionData
    {
        public CompareToExpressionData(string fieldName, string compareExpression, string comparerInstance=null)
        {
            FieldName         = fieldName;
            CompareExpression = compareExpression;
            ComparerInstance = comparerInstance;
        }

        public string FieldName         { get; }
        public string CompareExpression { get; }
        public string ComparerInstance { get; }
    }
}