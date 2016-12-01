using System.Collections.Generic;
using System.Linq;
using isukces.code.interfaces;

namespace isukces.code
{
    public class CsProperty : CsMethodParameter, ICsClassMember
    {
        #region Constructors

        /// <summary>
        ///     Tworzy instancję obiektu
        ///     <param name="name">nazwa parametru</param>
        /// </summary>
        public CsProperty(string name)
            : base(name)
        {
        }

        /// <summary>
        ///     Tworzy instancję obiektu
        ///     <param name="name">nazwa parametru</param>
        ///     <param name="type">typ parametru</param>
        /// </summary>
        public CsProperty(string name, string type)
            : base(name, type)
        {
        }

        /// <summary>
        ///     Tworzy instancję obiektu
        ///     <param name="name">nazwa parametru</param>
        ///     <param name="type">typ parametru</param>
        ///     <param name="description">Opis</param>
        /// </summary>
        public CsProperty(string name, string type, string description)
            : base(name, type, description)
        {
        }

        #endregion

        #region Instance Methods

        /// <summary>
        ///     Zwraca tekstową reprezentację obiektu
        /// </summary>
        /// <returns>Tekstowa reprezentacja obiektu</returns>
        public override string ToString()
        {
            return string.Format("property {0} {1}", Name, Type);
        }

        #endregion

        #region Properties

        /// <summary>
        /// </summary>
        public bool IsPropertyReadOnly { get; set; }

        /// <summary>
        /// </summary>
        public string OwnGetter
        {
            get { return _ownGetter; }
            set
            {
                value = value?.Trim() ?? string.Empty;
                _ownGetter = value;
            }
        }

        /// <summary>
        /// </summary>
        public string OwnSetter
        {
            get { return _ownSetter; }
            set
            {
                value = value?.Trim() ?? string.Empty;
                _ownSetter = value;
            }
        }

        /// <summary>
        ///     nazwa zmiennej dla własności; własność jest tylko do odczytu.
        /// </summary>
        public string PropertyFieldName => Name.PropertyBackingFieldName();

        /// <summary>
        /// </summary>
        public bool EmitField { get; set; } = true;

        /// <summary>
        /// </summary>
        public bool MakeAutoImplementIfPossible { get; set; }

        public Visibilities? SetterVisibility { get; set; }
        public Visibilities? GetterVisibility { get; set; }
        public Visibilities FieldVisibility { get; set; } = Visibilities.Private;



        #endregion

        #region Fields

        private string _ownGetter = string.Empty;
        private string _ownSetter = string.Empty;

        #endregion

        public string[] GetSetterLines()
        {
            return string.IsNullOrEmpty(OwnSetter)
                ? new[] { string.Format("{0} = value;", PropertyFieldName) }
                : OwnSetter.Split('\r', '\n').Where(ii => ii.Trim() != "").ToArray();
        }

        public string[] GetGetterLines()
        {
            return string.IsNullOrEmpty(OwnGetter)
                ? new[] { string.Format("return {0};", PropertyFieldName) }
                : OwnGetter.Split('\r', '\n').Where(ii => ii.Trim() != "").ToArray();
        }
    }
}