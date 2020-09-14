using System.Globalization;
using System.Linq;
using iSukces.Code.Interfaces.Ammy;

namespace iSukces.Code.Ammy
{
    public class AmmyCellMixin : IAmmyCodePieceConvertible
    {
        public void Append(AmmyCellMixin m)
        {
            if (m is null)
                return;
            if (m.Row != null) Row               = m.Row;
            if (m.Column != null) Column         = m.Column;
            if (m.RowSpan != null) RowSpan       = m.RowSpan;
            if (m.ColumnSpan != null) ColumnSpan = m.ColumnSpan;
        }

        public IAmmyCodePiece ToAmmyCode(IConversionCtx ctx)
        {
            return ToAmmyCode();
        }

        public override string ToString()
        {
            return IsEmpty ? "Empty" : ToAmmyCode().Code;
        }

        private SimpleAmmyCodePiece ToAmmyCode()
        {
            var args    = new[] {Row, Column, RowSpan, ColumnSpan};
            var encoded = args.Select(a => a is null ? "none" : a.Value.ToCsString());
            var q       = new AmmyCallBuilder("Cell");
            q.Arguments.AddRange(encoded);
            q.WithTrimLastNones();
            return q.BuildMixinCall();
        }

        public int? Row        { get; set; }
        public int? Column     { get; set; }
        public int? RowSpan    { get; set; }
        public int? ColumnSpan { get; set; }

        public bool IsEmpty
        {
            get { return Row is null && Column is null && RowSpan is null && ColumnSpan is null; }
        }
    }
}