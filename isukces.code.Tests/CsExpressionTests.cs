using iSukces.Code.AutoCode;
using iSukces.Code.Interfaces;
using Xunit;

namespace iSukces.Code.Tests
{
    public class CsExpressionTests
    {
        [Theory]
        [InlineData(CsOperatorPrecendence.Additive, CsOperatorPrecendence.Multiplicative, ExpressionAppend.After)]
        [InlineData(CsOperatorPrecendence.Additive, CsOperatorPrecendence.Multiplicative, ExpressionAppend.Before)]
        [InlineData(CsOperatorPrecendence.Additive, CsOperatorPrecendence.Additive, ExpressionAppend.After)]
        public void T01_Should_add_brackets(CsOperatorPrecendence codeP, CsOperatorPrecendence outerP,
            ExpressionAppend append)
        {
            var got = CsOperatorPrecendenceUtils.AddBrackets(codeP, outerP, append);
            Assert.True(got);
        }

        [Theory]
        [InlineData(CsOperatorPrecendence.Additive, CsOperatorPrecendence.Additive, ExpressionAppend.Before)]
        public void T02_Should_not_add_brackets(CsOperatorPrecendence codeP, CsOperatorPrecendence outerP,
            ExpressionAppend append)
        {
            var got = CsOperatorPrecendenceUtils.AddBrackets(codeP, outerP, append);
            Assert.False(got);
        }


        [Fact]
        public void T03_Should_create_conditional()
        {
            var          condition = (CsExpression)"x";
            CsExpression t         = 0;
            CsExpression f         = 1;

            void Check(string code)
            {
                var got = condition.Conditional(t, f);
                Assert.Equal(CsOperatorPrecendence.ConditionalExpression, got.Precedence);
                Assert.Equal(code, got.Code);
            }

            Check("x ? 0 : 1");
            // ===============
            condition =  new CsExpression("k=a + b > 3", CsOperatorPrecendence.SimpleAndCompoundAssignment);
            t         += 1;
            f         *= 12;
            Check("(k=a + b > 3) ? 1 : 12");
            condition = new CsExpression("a + b > 3", CsOperatorPrecendence.Relational);
            Check("a + b > 3 ? 1 : 12");
        }


        [Theory]
        [InlineData("1", CsOperatorPrecendence.Expression, false)]
        [InlineData("kuku", CsOperatorPrecendence.Expression, false)]
        [InlineData("1+2", CsOperatorPrecendence.Additive, true)]
        [InlineData("1*2", CsOperatorPrecendence.Multiplicative, true)]
        public void T04_Should_call_property(string code, CsOperatorPrecendence codePrecedence, bool addBrackets)
        {
            var got      = new CsExpression(code, codePrecedence).CallProperty("Length");
            var expected = (addBrackets ? "(" + code + ")" : code) + ".Length";
            Assert.Equal(expected, got.Code);
            Assert.Equal(CsOperatorPrecendence.Expression, got.Precedence);
        }


        [Theory]
        [InlineData("1", CsOperatorPrecendence.Expression, false)]
        [InlineData("kuku", CsOperatorPrecendence.Expression, false)]
        [InlineData("1+2", CsOperatorPrecendence.Additive, true)]
        [InlineData("(long)x", CsOperatorPrecendence.UnaryTypecast, false)]
        public void T05_Should_create_typecast(string code, CsOperatorPrecendence codePrecedence, bool addBrackets)
        {
            var got      = CsExpression.TypeCast("int", new CsExpression(code, codePrecedence));
            var expected = addBrackets ? "(" + code + ")" : code;
            expected = "(int)" + expected;
            Assert.Equal(expected, got.Code);
            Assert.Equal(CsOperatorPrecendence.UnaryTypecast, got.Precedence);
        }

        [Theory]
        [InlineData("1", CsOperatorPrecendence.Expression, false)]
        [InlineData("kuku", CsOperatorPrecendence.Expression, false)]
        [InlineData("1+2", CsOperatorPrecendence.Additive, true)]
        [InlineData("(long)x", CsOperatorPrecendence.UnaryTypecast, true)]
        public void T06_Should_create_OptionalNull(string code, CsOperatorPrecendence codePrecedence, bool addBrackets)
        {
            var got      = new CsExpression(code, codePrecedence).OptionalNull();
            var expected = addBrackets ? "(" + code + ")" : code;
            expected = expected + "?";
            Assert.Equal(expected, got.Code);
            // Assert.Equal(CsOperatorPrecendence.UnaryTypecast, got.Precedence);
            // var q = (int)(long)3.3;
            // object x = 23; var q = ((CsExpressionTests)x)?.T07_should_add()
        }


