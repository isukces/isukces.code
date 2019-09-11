using isukces.code.Ammy;
using isukces.code.AutoCode;
using isukces.code.Compatibility.System.Windows;
using isukces.code.Compatibility.System.Windows.Data;
using isukces.code.Compatibility.Telerik;
using isukces.code.interfaces.Ammy;

namespace isukces.code.Ui.DataGrid
{
    /// <summary>
    ///     Generates code for Telerik WPF components.
    ///     It uses fake classes exposing the same properties like real Telerik components.
    /// </summary>
    public class CompatTelerikGridAmmyMixinsGenerator : GridAmmyMixinsGenerator
    {
        public CompatTelerikGridAmmyMixinsGenerator(IAssemblyBaseDirectoryProvider directoryProvider) :
            base(directoryProvider)
        {
        }

        private static AmmyObjectBuilder<T> AddCommon<T>(AmmyObjectBuilder<T> obj, ColumnInfo col)
            where T : GridViewBoundColumnBase
        {
            obj = obj
                .WithPropertyNotNull(a => a.DataMemberBinding, ToBind(col.DataMemberBinding))
                .WithProperty(a => a.Header, col.Header);
            if (col.IsReadOnly)
                obj = obj.WithPropertyGeneric(a => a.IsReadOnly, true);
            if (col.Width.HasValue)
                obj = obj.WithProperty(a => a.Width, col.Width);

            if (!string.IsNullOrEmpty(col.CategoryName))
                obj = obj.WithPropertyGeneric(a => a.ColumnGroupName, col.CategoryName);
            if (col.AlignRight)
                obj = obj.WithPropertyGeneric(a => a.TextAlignment, TextAlignment.Right);
            return obj;
        }

        private AmmyObjectBuilder<T> AddTemplates<T>(AmmyObjectBuilder<T> obj, ColumnInfo col)
            where T : GridViewColumn
        {
            if (col.CellTemplate != null)
            {
                if (col.CellTemplate is AmmyStaticResource sr)
                {
                    obj = obj.WithProperty(a => a.CellTemplate, sr);
                }
                else
                {
                    var cellTemplate = new AmmyObjectBuilder<DataTemplate>()
                        .WithContent(col.CellTemplate);
                    obj = obj.WithProperty(a => a.CellTemplate, cellTemplate);
                }
            }

            if (col.EditTemplate != null)
            {
                var cellTemplate = new AmmyObjectBuilder<DataTemplate>()
                    .WithContent(col.EditTemplate);
                obj = obj.WithProperty(a => a.CellEditTemplate, cellTemplate);
                return obj;
            }

            var lookup = GetLookupInfo(col);
            if (lookup != null)
            {
                if (lookup.IsEmpty)
                    return obj;
                var checkBox = new AmmyObjectBuilder<RadComboBox>()
                    .WithProperty(a => a.Margin, AmmyExpression.FromString("5,0,0,0"))
                    .WithProperty(a => a.VerticalAlignment, VerticalAlignment.Center)
                    .WithProperty(a => a.ItemsSource, lookup.Source)
                    .WithPropertyNotNull(a => a.SelectedValuePath, lookup.SelectedValuePath)
                    .WithPropertyNotNull(a => a.DisplayMemberPath, lookup.DisplayMemberPath)
                    .WithProperty(a => a.SelectedValue, new AmmyBind(col.Name, XBindingMode.TwoWay));
                var cellEditTemplate = new AmmyObjectBuilder<DataTemplate>()
                    .WithContent(checkBox);
                obj = obj.WithProperty(a => a.CellEditTemplate, cellEditTemplate);
                return obj;
            }

            if (col.Type == typeof(bool)) return obj;

            return obj;
        }

        private IAmmyCodePieceConvertible ConvertColumn(ColumnInfo col)
        {
            var lookup = GetLookupInfo(col);
            if (lookup != null)
            {
                var obj = AddCommon(new AmmyObjectBuilder<GridViewComboBoxColumn>(), col);
                if (lookup.Source is AmmyBind)
                    obj = obj.WithProperty(a => a.ItemsSourceBinding, lookup.Source);
                else
                    obj = obj.WithProperty(a => a.ItemsSource, lookup.Source);

                obj = obj
                    .WithPropertyNotNull(a => a.DisplayMemberPath, lookup.DisplayMemberPath)
                    .WithPropertyNotNull(a => a.SelectedValueMemberPath, lookup.SelectedValuePath);
                return obj;
            }

            if (col.Type == typeof(bool))
                return AddCommon(new AmmyObjectBuilder<GridViewCheckBoxColumn>(), col);
            {
                var obj = new AmmyObjectBuilder<GridViewDataColumn>();
                obj = AddCommon(obj, col);
                obj = AddTemplates(obj, col);
                return obj;
            }
        }
    }
}