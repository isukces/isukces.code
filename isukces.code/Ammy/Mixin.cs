using System;
using System.Collections.Generic;
using System.Linq;
using isukces.code.interfaces.Ammy;
using JetBrains.Annotations;

namespace isukces.code.Ammy
{
    public class Mixin : IAmmyCodePieceConvertible
    {
        public Mixin(string name, [NotNull] Type forType)
        {
            Name    = name;
            ForType = forType ?? throw new ArgumentNullException(nameof(forType));
        }

        public void AddContent(object content)
        {
            _items.Add(new KeyValuePair<string, object>(null, content));
        }


        public IAmmyCodePiece ToCodePiece(IConversionCtx ctx)
        {
            var part1       = _items.Where(a => !string.IsNullOrEmpty(a.Key));
            var part2       = _items.Where(a => string.IsNullOrEmpty(a.Key));
            var codePieces1 = part1.Concat(part2);
            var codePieces  = codePieces1.ToAmmyPropertiesCodePieces(ctx, true);
            var openingCode = string.Format("mixin {0}() for {1}", Name, ctx.TypeName(ForType));
            return new ComplexAmmyCodePiece(codePieces, openingCode);
        }

        public Mixin WithProperty(string propertyName, object propertyValue)
        {
            _items.Add(new KeyValuePair<string, object>(propertyName, propertyValue));
            return this;
        }

        public Mixin WithPropertyAncestorBind([NotNull] Type ancestorType,
            string propertyName, string path,
            params KeyValuePair<string, string>[] bindingSettings)
        {
            if (ancestorType == null) throw new ArgumentNullException(nameof(ancestorType));

            var bind = new AmmyBind(path)
            {
                From = new AncestorSource(ancestorType)
            };

            //var value = $"bind {path} from $ancestor<{ancestorType.FullName}>";
            if (bindingSettings != null && bindingSettings.Any())
                foreach (var i in bindingSettings)
                    bind.AddParameter(i.Key, new SimpleAmmyCodePiece(i.Value));

            WithProperty(propertyName, bind);
            return this;
        }


        public string Name                           { get; }
        public Type   ForType                        { get; }

        private readonly List<KeyValuePair<string, object>> _items = new List<KeyValuePair<string, object>>();
    }
}