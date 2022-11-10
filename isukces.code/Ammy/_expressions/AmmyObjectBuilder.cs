#if AMMY
using System.Collections.Generic;
using iSukces.Code.Interfaces;
using iSukces.Code.Interfaces.Ammy;

namespace iSukces.Code.Ammy
{
    public abstract class AmmyObjectBuilder : AmmyContainerBase, IAnnotableByUser
    {
        public virtual AmmyObjectBuilder<TBrowser> Map<TBrowser>()
        {
            var a = new AmmyObjectBuilder<TBrowser>();
            foreach (var i in Properties)
                a.Properties.Add(i.Key, i.Value);
            foreach (var i in UserAnnotations)
                a.UserAnnotations.Add(i.Key, i.Value);
            foreach (var i in ContentItems)
                a.ContentItems.Add(i);

            a.Name     = Name;
            a.NameKind = NameKind;
            return a;
        }

        public abstract IAmmyCodePiece ToAmmyCode(IConversionCtx ctx);

        /// <summary>
        ///     Additional information used by custom generators
        /// </summary>
        public IDictionary<string, object> UserAnnotations { get; } = new Dictionary<string, object>();

        public string Name { get; set; }

        public ObjectNameKind NameKind { get; set; }
    }


    public partial class AmmyObjectBuilder<TPropertyBrowser> : AmmyObjectBuilder,
        IAmmyObjectBuilder<TPropertyBrowser>,
        IAmmyPropertyContainer<TPropertyBrowser>
    {
        public AmmyBuilderExtender<AmmyObjectBuilder<TPropertyBrowser>, TPropertyBrowser> Extend() =>
            new AmmyBuilderExtender<AmmyObjectBuilder<TPropertyBrowser>, TPropertyBrowser>(this);

        public override IAmmyCodePiece ToAmmyCode(IConversionCtx ctx) => ToComplexAmmyCode(ctx);

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


        public AmmyObjectBuilder<TPropertyBrowser> WithName(string name, ObjectNameKind nameKind = ObjectNameKind.Name)
        {
            Name     = name;
            NameKind = nameKind;
            return this;
        }
    }

    public enum ObjectNameKind
    {
        Name,
        Key
    }
}
#endif