using System;
using System.Collections.Generic;
using System.Linq;
using iSukces.Code.Interfaces;

namespace iSukces.Code
{
    public sealed class CsGenericArguments
    {
        public CsGenericArguments(params string[] types) => Types = types.Distinct().ToArray();

        public static CsGenericArguments operator +(CsGenericArguments arguments, string typeName)
        {
            if (arguments is null)
                return new CsGenericArguments(typeName);
            var types = arguments.Types.CreateNewWithLastElement(typeName).ToArray();
            var sum   = new CsGenericArguments(types);
            sum.Constraints.AddRange(arguments.Constraints);
            return sum;
        }

        public static CsGenericArguments operator +(string typeName, CsGenericArguments arguments)
        {
            if (arguments is null)
                return new CsGenericArguments(typeName);
            var types = arguments.Types.CreateNewWithFirstElement(typeName).ToArray();
            var sum   = new CsGenericArguments(types);
            sum.Constraints.AddRange(arguments.Constraints);
            return sum;
        }

        
        public IReadOnlyList<string> GetLines(bool addComma, ITypeNameResolver typeNameResolver)
        {
            if (Constraints.Count == 0) return addComma ? new[] {";"} : XArray.Empty<string>();
            var result = new List<string>();
            var q = Constraints.GroupBy(a => a.TypeName)
                .ToDictionary(a => a.Key, a => a.ToArray());
            foreach (var i in Types)
                if (q.TryGetValue(i, out var list))
                {
                    var items = list
                        .Select(a => new
                        {
                            a.Order,
                            CsCode = a.GetCode(typeNameResolver)
                        })
                        .OrderBy(a => a.Order).ThenBy(a => a.CsCode)
                        .Select(a => a.CsCode)
                        .ToArray();
                    result.Add(string.Format("where {0}: {1}", i, items.CommaJoin()));
                }

            if (addComma)
            {
                if (result.Count == 0)
                    return new[] {";"};
                result[result.Count - 1] += ";";
            }

            return result.ToArray();
        }

        public CsGenericArguments WithConstraint(Constraint constraint)
        {
            Constraints.Add(constraint);
            return this;
        }

        public CsGenericArguments WithConstraint<T>(string typeName)
        {
            Constraints.Add(Constraint.FromType<T>(typeName));
            return this;
        }

        public CsGenericArguments WithConstraint<T>(int index = 0)
        {
            Constraints.Add(Constraint.FromType<T>(Types[index]));
            return this;
        }

        public void WriteCode(ICsCodeWriter writer, bool addComma, ITypeNameResolver typeNameResolver)
        {
            var lines = GetLines(addComma, typeNameResolver);
            if (lines.Count == 0)
                return;
            writer.IncIndent();
            foreach (var line in lines)
                writer.WriteLine(line);
            writer.DecIndent();
        }

        public IReadOnlyList<string> Types
        {
            get => _types;
            set
            {
                if (value is null)
                    throw new ArgumentNullException(nameof(Types));
                if (value.Count == 0)
                    throw new ArgumentException(nameof(Types), "at leaset one element");
                _types = value;
            }
        }

        public List<Constraint> Constraints { get; } = new List<Constraint>();

        private IReadOnlyList<string> _types;

        public abstract class Constraint
        {
            protected Constraint(string typeName) => TypeName = typeName;

            public static Constraint FromType<T>(string typeName) => new TypeConstraint(typeName, typeof(T));
            public abstract string GetCode(ITypeNameResolver resolver);

            public          string TypeName { get; }
            public abstract int    Order    { get; }
        }

        public sealed class TypeConstraint : Constraint
        {
            public TypeConstraint(string typeName, Type type) : base(typeName) => Type = type;

            public override string GetCode(ITypeNameResolver resolver) => resolver.GetTypeName(Type);


            public          Type Type  { get; }
            public override int  Order => 99;
        }
    }
}