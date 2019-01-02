using System;
using System.Collections.Generic;
using System.Linq;

namespace isukces.code.interfaces.Ammy
{
    public interface IConversionCtx
    {
        IAmmyNamespaceProvider NamespaceProvider { get; }
        bool                   FullNamespaces    { get; }
        bool ResolveSeparateLines(string propertyName, IAmmyCodePiece value);
    }

    public static class ConversionCtxExt
    {
        public static string TypeName(this IConversionCtx ctx, Type t)
        {
            if (!ctx.FullNamespaces && ctx.NamespaceProvider.Namespaces.Contains(t.Namespace))
                return t.Name;
            return t.FullName;
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