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
            Name  = name?.Trim() ?? throw new ArgumentNullException(nameof(name));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public IAmmyCodePiece ToAmmyCode(IConversionCtx ctx)
        {
            return new SimpleAmmyCodePiece($"${Name} = {Value.CsEncode()}");
        }

        public string Name { get; }

        public string Value { get; }
    }
}