using System;
using System.Text;
using iSukces.Code.Interfaces;
using JetBrains.Annotations;

namespace iSukces.Code.Irony
{
    public struct TerminalName : IEquatable<TerminalName>, ICsExpression, ITerminalNameSource
    {
        public TerminalName([NotNull] string name)
        {
            Name = name?.Trim();
            if (string.IsNullOrEmpty(Name))
                throw new ArgumentNullException(nameof(name));
        }

        public static TerminalName operator +(TerminalName a, string b) => new TerminalName(a.Name + b);

        public static bool operator ==(TerminalName left, TerminalName right) => left.Equals(right);

        public static bool operator !=(TerminalName left, TerminalName right) => !left.Equals(right);

        public bool Equals(TerminalName other) => Name == other.Name;

        public override bool Equals(object obj) => obj is TerminalName other && Equals(other);

        public string GetCamelTerminalName()
        {
            var s       = new StringBuilder();
            var toUpper = true;
            foreach (var i in Name)
            {
                if (i == '_')
                {
                    toUpper = true;
                    continue;
                }

                s.Append(toUpper ? char.ToUpper(i) : i);
                toUpper = false;
            }

            return s.ToString();
        }

        public string GetCode(ITypeNameResolver resolver) => "__" + Name;

        public override int GetHashCode() => Name != null ? Name.GetHashCode() : 0;

        public TerminalName GetTerminalName() => this;

        public override string ToString() => $"Name = {Name}";

        public string Name { get; }
    }

    public interface ITerminalNameSource
    {
        TerminalName GetTerminalName();
    }
}