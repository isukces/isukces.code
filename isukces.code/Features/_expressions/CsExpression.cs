using System;
using System.Linq;
using iSukces.Code.Interfaces;
using JetBrains.Annotations;

namespace iSukces.Code.AutoCode
{
    public partial class CsExpression
    {
        public CsExpression([NotNull] string code, CsOperatorPrecendence precedence = CsOperatorPrecendence.Expression)
        {
            if (string.IsNullOrEmpty(code))
                throw new ArgumentException(nameof(code));
            Code       = code ?? throw new ArgumentNullException(nameof(code));
            Precedence = precedence;
        }

        protected CsExpression(CsOperatorPrecendence precedence)
        {
            Precedence = precedence;
        }

        public static string AddBrackets(string code, CsOperatorPrecendence codePrecedence,
            CsOperatorPrecendence outerExpression, ExpressionAppend append)
        {
            var add = CsOperatorPrecendenceUtils.AddBrackets(codePrecedence, outerExpression, append);
            return add ? "(" + code + ")" : code;
        }


        public static CsExpression operator +(CsExpression l, CsExpression r) =>
            new Binary(l, r, CsOperatorPrecendence.Additive, "+");


        public static CsExpression operator +(int l, CsExpression r)
        {
            if (l == 0)
                return r;
            return new Binary(l, r, CsOperatorPrecendence.Additive, "+");
        }

        public static CsExpression operator +(CsExpression l, int r)
        {
            if (r == 0)
                return l;
            return new Binary(l, r, CsOperatorPrecendence.Additive, "+");
        }

        public static CsExpression operator /(CsExpression a, CsExpression b) =>
            new Binary(a, b, CsOperatorPrecendence.Multiplicative, "/");


        public static CsExpression operator ^(CsExpression a, CsExpression b)
        {
            const CsOperatorPrecendence op    = CsOperatorPrecendence.BitwiseExOr;
            var                         code1 = a.GetCode(op, ExpressionAppend.Before);
            var                         code2 = b.GetCode(op, ExpressionAppend.After);
            return new CsExpression(code1 + " ^ " + code2, op);
        }

        public static explicit operator CsExpression(string code) => new CsExpression(code);

        public static implicit operator CsExpression(int code) => new CsExpression(code.ToString());

        public static implicit operator CsExpression(bool code) => new CsExpression(code ? "true" : "false");

        public static implicit operator CsExpression(double code) => new CsExpression(code.ToCsString() + "d");

        public static CsExpression operator *(CsExpression a, CsExpression b) =>
            new Binary(a, b, CsOperatorPrecendence.Multiplicative, "*");

        public static CsExpression operator *(CsExpression a, int b)
        {
            if (b == 1)
                return a;
            return new Binary(a, b, CsOperatorPrecendence.Multiplicative, "*");
        }

        public static CsExpression operator *(int a, CsExpression b)
        {
            if (a == 1)
                return b;
            return new Binary(a, b, CsOperatorPrecendence.Multiplicative, "*");
        }

        public static CsExpression operator -(CsExpression a, CsExpression b) =>
            new Binary(a, b, CsOperatorPrecendence.Additive, "-");

        public static CsExpression operator -(CsExpression a, int b)
        {
            if (b == 0)
                return a;
            return new Binary(a, b, CsOperatorPrecendence.Additive, "-");
        }

        public static CsExpression TypeCast(string type, CsExpression expression)
        {
            const CsOperatorPrecendence resultPrecedence = CsOperatorPrecendence.UnaryTypecast;
            var                         code             = expression.GetCode(resultPrecedence, ExpressionAppend.After);
            return new CsExpression("(" + type + ")" + code, resultPrecedence);
        }

        public CsExpression CallMethod(string methodName, params CsExpression[] args)
        {
            var code = GetCode(CsOperatorPrecendence.Expression, ExpressionAppend.Before);
            code += "." + methodName + "(" + string.Join(", ", args.Select(a => a.Code)) + ")";
            return new CsExpression(code);
        }

        public CsExpression CallProperty(string propertyName)
        {
            var code = GetCode(CsOperatorPrecendence.Expression, ExpressionAppend.Before);
            return new CsExpression(code + "." + propertyName);
        }

        public CsExpression Coalesce([NotNull] CsExpression expr)
        {
            if (expr == null) throw new ArgumentNullException(nameof(expr));
            const CsOperatorPrecendence op    = CsOperatorPrecendenceUtils.DoubleQuestion;
            var                         code1 = GetCode(op, ExpressionAppend.Before);
            var                         code2 = expr.GetCode(op, ExpressionAppend.After);
            return new CsExpression(code1 + " ?? " + code2, op);
        }

        public CsExpression Conditional(CsExpression trueEx, CsExpression falseEx)
        {
            const CsOperatorPrecendence op                  = CsOperatorPrecendence.ConditionalExpression;
            var                         conditionCode       = GetCode(op, ExpressionAppend.Before);
            var                         trueExpressionCode  = trueEx.GetCode(op, ExpressionAppend.After);
            var                         falseExpressionCode = falseEx.GetCode(op, ExpressionAppend.After);
            return new CsExpression($"{conditionCode} ? {trueExpressionCode} : {falseExpressionCode}", op);
        }


        public CsExpression Format(params object[] args)
        {
            var code = string.Format(Code, args);
            return new CsExpression(code, Precedence);
        }


        public string GetCode(CsOperatorPrecendence outerPrecedence, ExpressionAppend append) =>
            AddBrackets(Code, Precedence, outerPrecedence, append);

        public CsExpression Is(string isWhat)
        {
            // todo I don't know if it's relational but seems to be
            // (a < b) is bool requires ()
            // a >> b is T not requires ()
            const CsOperatorPrecendence resultPrecedence = CsOperatorPrecendence.Relational;
            var                         code             = GetCode(resultPrecedence, ExpressionAppend.After);
            return new CsExpression(code + " is " + isWhat, resultPrecedence);
        }

        public CsExpression OptionalNull()
        {
            // todo I don't know if it's Expression but seems to be
            // I've checked Unary and it's wrong
            const CsOperatorPrecendence resultPrecedence = CsOperatorPrecendence.Expression;
            var                         code             = GetCode(resultPrecedence, ExpressionAppend.Before);
            return new CsExpression(code + "?", resultPrecedence);
        }


        public override string ToString() => Code;


        public virtual CsOperatorPrecendence Precedence { get; }


        public virtual string Code { get; }
    }
}