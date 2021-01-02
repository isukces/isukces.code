using System;
using iSukces.Code.Interfaces;
using JetBrains.Annotations;

namespace iSukces.Code.Irony
{
    public struct TokenName : IEquatable<TokenName>, ICsExpression, ITokenNameSource
    {
        public TokenName([NotNull] string name)
        {
            Name = name?.Trim();
            if (string.IsNullOrEmpty(Name))
                throw new ArgumentNullException(nameof(name));
        }

        public static TokenName operator +(TokenName a, string b) => new TokenName(a.Name + b);

        public static bool operator ==(TokenName left, TokenName right) => left.Equals(right);

        public static bool operator !=(TokenName left, TokenName right) => !left.Equals(right);

        public bool Equals(TokenName other) => Name == other.Name;

        public override bool Equals(object obj) => obj is TokenName other && Equals(other);

        public string GetCamelTerminalName() => CSharpExtension.GetCamelTerminalName(Name);


        public string GetCode(ITypeNameResolver resolver) => "__" + Name;

        public override int GetHashCode() => Name != null ? Name.GetHashCode() : 0;

        public TokenName GetTokenName() => this;
        public TokenNameTarget GetTokenNameIsNonterminal() => TokenNameTarget.Unknown;

        public override string ToString() => $"Name = {Name}";

        public string Name { get; }
    }

    public interface ITokenNameSource
    {
        TokenName GetTokenName();
        TokenNameTarget GetTokenNameIsNonterminal();
    }

    public enum TokenNameTarget : byte
    {
        Unknown,
        Nonterminal,
        Terminal
    }
}