        [Theory]
        [InlineData("1", CsOperatorPrecendence.Expression, "1", CsOperatorPrecendence.Expression, "1 + 1")]
        [InlineData("1 - 2", CsOperatorPrecendence.Additive, "3 - 4", CsOperatorPrecendence.Additive, "1 - 2 + 3 - 4")]
        [InlineData("1", CsOperatorPrecendence.Expression, "0", CsOperatorPrecendence.Expression, "1")]
        [InlineData("0", CsOperatorPrecendence.Expression, "3", CsOperatorPrecendence.Expression, "3")]
        public void T07_should_add(string code1, CsOperatorPrecendence codePrecedence1, string code2,
            CsOperatorPrecendence codePrecedence2, string expected)
        {
            var a   = new CsExpression(code1, codePrecedence1);
            var b   = new CsExpression(code2, codePrecedence2);
            var got = a + b;
            Assert.Equal(expected, got.Code);
            if (got.Code!= a.Code && got.Code!= b.Code)
                Assert.Equal(CsOperatorPrecendence.Additive, got.Precedence);
        }

        [Theory]
        [InlineData("1", CsOperatorPrecendence.Expression, "1", CsOperatorPrecendence.Expression, "1 - 1")]
        [InlineData("1 - 2", CsOperatorPrecendence.Additive, "3 - 4", CsOperatorPrecendence.Additive,
            "1 - 2 - (3 - 4)")]
        [InlineData("1", CsOperatorPrecendence.Expression, "0", CsOperatorPrecendence.Expression, "1")]
        public void T08_should_sub(string code1, CsOperatorPrecendence codePrecedence1, string code2,
            CsOperatorPrecendence codePrecedence2, string expected)
        {
            var a   = new CsExpression(code1, codePrecedence1);
            var b   = new CsExpression(code2, codePrecedence2);
            var got = a - b;
            Assert.Equal(expected, got.Code);
            if (got.Code!= a.Code && got.Code!= b.Code)
                Assert.Equal(CsOperatorPrecendence.Additive, got.Precedence);
        }

        [Theory]
        [InlineData("2", CsOperatorPrecendence.Expression, "1", CsOperatorPrecendence.Expression, "2")]
        [InlineData("1", CsOperatorPrecendence.Expression, "3", CsOperatorPrecendence.Expression, "3")]
        [InlineData("2", CsOperatorPrecendence.Expression, "3", CsOperatorPrecendence.Expression, "2 * 3")]
        [InlineData("1 - 2", CsOperatorPrecendence.Additive, "3 - 4", CsOperatorPrecendence.Additive,
            "(1 - 2) * (3 - 4)")]
        [InlineData("2 * 2", CsOperatorPrecendence.Multiplicative, "3 * 4", CsOperatorPrecendence.Multiplicative,
            "2 * 2 * 3 * 4")]
        [InlineData("2 / 2", CsOperatorPrecendence.Multiplicative, "3 / 4", CsOperatorPrecendence.Multiplicative,
            "2 / 2 * 3 / 4")]
        public void T09_should_multiply(string code1, CsOperatorPrecendence codePrecedence1, string code2,
            CsOperatorPrecendence codePrecedence2, string expected)
        {
            var a   = new CsExpression(code1, codePrecedence1);
            var b   = new CsExpression(code2, codePrecedence2);
            var got = a * b;
            Assert.Equal(expected, got.Code);
            if (got.Code!= a.Code && got.Code!= b.Code)
                Assert.Equal(CsOperatorPrecendence.Multiplicative, got.Precedence);
        }

        [Theory]
        [InlineData("2", CsOperatorPrecendence.Expression, "1", CsOperatorPrecendence.Expression, "2")]
        [InlineData("1", CsOperatorPrecendence.Expression, "2", CsOperatorPrecendence.Expression, "1 / 2")]
        [InlineData("1 - 2", CsOperatorPrecendence.Additive, "3 - 4", CsOperatorPrecendence.Additive,
            "(1 - 2) / (3 - 4)")]
        [InlineData("2 * 2", CsOperatorPrecendence.Multiplicative, "3 * 4", CsOperatorPrecendence.Multiplicative,
            "2 * 2 / (3 * 4)")]
        [InlineData("2 / 2", CsOperatorPrecendence.Multiplicative, "3 / 4", CsOperatorPrecendence.Multiplicative,
            "2 / 2 / (3 / 4)")]
        public void T10_should_div(string code1, CsOperatorPrecendence codePrecedence1, string code2,
            CsOperatorPrecendence codePrecedence2, string expected)
        {
            var a   = new CsExpression(code1, codePrecedence1);
            var b   = new CsExpression(code2, codePrecedence2);
            var got = a / b;
            Assert.Equal(expected, got.Code);
            if (got.Code!= a.Code && got.Code!= b.Code)
                Assert.Equal(CsOperatorPrecendence.Multiplicative, got.Precedence);
        }
    }
}