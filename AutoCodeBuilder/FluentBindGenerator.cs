using System;
using System.Collections.Generic;
using System.Linq;
using isukces.code;
using isukces.code.Ammy;
using isukces.code.AutoCode;
using isukces.code.Compatibility.System.Windows.Data;
using isukces.code.interfaces;

namespace AutoCodeBuilder
{
    internal class FluentBindGenerator : BaseGenerator, IAutoCodeGenerator
    {
        static FluentBindGenerator()
        {
            void Add<T>(string name)
            {
                var a = new BindingParamInfo(typeof(T), name);
                BindingParams.Add(a);
            }

            Add<XBindingMode>("Mode");

            Add<object>(ValidationRules);
            Add<string>("BindingGroupName");
            Add<bool>("BindsDirectlyToSource");

            Add<object>("Converter");
            Add<object>("ConverterCulture");
            Add<object>("ConverterParameter");

            Add<bool>("IsAsync");

            Add<bool>("NotifyOnSourceUpdated");
            Add<bool>("NotifyOnTargetUpdated");
            Add<bool>("NotifyOnValidationError");

            Add<string>("StringFormat");
            Add<object>("TargetNullValue");

            Add<XUpdateSourceTrigger>("UpdateSourceTrigger");

            Add<bool>("ValidatesOnDataErrors");
            Add<bool>("ValidatesOnExceptions");
            Add<string>("XPath");

            Add<object>("From");
            Add<string>("Path");

            var bindableToStaticOrResourceNames = @"XPath

Converter
ConverterParameter
ConverterCulture


From
Source
RelativeSource
ElementName
AsyncState
UpdateSourceExceptionFilter";

            var hs = new HashSet<string>(SplitEnumerable(bindableToStaticOrResourceNames));
            foreach (var i in BindingParams)
                if (hs.Contains(i.Name))
                    i.BindableToStaticOrResource = true;
        }

        public static bool IgnoreWithConverterStatic(Type type) =>
            type == typeof(AmmyBind) || type == typeof(AmmyBindBuilder);


        private static Type MakeNullable(Type type) =>
            type.IsValueType
                ? typeof(Nullable<>).MakeGenericType(type)
                : type;

        private static string SetPropertyAndReturnThis(string propertyName, string value) =>
            $"{propertyName} = {value}; return this;";

        private static IEnumerable<string> SplitEnumerable(string text)
        {
            if (string.IsNullOrEmpty(text))
                return new string[0];
            return text.Split('\r', '\n').Select(a => a.Trim()).Where(a => !string.IsNullOrEmpty(a));
        }


        public void Generate(Type type, IAutoCodeGeneratorContext context)
        {
            if (type == typeof(AmmyBind))
            {
                _currentClass = context.GetOrCreateClass(type);
                foreach (var i in BindingParams)
                {
                    var fl = i.GetFlags(type);
                    AddFluentMethod(
                        (paramName, argName) =>
                        {
                            if (i.HasProperty(type))
                                return SetPropertyAndReturnThis(paramName, argName);
                            return $"return WithSetParameter({paramName.CsEncode()}, {argName});";
                        },
                        i.Type, i.Name, fl);
                }
            }
            else if (type == typeof(AmmyBindBuilder))
            {
                BuildAmmyBindBuilder(context, type);
            }
        }

