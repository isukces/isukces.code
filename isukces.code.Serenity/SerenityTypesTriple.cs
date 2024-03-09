using System;

namespace iSukces.Code.Serenity
{
    public struct SerenityTypesTriple
    {
        public SerenityTypesTriple(Type facade, Type fieldWrapped, CsType rowFieldType)
        {
            Facade       = facade;
            FieldWrapped = fieldWrapped;
            RowFieldType = rowFieldType;
        }

        public static SerenityTypesTriple FromType(Type t)
        {
            var w = new ReflectionTypeWrapper(t);
            w = new ReflectionTypeWrapper(w.UnwrapNullable());
            if (w.IsEnum)
            {
                var code         = Type.GetTypeCode(t);
                var nullableEnum = w.MakeNullableIfPossible();
                switch (code)
                {
                    case TypeCode.Int32: return new SerenityTypesTriple(nullableEnum, typeof(int?), (CsType)"Int32Field");
                    case TypeCode.Int16: return new SerenityTypesTriple(nullableEnum, typeof(short?), (CsType)"Int16Field");
                    default:
                        throw new Exception("Unsupported enum type " + t);
                }
            }

            if (t == typeof(string)) return Make<string>("StringField");
            if (t == typeof(int)) return Make<int?>("Int32Field");
            if (t == typeof(short)) return Make<short?>("Int16Field");
            if (t == typeof(long)) return Make<long?>("Int64Field");

            if (t == typeof(DateTime)) return Make<DateTime?>("DateTimeField");
            if (t == typeof(bool)) return Make<bool?>("BooleanField");
            if (t == typeof(Guid)) return Make<Guid?>("GuidField");
            throw new Exception("Unsupported type " + t);
        }

        public static SerenityTypesTriple Make<T>(string field)
        {
            return new SerenityTypesTriple(typeof(T), typeof(T), (CsType)field);
        }

        public Type   Facade       { get; }
        public Type   FieldWrapped { get; }
        public CsType RowFieldType { get; }
    }
}