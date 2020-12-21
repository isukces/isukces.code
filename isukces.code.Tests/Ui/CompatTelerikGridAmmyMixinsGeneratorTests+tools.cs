using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using iSukces.Code.Ammy;
using iSukces.Code.AutoCode;
using iSukces.Code.Compatibility.System.Windows.Data;
using iSukces.Code.Tests.Ammy;
using iSukces.Code.Ui.DataGrid;

namespace iSukces.Code.Tests.Ui
{
    public partial class CompatTelerikGridAmmyMixinsGeneratorTests
    {
        public class TModel
        {
            public int    Number { get; set; }
            public string Name   { get; set; }

            public DateTime Date { get; set; }

            public bool Flag { get; set; }
        }


        private class FakeAssemblyBaseDirectoryProvider : IAssemblyBaseDirectoryProvider
        {
            public DirectoryInfo GetBaseDirectory(Assembly assembly) => throw new NotImplementedException();
        }

        private sealed class GridDefinition1 : DataGridConfigurationProvider<TModel>
        {
            public override IEnumerable<GridColumn> GetColumns()
            {
                yield return Col(a => a.Number, "Number", 160).WithReadOnly();
                yield return Col(a => a.Name, new SampleStaticTextSource("*Name"), 160).WithReadOnly();
                yield return Col(a => a.Date, new SampleTranslatedTextSource("translations.common.date"), 120)
                    .WithReadOnly();
            }

            public override bool AddExpandColumn
            {
                get { return false; }
            }
        }

        private sealed class GridDefinition2 : DataGridConfigurationProvider<TModel>
        {
            public override IEnumerable<GridColumn> GetColumns()
            {
                var bind         = new AmmyBind(nameof(TModel.Flag), XBindingMode.TwoWay)
                .WithUpdateSourceTrigger(XUpdateSourceTrigger.PropertyChanged);
                var cellTemplate = new AmmyObjectBuilder<CheckBox>();
                    cellTemplate.WithProperty(a=>a.IsChecked,bind);

                    yield return Col(a => a.Flag, "Flag1", 160)
                        .WithCellTemplate(cellTemplate)
                        .WithEditTemplate(cellTemplate);
            }

            public override bool AddExpandColumn
            {
                get { return false; }
            }
        }
    }
}