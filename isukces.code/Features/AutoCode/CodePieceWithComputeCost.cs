namespace isukces.code.AutoCode
{
    public struct CodePieceWithComputeCost
    {
        public CodePieceWithComputeCost(string code, int cost = 999, bool brackets = false)
        {
            Code     = code;
            Cost     = cost;
            Brackets = brackets;
        }

        
        public string GetCode()
        {
            return Brackets ? $"({Code})" : Code;
        }

        public string Code     { get; }
        public int    Cost     { get; }
        public bool   Brackets { get; }
    }
}