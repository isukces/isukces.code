using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using iSukces.Code.Interfaces;

namespace iSukces.Code.AutoCode;

public abstract partial class Generators
{
    public class BuilderForTypeGenerator : SingleClassGenerator<Auto.BuilderForTypeAttribute>
    {
        private void AddBuildMethod()
        {
            var constructorParametersNames = _builderPropertyInfos.MapToArray(a => a.PropertyName);
            var m = _class
                .AddMethod("Build", _class.GetTypeName(Attribute.TargetType))
                .WithAggressiveInlining(_class);
            var constructorCall = constructorParametersNames
                .CommaJoin()
                .New(m.ResultType.GetTypeNameOrThrowIfVoid(false));
            m.WithBodyAsExpression(constructorCall);
        }

        private void AddConstructors()
        {
            // pusty konstruktor
            var c1 = _class.AddConstructor();
            if (AddCs8618WarningDisable)
                c1.PragmasWarnings.Add(CsPragmaWarning.Disable("CS8618"));
            // konstruktor2
            CodeWriter c = new CsCodeWriter();
            if (Attribute.TargetType.IsClass)
                c.WriteLine("if (source is null) return;");
            foreach (var i in _builderPropertyInfos)
                c.WriteLine("{0} = source.{0};", i.PropertyName);
            var m = _class.AddConstructor()
                .WithBody(c);
            m.Parameters.Add(new CsMethodParameter("source", _class.GetTypeName(Attribute.TargetType)));
            if (AddCs8618WarningDisable)
                m.PragmasWarnings.Add(CsPragmaWarning.Disable("CS8618"));
        }

        protected virtual void AddIBuilderAttribute()
        {
#if SAMPLE
                given interface :

                public interface IBuilder<out TResult>
                {
                    TResult Build();
                }

                following code decorates builder class with IBuilder<> implementation
                
                var atTargetType = Attribute.TargetType;
                var t = typeof(IBuilder<>).MakeGenericType(atTargetType);
                var typeName = _class.GetTypeName(t);
                // if (typeName=="IBuilder<PolyLinePoint>") Developer.Nop();
                _class.ImplementedInterfaces.Add(typeName);
#endif
        }

        protected virtual void AddWithMethod(string propName, Type propertyType, bool referenceNullable)
        {
            var pName            = "new" + propName;
            var propertyTypeName = _class.GetTypeName(propertyType);
            if (referenceNullable)
                propertyTypeName = propertyTypeName.WithReferenceNullable();
            
            var m = _class.AddMethod($"With{propName}", _class.Name)
                .WithAggressiveInlining(_class)
                .WithBody($"{propName} = {pName};\r\nreturn this;");
            m.AddParam(pName, propertyTypeName);

            var property = new NameAndTypeName(propName, propertyTypeName);
            AfterAddWithMethod(propertyType, property);
        }

        private void AddWithMethodsAndProperties()
        {
            for (var index = 0; index < _builderPropertyInfos.Length; index++)
            {
                var info             = _builderPropertyInfos[index];
                var propertyTypeName = info.GetTypeName(_class);
                var prop = _class.AddProperty(info.PropertyName, propertyTypeName)
                    .WithMakeAutoImplementIfPossible();

                if (info.Create)
                    prop.WithConstValue(propertyTypeName.New());

                if (!info.SkipWithMethod)
                    AddWithMethod(info.PropertyName, info.PropertyType, info.ReferenceNullable);

                if (!info.ExpandFlags) continue;
                // var enumUnderlyingType = Enum.GetUnderlyingType(info.PropertyType);
                foreach (var enumValue in Enum.GetValues(info.PropertyType))
                {
                    // var value1 = Convert.ChangeType(enumValue, enumUnderlyingType);
                    var value2 = (int)Convert.ChangeType(enumValue, typeof(int));
                    if (value2 == 0)
                        continue;

                    var enumName = Enum.GetName(info.PropertyType, enumValue);
                    var propName = enumName!.FirstUpper();
                    var prop1 = _class.AddProperty(propName, CsType.Bool)
                        .WithNoEmitField();
                    var v = $"{propertyTypeName.Declaration}.{enumName}";
                    prop1.WithOwnGetterAsExpression($"({info.PropertyName} & {v}) != 0");


                    prop1.OwnSetter             = $"{info.PropertyName} = value ? {info.PropertyName} | {v} : {info.PropertyName} & ~{v};";
                    prop1.OwnSetterIsExpression = true; //!prop1.EffectiveBackingField;
                    
                    // metoda
                    if (!info.SkipWithMethod)
                        AddWithMethod(propName, typeof(bool), false);
                }
            }
        }


