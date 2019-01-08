using System.Collections.Generic;
using isukces.code.interfaces;
using isukces.code.interfaces.Ammy;

namespace isukces.code.Ammy
{
    public partial class AmmyObjectBuilder<TPropertyBrowser> : AmmyContainerBase,
        IAmmyObjectBuilder<TPropertyBrowser>
    {
        public IAmmyCodePiece ToAmmyCode(IConversionCtx ctx)
        {
            return ToComplexAmmyCode(ctx);
        }

        public IComplexAmmyCodePiece ToComplexAmmyCode(IConversionCtx ctx)
        {
            var opening = ctx.TypeName<TPropertyBrowser>();
            if (!string.IsNullOrEmpty(Name))
            {
                if (NameKind == ObjectNameKind.Name)
                    opening += " " + Name.CsEncode() + " ";
                else
                    opening += " Key=" + Name.CsEncode() + " ";
            }

            return this.ToComplexAmmyCode(ctx, opening);
        }

        /// <summary>
        ///     Additional information used by custom generators
        /// </summary>
        public Dictionary<string, object> UserFlags { get; } = new Dictionary<string, object>();

        public string Name { get; set; }

        public ObjectNameKind NameKind { get; set; }
    }

    public enum ObjectNameKind
    {
        Name,
        Key
    }
}