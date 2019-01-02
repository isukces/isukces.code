﻿using System;
using isukces.code.interfaces;
using isukces.code.interfaces.Ammy;

namespace isukces.code.Ammy
{
    public class AmmyExpression : IAmmyCodePieceConvertible
    {
        public AmmyExpression(string code)
        {
            _code = code;
        }

        public static IAmmyCodePieceConvertible FromStatic<T>(string propertyName)
        {
            return new StaticSource(typeof(T), propertyName);
        }

        public static IAmmyCodePieceConvertible FromString(string s)
        {
            return new AmmyExpression(s.CsEncode());
        }

        public IAmmyCodePiece ToCodePiece(IConversionCtx ctx)
        {
            return new SimpleAmmyCodePiece(_code);
        }

        public override string ToString()
        {
            return _code;
        }

        private readonly string _code;

        private class StaticSource : IAmmyCodePieceConvertible
        {
            public StaticSource(Type type, string propertyName)
            {
                _type         = type;
                _propertyName = propertyName;
            }

            public IAmmyCodePiece ToCodePiece(IConversionCtx ctx)
            {
                var q = ctx.TypeName(_type) + "." + _propertyName;
                return new SimpleAmmyCodePiece(q);
            }


            private readonly Type _type;
            private readonly string _propertyName;
        }
    }
}