        protected void AddWithMethodUsingPropertyTypeConstructor(NameAndTypeName property,
            params NameAndTypeName[] constructorParams)
        {
            var m = _class.AddMethod("With" + property.Name, _class.Name)
                .WithAggressiveInlining(_class);
            var l = new List<string>();
            foreach (var i in constructorParams)
            {
                var pName = $"new{i.Name}X";
                l.Add(pName);
                m.AddParam(pName, i.Type);
            }

            var constructorCall = l.CommaJoin()
                .New(property.Type.Declaration);

            m.Body = $"{property.Name} = {constructorCall};\r\nreturn this;";
        }

        protected virtual void AfterAddWithMethod(Type propertyType, NameAndTypeName property)
        {
#if SAMPLE
                // sample code for creating custom Withxxx methods
                if (propertyType == typeof(Point))
                    AddWithMethodUsingPropertyTypeConstructor(property, new NameAndType("X", null), new NameAndType("Y", null));

                if (propertyType == typeof(PdAngleRange))
                    AddWithMethodUsingPropertyTypeConstructor(property, new NameAndType("Begin", null), new Tuple<string, Type>("End", null));
#endif
        }

        protected override void GenerateInternal()
        {
            try
            {
                var ats = Type.GetCustomAttributes<Auto.BuilderForTypePropertyAttribute>().ToArray();
                _attributesForProperties = ats.ToDictionary(a => a.PropertyName, a => a);
                var constructors = Attribute.TargetType.GetConstructors()
                    .Select(a => a.GetParameters())
                    .OrderByDescending(a => a.Length)
                    .ToArray();
                if (constructors.Length == 0)
                    return;
                var constructorParameterInfos = constructors[0];
                if (constructorParameterInfos.Length == 0)
                    return;

                _builderPropertyInfos = constructorParameterInfos.MapToArray(q =>
                {
                    var info = BuilderPropertyInfo.MakeInfo(q, _attributesForProperties, Attribute);
                    return info;
                });

                _class           = Context.GetOrCreateClass(Type);
                _class.IsPartial = true;
                AddIBuilderAttribute();
                AddConstructors();
                AddWithMethodsAndProperties();
                AddBuildMethod();
            }
            finally
            {
                _class                   = null!;
                _attributesForProperties = null!;
                _builderPropertyInfos    = null!;
            }
        }

        public bool AddCs8618WarningDisable { get; set; } = true;

        private IReadOnlyDictionary<string, Auto.BuilderForTypePropertyAttribute> _attributesForProperties = null!;
        private BuilderPropertyInfo[] _builderPropertyInfos = null!;
        private CsClass _class = null!;

        private sealed class BuilderPropertyInfo
        {
            public static BuilderPropertyInfo MakeInfo(ParameterInfo parameterInfo,
                IReadOnlyDictionary<string, Auto.BuilderForTypePropertyAttribute> attributesForProperties,
                Auto.BuilderForTypeAttribute at)
            {
                var nullable = ReferenceNullableTools.IsReferenceTypeNullable(parameterInfo);
                var propName = parameterInfo.Name!.FirstUpper();

                attributesForProperties.TryGetValue(propName, out var pa);

                var a = new BuilderPropertyInfo
                {
                    PropertyName = propName,
                    PropertyType = pa?.Type ?? parameterInfo.ParameterType,
                    SkipWithMethod =
                        at.SkipWithForAll || at.SkipWithFor.Length > 0 && at.SkipWithFor.Contains(propName),
                    Create            = pa?.Create ?? false,
                    ReferenceNullable = nullable
                };
                if (pa?.ExpandFlags ?? false)
                    if (a.PropertyType.IsEnum)
                        if (a.PropertyType.GetCustomAttribute<FlagsAttribute>() is not null)
                            a.ExpandFlags = true;
                return a;
            }

            public CsType GetTypeName(ITypeNameResolver resolver)
            {
                var propertyTypeName = resolver.GetTypeName(PropertyType);
                if (ReferenceNullable)
                    propertyTypeName = propertyTypeName.WithReferenceNullable();
                return propertyTypeName;
            }

            public string PropertyName      { get; private init; } = null!;
            public Type   PropertyType      { get; private init; } = null!;
            public bool   SkipWithMethod    { get; private init; }
            public bool   Create            { get; private init; }
            public bool   ExpandFlags       { get; private set; }
            public bool   ReferenceNullable { get; private init; }
        }
    }
}
