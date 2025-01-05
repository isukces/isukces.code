using System;
using System.Collections.Generic;
using System.Linq;
using iSukces.Code.Interfaces;

namespace iSukces.Code.Translations
{
    public sealed class CsMethodCodeWriter : CsCodeWriter, ITypeNameResolver
    {
        public CsMethodCodeWriter(ITypeNameResolver resolver)
        {
            _resolver = resolver;
        }

        public string Create(Type createdType, params object[] o)
        {
            var args = o.Select(EncodeValue);
            return _resolver.GetTypeName(createdType).New(string.Join(", ", args));
            // return string.Format("new {0}({1})", _resolver.GetTypeName(createdType), string.Join(", ", args));
        }

        public void DeclareVariable(string name, string code)
        {
            var code2 = $"{name} = {code};";
            if (_variables.Add(name))
                code2 = "var " + code2;
            this.WriteLine(code2);
        }

        public string EncodeValue(object? value)
        {
            if (value is null)
                return "null";
            var type = value.GetType();
            if (type.IsEnum) return _resolver.GetTypeName(type).GetMemberCode(value.ToString());
            switch (value)
            {
                case StaticPropertyReference r:
                    var a  = _resolver.GetTypeName(r.Type).GetMemberCode(r.SingletonPropertyName);
                    var tn = $"nameof({a})";
                    return Create(typeof(StaticPropertyReference), r.Type, tn);
                case Type typeValue:
                {
                    if (_typecode.TryGetValue(typeValue, out var variable))
                        return variable;
                    _typecode[typeValue] = variable = "t" + typeValue.FullName.Replace(".", "").Replace("+", "_");
                    DeclareVariable(variable, _resolver.GetTypeName(typeValue).TypeOf());
                    return variable;
                }
                default:
                    return value.ToString();
            }
        }

        public CsType GetTypeName(Type type)
        {
            return _resolver.GetTypeName(type);
        }

        private readonly ITypeNameResolver _resolver;
        private readonly Dictionary<Type, string> _typecode = new Dictionary<Type, string>();
        private readonly HashSet<string> _variables = new HashSet<string>();
    }
}