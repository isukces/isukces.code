#if AMMY
using iSukces.Code.Ammy;
using Xunit;

namespace iSukces.Code.Tests.Ui
{
    public partial class CompatTelerikGridAmmyMixinsGeneratorTests
    {
        [Fact]
        public void T01_Should_create()
        {
            AmmyHelper.ConvertToIAmmyCodePiece += (a, b) =>
            {
                if (b.SourceValue is SampleStaticTextSource s)
                {
                    var value = new SimpleAmmyCodePiece(s.Value.CsEncode());
                    b.Handle(value);
                }
                else if (b.SourceValue is SampleTranslatedTextSource s2)
                {
                    var value = new SimpleAmmyCodePiece("SomeFakeClass.SomeFakeProperty");
                    b.Handle(value);
                }
            };
            var g   = new SampleGridViewAmmyMixinsGenerator(new FakeAssemblyBaseDirectoryProvider());
            var ctx = new TestContext();

            var type = typeof(GridDefinition1);
            g.AssemblyStart(type.Assembly, ctx);
            g.Generate(type, ctx);

            var code = g.GetFullCode();
            var exp = @"using Telerik.Windows.Controls

mixin GridDefinition1() for RadGridView
{
    Resources: ResourceDictionary
    {
        combine MergedDictionaries: [ ResourceDictionary {Source: ""pack://application:,,,/MyApplication;component/resources/gridviewstyles.g.xaml""} ]
    }
    IsLocalizationLanguageRespected: false
    MyGridProperties
    combine Columns: [
        GridViewDataColumn { DataMemberBinding: bind ""Number"", Header: ""Number"", IsReadOnly: true, Width: 160, TextAlignment: Right }
        GridViewDataColumn { DataMemberBinding: bind ""Name"", Header: ""*Name"", IsReadOnly: true, Width: 160 }
        GridViewDataColumn { DataMemberBinding: bind ""Date"", Header: SomeFakeClass.SomeFakeProperty, IsReadOnly: true, Width: 120 }
    ]
}

";
            Assert.Equal(exp.Trim(), code.Trim());

            const string exp2 = @"// ReSharper disable All
namespace iSukces.Code.Tests.Ui
{
    partial class CompatTelerikGridAmmyMixinsGeneratorTests
    {
        partial class GridDefinition1
        {
            private static string GetHeader(string name)
            {
                switch(name)
                {
                    case ""Date"": return Translations.GetCurrentTranslation(""translations.common.date"");
                    case ""Name"": return ""*Name"";
                }
                return name;
            }

        }

    }
}
";
            Assert.Equal(exp2.Trim(), ctx.Code.Trim());
        }


        [Fact]
        public void T02_Should_create_bool_with_data_template()
        {
            AmmyHelper.ConvertToIAmmyCodePiece += (a, b) =>
            {
                if (b.SourceValue is SampleStaticTextSource s)
                {
                    var value = new SimpleAmmyCodePiece(s.Value.CsEncode());
                    b.Handle(value);
                }
                else if (b.SourceValue is SampleTranslatedTextSource s2)
                {
                    var value = new SimpleAmmyCodePiece("SomeFakeClass.SomeFakeProperty");
                    b.Handle(value);
                }
            };
            var g   = new SampleGridViewAmmyMixinsGenerator(new FakeAssemblyBaseDirectoryProvider());
            var ctx = new TestContext();

            var type = typeof(GridDefinition2);
            g.AssemblyStart(type.Assembly, ctx);
            g.Generate(type, ctx);

            var code = g.GetFullCode();
            var exp = @"
using Telerik.Windows.Controls

mixin GridDefinition2() for RadGridView
{
    Resources: ResourceDictionary
    {
        combine MergedDictionaries: [ ResourceDictionary {Source: ""pack://application:,,,/MyApplication;component/resources/gridviewstyles.g.xaml""} ]
    }
    IsLocalizationLanguageRespected: false
    MyGridProperties
    combine Columns: [
        GridViewDataColumn { DataMemberBinding: bind ""Flag"", Header: ""Flag1"", Width: 160
            CellTemplate: System.Windows.DataTemplate {
                iSukces.Code.Tests.Ammy.CheckBox {
                    IsChecked: bind ""Flag"" set [Mode: TwoWay, UpdateSourceTrigger: PropertyChanged]
                }
            }
            CellEditTemplate: System.Windows.DataTemplate {
                iSukces.Code.Tests.Ammy.CheckBox {
                    IsChecked: bind ""Flag"" set [Mode: TwoWay, UpdateSourceTrigger: PropertyChanged]
                }
            }
            EditTriggers: CellClick
        }
    ]
}


";
            Assert.Equal(exp.Trim(), code.Trim());

            const string exp2 = @"// ReSharper disable All
namespace iSukces.Code.Tests.Ui
{
    partial class CompatTelerikGridAmmyMixinsGeneratorTests
    {
        partial class GridDefinition2
        {
            private static string GetHeader(string name)
            {
                switch(name)
                {
                    case ""Flag"": return ""Flag1"";
                }
                return name;
            }

        }

    }
}
";
            Assert.Equal(exp2.Trim(), ctx.Code.Trim());
        }

        [Fact]
        public void T03_Should_create_bool_with_nested_values()
        {
            AmmyHelper.ConvertToIAmmyCodePiece += (a, b) =>
            {
                if (b.SourceValue is SampleStaticTextSource s)
                {
                    var value = new SimpleAmmyCodePiece(s.Value.CsEncode());
                    b.Handle(value);
                }
                else if (b.SourceValue is SampleTranslatedTextSource s2)
                {
                    var value = new SimpleAmmyCodePiece("SomeFakeClass.SomeFakeProperty");
                    b.Handle(value);
                }
            };
            var g   = new SampleGridViewAmmyMixinsGenerator(new FakeAssemblyBaseDirectoryProvider());
            var ctx = new TestContext();

            var type = typeof(GridDefinition3);
            g.AssemblyStart(type.Assembly, ctx);
            g.Generate(type, ctx);

            var code = g.GetFullCode();
            var exp = @"
using Telerik.Windows.Controls

mixin GridDefinition3() for RadGridView
{
    Resources: ResourceDictionary
    {
        combine MergedDictionaries: [ ResourceDictionary {Source: ""pack://application:,,,/MyApplication;component/resources/gridviewstyles.g.xaml""} ]
    }
    IsLocalizationLanguageRespected: false
    MyGridProperties
    combine Columns: [
        GridViewDataColumn { DataMemberBinding: bind ""Obj.Name"", Header: ""Name"", Width: 160 }
        GridViewDataColumn { DataMemberBinding: bind ""Obj.Number"", Header: ""Number"", Width: 130 }
        GridViewDataColumn { DataMemberBinding: bind, Header: ""Whole"", Width: 120 }
    ]
}
";

            Assert.Equal(exp.Trim(), code.Trim());

            const string exp2 = @"
// ReSharper disable All
namespace iSukces.Code.Tests.Ui
{
    partial class CompatTelerikGridAmmyMixinsGeneratorTests
    {
        partial class GridDefinition3
        {
            private static string GetHeader(string name)
            {
                switch(name)
                {
                    case """": return ""Whole"";
                }
                return name;
            }

        }

    }
}
";
            Assert.Equal(exp2.Trim(), ctx.Code.Trim());
        }
    }
}
#endif