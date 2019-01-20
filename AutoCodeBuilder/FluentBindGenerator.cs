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
        static FluentBindGenerator()
        {
            void Add<T>(string name, bool addStatic = false, bool isNormalProperty = false)
            {
                BindingParams.Add(new BindingParamInfo(typeof(T), name, addStatic, isNormalProperty));
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

            Add<object>("From", true, true);
            Add<string>("Path", false, true);
        }

        public static bool IgnoreWithConverterStatic(Type type)
        {
            return type == typeof(AmmyBind) || type == typeof(AmmyBindBuilder);
        }


        private static Type MakeNullable(Type type)
        {
            return type.IsValueType
                ? typeof(Nullable<>).MakeGenericType(type)
                : type;
        }

        private static string SetPropertyAndReturnThis(string propertyName, string value)
        {
            return $"{propertyName} = {value}; return this;";
        }


        public void Generate(Type type, IAutoCodeGeneratorContext context)
        {
            if (type == typeof(AmmyBind))
            {
                _currentClass = context.GetOrCreateClass(type);
                foreach (var i in BindingParams)
                    AddFluentMethod(
                        (paramName, argName) =>
                        {
                            if (i.IsNormalProperty)
                                return SetPropertyAndReturnThis(paramName, argName);
                            return $"return WithSetParameter({paramName.CsEncode()}, {argName});";
                        },
                        i.Type, i.Name, i.AddStatic, !i.IsNormalProperty);
            }
            else if (type == typeof(AmmyBindBuilder))
            {
                BuildAmmyBindBuilder(context);
            }
        }


        private void AddFluentMethod(Func<string, string, string> creator, Type type, string paramName, bool addStatic,
            bool addObjectOverload)
        {
            var flu = new FluentMethodInfo(paramName);

            {
                var argName = paramName.FirstLower();
                // var body       = $"return WithSetParameter({paramName.CsEncode()}, {argName});";
                var body = CreateCode(nameof(FluentBindGenerator), "G1")
                    .WriteLine(creator(paramName, argName));
                var m = _currentClass.AddMethod(flu.FluentPrefix, _currentClass.Name)
                    .WithBody(body);
                m.AddParam(argName, _currentClass.TypeName(type));
                m.WithAttribute(_currentClass, typeof(AutocodeGeneratedAttribute));
                if (addObjectOverload && type != typeof(object))
                    AddFluentMethod(creator, typeof(object), paramName, false, false);
            }
            if (!addStatic) return;
            {
                const string argName = "propertyName";
                var          value   = $"new StaticBindingSource(typeof(TStaticPropertyOwner), {argName})";
                var code = CreateCode(nameof(FluentBindGenerator), "G2")
                    .WriteLine(creator(paramName, value));
                _currentClass.AddMethod(flu.Static+"<TStaticPropertyOwner>", _currentClass.Name)
                    .WithBody(code)
                    .WithAutocodeGeneratedAttribute(_currentClass)
                    .AddParam<string>(argName, _currentClass);
            }
            {
                const string argName = "resourceName";
                var          value   = $"new AmmyStaticResource({argName})";
                var code = CreateCode(nameof(FluentBindGenerator), "G3")
                    .WriteLine(creator(paramName, value));
                _currentClass.AddMethod(flu.StaticResource, _currentClass.Name)
                    .WithBody(code)
                    .WithAutocodeGeneratedAttribute(_currentClass)
                    .AddParam<string>(argName, _currentClass);
            }
            if (flu.AllowAncestor)
            {
                {
                    const string value = "new AncestorBindingSource(ancestorType, level)";
                    var code = CreateCode(nameof(FluentBindGenerator), "G4")
                        .WriteLine(creator(paramName, value));
                    var m = _currentClass.AddMethod(flu.Ancestor, _currentClass.Name)
                        .WithBody(code)
                        .WithAutocodeGeneratedAttribute(_currentClass);
                    m.AddParam<Type>("ancestorType", _currentClass);
                    m.AddParam<int?>("level", _currentClass).WithConstValueNull();
                }
                {
                    var          methodName = flu.Ancestor + "<TAncestor>";
                    const string value      = "new AncestorBindingSource(typeof(TAncestor),  level)";
                    var code = CreateCode(nameof(FluentBindGenerator), "G5")
                        .WriteLine(creator(paramName, value));
                    var m = _currentClass.AddMethod(methodName, _currentClass.Name)
                        .WithBody(code)
                        .WithAutocodeGeneratedAttribute(_currentClass);
                    m.AddParam<int?>("level", _currentClass).WithConstValueNull();
                }
            }
        }

        private void BuildAmmyBindBuilder(IAutoCodeGeneratorContext context)
        {
            _currentClass = context.GetOrCreateClass(typeof(AmmyBindBuilder));
            var setupCode = CreateCode(nameof(FluentBindGenerator), "G11");
            foreach (var i in BindingParams)
            {
                var    flu        = new FluentMethodInfo(i.Name);
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
                        $"bind.{flu.FluentPrefix}({i.Name});");
                }

                var p = _currentClass.AddProperty(i.Name, type2)
                    .WithNoEmitField()
                    .WithMakeAutoImplementIfPossible()
                    .WithIsPropertyReadOnly(isReadOnly);
                p.ConstValue = init;

                if (!isReadOnly)
                    AddFluentMethod(
                        SetPropertyAndReturnThis,
                        type2, i.Name, i.AddStatic, false);
            }

            _currentClass.AddMethod("SetupAmmyBind", "void")
                .WithVisibility(Visibilities.Private)
                .WithBody(setupCode)
                .WithAutocodeGeneratedAttribute(_currentClass
                )
                .AddParam<AmmyBind>("bind", _currentClass);
        }

        private static readonly List<BindingParamInfo> BindingParams = new List<BindingParamInfo>();

        private CsClass _currentClass;
        private const string ValidationRules = "ValidationRules";

        private struct BindingParamInfo
        {
            public BindingParamInfo(Type type, string name, bool addStatic, bool isNormalProperty)
            {
                Type             = type;
                Name             = name;
                AddStatic        = addStatic;
                IsNormalProperty = isNormalProperty;
            }

            public Type   Type             { get; }
            public string Name             { get; }
            public bool   AddStatic        { get; }
            public bool   IsNormalProperty { get; }
        }
    }
}