using System;
using System.Collections.Generic;
using isukces.code;
using isukces.code.Ammy;
using isukces.code.AutoCode;
using isukces.code.interfaces;

namespace AutoCodeBuilder
{
    internal class FluentBindGenerator : BaseGenerator, IAutoCodeGenerator
    {
        const string ValidationRules = "ValidationRules";
        static FluentBindGenerator()
        {
            void Add<T>(string name, bool addStatic = false)
            {
                BindingParams.Add(new BindingParamInfo(typeof(T), name, addStatic));
            }

            Add<DataBindingMode>("Mode");
            Add<object>(ValidationRules);
            Add<string>("BindingGroupName");
            Add<bool>("BindsDirectlyToSource");

            Add<object>("Converter", true);
            Add<object>("ConverterCulture");
            Add<object>("ConverterParameter");

            Add<bool>("IsAsync");

            Add<bool>("NotifyOnSourceUpdated");
            Add<bool>("NotifyOnTargetUpdated");
            Add<bool>("NotifyOnValidationError");

            Add<string>("StringFormat");
            Add<object>("TargetNullValue");

            Add<DataUpdateSourceTrigger>("UpdateSourceTrigger");

            Add<bool>("ValidatesOnDataErrors");
            Add<bool>("ValidatesOnExceptions");
            Add<string>("XPath");
        }

        public void Generate(Type type, IAutoCodeGeneratorContext context)
        {
            if (type == typeof(AmmyBind))
            {
                _currentClass = context.GetOrCreateClass(type);
                foreach (var i in BindingParams)
                    AddFluentMethod(i.Type, i.Name, i.AddStatic);
            }else if (type == typeof(AmmyBindBuilder))
            {
                BuildAmmyBindBuilder(context);
            }
        }

        private void BuildAmmyBindBuilder(IAutoCodeGeneratorContext context)
        {
            
            _currentClass = context.GetOrCreateClass(typeof(AmmyBindBuilder));
            var setupCode = new CsCodeWriter();
            foreach (var i in BindingParams)
            {
                var    type2      = i.Type;
                string init       = null;
                var    isReadOnly = false;
                if (i.Name == ValidationRules)
                {
                    type2      = typeof(List<object>);
                    init       = "new " + _currentClass.TypeName(type2) + "()";
                    isReadOnly = true;
                }
                else
                {
                    type2 = MakeNullable(type2);
                    setupCode.SingleLineIf(
                        $"{i.Name} != null", 
                        $"bind.With{i.Name}({i.Name});");
                }

                var p = _currentClass.AddProperty(i.Name, type2)
                    .WithNoEmitField()
                    .WithMakeAutoImplementIfPossible()
                    .WithIsPropertyReadOnly(isReadOnly);
                p.ConstValue = init;
            }

            _currentClass.AddMethod("SetupAmmyBind", "void")
                .WithVisibility(Visibilities.Private)
                .WithBody(setupCode)
                .WithAutocodeGeneratedAttribute(_currentClass
                )
                .AddParam<AmmyBind>("bind", _currentClass);
        }

        private static Type MakeNullable(Type type)
        {
            return type.IsValueType 
                ? typeof(Nullable<>).MakeGenericType(type) 
                : type;
        }


        private void AddFluentMethod(Type type, string paramName, bool addStatic)
        {
            {
                var methodName = "With" + paramName;
                var argName    = paramName.FirstLower();
                var body       = $"return WithSetParameter({paramName.CsEncode()}, {argName});";
                var m = _currentClass.AddMethod(methodName, _currentClass.Name)
                    .WithBody(body);
                m.AddParam(argName, _currentClass.TypeName(type));
                m.WithAttribute(_currentClass, typeof(AutocodeGeneratedAttribute));
                if (type == typeof(object))
                    return;
                AddFluentMethod(typeof(object), paramName, false);
            }
            if (addStatic)
            {
                var          methodName = "With" + paramName + "Static<T>";
                const string argName    = "propertyName";
                var          variable   = paramName.FirstLower();
                var          c          = new CsCodeWriter();
                c.WriteLine($"var {variable} = new StaticBindingSource(typeof(T), {argName});");
                c.WriteLine($"return WithSetParameter({paramName.CsEncode()}, {variable});");
                var m = _currentClass.AddMethod(methodName, _currentClass.Name)
                    .WithBody(c)
                    .WithAutocodeGeneratedAttribute(_currentClass);
                m.AddParam(argName, _currentClass.TypeName(typeof(string)));                
            }
        }
        
        private static readonly List<BindingParamInfo> BindingParams = new List<BindingParamInfo>();

        private CsClass _currentClass;

        private struct BindingParamInfo
        {
            public BindingParamInfo(Type type, string name, bool addStatic)
            {
                Type      = type;
                Name      = name;
                AddStatic = addStatic;
            }

            public Type   Type      { get; }
            public string Name      { get; }
            public bool   AddStatic { get; }
        }
    }
}