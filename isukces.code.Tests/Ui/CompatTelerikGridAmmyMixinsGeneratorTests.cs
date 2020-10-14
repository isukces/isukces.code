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
            var g = new SampleGridViewAmmyMixinsGenerator(new FakeAssemblyBaseDirectoryProvider());
            var ctx = new TestContext();

            var type = typeof(ImpulsAlarmMeasureItemGridDefinition);
            g.AssemblyStart(type.Assembly, ctx);
            g.Generate(type, ctx);

            var code = g.GetFullCode();
            var exp = @"using Telerik.Windows.Controls

mixin ImpulsAlarmMeasureItemGridDefinition() for RadGridView
{
    Resources: ResourceDictionary
    {
        combine MergedDictionaries: [ ResourceDictionary {Source: ""pack://application:,,,/MyApplication;component/resources/gridviewstyles.g.xaml""} ]
    }
    IsLocalizationLanguageRespected: false
    #MyGridProperties
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
        partial class ImpulsAlarmMeasureItemGridDefinition
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
    }
}