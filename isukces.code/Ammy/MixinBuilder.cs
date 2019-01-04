using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace isukces.code.Ammy
{
    public class MixinBuilder<T>
    {
        public MixinBuilder(string mixinName)
        {
            WrappedMixin = new Mixin(mixinName, typeof(T));
        }

        public MixinBuilder<T> WithProperty(Expression<Func<T, object>> propertyNameE, object value)
        {
            var propertyName = ExpressionTools.GetBindingPath(propertyNameE);
            WrappedMixin.WithProperty(propertyName, value);
            return this;
        }


        public MixinBuilder<T> WithContent(object content)
        {
            WrappedMixin.AddContent(content);
            return this;
        }

        public MixinBuilder<T> WithPropertyAncestorBind(string propertyName, string path,
            params KeyValuePair<string, string>[] bindingSettings)
        {
            return WithPropertyAncestorBind(null, propertyName, path, bindingSettings);
        }


        public MixinBuilder<T> WithPropertyAncestorBind<T1>(Expression<Func<T, object>> propertyNameE,
            Expression<Func<T1, object>> action,
            params KeyValuePair<string, string>[] bindingSettings)
        {
            var path         = ExpressionTools.GetBindingPath(action);
            var propertyName = ExpressionTools.GetBindingPath(propertyNameE);
            WithPropertyAncestorBind(propertyName, path, bindingSettings);
            return this;
        }


        public MixinBuilder<T> WithPropertyAncestorBind(Type ancestorType, string propertyName, string path,
            params KeyValuePair<string, string>[] bindingSettings)
        {
            if (ancestorType == null)
                ancestorType = DefaultAncestorType;
            var bind = new AmmyBind(path)
            {
                From = new AncestorSource(ancestorType)
            };
            
            //var value = $"bind {path} from $ancestor<{ancestorType.FullName}>";
            if (bindingSettings != null && bindingSettings.Any())
            {
                foreach (var i in bindingSettings)
                    bind.AddParameter(i.Key, new SimpleAmmyCodePiece(i.Value));
                 
            }

            WrappedMixin.WithProperty(propertyName, bind);
            return this;
        }

        public Type DefaultAncestorType { get; set; }

        public Mixin WrappedMixin { get; }
    }
}