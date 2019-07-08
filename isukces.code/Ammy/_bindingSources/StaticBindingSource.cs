using System;
using isukces.code.interfaces.Ammy;

namespace isukces.code.Ammy
{
    public class StaticBindingSource : IAmmyCodePieceConvertible
    {
        public StaticBindingSource(Type type, string property)
        {
            _type     = type;
            _property = property;
        }

        public static StaticBindingSource Make<T>(string property)
        {
            return new StaticBindingSource(typeof(T), property);
        }

        public IAmmyCodePiece ToAmmyCode(IConversionCtx ctx)
        {
            var code = ctx.TypeName(_type) + "." + _property;
            return new SimpleAmmyCodePiece(code);
        }

        public override string ToString()
        {
            return $"StaticBindingSource {_type}.{_property}";
        }

        private readonly Type _type;
        private readonly string _property;
    }
}