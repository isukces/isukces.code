using System;
using System.Collections.Generic;
using iSukces.Code.Interfaces;

namespace iSukces.Code
{
    public class CsMethodParameter : IComparable, IAttributable, IAnnotableByUser
    {
        /// <summary>
        ///     Tworzy instancję obiektu
        ///     <param name="name">nazwa parametru</param>
        ///     <param name="type">typ parametru</param>
        ///     <param name="description">Opis</param>
        /// </summary>
        public CsMethodParameter(string name, string type = null, string description = null)
        {
            Name        = name;
            Type        = type;
            Description = description;
        }

        /// <summary>
        ///     Realizuje operator ==
        /// </summary>
        /// <param name="left">lewa strona porównania</param>
        /// <param name="right">prawa strona porównania</param>
        /// <returns><c>true</c> jeśli obiekty są równe</returns>
        public static bool operator ==(CsMethodParameter left, CsMethodParameter right) =>
            ReferenceEquals(left, null)
                ? ReferenceEquals(right, null)
                : !ReferenceEquals(right, null) && left.Name == right.Name && left.Type == right.Type;

        public static bool operator >(CsMethodParameter left, CsMethodParameter right) => left.CompareTo(right) > 0;

        public static bool operator >=(CsMethodParameter left, CsMethodParameter right) => left.CompareTo(right) >= 0;

        /// <summary>
        ///     Realizuje operator !=
        /// </summary>
        /// <param name="left">lewa strona porównania</param>
        /// <param name="right">prawa strona porównania</param>
        /// <returns><c>true</c> jeśli obiekty są różne</returns>
        public static bool operator !=(CsMethodParameter left, CsMethodParameter right) => !(left == right);

        public static bool operator <(CsMethodParameter left, CsMethodParameter right) => left.CompareTo(right) < 0;

        public static bool operator <=(CsMethodParameter left, CsMethodParameter right) => left.CompareTo(right) <= 0;

        /// <summary>
        ///     Realizuje interfejs IComparable
        /// </summary>
        public int CompareTo(CsMethodParameter other)
        {
            // Fail-safe check return
            // see Albahari, Joseph; Ben Albahari (2010-01-20). C# 4.0 in a Nutshell: The Definitive Reference (p. 258).
            if (Equals(other)) return 0;
            var tmp = Name.CompareTo(other.Name);
            return tmp != 0 ? tmp : Type.CompareTo(other.Type);
        }

        /// <summary>
        ///     Sprawdza, czy wskazany obiekt jest równy bieżącemu
        /// </summary>
        /// <param name="other">obiekt do porównania z obiektem bieżącym</param>
        /// <returns><c>true</c> jeśli wskazany obiekt jest równy bieżącemu; w przeciwnym wypadku<c>false</c></returns>
        public bool Equals(CsMethodParameter other) => other == this;

        /// <summary>
        ///     Sprawdza, czy wskazany obiekt jest równy bieżącemu
        /// </summary>
        /// <param name="other">obiekt do porównania z obiektem bieżącym</param>
        /// <returns><c>true</c> jeśli wskazany obiekt jest równy bieżącemu; w przeciwnym wypadku<c>false</c></returns>
        public override bool Equals(object other) => other is CsMethodParameter && Equals((CsMethodParameter)other);

        /// <summary>
        ///     Zwraca kod HASH obiektu
        /// </summary>
        /// <returns>kod HASH obiektu</returns>
        public override int GetHashCode()
        {
            // Good implementation suggested by Josh Bloch
            var hash = 17;
            hash = hash * 31 + Name.GetHashCode();
            hash = hash * 31 + Type.GetHashCode();
            return hash;
        }

        public override string ToString()
        {
            if (IsConst)
                return string.Format("{0} const {1} {2} = {3}", Visibility.ToString().ToLower(), Type, Name,
                    _constValue);
            return string.Format("{0} {1} {2}", Visibility.ToString().ToLower(), Type, Name);
        }

        /// <summary>
        ///     Realizuje interfejs IComparable
        /// </summary>
        int IComparable.CompareTo(object other)
        {
            if (!(other is CsMethodParameter))
                throw new InvalidOperationException("CompareTo: Object is not Parameter");
            return CompareTo((CsMethodParameter)other);
        }

        /// <summary>
        /// </summary>
        public string ConstValue
        {
            get { return _constValue; }
            set { _constValue = value?.Trim(); }
        }

        public bool IsConst { get; set; }

        /// <summary>
        ///     nazwa parametru; własność jest tylko do odczytu.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     typ parametru; własność jest tylko do odczytu.
        /// </summary>
        public string Type { get; } = "string";

        /// <summary>
        /// </summary>
        public ParameterCallTypes CallType { get; set; }

        /// <summary>
        ///     Opis
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value?.Trim() ?? string.Empty; }
        }

        /// <summary>
        ///     atrybuty
        /// </summary>
        public IList<ICsAttribute> Attributes
        {
            get { return _attributes; }
            set { _attributes = value ?? new List<ICsAttribute>(); }
        }

        /// <summary>
        /// </summary>
        public Visibilities Visibility { get; set; } = Visibilities.Public;

        /// <summary>
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// </summary>
        public bool UseThis { get; set; }

        /// <summary>
        /// </summary>
        public bool IsStatic { get; set; }

        public bool                        IsVolatile      { get; set; }
        public IDictionary<string, object> UserAnnotations { get; } = new Dictionary<string, object>();

        private string _constValue = string.Empty;
        private string _description = string.Empty;
        private IList<ICsAttribute> _attributes = new List<ICsAttribute>();
    }

    public static class CsMethodParameterExt
    {
        public static T WithConstValue<T>(this T src, string constValue)
            where T : CsMethodParameter
        {
            src.ConstValue = constValue;
            return src;
        }

        public static T WithConstValueNull<T>(this T src) where T : CsMethodParameter
        {
            src.ConstValue = "null";
            return src;
        }

        public static T WithDescription<T>(this T src, string description) where T : CsMethodParameter
        {
            src.Description = description;
            return src;
        }

        public static T WithIsReadOnly<T>(this T src, bool isReadOnly = true)
            where T : CsMethodParameter
        {
            src.IsReadOnly = isReadOnly;
            return src;
        }

        public static T WithIsVolatile<T>(this T src, bool isVolatile = true)
            where T : CsMethodParameter
        {
            src.IsVolatile = isVolatile;
            return src;
        }
    }
}