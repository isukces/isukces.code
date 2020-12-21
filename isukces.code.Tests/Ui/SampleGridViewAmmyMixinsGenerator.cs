using System;
using System.Linq;
using iSukces.Code.Ammy;
using iSukces.Code.AutoCode;
using iSukces.Code.Compatibility.Telerik;
using iSukces.Code.Interfaces;
using iSukces.Code.Interfaces.Ammy;
using iSukces.Code.Tests.Ammy;
using iSukces.Code.Ui.DataGrid;

namespace iSukces.Code.Tests.Ui
{
    public sealed class SampleGridViewAmmyMixinsGenerator : CompatTelerikGridAmmyMixinsGenerator,
        IAssemblyAutoCodeGenerator
    {
        public SampleGridViewAmmyMixinsGenerator(IAssemblyBaseDirectoryProvider directoryProvider)
            : base(directoryProvider)
        {
        }

        public string GetFullCode() => Mixins.FullCode;

        protected override void AfterConvertColumn(AmmyContainerBase builder, ColumnInfo col)
        {
            base.AfterConvertColumn(builder, col);
            if (builder is AmmyObjectBuilder<GridViewComboBoxColumn> b)
                b.WithPropertyStaticResource("EditorStyle", "GridComboBoxStyle");
        }
        
        protected override IAmmyCodePieceConvertible ConvertColumn(ColumnInfo col)
        {
            var code = base.ConvertColumn(col);
            if (col.Type != typeof(bool))
                return code;
            var aob = (AmmyObjectBuilder)code;
            var t   = aob.Map<GridViewDataColumn>();
            t.WithPropertyGeneric(q => q.EditTriggers, GridViewEditTriggers.CellClick);
            return t;
        }

        private static UiTestTextSourceInfo GetText(object textSource)
        {
            if (textSource is null)
                return UiTestTextSourceInfo.Null;
            if (textSource is string s)
                return UiTestTextSourceInfo.FromString(s);
            
            
            switch (textSource)
            {
                case SampleStaticTextSource staticTextSource:
                    return UiTestTextSourceInfo.FromString(staticTextSource.Value);
                case SampleTranslatedTextSource translated:
                {
                    var key = translated.TranslationKey.CsEncode();
                    var ti  = new UiTestTextSourceInfo(null, "Translations.GetCurrentTranslation(" + key + ")");
                    return ti;
                }
            }
            throw new Exception("Unable to get source from " + textSource.GetType());
        }

        protected override void AfterCreateModel(Type type, Model model)
        {
            base.AfterCreateModel(type, model);
            var writer = new CsCodeWriter();
            var c = model.Columns.Where(a =>
            {
                var header = GetText(a.HeaderSource);
                return !header.HasConstantValue || a.Name != header.ConstantValue;
            }).OrderBy(a => a.Name).ToArray();
            if (c.Any())
            {
                writer.Open("switch(name)");
                foreach (var a in c)
                {
                    var header = GetText(a.HeaderSource);
                    writer.WriteLine($"case {a.Name.CsEncode()}: return {header.CsExpression};");
                }

                writer.Close();
            }

            writer.WriteLine("return name;");

            var cl = Context.GetOrCreateClass(type);
            cl.AddMethod("GetHeader", "string")
                .WithStatic()
                .WithVisibility(Visibilities.Private)
                .WithParameter(new CsMethodParameter("name", "string"))
                .WithBody(writer);
        }


        protected override void AfterStartMixin(string name, IConversionCtx ctx, Model model)
        {
            const string src = "pack://application:,,,/MyApplication;component/resources/gridviewstyles.g.xaml";
            Mixins.Open("Resources: ResourceDictionary");
            Mixins.WriteLine("combine MergedDictionaries: [ ResourceDictionary {Source: " + src.CsEncode() + "} ]");
            Mixins.Close();
            const string respectedName = nameof(RadGridView.IsLocalizationLanguageRespected);
            Mixins.WriteLine(respectedName + ": false");
            Mixins.WriteLine("#MyGridProperties");
        }

        protected override IConversionCtx CreateConversionContext()
        {
            var ctx = new ConversionCtx(Mixins);
            ctx.OnResolveSeparateLines += AmmyPretty.VeryPretty;
            return ctx;
        }

        protected override LookupInfo GetLookupSource(Type t)
        {
            try
            {
                return base.GetLookupSource(t);
            }
            catch
            {
                return null;
            }
        }

        protected override void WriteAmmyMixin(string name, Model model)
        {
            Mixins.AddNamespace<GridViewColumnGroup>();
            Mixins.AddNamespace<GridViewToggleRowDetailsColumn>();
            base.WriteAmmyMixin(name, model);
        }
    }
}