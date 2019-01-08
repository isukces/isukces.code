using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using isukces.code.interfaces.Ammy;
using JetBrains.Annotations;

namespace isukces.code.Ammy
{
    public static class AmmyContainerCodeExtensions
    {
        public static IAmmyCodePiece[] GetCodePiecesUniversal(this IAmmyContainer self, IConversionCtx ctx)
        {
            var properites   = self.Properties.ToAmmyPropertiesCodeWithLineSeparators(ctx, self);
            var contentItems = self.ContentItems.ToAmmyContentItemsCode(ctx, self, true);

            var codePieces = properites.Concat(contentItems).ToArray();
            return codePieces;
        }
        
        public static IComplexAmmyCodePiece ToComplexAmmyCode(this IAmmyContainer self, IConversionCtx ctx, string openingCode)
        {
            var codePieces = self.GetCodePiecesUniversal(ctx);
            return new ComplexAmmyCodePiece(codePieces, openingCode);
        }

        public static T WithPropertyAncestorBind<T>(this T self, 
            string propertyName, 
            string path,
            [NotNull] Type ancestorType,
            [CanBeNull] KeyValuePair<string, string>[] bindingSettings = null)
            where T : IAmmyPropertyContainer
        {
            if (ancestorType == null) throw new ArgumentNullException(nameof(ancestorType));
            if (ancestorType == null) throw new ArgumentNullException(nameof(ancestorType));
            var bind = AmmyBind.FromAncestor(path, ancestorType);

            //var value = $"bind {path} from $ancestor<{ancestorType.FullName}>";
            if (bindingSettings != null && bindingSettings.Any())
                foreach (var i in bindingSettings)
                    bind.WithSetParameter(i.Key, new SimpleAmmyCodePiece(i.Value));
            self.WithProperty(propertyName, bind);
            return self;
        }
        
        
        public static T WithPropertyNotNull<T, TValue>(
            this T self, 
            Expression<Func<T, TValue>> func, object v)
            where T : IAmmyPropertyContainer
        {
            var mi = AmmyHelper.GetMemberInfo(func);
            return self.WithPropertyNotNull(mi.Member.Name, v);
        }
        
    }
 
}