using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using isukces.code.Ammy;
using isukces.code.AutoCode;
using isukces.code.IO;

namespace isukces.code.Ui.DataGrid
{
    public abstract partial class GridAmmyMixinsGenerator : IAutoCodeGenerator, IAssemblyAutoCodeGenerator
    {
        static GridAmmyMixinsGenerator()
        {
            RightAligned = new[]
            {
                typeof(int),
                typeof(int?),
                typeof(long),
                typeof(long?),
                typeof(short),
                typeof(short?),
                typeof(double),
                typeof(double?),
                typeof(decimal),
                typeof(decimal?)
            }.ToHashSet();
        }

        public GridAmmyMixinsGenerator(IAssemblyBaseDirectoryProvider directoryProvider) =>
            DirectoryProvider = directoryProvider;


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

        public void AssemblyEnd(Assembly assembly, IAutoCodeGeneratorContext context)
        {
            var dir      = DirectoryProvider.GetBaseDirectory(assembly);
            var filename = Path.Combine(dir.FullName, "+grid.Auto.ammy");
            var code     = Mixins.FullCode;
            var saved    = CodeFileUtils.SaveIfDifferent(code, filename, false);
            if (saved)
                context.FileSaved(new FileInfo(filename));
            Context = null;
        }

        public void AssemblyStart(Assembly assembly, IAutoCodeGeneratorContext context)
        {
            Context = context;
            Mixins  = new AmmyCodeWriter();
        }

        public void Generate(Type type, IAutoCodeGeneratorContext context)
        {
            var model = Model.MakeFromAttributes(type);
            if (model == null)
                return;
            AfterCreateModel(type, model);
            WriteAmmyMixin(type.Name, model);
        }

        protected virtual void AfterCreateModel(Type type, Model model)
        {
        }

        protected virtual LookupInfo GetLookupInfo(ColumnInfo col)
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

        protected abstract void WriteAmmyMixin(string name, Model model);

        public    IAutoCodeGeneratorContext      Context           { get; private set; }
        protected IAssemblyBaseDirectoryProvider DirectoryProvider { get; }
        protected AmmyCodeWriter                 Mixins            { get; private set; }

        private static readonly HashSet<Type> RightAligned;
    }
}