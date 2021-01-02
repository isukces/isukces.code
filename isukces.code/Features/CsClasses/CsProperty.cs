using System.Linq;
using System.Text;
using iSukces.Code.Interfaces;

namespace iSukces.Code
{
    public class CsProperty : CsMethodParameter, ICsClassMember, ICommentable
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

        public void AddComment(string x)
        {
            _extraComment.AppendLine(x);
        }
        
        public string GetComments() => _extraComment.ToString();

        public CodeLines GetGetterLines(bool allowExpressionBodies)
        {
            var tmp = string.IsNullOrEmpty(OwnGetter)
                ? new CodeLines(new[] {string.Format("{0};", PropertyFieldName)}, true)
                : new CodeLines(OwnGetter.Split('\r', '\n'), OwnGetterIsExpression);
            if (allowExpressionBodies)
                return tmp;
            return tmp.MakeReturnNoExpressionBody();
        }

        public CodeLines GetSetterLines(bool allowExpressionBodies)
        {
            var tmp = string.IsNullOrEmpty(OwnSetter)
                ? new CodeLines(new[] {string.Format("{0} = value;", PropertyFieldName)}, allowExpressionBodies)
                : new CodeLines(OwnSetter.Split('\r', '\n'), OwnSetterIsExpression && allowExpressionBodies);
            return tmp;
        }

        /// <summary>
        ///     Zwraca tekstową reprezentację obiektu
        /// </summary>
        /// <returns>Tekstowa reprezentacja obiektu</returns>
        public override string ToString() => string.Format("property {0} {1}", Name, Type);

        public CsProperty WithIsPropertyReadOnly(bool isPropertyReadOnly = true)
        {
            IsPropertyReadOnly = isPropertyReadOnly;
            return this;
        }

        public CsProperty WithMakeAutoImplementIfPossible(bool value = true)
        {
            MakeAutoImplementIfPossible = value;
            return this;
        }

        public CsProperty WithNoEmitField()
        {
            EmitField = false;
            return this;
        }

        public CsProperty WithOwnGetter(string ownGetter)
        {
            OwnGetter = ownGetter;
            return this;
        }

        /// <summary>
        /// </summary>
        public bool IsPropertyReadOnly { get; set; }

        /// <summary>
        /// </summary>
        public string OwnGetter
        {
            get { return _ownGetter; }
            set { _ownGetter = value?.Trim() ?? string.Empty; }
        }

        /// <summary>
        /// </summary>
        public string OwnSetter
        {
            get { return _ownSetter; }
            set { _ownSetter = value?.Trim() ?? string.Empty; }
        }

        public bool OwnGetterIsExpression { get; set; }
        public bool OwnSetterIsExpression { get; set; }

        /// <summary>
        ///     nazwa zmiennej dla własności; własność jest tylko do odczytu.
        /// </summary>
        public string PropertyFieldName
        {
            get { return Name.PropertyBackingFieldName(); }
        }

        /// <summary>
        /// </summary>
        public bool EmitField { get; set; } = true;

        public bool IsVirtual { get; set; }

        public bool IsOverride { get; set; }

        /// <summary>
        /// </summary>
        public bool MakeAutoImplementIfPossible { get; set; }

        public Visibilities? SetterVisibility { get; set; }
        public Visibilities? GetterVisibility { get; set; }
        public Visibilities  FieldVisibility  { get; set; } = Visibilities.Private;

        public string CompilerDirective { get; set; }

        private readonly StringBuilder _extraComment = new StringBuilder();


        private string _ownGetter = string.Empty;
        private string _ownSetter = string.Empty;
    }

    public class CodeLines
    {
        public CodeLines(string[] lines, bool isExpressionBody = false)
        {
            Lines            = lines?.Where(a => a != null && a.Trim() != "").ToArray();
            IsExpressionBody = isExpressionBody;
        }

        public CodeLines MakeReturnNoExpressionBody()
        {
            if (!IsExpressionBody || Lines == null || Lines.Length == 0)
                return this;
            Lines[0]         = "return " + Lines[0].TrimEnd(' ', ';') + ";";
            IsExpressionBody = false;
            return this;
        }


        public override string ToString()
        {
            if (IsExpressionBody)
                return "=>" + string.Join("\r\n", Lines);
            return string.Join("\r\n", Lines);
        }

        public string[] Lines            { get; set; }
        public bool     IsExpressionBody { get; set; }
    }
}