namespace iSukces.Code.AutoCode
{
    public struct EqualsExpressionData
    {
        public EqualsExpressionData(CsExpression code, int cost = 999)
        {
            Code = code;
            Cost = cost;
        }

        public EqualsExpressionData(CsExpression code)
        {
            Code = code;
            Cost = 0;
        }



        public EqualsExpressionData WithCost(int cost)
        {
            return new EqualsExpressionData(Code, cost);
        }

        public CsExpression Code { get; }
        public int          Cost { get; }
    }
}
