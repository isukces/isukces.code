using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iSukces.Code.Interfaces;
using JetBrains.Annotations;

namespace iSukces.Code
{
    public partial class CsMethod : ClassMemberBase, ICommentable, IAnnotableByUser, IGenericDefinition
    {
        static CsMethod()
        {
            operators = new HashSet<string>();
            const string tmp = "+, -, !, ~, ++, --, +, -, *, /, %, &, |, ^, <<, >>,==, !=, <, >, <=, >=,&&, ||";
            foreach (var i in tmp.Split(','))
                operators.Add(i.Trim());
        }

        /// <summary>
        ///     Tworzy instancję obiektu
        /// </summary>
        public CsMethod()
        {
        }

        /// <summary>
        ///     Tworzy instancję obiektu
        ///     <param name="name">Nazwa metody</param>
        /// </summary>
        public CsMethod(string name) => Name = name;


        /// <summary>
        ///     Tworzy instancję obiektu
        ///     <param name="name">Nazwa metody</param>
        ///     <param name="resultType"></param>
        /// </summary>
        public CsMethod(string name, string resultType)
        {
            Name       = name;
            ResultType = resultType;
            if (IsOperator(name))
            {
                Kind     = MethodKind.Operator;
                IsStatic = true;
            }
        }

        public static bool IsOperator(string name) => operators.Contains(name);

        private static string FormatMethodParameter(CsMethodParameter i)
        {
            var sb = new StringBuilder();
            if (i.Attributes.Any())
            {
                var joioned = string.Join(", ", i.Attributes);
                sb.Append($"[{joioned}] ");
            }

            if (i.UseThis)
                sb.Append("this ");
            switch (i.CallType)
            {
                case ParameterCallTypes.Output:
                    sb.Append("out ");
                    break;
                case ParameterCallTypes.Reference:
                    sb.Append("ref ");
                    break;
            }

            sb.AppendFormat("{0} {1}", i.Type, i.Name);
            if (!string.IsNullOrEmpty(i.ConstValue))
                sb.Append(" = " + i.ConstValue);
            return sb.ToString();
        }


        public void AddComment(string x)
        {
            _extraComment.AppendLine(x);
        }

        public CsMethodParameter AddParam(string name, string type, string description = null)
        {
            var parameter = new CsMethodParameter(name, type, description);
            _parameters.Add(parameter);
            return parameter;
        }

        public CsMethodParameter AddParam<T>(string name, CsClass owner, string description = null) =>
            AddParam(name, typeof(T), owner, description);

        public CsMethodParameter AddParam(string name, Type type, CsClass owner, string description = null)
        {
            var parameter = new CsMethodParameter(name, owner.GetTypeName(type), description);
            _parameters.Add(parameter);
            return parameter;
        }

        public string GetComments() => _extraComment.ToString();

        /// <summary>
        ///     Tworzy kod
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="inInterface"></param>
        /// <param name="typeNameResolver"></param>
        /// <returns></returns>
        public void MakeCode(ICsCodeWriter writer, bool inInterface, ITypeNameResolver typeNameResolver)
        {
            writer.OpenCompilerIf(CompilerDirective);
            Check();
            WriteMethodDescription(writer);
            foreach (var i in Attributes)
                writer.WriteLine("[{0}]", i);
            // ================
            writer.WriteComment(this);
            writer.SplitWriteLine(AdditionalContentOverMethod);
            var query = from i in _parameters
                select FormatMethodParameter(i);
            var mDefinition = string.Format("{0}{2}({1})",
                string.Join(" ", GetMethodAttributes(inInterface)),
                string.Join(", ", query),
                GenericArguments.GetTriangleBracketsInfo());
            if (inInterface)
            {
                if (Kind != MethodKind.Normal || IsStatic)
                    return;
                if (GenericArguments.HasConstraints())
                {
                    writer.WriteLine(mDefinition);
                    GenericArguments?.WriteCode(writer, true, typeNameResolver);
                }
                else
                {
                    writer.WriteLine(mDefinition + ";");
                }

                return;
            }

            if (Overriding == OverridingType.Abstract && Kind == MethodKind.Normal)
            {
                writer.WriteLine(mDefinition + ";");
                return;
            }

            if (Kind == MethodKind.Constructor)
            {
                writer.OpenConstructor(mDefinition, _baseConstructorCall);
            }
            else
            {
                writer.WriteLine(mDefinition);
                if (GenericArguments.HasConstraints())
                    GenericArguments?.WriteCode(writer, false, typeNameResolver);
                writer.WriteLine(writer.LangInfo.OpenText);
                writer.IncIndent();
            }

            writer.SplitWriteLine(_body);
            writer.Close();
            writer.CloseCompilerIf(CompilerDirective);
        }

        private void Check()
        {
            if (GenericArguments != null)
            {
                if (Kind == MethodKind.Constructor)
                    throw new Exception("Construction can't have generic arguments");
                if (Kind == MethodKind.Finalizer)
                    throw new Exception("Finalizer can't have generic arguments");
            }

            var g = Kind.GetStaticInstanceStatus();
            switch (g)
            {
                case StaticInstanceStatus.Instance
                    when IsStatic:
                    throw new Exception("Method marked as " + Kind + " can't be static");
                case StaticInstanceStatus.Static
                    when !IsStatic:
                    throw new Exception("Method marked as " + Kind + " have to be static");
            }

            if (Kind == MethodKind.Constructor || Kind == MethodKind.Finalizer)
                if (Overriding != OverridingType.None)
                    throw new Exception("Constructor nor finalizer can't be " + Overriding);
        }


