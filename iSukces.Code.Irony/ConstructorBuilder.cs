using System;
using System.Collections.Generic;
using System.Reflection;
using iSukces.Code.Interfaces;
using JetBrains.Annotations;

namespace iSukces.Code.Irony;

public class ConstructorBuilder
{
    public ConstructorBuilder(CsClass target)
    {
        Target                  = target;
        _allowReferenceNullable = target.AllowReferenceNullable();
    }

    public void AddBaseConstructor(ConstructorInfo constructor)
    {
        if (constructor is null)
            return;
        foreach (var cp in constructor.GetParameters())
            AddBaseConstructorParameter(cp.Name, Target.GetTypeName(cp.ParameterType));
    }

    public void AddBaseConstructorParameter(string baseParameterName, CsType baseParameterType)
    {
        _constructorArguments.Add(new Argument(baseParameterName, baseParameterType,
            Kinds.BaseConstructor));
    }

    public void AddPropertyToSet(CsProperty property)
    {
        var typeName = property.Type;
        var argument = new Argument(property.Name, typeName, Kinds.PropertySet);
        _constructorArguments.Add(argument);
    }

    public void CreateConstructor()
    {
        var constr                    = Target.AddConstructor();
        var arguments                 = ConstructorArguments;
        var baseConstructorParameters = new CsArgumentsBuilder();
        var cw                        = CsCodeWriter.Create<ConstructorBuilder>();
        if (!string.IsNullOrEmpty(CustomCodeHeader))
            cw.WriteLine("CustomCodeHeader");
        foreach (var arg in arguments)
        {
            var fieldName = arg.Name.FirstLower();
            constr.AddParam(fieldName, arg.Type);
            switch (arg.Kind)
            {
                case Kinds.PropertySet:
                    cw.WriteLine($"{arg.Name} = {fieldName};");
                    break;
                case Kinds.BaseConstructor:
                    baseConstructorParameters.AddCode(fieldName);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        if (baseConstructorParameters.Any())
            constr.BaseConstructorCall = baseConstructorParameters.CallMethod("base", false);
        constr.WithBody(cw);
    }

    #region Properties

    public string CustomCodeHeader { get; set; }

    public CsClass Target { get; }

    public IReadOnlyList<Argument> ConstructorArguments => _constructorArguments;

    #endregion

    #region Fields

    private readonly List<Argument> _constructorArguments = new List<Argument>();
    private readonly bool _allowReferenceNullable;

    #endregion

    public enum Kinds
    {
        PropertySet,
        BaseConstructor
    }

    public class Argument
    {
        public Argument([NotNull] string name, CsType type, Kinds kind)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            type.ThrowIfVoid();
            Type = type;
            Kind = kind;
        }

        public override string ToString() => $"{Type} {Name}";

        #region Properties

        public string Name { get; }
        public CsType Type { get; }
        public Kinds  Kind { get; }

        #endregion
    }
}