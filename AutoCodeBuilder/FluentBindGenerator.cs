using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using iSukces.Code;
using iSukces.Code.Ammy;
using iSukces.Code.AutoCode;
using iSukces.Code.Compatibility.System.Windows.Data;
using iSukces.Code.Interfaces;

namespace AutoCodeBuilder
{
    internal class FluentBindGenerator : BaseGenerator, IAutoCodeGenerator
    {
        static FluentBindGenerator()
        {
            BindingParams      = CreateBindingParams();
            MultiBindingParams = CreateMultiBindingParams();
        }

        public static bool IgnoreWithConverterStatic(Type type) =>
            type == typeof(AmmyBind) || type == typeof(AmmyBindBuilder);

        private static BindingParamInfoCollection CreateBindingParams()
        {
            var result = new BindingParamInfoCollection();
            AddCommon(result);

            result.Add<bool>("BindsDirectlyToSource");
            result.Add<bool>("IsAsync");
            result.Add<string>("XPath", true);
            result.Add<object>("From", true);
            result.Add<string>("Path");
            return result;
        }

        private static void AddCommon(BindingParamInfoCollection result)
        {
            result.Add<XBindingMode>("Mode");
            result.Add<object>("Converter", true);
            result.Add<object>("ConverterCulture", true);
            result.Add<object>("ConverterParameter", true);
            result.Add<XUpdateSourceTrigger>("UpdateSourceTrigger");
            result.Add<bool>("NotifyOnSourceUpdated");
            result.Add<bool>("NotifyOnTargetUpdated");
            result.Add<bool>("NotifyOnValidationError");
            result.Add<object>(ValidationRules);
            result.Add<bool>("ValidatesOnExceptions");
            result.Add<bool>("ValidatesOnDataErrors");
            result.Add<bool>("ValidatesOnNotifyDataErrors");
            result.Add<string>("StringFormat");
            result.Add<string>("BindingGroupName");
            result.Add<object>("TargetNullValue");
        }

