using System;
using System.Text;
using iSukces.Code.Interfaces;
using iSukces.Code.Irony._codeSrc;
using JetBrains.Annotations;

namespace iSukces.Code.Irony
{
    public struct TerminalName : IEquatable<TerminalName>, ICsExpression
    {
        public TerminalName([NotNull] string name)
        {
            Name = name?.Trim();
            if (string.IsNullOrEmpty(Name))
                throw new ArgumentNullException(nameof(name));
        }

        public static TerminalName operator +(TerminalName a, string b)
        {
            return new TerminalName(a.Name + b);
        }

        public static bool operator ==(TerminalName left, TerminalName right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TerminalName left, TerminalName right)
        {
            return !left.Equals(right);
        }

        public bool Equals(TerminalName other)
        {
            return Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            return obj is TerminalName other && Equals(other);
        }

        public StringBuilder GetCamelTerminalName()
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

            return s;
        }

        public string GetCode(ITypeNameResolver resolver)
        {
            return "__" + Name;
        }

        public override int GetHashCode()
        {
            return Name != null ? Name.GetHashCode() : 0;
        }

        public string Name { get; }
    }
}