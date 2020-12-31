using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using iSukces.Code.Interfaces;

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
            _baseConstructorParameters.Add(new NameAndType(baseParameterName, baseParameterType));
        }

        public void AddPropertyToSet(CsProperty property)
        {
            _properties.Add(new NameAndType(property.Name, property.Type));
        }

        public void CreateConstructor()
        {
            var constr = Target.AddConstructor();

            if (_baseConstructorParameters.Any())
            {
                foreach (var p in _baseConstructorParameters)
                    constr.AddParam(p.Name.FirstLower(), p.Type);
                var args = string.Join(", ", _baseConstructorParameters.Select(a => a.Name.FirstLower()));
                constr.BaseConstructorCall = "base(" + args + ")";
            }

            if (_properties.Any())
            {
                var cw = new CsCodeWriter();
                foreach (var p in _properties)
                {
                    var fieldName = p.Name.FirstLower();
                    constr.AddParam(fieldName, p.Type);
                    cw.WriteLine(p.Name + " = " + fieldName + ";");
                }

                constr.WithBody(cw);
            }
        }

        public CsClass Target { get; }
        private readonly List<NameAndType> _baseConstructorParameters = new List<NameAndType>();
        private readonly List<NameAndType> _properties = new List<NameAndType>();

        private struct NameAndType
        {
            public NameAndType(string name, string type)
            {
                Name = name;
                Type = type;
            }

            public string Name { get; }
            public string Type { get; }
        }
    }
}