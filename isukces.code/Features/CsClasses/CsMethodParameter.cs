﻿#region using

using System;
using System.Collections.Generic;
using isukces.code.interfaces;

#endregion

namespace isukces.code
{
    public class CsMethodParameter : IComparable, IAttributable
    {
        #region Constructors

        /// <summary>
        ///     Tworzy instancję obiektu
        ///     <param name="name">nazwa parametru</param>
        /// </summary>
        public CsMethodParameter(string name)
        {
            Name = name;
        }

        /// <summary>
        ///     Tworzy instancję obiektu
        ///     <param name="name">nazwa parametru</param>
        ///     <param name="type">typ parametru</param>
        /// </summary>
        public CsMethodParameter(string name, string type)
        {
            Name = name;
            Type = type;
        }

        /// <summary>
        ///     Tworzy instancję obiektu
        ///     <param name="name">nazwa parametru</param>
        ///     <param name="type">typ parametru</param>
        ///     <param name="description">Opis</param>
        /// </summary>
        public CsMethodParameter(string name, string type, string description)
        {
            Name = name;
            Type = type;
            Description = description;
        }

        #endregion

        #region Static Methods

        /// <summary>
        ///     Realizuje operator ==
        /// </summary>
        /// <param name="left">lewa strona porównania</param>
        /// <param name="right">prawa strona porównania</param>
        /// <returns><c>true</c> jeśli obiekty są równe</returns>
        public static bool operator ==(CsMethodParameter left, CsMethodParameter right)
        {
            return ReferenceEquals(left, null)
                ? ReferenceEquals(right, null)
                : !ReferenceEquals(right, null) && (left.Name == right.Name) && (left.Type == right.Type);
        }

        public static bool operator >(CsMethodParameter left, CsMethodParameter right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(CsMethodParameter left, CsMethodParameter right)
        {
            return left.CompareTo(right) >= 0;
        }

        /// <summary>
        ///     Realizuje operator !=
        /// </summary>
        /// <param name="left">lewa strona porównania</param>
        /// <param name="right">prawa strona porównania</param>
        /// <returns><c>true</c> jeśli obiekty są różne</returns>
        public static bool operator !=(CsMethodParameter left, CsMethodParameter right)
        {
            return !(left == right);
        }

        public static bool operator <(CsMethodParameter left, CsMethodParameter right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(CsMethodParameter left, CsMethodParameter right)
        {
            return left.CompareTo(right) <= 0;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        ///     Realizuje interfejs IComparable
        /// </summary>
        public int CompareTo(CsMethodParameter other)
        {
            // Fail-safe check return
            // see Albahari, Joseph; Ben Albahari (2010-01-20). C# 4.0 in a Nutshell: The Definitive Reference (p. 258).
            if (Equals(other)) return 0;
            var _tmp_ = Name.CompareTo(other.Name);
            if (_tmp_ != 0) return _tmp_;
            return Type.CompareTo(other.Type);
        }


        /// <summary>
        ///     Sprawdza, czy wskazany obiekt jest równy bieżącemu
        /// </summary>
        /// <param name="other">obiekt do porównania z obiektem bieżącym</param>
        /// <returns><c>true</c> jeśli wskazany obiekt jest równy bieżącemu; w przeciwnym wypadku<c>false</c></returns>
        public bool Equals(CsMethodParameter other)
        {
            return other == this;
        }

        /// <summary>
        ///     Sprawdza, czy wskazany obiekt jest równy bieżącemu
        /// </summary>
        /// <param name="other">obiekt do porównania z obiektem bieżącym</param>
        /// <returns><c>true</c> jeśli wskazany obiekt jest równy bieżącemu; w przeciwnym wypadku<c>false</c></returns>
        public override bool Equals(object other)
        {
            return other is CsMethodParameter && Equals((CsMethodParameter)other);
        }

        /// <summary>
        ///     Zwraca kod HASH obiektu
        /// </summary>
        /// <returns>kod HASH obiektu</returns>
        public override int GetHashCode()
        {
            // Good implementation suggested by Josh Bloch
            var _hash_ = 17;
            _hash_ = _hash_ * 31 + Name.GetHashCode();
            _hash_ = _hash_ * 31 + Type.GetHashCode();
            return _hash_;
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

        #endregion

        #region Properties

        /// <summary>
        /// </summary>
        public string ConstValue
        {
            get { return _constValue; }
            set
            {
                if (value != null)
                    value = value.Trim();
                _constValue = value;
            }
        }

        public bool IsConst { get; set; }

        /// <summary>
        ///     nazwa parametru; własność jest tylko do odczytu.
        /// </summary>
        public string Name { get; } = string.Empty;

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
            set
            {
                value = value?.Trim() ?? string.Empty;
                _description = value;
            }
        }

        /// <summary>
        ///     atrybuty
        /// </summary>
        public IList<ICsAttribute> Attributes
        {
            get { return _attributes; }
            set
            {
                if (value == null) value = new List<ICsAttribute>();
                _attributes = value;
            }
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

        public bool IsVolatile { get; set; }

        #endregion

        #region Fields

        private string _constValue = string.Empty;
        private string _description = string.Empty;
        private IList<ICsAttribute> _attributes = new List<ICsAttribute>();

        #endregion
    }
}