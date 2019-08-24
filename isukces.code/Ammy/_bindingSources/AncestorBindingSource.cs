using System;
using System.Globalization;
using isukces.code.interfaces.Ammy;

namespace isukces.code.Ammy
{
    public class AncestorBindingSource : IAmmyCodePieceConvertible, IRelativeBindingSource
    {
        public AncestorBindingSource(Type ancestorType, int? level = null)
        {
            _ancestorType = ancestorType;
            _level        = level;
        }

        public IComplexAmmyCodePiece GetObjectSyntaxCode(IConversionCtx ctx)
        {
            var aaa = new AmmyContainerBase();

            void Add(string name, object value)
            {
                while (true)
                    switch (value)
                    {
                        case null:
                            return;
                        case string txt:
                            value = new SimpleAmmyCodePiece(txt.CsEncode());
                            break;
                        case int l:
                            value = new SimpleAmmyCodePiece(l.ToCsString());
                            break;
                        case IAmmyCodePiece cp:
                            aaa.Properties[name] = cp;
                            return;
                        default:
                            throw new NotImplementedException(value.GetType().ToString());
                    }
            }

            Add("Mode", "FindAncestor");
            Add("AncestorType", ctx.TypeName(_ancestorType));
            if (_level != null)
                Add("AncestorLevel", _level.Value);

            return aaa.ToComplexAmmyCode(ctx, "RelativeSource");
        }

        public IAmmyCodePiece ToAmmyCode(IConversionCtx ctx)
        {
            var txt = "$ancestor<" + ctx.TypeName(_ancestorType) + ">";
            if (_level != null)
                txt += "(" + _level.Value.ToCsString() + ")";
            return new SimpleAmmyCodePiece(txt);
        }

        private readonly Type _ancestorType;
        private readonly int? _level;
    }
}