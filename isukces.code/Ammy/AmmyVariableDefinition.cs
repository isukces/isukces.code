using System;
using iSukces.Code.Interfaces;
using iSukces.Code.Interfaces.Ammy;
using JetBrains.Annotations;

namespace iSukces.Code.Ammy
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