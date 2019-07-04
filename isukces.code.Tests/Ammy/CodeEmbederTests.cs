using System;
using System.Collections.Generic;
using isukces.code.Ammy;
using Xunit;
using Xunit.Abstractions;

namespace isukces.code.Tests.Ammy
{
    public class CodeEmbederTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public CodeEmbederTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        public static IEnumerable<object[]> DataFor_T01_Should_find_marker()
        {
            var result = new List<object[]>();

            void Add(string d)
            {
                result.Add(new object[] {d});
            }

            void AddSerie(string d)
            {
                Add(d);
                var befores = new[] {"\r\n", "\n", "line1\nline2\r\n"};
                var afters  = new[] {"\r\n", "\n", "\r\nline1\nline2\r\n"};
                foreach (var before in befores)
                foreach (var after in afters)
                    Add(before + d + after);
            }

            IEnumerable<string> Texts()
            {
                yield return "Bla";
                yield return " Bla";
                yield return " Bla \t";
            }

            foreach (var text in Texts())
            {
                var core = CodeEmbeder.Limiter1 + text + CodeEmbeder.Limiter2;
                AddSerie("// " + core);
                AddSerie("//   " + core);
                AddSerie("//  \t " + core);
                AddSerie("//  \t " + core + " other");
            }

            return result;
        }

        [Theory]
        [MemberData(nameof(DataFor_T01_Should_find_marker))]
        public void T01_Should_find_marker(string text)
        {
            var a = CodeEmbeder.Limiter.Match(text);
            Assert.True(a.Success);
            Assert.Equal("Bla", a.Groups[1].Value.TrimEnd('=').Trim());
        }

        [Fact]
        public void T02_Should_embed_into_empty()
        {
            var result = CodeEmbeder.Embed(null, null);
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void T03_Should_embed_into_empty()
        {
            //  #############################
            var exp1 = @"// -----===== autocode begin =====-----
Some code 1
Some code 2
// -----===== autocode end =====-----
";

            var result = CodeEmbeder.Embed(null, "Some code 1\r\nSome code 2");
            WriteExpected("exp1", result);
            Assert.Equal(exp1, result);
            
            //  #############################
            var exp2 = @"// -----===== autocode begin =====-----
Some code 3
Some code 4
// -----===== autocode end =====-----
";
            
            result = CodeEmbeder.Embed(result, "Some code 3\r\nSome code 4");
            WriteExpected("exp2", result);
            Assert.Equal(exp2, result);
            //  #############################
            var exp3 = string.Empty;
            result = CodeEmbeder.Embed(result, null);
            WriteExpected("exp3", result);
            Assert.Equal(exp3, result);

        }
        
        [Fact]
        public void T04_Should_embed_at_beginning()
        {
            const string sourceText = @"Line 1
Line2

Line3

";
            //  #############################
            var exp1 = @"// -----===== autocode begin =====-----
Some code 1
Some code 2
// -----===== autocode end =====-----
Line 1
Line2

Line3

";

            var result = CodeEmbeder.Embed(sourceText, "Some code 1\r\nSome code 2");
            WriteExpected("exp1", result);
            Assert.Equal(exp1, result);
            
            //  #############################
            var exp2 = @"// -----===== autocode begin =====-----
Some code 3
Some code 4
// -----===== autocode end =====-----
Line 1
Line2

Line3

";
            
            result = CodeEmbeder.Embed(result, "Some code 3\r\nSome code 4");
            WriteExpected("exp2", result);
            Assert.Equal(exp2, result);
            
            //  #############################
            var exp3 = @"Line 1
Line2

Line3

";
            
            result = CodeEmbeder.Embed(result, null);
            WriteExpected("exp3", result);
            Assert.Equal(exp3, result);

        }

        void WriteExpected(string name, string code)
        {
            _testOutputHelper.WriteLine("var " + name + " = " + code.CsVerbatimEncode() + ";");
        }
        
        [Fact]
        public void T05_Should_embed_in_the_middle()
        {
            const string sourceText = @"Line 1
Line2

Line3

//    -----========= autocode begin =====----- other text
// -----===== autocode   END   =========-------- other text
Line 4



";
            var result = CodeEmbeder.Embed(sourceText, "Some code 1\r\nSome code 2");
            var exp1 = @"Line 1
Line2

Line3
// -----===== autocode begin =====-----
Some code 1
Some code 2
// -----===== autocode end =====-----
Line 4



";

            WriteExpected("exp1", result);
            Assert.Equal(exp1, result);
            
            result = CodeEmbeder.Embed(result, "Some code 3\r\nSome code 4");
            var exp2 = @"Line 1
Line2

Line3
// -----===== autocode begin =====-----
Some code 3
Some code 4
// -----===== autocode end =====-----
Line 4



";

            WriteExpected("exp2", result);
            Assert.Equal(exp2, result);
            
            
            result = CodeEmbeder.Embed(result, null);
            var exp3 = @"Line 1
Line2

Line3
// -----===== autocode begin =====-----
// -----===== autocode end =====-----
Line 4



";

            WriteExpected("exp3", result);
            Assert.Equal(exp3, result);

        }
    }
}