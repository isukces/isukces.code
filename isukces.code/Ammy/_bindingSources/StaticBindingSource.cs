using System;
using iSukces.Code.Interfaces.Ammy;

namespace iSukces.Code.Ammy
{
    public class StaticBindingSource : IAmmyCodePieceConvertible
    {
        public StaticBindingSource(Type ownerType, string propertyName)
        {
            OwnerType     = ownerType;
            PropertyName = propertyName;
        }

        public static StaticBindingSource Make<T>(string property)
        {
            return new StaticBindingSource(typeof(T), property);
        }

        public IAmmyCodePiece ToAmmyCode(IConversionCtx ctx)
        {
            var code = ctx.TypeName(OwnerType) + "." + PropertyName;
            return new SimpleAmmyCodePiece(code);
        }

        public override string ToString()
        {
            return $"StaticBindingSource {OwnerType}.{PropertyName}";
        }

        public Type   OwnerType    { get; }
        public string PropertyName { get; }
    }
}