        private string[] GetMethodAttributes(bool inInterface)
        {
            var a = new List<string>();
            if (Name == Implicit || Name == Explicit)
            {
                //  public static implicit operator double(Force src)
                if (Visibility != Visibilities.InterfaceDefault)
                    a.Add(Visibility.ToString().ToLower());
                a.Add("static");
                a.Add(Name);
                a.Add("operator");
                a.Add(_resultType);
                return a.ToArray();
            }

            if (IsOperator(Name))
            {
                //  public static Meter operator +(Meter a, Meter b)
                // public static Fraction operator +(Fraction a, Fraction b)
                if (Visibility != Visibilities.InterfaceDefault)
                    a.Add(Visibility.ToString().ToLower());
                a.Add("static");
                a.Add(_resultType);
                a.Add("operator");
                a.Add(Name);
                return a.ToArray();
            }

            bool EmitVisibility()
            {
                if (Visibility == Visibilities.InterfaceDefault)
                    return false;
                if (Kind == MethodKind.Finalizer)
                    return false;
                if (Kind == MethodKind.Constructor)
                    return !IsStatic;
                return !inInterface;
            }

            if (EmitVisibility())
                a.Add(Visibility.ToString().ToLower());
            if (IsStatic)
                a.Add("static");
            if (Kind == MethodKind.Normal)
            {
                if (!inInterface)
                    switch (Overriding)
                    {
                        case OverridingType.None:
                            break;
                        case OverridingType.Virtual:
                            a.Add("virtual");
                            break;
                        case OverridingType.Abstract:
                            a.Add("abstract");
                            break;
                        case OverridingType.Override:
                            a.Add("override");
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                a.Add(_resultType);
            }

            a.Add(_name);
            return a.ToArray();
        }

        private void WriteMethodDescription(ICsCodeWriter writer)
        {
            var anyParameterHasDescription = _parameters.Any(a => !string.IsNullOrEmpty(a.Description));
            var hasMethodDescription       = !string.IsNullOrEmpty(Description);
            if (!hasMethodDescription && !anyParameterHasDescription) return;
            if (hasMethodDescription)
            {
                writer.WriteLine("/// <summary>");
                var lines = Description.Replace("\r\n", "\n").Split('\r', '\n');
                foreach (var i in lines)
                    writer.WriteLine("/// " + i.XmlEncode());
                writer.WriteLine("/// </summary>");
            }

            foreach (var i in _parameters)
                writer.WriteLine("/// <param name=\"{0}\">{1}</param>", i.Name.XmlEncode(),
                    i.Description.XmlEncode());
        }

        /// <summary>
        ///     Nazwa metody
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                value = value?.Trim() ?? string.Empty;
                if (_name == value)
                    return;
                _name = value;
                if (_name == Implicit)
                {
                    Kind       = MethodKind.Implicit;
                    IsStatic   = true;
                    Overriding = OverridingType.None;
                }
                else if (_name == Explicit)
                {
                    Kind       = MethodKind.Explicit;
                    Overriding = OverridingType.None;
                    IsStatic   = true;
                }
                else if (IsOperator(_name))
                {
                    Kind       = MethodKind.Operator;
                    Overriding = OverridingType.None;
                    IsStatic   = true;
                }
            }
        }


        /// <summary>
        /// </summary>
        public string ResultType
        {
            get => _resultType;
            set => _resultType = value?.Trim() ?? string.Empty;
        }

        /// <summary>
        /// </summary>
        public List<CsMethodParameter> Parameters
        {
            get => _parameters;
            set => _parameters = value ?? new List<CsMethodParameter>();
        }


        public OverridingType Overriding { get; set; }


        /// <summary>
        /// </summary>
        public string Body
        {
            get => _body;
            set => _body = value?.Trim() ?? string.Empty;
        }

        /// <summary>
        ///     wywołanie kontruktora bazowego
        /// </summary>
        public string BaseConstructorCall
        {
            get => _baseConstructorCall;
            set => _baseConstructorCall = value?.Trim() ?? string.Empty;
        }

        [CanBeNull]
        public CsGenericArguments GenericArguments { get; set; }


        public IDictionary<string, object> UserAnnotations { get; } = new Dictionary<string, object>();


        public MethodKind Kind
        {
            get => _kind;
            set
            {
                if (_kind == value)
                    return;
                _kind = value;
                switch (value)
                {
                    case MethodKind.Explicit:
                        Name       = Explicit;
                        Overriding = OverridingType.None;
                        break;
                    case MethodKind.Implicit:
                        Name       = Implicit;
                        Overriding = OverridingType.None;
                        break;
                }

                var s = Kind.GetStaticInstanceStatus();
                switch (s)
                {
                    case StaticInstanceStatus.Instance:
                        IsStatic = false;
                        break;
                    case StaticInstanceStatus.Static:
                        IsStatic = true;
                        break;
                }
            }
        }

        private static readonly HashSet<string> operators;

        public static string Implicit = "implicit";
        public static string Explicit = "explicit";
        private readonly StringBuilder _extraComment = new StringBuilder();

        private string _name = string.Empty;
        private string _resultType = "void";
        private List<CsMethodParameter> _parameters = new List<CsMethodParameter>();
        private string _body = string.Empty;
        private string _baseConstructorCall = string.Empty;
        private MethodKind _kind;
    }
}