        private void AddFluentMethod(Func<string, string, string> creator, Type type, string paramName,
            Fl flags)
        {
            void AddStaticAndResource(FluentMethodInfo mi)
            {
                {
                    const string argName = "propertyName";
                    var          value   = $"new StaticBindingSource(typeof(TStaticPropertyOwner), {argName})";
                    var code = CreateCodeWriter()
                        .WriteLine(creator(paramName, value));
                    _currentClass.AddMethod(mi.Static + "<TStaticPropertyOwner>", _currentClass.Name)
                        .WithBody(code)
                        .WithAutocodeGeneratedAttribute(_currentClass, /*code.Info*/"")
                        .AddParam<string>(argName, _currentClass);
                }
                {
                    const string argName = "staticResourceName";
                    var          value   = $"new {nameof(AmmyStaticResource)}({argName})";
                    var code = CreateCodeWriter()
                        .WriteLine(creator(paramName, value));
                    _currentClass.AddMethod(mi.StaticResource, _currentClass.Name)
                        .WithBody(code)
                        .WithAutocodeGeneratedAttribute(_currentClass, ""/*code.Info*/)
                        .AddParam<string>(argName, _currentClass);
                }

                {
                    const string argName = "dynamicResourceName";
                    var          value   = $"new {nameof(AmmyDynamicResource)}({argName})";
                    var code = CreateCodeWriter()
                        .WriteLine(creator(paramName, value));
                    _currentClass.AddMethod(mi.DynamicResource, _currentClass.Name)
                        .WithBody(code)
                        .WithAutocodeGeneratedAttribute(_currentClass, /*code.Info*/"")
                        .AddParam<string>(argName, _currentClass);
                }
            }

            void AddAncestor(FluentMethodInfo mi)
            {
                {
                    const string value = "new AncestorBindingSource(ancestorType, level)";
                    var code = CreateCodeWriter()
                        .WriteLine(creator(paramName, value));
                    var m = _currentClass.AddMethod(mi.Ancestor, _currentClass.Name)
                        .WithBody(code)
                        .WithAutocodeGeneratedAttribute(_currentClass, ""/*code.Info*/);
                    m.AddParam<Type>("ancestorType", _currentClass);
                    m.AddParam<int?>("level", _currentClass).WithConstValueNull();
                }
                {
                    var          methodName = mi.Ancestor + "<TAncestor>";
                    const string value      = "new AncestorBindingSource(typeof(TAncestor),  level)";
                    var code = CreateCodeWriter()
                        .WriteLine(creator(paramName, value));
                    var m = _currentClass.AddMethod(methodName, _currentClass.Name)
                        .WithBody(code)
                        .WithAutocodeGeneratedAttribute(_currentClass, ""/*code.Info*/);
                    m.AddParam<int?>("level", _currentClass).WithConstValueNull();
                }
            }

            void AddDirectValue(Type type1, FluentMethodInfo flu1)
            {
                var argName = paramName.FirstLower();
                var body = CreateCodeWriter()
                    .WriteLine(creator(paramName, argName));
                var m = _currentClass.AddMethod(flu1.FluentPrefix, _currentClass.Name)
                    .WithBody(body);
                m.AddParam(argName, _currentClass.TypeName(type1));
                m.WithAutocodeGeneratedAttribute(_currentClass, ""/*body.Info*/);
            }

            var flu = new FluentMethodInfo(paramName);

            if ((flags & Fl.DirectValue) != 0)
            {
                AddDirectValue(type, flu);
                if ((flags & Fl.AddObjectOverload) != 0 && type != typeof(object))
                    AddDirectValue(typeof(object), flu);
            }

            if ((flags & Fl.AddStaticAndResource) != 0)
                AddStaticAndResource(flu);
            if (flu.AllowAncestor)
                AddAncestor(flu);
        }

        private void BuildAmmyBindBuilder(IAutoCodeGeneratorContext context, Type ownerType)
        {
            _currentClass = context.GetOrCreateClass(typeof(AmmyBindBuilder));
            var setupCode = CreateCodeWriter();
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
                {
                    var fl = i.GetFlags(ownerType);
                    AddFluentMethod(
                        SetPropertyAndReturnThis,
                        type2, i.Name, fl);
                }
            }

            _currentClass.AddMethod("SetupAmmyBind", "void")
                .WithVisibility(Visibilities.Private)
                .WithBody(setupCode)
                .WithAutocodeGeneratedAttribute(_currentClass, "" /*setupCode.Info*/)
                .AddParam<AmmyBind>("bind", _currentClass);
        }

        private static readonly List<BindingParamInfo> BindingParams = new List<BindingParamInfo>();

        private CsClass _currentClass;


        [Flags]
        private enum Fl
        {
            None = 0,
            DirectValue = 1,
            AddObjectOverload = 2,
            AddStaticAndResource = 4
        }

        private const string ValidationRules = "ValidationRules";

        private class BindingParamInfo
        {
            public BindingParamInfo(Type type, string name)
            {
                Type             = type;
                Name             = name;
            }

            public Fl GetFlags(Type ownerType)
            {
                // var fl = BindableToStaticOrResource ? Fl.AddStaticAndResource : Fl.None;
                var flags = Fl.DirectValue;
                {
                    var p = ownerType.GetProperty(Name);
                    if (p is null)
                    {
                        flags |= Fl.AddObjectOverload;
                        if (BindableToStaticOrResource)
                            flags |= Fl.AddStaticAndResource;
                    }
                    else
                    {
                        if (p.PropertyType == typeof(object)) 
                            flags |= Fl.AddObjectOverload | Fl.AddStaticAndResource;
                    }
                }
                return flags;
            }

            public bool HasProperty(Type ownerType)
            {
                var p = ownerType.GetProperty(Name);
                return p != null;
            }

            public override string ToString() => Name;

            public Type   Type { get; }
            public string Name { get; }

            /// <summary>
            ///     If Binding object has this property bindable
            /// </summary>
            public bool BindableToStaticOrResource { get; set; }
        }
    }
}