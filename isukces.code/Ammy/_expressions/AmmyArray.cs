using System.Collections.Generic;
using iSukces.Code.Interfaces.Ammy;

namespace iSukces.Code.Ammy
{
    public class AmmyArray : IAmmyCodePieceConvertible
    {
        public static AmmyArray FromItems(params object[] items)
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

        public AmmyArray WithItem(object o)
        {
            Items.Add(o);
            return this;
        }

        public List<object> Items { get; } = new List<object>();
    }
}