namespace isukces.code.AutoCode
{
    public struct EqualsExpressionData
    {
        public EqualsExpressionData(string code, int cost = 999, bool brackets = false)
        {
            Code     = code;
            Cost     = cost;
            Brackets = brackets;
        }

        public EqualsExpressionData(string code, bool brackets)
        {
            Code     = code;
            Cost     = 0;
            Brackets = brackets;
        }


        public string GetCode()
        {
            return Brackets ? $"({Code})" : Code;
        }

        public EqualsExpressionData WithCost(int cost)
        {
            return new EqualsExpressionData(Code, cost, Brackets);
        }

        public string Code     { get; }
        public int    Cost     { get; }
        public bool   Brackets { get; }
    }
}