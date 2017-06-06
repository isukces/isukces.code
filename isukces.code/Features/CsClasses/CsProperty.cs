using System.Linq;
using isukces.code.interfaces;

namespace isukces.code
{
    public class CsProperty : CsMethodParameter, ICsClassMember
    {
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

        public CodeLines GetGetterLines(bool allowExpressionBodies)
        {
            var tmp = string.IsNullOrEmpty(OwnGetter)
                ? new CodeLines(new[] { string.Format("{0};", PropertyFieldName) }, true)
                : new CodeLines(OwnGetter.Split('\r', '\n'), OwnGetterIsExpression);
            if (allowExpressionBodies)
                return tmp;
            return tmp.MakeReturnNoExpressionBody();
        }

        public CodeLines GetSetterLines(bool allowExpressionBodied)
        {
            return string.IsNullOrEmpty(OwnSetter)
                ? new CodeLines(new[] { string.Format("{0} = value;", PropertyFieldName) })
                : new CodeLines(OwnSetter.Split('\r', '\n'), OwnSetterIsExpression);
        }

        /// <summary>
        ///     Zwraca tekstową reprezentację obiektu
        /// </summary>
        /// <returns>Tekstowa reprezentacja obiektu</returns>
        public override string ToString()
        {
            return string.Format("property {0} {1}", Name, Type);
        }

        /// <summary>
        /// </summary>
        public bool IsPropertyReadOnly { get; set; }

        /// <summary>
        /// </summary>
        public string OwnGetter
        {
            get => _ownGetter;
            set => _ownGetter = value?.Trim() ?? string.Empty;
        }

        /// <summary>
        /// </summary>
        public string OwnSetter
        {
            get => _ownSetter;
            set => _ownSetter = value?.Trim() ?? string.Empty;
        }

        public bool OwnGetterIsExpression { get; set; }
        public bool OwnSetterIsExpression { get; set; }

        /// <summary>
        ///     nazwa zmiennej dla własności; własność jest tylko do odczytu.
        /// </summary>
        public string PropertyFieldName => Name.PropertyBackingFieldName();

        /// <summary>
        /// </summary>
        public bool EmitField { get; set; } = true;

        public bool IsVirtual { get; set; }

        /// <summary>
        /// </summary>
        public bool MakeAutoImplementIfPossible { get; set; }

        public Visibilities? SetterVisibility { get; set; }
        public Visibilities? GetterVisibility { get; set; }
        public Visibilities FieldVisibility { get; set; } = Visibilities.Private;


        private string _ownGetter = string.Empty;
        private string _ownSetter = string.Empty;
    }

    public class CodeLines
    {
        public CodeLines(string[] lines, bool isExpressionBody = false)
        {
            Lines = lines?.Where(a => a != null && a.Trim() != "").ToArray();
            IsExpressionBody = isExpressionBody;
        }

        public string[] Lines { get; set; }
        public bool IsExpressionBody { get; set; }

        public CodeLines MakeReturnNoExpressionBody()
        {
            if (!IsExpressionBody || Lines == null || Lines.Length == 0)
                return this;
            Lines[0] = "return " + Lines[0];
            IsExpressionBody = false;
            return this;

        }
    }
}