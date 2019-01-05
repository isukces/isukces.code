using System;
using isukces.code.interfaces.Ammy;
using JetBrains.Annotations;

namespace isukces.code.Ammy
{
    public class AmmyMixin : AmmyContainerBase, IAmmyCodePieceConvertible
    {
        public AmmyMixin(string name, [NotNull] Type forType)
        {
            Name            = name;
            MixinTargetType = forType ?? throw new ArgumentNullException(nameof(forType));
        }

        public IAmmyCodePiece ToAmmyCode(IConversionCtx ctx)
        {
            var openingCode = string.Format("mixin {0}() for {1}", Name, ctx.TypeName(MixinTargetType));
            return this.ToComplexAmmyCode(ctx, openingCode);
        }

        public string Name            { get; }
        public Type   MixinTargetType { get; }
    }
}