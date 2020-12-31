using System;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace iSukces.Code.Irony
{
    public enum SpecialTerminalKind
    {
        CreateCSharpIdentifier,
        CreateCSharpNumber,
        CreateCSharpString
    }

    public static class SpecialTerminalKindExt
    {
        public static Type GetAstClass(this SpecialTerminalKind kind)
        {
            switch (kind)
            {
                case SpecialTerminalKind.CreateCSharpIdentifier:
                    return typeof(IdentifierTerminal);
                case SpecialTerminalKind.CreateCSharpNumber:
                    return typeof(NumberLiteral);
                case SpecialTerminalKind.CreateCSharpString:
                    return typeof(StringLiteral);
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }

        public static Type GetDataClass(this SpecialTerminalKind kind)
        {
            switch (kind)
            {
                case SpecialTerminalKind.CreateCSharpIdentifier:
                    return typeof(string);
                case SpecialTerminalKind.CreateCSharpNumber:
                    return typeof(int);
                case SpecialTerminalKind.CreateCSharpString:
                    return typeof(string);
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }

        public static Type GetNodeType(this SpecialTerminalKind kind)
        {
            switch (kind)
            {
                case SpecialTerminalKind.CreateCSharpIdentifier:
                    return typeof(IdentifierNode);
                case SpecialTerminalKind.CreateCSharpString:
                    return typeof(LiteralValueNode);
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }
    }
}