using System;
using System.Collections.Generic;
using System.Reflection;
using iSukces.Code.Interfaces;
using JetBrains.Annotations;

namespace iSukces.Code.Irony
{
    public class ConstructorBuilder
    {
        public ConstructorBuilder(CsClass target) => Target = target;

        public void AddBaseConstructor(ConstructorInfo constructor)
        {
            if (constructor is null)
                return;
            foreach (var cp in constructor.GetParameters())
                AddBaseConstructorParameter(cp.Name, Target.GetTypeName(cp.ParameterType));
        }

        public void AddBaseConstructorParameter(string baseParameterName, string baseParameterType)
        {
            _constructorArguments.Add(new Argument(baseParameterName, baseParameterType,
                Kinds.BaseConstructor));
        }

        public void AddPropertyToSet(CsProperty property)
        {
            _constructorArguments.Add(new Argument(property.Name, property.Type, Kinds.PorpertySet));
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
                    case Kinds.PorpertySet:
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

        public string CustomCodeHeader { get; set; }

        public CsClass Target { get; }

        public IReadOnlyList<Argument> ConstructorArguments
        {
            get { return _constructorArguments; }
        }


        private readonly List<Argument> _constructorArguments = new List<Argument>();

        public enum Kinds
        {
            PorpertySet,
            BaseConstructor
        }

        public class Argument
        {
            public Argument([NotNull] string name, [NotNull] string type, Kinds kind)
            {
                Name = name ?? throw new ArgumentNullException(nameof(name));
                Type = type ?? throw new ArgumentNullException(nameof(type));
                Kind = kind;
            }

            public override string ToString() => $"{Type} {Name}";

            public string Name { get; }
            public string Type { get; }
            public Kinds  Kind { get; }
        }
    }
}