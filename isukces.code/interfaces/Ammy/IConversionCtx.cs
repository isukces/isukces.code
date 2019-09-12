using System;
using System.Collections.Generic;
using System.Linq;
using isukces.code.AutoCode;
using JetBrains.Annotations;

namespace isukces.code.interfaces.Ammy
{
    public interface IConversionCtx
    {
        IAmmyNamespaceProvider NamespaceProvider { get; }
        bool                   FullNamespaces    { get; }
        bool ResolveSeparateLines([CanBeNull]string propertyName, [NotNull]IAmmyCodePiece code, [CanBeNull]object sourceValue, [CanBeNull]object sourceValueHost);
    }

    public static class ConversionCtxExt
    {
        public static string TypeName(this IConversionCtx ctx, Type t)
        {
            return ctx.NamespaceProvider.GetTypeName(t);
        }

        public static string TypeName<T>(this IConversionCtx ctx)
        {
            return TypeName(ctx, typeof(T));
        }
    }

    public interface IAmmyCodePiece
    {       
        bool WriteInSeparateLines { get; set; }
    }

    public interface ISimpleAmmyCodePiece:IAmmyCodePiece
    { 
        string Code { get; }        
    }

    public interface IComplexAmmyCodePiece : IAmmyCodePiece
    {
        IReadOnlyList<IAmmyCodePiece> GetNestedCodePieces();
        string GetOpeningCode();
        AmmyBracketKind Brackets { get; }
    }

    public enum AmmyBracketKind
    {
        Mustache,
        Square
    }

   
        
}