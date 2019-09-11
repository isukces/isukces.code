using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using isukces.code.Ammy;
using isukces.code.AutoCode;
using isukces.code.IO;

namespace isukces.code.Ui.DataGrid
{
    public partial class GridAmmyMixinsGenerator : IAutoCodeGenerator, IAssemblyAutoCodeGenerator
    {
        static GridAmmyMixinsGenerator()
        {
            RightAligned = new[]
            {
                typeof(int),
                typeof(int?),
                typeof(double),
                typeof(double?),
                typeof(decimal),
                typeof(decimal?)
            }.ToHashSet();
        }

        public GridAmmyMixinsGenerator(IAssemblyBaseDirectoryProvider directoryProvider) =>
            _directoryProvider = directoryProvider;

#if TELERIK
        private static AmmyObjectBuilder<T> AddCommon<T>(AmmyObjectBuilder<T> obj, ColumnInfo col)
            where T : GridViewBoundColumnBase
        {
            obj = obj
                .WithPropertyNotNull(a => a.DataMemberBinding, ToBind(col.DataMemberBinding))
                .WithProperty<>(a => a.Header, col.Header);
            if (col.IsReadOnly)
                obj = obj.WithPropertyGeneric(a => a.IsReadOnly, true);
            if (col.Width.HasValue)
                obj = obj.WithProperty<>(a => a.Width, col.Width);

            if (!string.IsNullOrEmpty(col.CategoryName))
                obj = obj.WithPropertyGeneric(a => a.ColumnGroupName, col.CategoryName);
            if (col.AlignRight)
                obj = obj.WithPropertyGeneric(a => a.TextAlignment, TextAlignment.Right);
            return obj;
        }
#endif

        private static PropertyInfo[] GetProperties(Type attModelType)
        {
            return attModelType
#if COREFX
                .GetTypeInfo()
#endif
                .GetProperties(BindingFlags.Instance | BindingFlags.Public);
        }

        private static Type GetRowType(Type t)
        {
            if (t == typeof(DataGridConfigurationProvider<>))
                return null;
            while (t != null)
            {
#if COREFX
                var ti = t.GetTypeInfo();
#else
                var ti = t;
#endif

                if (ti.IsGenericType)
                {
                    var tt = t.GetGenericTypeDefinition();
                    if (tt == typeof(DataGridConfigurationProvider<>))
                        return ti.GetGenericArguments()[0];
                }

                t = ti.BaseType;
            }

            return null;
        }


        private static AmmyBind ToBind(string x)
        {
            var dataMemberBinding = string.IsNullOrEmpty(x)
                ? null
                : new AmmyBind(x);
            return dataMemberBinding;
        }

        public void AssemblyEnd(Assembly assembly, IAutoCodeGeneratorContext context)
        {
            var dir      = _directoryProvider.GetBaseDirectory(assembly);
            var filename = Path.Combine(dir.FullName, "+grid.Auto.ammy");
            var code     = _mixins.FullCode;
            var saved    = CodeFileUtils.SaveIfDifferent(code, filename, false);
            if (saved)
                context.FileSaved(new FileInfo(filename));
        }

        public void AssemblyStart(Assembly assembly, IAutoCodeGeneratorContext context)
        {
            _mixins = new AmmyCodeWriter();
        }

        public void Generate(Type type, IAutoCodeGeneratorContext context)
        {
            var model = Model.MakeFromAttributes(type);
            if (model == null)
                return;
            WriteAmmyMixin(type.Name, model);
        }
#if TELERIK
        private AmmyObjectBuilder<T> AddTemplates<T>(AmmyObjectBuilder<T> obj, ColumnInfo col)
            where T : GridViewColumn
        {
            if (col.CellTemplate != null)
            {
                if (col.CellTemplate is AmmyStaticResource sr)
                {
                    obj = obj.WithProperty<>(a => a.CellTemplate, sr);
                }
                else
                {
                    var cellTemplate = new AmmyObjectBuilder<DataTemplate>()
                        .WithContent(col.CellTemplate);
                    obj = obj.WithProperty<>(a => a.CellTemplate, cellTemplate);
                }
            }

            if (col.EditTemplate != null)
            {
                var cellTemplate = new AmmyObjectBuilder<DataTemplate>()
                    .WithContent(col.EditTemplate);
                obj = obj.WithProperty<>(a => a.CellEditTemplate, cellTemplate);
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
                    .WithPropertyNotNull(a => a.DisplayMemberPath, lookup.DisplayMemberPath).WithProperty<>(a => a.SelectedValue, new AmmyBind(col.Name, DataBindingMode.TwoWay));
                var cellEditTemplate = new AmmyObjectBuilder<DataTemplate>()
                    .WithContent(checkBox);
                obj = obj.WithProperty<>(a => a.CellEditTemplate, cellEditTemplate);
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
#endif
        private LookupInfo GetLookupInfo(ColumnInfo col)
        {
            return col.Lookup ?? (col.Type
#if COREFX
                       .GetTypeInfo()
#endif
                       .IsEnum
                       ? GetLookupSource(col.Type)
                       : null);
        }

        protected virtual LookupInfo GetLookupSource(Type t)
        {
#if COREFX
            var ti = t.GetTypeInfo();
#else
            var ti = t;
#endif

            if (ti.IsEnum)
            {
                var at = ti.GetCustomAttribute<LookupInfoAttribute>();
                if (at != null)
                {
                    var instance = (IEnumLookupProvider)Activator.CreateInstance(at.LookupProvider);
                    var sp       = instance.GetSourceStaticProperty();
                    return new LookupInfo
                    {
                        Source            = new StaticBindingSource(sp.Item1, sp.Item2),
                        DisplayMemberPath = instance.GetDisplayMemberPath(),
                        SelectedValuePath = instance.GetSelectedValuePath()
                    };
                }
            }

            throw new NotSupportedException(t.ToString());
        }

        protected virtual void WriteAmmyMixin(string name, Model model)
        {
#if TELERIK
            _mixins.AddNamespace<GridViewColumnGroup>();
            _mixins.AddNamespace<GridViewToggleRowDetailsColumn>();
            _mixins.AddNamespace<Enums>();

            var ctx = new ConversionCtx(_mixins);
            ctx.OnResolveSeparateLines += AmmyPretty.VeryPretty;
            _mixins.Open($"mixin {name}() for {ctx.TypeName<RadGridView>()}");

            if (model.Categories.Any())
            {
                _mixins.OpenArray("combine ColumnGroups:");
                foreach (var col in model.Categories)
                {
                    var q = new AmmyObjectBuilder<GridViewColumnGroup>().WithProperty<>(a => a.Name, col.Name)
                        .WithPropertyGeneric(a => a.Header, col.Header);
                    q.WriteLineTo(_mixins, ctx);
                }

                _mixins.CloseArray();
            }

            if (model.Columns.Any())
            {
                _mixins.OpenArray("combine Columns:");
                if (model.AddExpandColumn)
                {
                    var obj =
 new AmmyObjectBuilder<GridViewToggleRowDetailsColumn>().WithProperty<>(a => a.ExpandMode, GridViewToggleRowDetailsColumnExpandMode.Single);
                    obj.WriteLineTo(_mixins, ctx);
                }

                foreach (var col in model.Columns)
                {
                    var expression = ConvertColumn(col);
                    expression.WriteLineTo(_mixins, ctx);
                }

                _mixins.CloseArray();
            }

            _mixins.CloseNl();
#endif
        }


        private static readonly HashSet<Type> RightAligned;
        private readonly IAssemblyBaseDirectoryProvider _directoryProvider;
        private AmmyCodeWriter _mixins;
    }
}