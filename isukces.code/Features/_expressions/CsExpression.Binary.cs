using iSukces.Code.Interfaces;
using JetBrains.Annotations;

namespace iSukces.Code.AutoCode;

public partial class CsExpression
{
    public class Cast : CsExpression
    {
        public Cast(CsType type, CsExpression castedExpression)
            : base(CsOperatorPrecendence.UnaryTypecast)
        {
            type.ThrowIfVoid();
            Type             = type;
            CastedExpression = castedExpression;
        }

        public CsType Type { get; }

        public CsExpression CastedExpression { get; }

        public override string Code
        {
            get
            {
                var code = CastedExpression.GetCode(Precedence, ExpressionAppend.After);
                return $"({Type.Declaration}){code}";
            }
        }
    }

    public class Binary : CsExpression
    {
        public Binary(CsExpression left, CsExpression right,
            CsOperatorPrecendence precedence,
            string operatorText)
            : base(precedence)
        {
            Left         = left;
            Right        = right;
            OperatorText = operatorText;
            _reduced     = Reduce(left, right, precedence, operatorText);
        }

        private static CsExpression Reduce(CsExpression left, CsExpression right, CsOperatorPrecendence precedence,
            string operatorText)
        {
            var code1 = left.GetCode(precedence, ExpressionAppend.Before);
            var code2 = right.GetCode(precedence, ExpressionAppend.After);
            switch (operatorText)
            {
                case "+" when left.Code == "0": return right;
                case "+" when right.Code == "0": return left;
                case "+":
                {
                    if (right.Precedence == CsOperatorPrecendence.Additive)
                        code2 = right.Code;
                    break;
                }
                case "-" when right.Code == "0": return left;

                case "*" when left.Code == "1": return right;
                case "*" when right.Code == "1": return left;

                case "*":
                {
                    if (right.Precedence == CsOperatorPrecendence.Multiplicative)
                        code2 = right.Code;
                    break;
                }

                case "/" when right.Code == "1": return left;
            }

            return new CsExpression(code1 + " " + operatorText + " " + code2, precedence);
        }


        public override string Code => _reduced.Code;

        public CsExpression Left         { get; }
        public CsExpression Right        { get; }
        public string       OperatorText { get; }

        public override CsOperatorPrecendence Precedence => _reduced.Precedence;

        private readonly CsExpression _reduced;
    }
}