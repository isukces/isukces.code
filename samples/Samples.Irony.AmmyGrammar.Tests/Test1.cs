using Samples.Irony.AmmyGrammar.Data;
using Xunit;

namespace Samples.Irony.AmmyGrammar.GrammarTest
{
    public class Test1 : GrammarTestsBase<AmmyGrammar, AmmyProgram>
    {
        public static void Doall()
        {
            var i = new Test1();
            i.T01_1_should_parse_using();
        }

        [Fact]
        public void T01_1_should_parse_using()
        {
            const string ammyCode = @"using Telerik.Windows.Controls
ObjectName ""SomeObject.SomeNamespace"" { }
";
            var language = GetLanguageData();
            var ast      = ParseAst(language, ammyCode);
            var o        = ParseTree(language, ammyCode);
            var json     = Serialize(o);
            var jsonexp = @"{
  'Usings': {
    'Items': [
      {
        'NamespaceName': {
          'Items': [
            'Telerik',
            'Windows',
            'Controls'
          ],
          'Span': '0,6,24'
        },
        'Span': '0,0,32'
      }
    ],
    'Span': '0,0,32'
  },
  'ObjectDefinition': {
    'BaseObjectType': {
      'Items': [
        'ObjectName'
      ],
      'Span': '1,0,10'
    },
    'FullTypeName': 'SomeObject.SomeNamespace',
    'Span': '1,0,41'
  },
  'Span': '0,0,75'
}";
            Assert.Equal(jsonexp, json);
        }


        [Fact]
        public void T02_a_should_parse_object()
        {
            const string ammyCode = @"using Telerik.Windows.Controls
using Windows.Bla
ObjectName ""SomeObject.SomeNamespace"" {}";
            ParseAst(GetLanguageData(), ammyCode);
        }


        [Fact]
        public void T02_b_should_parse_object()
        {
            const string ammyCode = @"using Telerik.Windows.Controls
using Windows.Bla
ObjectName ""SomeObject.SomeNamespace"" { 
}

";
            ParseAst(GetLanguageData(), ammyCode);
        }


        [Fact]
        public void T02_c_should_parse_object()
        {
            const string ammyCode = @"using Telerik.Windows.Controls
using Windows.Bla
ObjectName ""SomeObject.SomeNamespace"" { a: 3}
";
            ParseAst(GetLanguageData(), ammyCode);
        }

        [Fact]
        public void T02_d_should_parse_object()
        {
            const string ammyCode = @"using Telerik.Windows.Controls
using Windows.Bla
ObjectName ""SomeObject.SomeNamespace"" { 
a: 3}
";
            ParseAst(GetLanguageData(), ammyCode);
        }

        [Fact]
        public void T02_e_should_parse_object()
        {
            const string ammyCode = @"using Telerik.Windows.Controls
using Windows.Bla
ObjectName ""SomeObject.SomeNamespace"" { a: 3
}
";
            ParseAst(GetLanguageData(), ammyCode);
        }

        [Fact]
        public void T02_f_should_parse_object()
        {
            const string ammyCode = @"using Telerik.Windows.Controls
using Windows.Bla
ObjectName ""SomeObject.SomeNamespace"" { 
  a: 3
}
";
            ParseAst(GetLanguageData(), ammyCode);
        }


        [Fact]
        public void T02_x_should_parse_object()
        {
            const string ammyCode = @"using Telerik.Windows.Controls
using Windows.Bla

ObjectName ""SomeObject.SomeNamespace"" {
Name: ""Piotr"" , NumberInt: -1, NumberFloat1: 2.2, NumberFloat2: -3.2e-1 
Octal: 012
Hex: 0xff
}
";
            var ast = ParseAst(GetLanguageData(), ammyCode);
        }
    }
}