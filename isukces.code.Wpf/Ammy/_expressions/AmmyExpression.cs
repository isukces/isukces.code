using System;
using isukces.code.interfaces;
using isukces.code.interfaces.Ammy;

namespace isukces.code.Wpf.Ammy
{
    internal class AmmyExpression : IAmmyExpression
    {
        public AmmyExpression(string code)
        {
            _code = code;
        }

        public static IAmmyExpression FromStatic<T>(string propertyName)
        {
            return new Sf(typeof(T), propertyName);
        }

        public static IAmmyExpression FromString(string s)
        {
            return new AmmyExpression(s.CsEncode());
        }

        public string GetAmmyCode(IConversionCtx ctx)
        {
            return _code;
        }

        public override string ToString()
        {
            return _code;
        }

        private readonly string _code;

        private class Sf : IAmmyExpression
        {
            public Sf(Type type, string propertyName)
            {
                _type = type;
                _propertyName = propertyName;
            }

            public string GetAmmyCode(IConversionCtx ctx)
            {
                var q = ctx.TypeName(_type) + "." + _propertyName;
                return q;
            }

            private readonly Type _type;
            private readonly string _propertyName;
        }
    }
}