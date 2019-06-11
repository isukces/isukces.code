using System.Collections.Generic;
using isukces.code.interfaces.Ammy;

namespace isukces.code.Ammy
{
    public class AmmyArray : IAmmyCodePieceConvertible
    {
        private static AmmyArray FromItems(params object[] items)
        {
            var ammyArray = new AmmyArray();
            ammyArray.Items.AddRange(items);
            return ammyArray;
        }

        public IAmmyCodePiece ToAmmyCode(IConversionCtx ctx)
        {
            var converted = new IAmmyCodePiece[Items.Count];
            for (var index = 0; index < Items.Count; index++)
            {
                var a = Items[index];
                converted[index] = ctx.ToCodePieceWithLineSeparators(a, null, this);
            }

            return new ComplexAmmyCodePiece(converted, null, AmmyBracketKind.Square);
        }

        public List<object> Items { get; } = new List<object>();
    }
}