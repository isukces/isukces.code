using System;
using isukces.code.interfaces;
using isukces.code.interfaces.Ammy;
using JetBrains.Annotations;

namespace isukces.code.Ammy
{
    public class AmmyVariableDefinition : IAmmyCodePieceConvertible
    {
        public AmmyVariableDefinition([NotNull] string name, [NotNull] string value)
        {
            _name  = name?.Trim() ?? throw new ArgumentNullException(nameof(name));
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public IAmmyCodePiece ToAmmyCode(IConversionCtx ctx)
        {
            return new SimpleAmmyCodePiece($"${_name} = {_value.CsEncode()}");
        }

        private readonly string _name;
        private readonly string _value;
    }
}