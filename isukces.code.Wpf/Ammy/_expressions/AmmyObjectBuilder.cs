using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Windows.Controls;
using isukces.code.interfaces.Ammy;

namespace isukces.code.Wpf.Ammy
{
    internal class AmmyObjectBuilder<T> : IAmmyExpression
    {
        public string GetAmmyCode(IConversionCtx ctx)
        {
            return AmmyHelper.EmitObject<T>(_props, ctx, _content);
        }

        public AmmyObjectBuilder<T> With<TValue>(Expression<Func<T, TValue>> func, TValue v)
        {
            return WithAny(func, v);
        }

        public AmmyObjectBuilder<T> With<TValue>(Expression<Func<T, TValue>> func, AmmyObjectBuilder<TValue> v)
        {
            return WithAny(func, v);
        }

        public AmmyObjectBuilder<T> WithAny(string name, object v)
        {
            _props[name] = v;
            return this;
        }

        public AmmyObjectBuilder<T> WithAny<TValue>(Expression<Func<T, TValue>> func, object v)
        {
            var mi = AmmyHelper.GetMemberInfo(func);
            _props[mi.Member.Name] = v;
            return this;
        }

        public AmmyObjectBuilder<T> WithAnyNotNull<TValue>(Expression<Func<T, TValue>> func, object v)
        {
            var mi = AmmyHelper.GetMemberInfo(func);
            return WithAnyNotNull(mi.Member.Name, v);
        }

        public AmmyObjectBuilder<T> WithAnyNotNull(string propName, object v)
        {
            if (v == null)
                return this;
            _props[propName] = v;
            return this;
        }

        public AmmyObjectBuilder<T> WithContent(object content)
        {
            _content.Add(content);
            return this;
        }

        public AmmyObjectBuilder<T> WithNotNull<TValue>(Expression<Func<T, TValue>> func, TValue v)
        {
            if (v == null)
                return this;
            return WithAny(func, v);
        }

        private readonly Dictionary<string, object> _props = new Dictionary<string, object>();
        public readonly List<object> _content = new List<object>();
    }

    internal static class AmmyObjectBuilderExt
    {
        public static AmmyObjectBuilder<TextBlock> WithDefaultStyle(this AmmyObjectBuilder<TextBlock> x)
        {
            return x.WithAny(nameof(TextBlock.FontFamily), new AmmyVariable("FontFamily"));
        }
    }
}