        private static BindingParamInfoCollection CreateMultiBindingParams()
        {
            var result = new BindingParamInfoCollection();
            AddCommon(result);
            result.Add<List<object>>("Bindings");
            return result;
        }


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
                        i.Type, i.PropertyName, fl);
                }
            }
            else if (type == typeof(AmmyBindBuilder))
            {
                BuildPropertiesAndFluentMethods(context, type, BindingParams, typeof(AmmyBind));
            }
            else if (type == typeof(XMultiBinding))
            {
                BuildPropertiesAndFluentMethods(context, type, MultiBindingParams, null, false);
            }
            else if (type == typeof(AmmyMultiBindingBuilder))
            {
                BuildPropertiesAndFluentMethods(context, type, MultiBindingParams, typeof(AmmyMultiBind));
            }
            else if (type == typeof(AmmyMultiBind))
            {
                BuildPropertiesAndFluentMethods(context, type, MultiBindingParams, null);
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
                        .WithAutocodeGeneratedAttribute(_currentClass)
                        .AddParam<string>(argName, _currentClass);
                }
                {
                    const string argName = "staticResourceName";
                    var          value   = $"new {nameof(AmmyStaticResource)}({argName})";
                    var code = CreateCodeWriter()
                        .WriteLine(creator(paramName, value));
                    _currentClass.AddMethod(mi.StaticResource, _currentClass.Name)
                        .WithBody(code)
                        .WithAutocodeGeneratedAttribute(_currentClass /*code.Info*/)
                        .AddParam<string>(argName, _currentClass);
                }

                {
                    const string argName = "dynamicResourceName";
                    var          value   = $"new {nameof(AmmyDynamicResource)}({argName})";
                    var code = CreateCodeWriter()
                        .WriteLine(creator(paramName, value));
                    _currentClass.AddMethod(mi.DynamicResource, _currentClass.Name)
                        .WithBody(code)
                        .WithAutocodeGeneratedAttribute(_currentClass)
                        .AddParam<string>(argName, _currentClass);
                }
                {
                    const string argName = "elementName";
                    var          value   = $"new {nameof(ElementNameBindingSource)}({argName})";
                    var code = CreateCodeWriter()
                        .WriteLine(creator(paramName, value));
                    _currentClass.AddMethod(mi.ElementName, _currentClass.Name)
                        .WithBody(code)
                        .WithAutocodeGeneratedAttribute(_currentClass)
                        .AddParam<string>(argName, _currentClass);
                }
            }

            void AddSelf(FluentMethodInfo mi)
            {
                const string value = nameof(SelfBindingSource) + "." + nameof(SelfBindingSource.Instance);
                {
                    var code = CreateCodeWriter()
                        .WriteLine(creator(paramName, value));
                    var m = _currentClass.AddMethod(mi.Self, _currentClass.Name)
                        .WithBody(code)
                        .WithAutocodeGeneratedAttribute(_currentClass /*code.Info*/);
                }
                /*{

                    //var name = CodeUtils.GetMemberPath(func);
                    // this.WithProperty(name, value);
                    var code = CreateCodeWriter()
                        .WriteLine("var name = CodeUtils.GetMemberPath(func);")
                        .WriteLine("this.WithProperty(name, value);")
                        .WriteLine(creator(paramName, value));
                    var m = _currentClass.AddMethod(mi.Self+"<TPropertyBrowser>", _currentClass.Name)
                        .WithBody(code)
                        .WithAutocodeGeneratedAttribute(_currentClass /*code.Info#1#);
                    m.AddParam("ancestorType", "Expression<Func<TPropertyBrowser, TValue>> func");
                    
                }*/
               
            }

            void AddAncestor(FluentMethodInfo mi)
            {
                {
                    const string value = "new AncestorBindingSource(ancestorType, level)";
                    var code = CreateCodeWriter()
                        .WriteLine(creator(paramName, value));
                    var m = _currentClass.AddMethod(mi.Ancestor, _currentClass.Name)
                        .WithBody(code)
                        .WithAutocodeGeneratedAttribute(_currentClass /*code.Info*/);
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
                        .WithAutocodeGeneratedAttribute(_currentClass /*code.Info*/);
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
                m.WithAutocodeGeneratedAttribute(_currentClass /*body.Info*/);
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
            {
                AddAncestor(flu);
                AddSelf(flu);
            }
        }

        private void BuildPropertiesAndFluentMethods(IAutoCodeGeneratorContext context, Type ownerType,
            List<BindingParamInfo> bindingParams, Type setup, bool addFluetMethods=true)
        {
            _currentClass = context.GetOrCreateClass(ownerType);
            var setupCode = CreateCodeWriter();
            foreach (var i in bindingParams)
            {
                var    flu        = new FluentMethodInfo(i.PropertyName);
                var    type2      = i.Type;
                string init       = null;
                var    isReadOnly = false;
                if (i.PropertyName == ValidationRules)
                {
                    type2      = typeof(List<object>);
                    init       = "new " + _currentClass.TypeName(type2) + "()";
                    isReadOnly = true;
                }
                else
                {
                    type2 = MakeNullable(type2);
                    setupCode.SingleLineIf(
                        $"{i.PropertyName} != null",
                        $"bind.{flu.FluentPrefix}({i.PropertyName});");
                }

                bool CreateProperty()
                {
                    var pi = ownerType.GetProperty(i.PropertyName);
                    if (pi is null)
                        return true;
                    return !(pi.GetCustomAttribute<AutocodeGeneratedAttribute>() is null);
                }

                if (CreateProperty())
                {
                    var p = _currentClass.AddProperty(i.PropertyName, type2)
                        .WithNoEmitField()
                        .WithMakeAutoImplementIfPossible()
                        .WithIsPropertyReadOnly(isReadOnly)
                        .WithAutocodeGeneratedAttribute(_currentClass);
                    p.ConstValue = init;
                }

                if (!isReadOnly && addFluetMethods)
                {
                    var fl = i.GetFlags(ownerType);
                    AddFluentMethod(
                        SetPropertyAndReturnThis,
                        type2, i.PropertyName, fl);
                }
            }

            if (setup != null)
                _currentClass.AddMethod("SetupAmmyBind", "void")
                    .WithVisibility(Visibilities.Private)
                    .WithBody(setupCode)
                    .WithAutocodeGeneratedAttribute(_currentClass /*setupCode.Info*/)
                    .AddParam("bind", setup, _currentClass);
        }

        private static readonly BindingParamInfoCollection BindingParams;
        private static readonly BindingParamInfoCollection MultiBindingParams;

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

        private class BindingParamInfoCollection : List<BindingParamInfo>
        {
            public void Add<T>(string name, bool bindableToStaticOrResource = false)
            {
                var a = new BindingParamInfo(typeof(T), name, bindableToStaticOrResource);
                Add(a);
            }

           
        }

        private class BindingParamInfo
        {
            public BindingParamInfo(Type type, string propertyName, bool bindableToStaticOrResource = false)
            {
                Type                       = type;
                PropertyName               = propertyName;
                BindableToStaticOrResource = bindableToStaticOrResource;
            }

            public Fl GetFlags(Type ownerType)
            {
                // var fl = BindableToStaticOrResource ? Fl.AddStaticAndResource : Fl.None;
                var flags = Fl.DirectValue;
                {
                    var p = ownerType.GetProperty(PropertyName);
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
                var p = ownerType.GetProperty(PropertyName);
                return p != null;
            }

            public override string ToString() => PropertyName;

            public Type   Type         { get; }
            public string PropertyName { get; }

            /// <summary>
            ///     If Binding object has this property bindable
            /// </summary>
            public bool BindableToStaticOrResource { get; }
        }
    }
}