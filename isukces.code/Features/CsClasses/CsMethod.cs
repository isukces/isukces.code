using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using isukces.code.interfaces;

namespace isukces.code
{
    public class CsMethod : ClassMemberBase
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
        public CsMethod(string name)
        {
            Name = name;
        }


        /// <summary>
        ///     Tworzy instancję obiektu
        ///     <param name="name">Nazwa metody</param>
        ///     <param name="resultType"></param>
        /// </summary>
        public CsMethod(string name, string resultType)
        {
            Name       = name;
            ResultType = resultType;
        }

        public static bool IsOperator(string name)
        {
            return operators.Contains(name);
        }

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
            sb.AppendFormat("{0} {1}", i.Type, i.Name);
            if (!string.IsNullOrEmpty(i.ConstValue))
                sb.Append(" = " + i.ConstValue);
            return sb.ToString();
        }

        public CsMethodParameter AddParam(string name, string type, string description = null)
        {
            var parameter = new CsMethodParameter(name, type, description);
            _parameters.Add(parameter);
            return parameter;
        }

        public CsMethodParameter AddParam<T>(string name, CsClass owner, string description = null)
        {
            return AddParam(name, typeof(T), owner, description);
        }

        public CsMethodParameter AddParam(string name, Type type, CsClass owner, string description = null)
        {
            var parameter = new CsMethodParameter(name, owner.TypeName(type), description);
            _parameters.Add(parameter);
            return parameter;
        }

        /// <summary>
        ///     Tworzy kod
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public void MakeCode(ICsCodeWriter writer, bool inInterface)
        {
            WriteMethodDescription(writer);
            foreach (var i in Attributes)
                writer.WriteLine("[{0}]", i);
            // ================
            writer.SplitWriteLine(AdditionalContentOverMethod);
            var query = from i in _parameters
                select FormatMethodParameter(i);
            var mDefinition = string.Format("{0}({1})",
                string.Join(" ", GetMethodAttributes(inInterface)),
                string.Join(", ", query));
            if (inInterface)
            {
                if (IsConstructor || IsStatic)
                    return;
                writer.WriteLine(mDefinition + ";");
                return;
            }

            if (IsAbstract && !IsConstructor)
            {
                writer.WriteLine(mDefinition + ";");
                return;
            }

            if (IsConstructor)
                writer.OpenConstructor(mDefinition, _baseConstructorCall);
            else
                writer.Open(mDefinition);

            writer.SplitWriteLine(_body);
            writer.Close();            
        }

        public CsMethod WithAutocodeGeneratedAttribute(CsClass csClass)
        {
            return this.WithAttribute(csClass, typeof(AutocodeGeneratedAttribute));
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

            if (!(IsConstructor && IsStatic))
                if (!inInterface)
                    if (Visibility != Visibilities.InterfaceDefault)
                        a.Add(Visibility.ToString().ToLower());
            if (IsStatic)
                a.Add("static");
            if (!IsConstructor)
            {
                if (!inInterface)
                {
                    if (IsAbstract)
                        a.Add("abstract");
                    if (IsOverride)
                        a.Add("override");
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
            set => _name = value?.Trim() ?? string.Empty;
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

        /// <summary>
        /// </summary>
        public bool IsAbstract { get; set; }

        /// <summary>
        /// </summary>
        public bool IsOverride { get; set; }

        /// <summary>
        ///     Czy konstruktor
        /// </summary>
        public bool IsConstructor { get; set; }

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
        
        public string AdditionalContentOverMethod { get; set; }

        private static readonly HashSet<string> operators;

        public static string Implicit = "implicit";
        public static string Explicit = "explicit";

        private string _name = string.Empty;
        private string _resultType = "void";
        private List<CsMethodParameter> _parameters = new List<CsMethodParameter>();
        private string _body = string.Empty;
        private string _baseConstructorCall = string.Empty;
    }

    /*
    public enum MethodKind
    {
        Normal,
        Constructor,
        Operator,
        // np.  public static implicit operator double(Force src)
        Implicit,
        Explicit
    }
    */
}