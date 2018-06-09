using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using isukces.code.AutoCode;
using isukces.code.CodeWrite;
using isukces.code.interfaces;

// ReSharper disable once CheckNamespace
namespace isukces.code
{
    public class CsClass : ClassMemberBase, IClassOwner
    {
        /// <summary>
        ///     Tworzy instancję obiektu
        ///     <param name="name">Nazwa klasy</param>
        /// </summary>
        public CsClass(string name)
        {
            Name = name;
        }

        /// <summary>
        ///     Tworzy instancję obiektu
        ///     <param name="name">Nazwa klasy</param>
        ///     <param name="baseClass">klasa Bazowa</param>
        /// </summary>
        public CsClass(string name, string baseClass)
        {
            Name      = name;
            BaseClass = baseClass;
        }

        public static CsAttribute MkAttribute(string attributeName)
        {
            return new CsAttribute(attributeName);
        }

        private static string OptionalVisibility(Visibilities? memberVisibility)
        {
            var v = memberVisibility == null ? "" : memberVisibility.Value.ToString().ToLower() + " ";
            return v;
        }

        private static string VisibilityToString(Visibilities visibility)
        {
            switch (visibility)
            {
                case Visibilities.Public:
                    return "public";
                case Visibilities.Protected:
                    return "protected";
                case Visibilities.Private:
                    return "private";
                case Visibilities.InterfaceDefault:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void WriteAttributes(ICodeWriter writer, ICollection<ICsAttribute> attributes)
        {
            if (attributes == null || attributes.Count == 0)
                return;
            foreach (var j in attributes)
                writer.WriteLine("[{0}]", j.Code);
        }

        private static void WriteGetterOrSetter(ICodeWriter writer, CodeLines code, string keyWord,
            Visibilities?                                   memberVisibility)
        {
            if (code?.Lines == null || code.Lines.Length == 0) return;
            if (code.IsExpressionBody)
            {
                if (code.Lines.Length == 1)
                {
                    writer.WriteLine("{0}{1} => {2}", OptionalVisibility(memberVisibility), keyWord, code.Lines[0]);
                }
                else
                {
                    writer.Indent++;
                    foreach (var iii in code.Lines)
                        writer.WriteLine(iii);
                    writer.Indent--;
                }
            }
            else
            {
                if (code.Lines.Length == 1)
                {
                    writer.WriteLine("{0}{1} {{ {2} }}", OptionalVisibility(memberVisibility), keyWord,
                        code.Lines[0]?.Trim());
                }
                else
                {
                    writer.Open(keyWord);
                    foreach (var iii in code.Lines)
                        writer.WriteLine(iii);
                    writer.Close();
                }
            }
        }

        private static void WriteSummary(ICodeWriter writer, string description)
        {
            description = description?.Trim();
            if (string.IsNullOrEmpty(description)) return;
            writer.WriteLine("/// <summary>");
            var lines = description.Split('\r', '\n').Where(q => !string.IsNullOrEmpty(q?.Trim()));
            foreach (var line in lines)
                writer.WriteLine("/// " + line.XmlEncode());
            writer.WriteLine("/// </summary>");
        }

        // Public Methods 

        public CsMethodParameter AddConst(string name, string type, string encodedValue)
        {
            var constValue = new CsMethodParameter(name, type)
            {
                ConstValue = encodedValue,
                IsConst    = true
            };
            Fields.Add(constValue);
            return constValue;
        }

        public CsMethodParameter AddConstInt(string name, int encodedValue)
        {
            return AddConst(name, "int", encodedValue.ToString(CultureInfo.InvariantCulture));
        }

        public CsMethodParameter AddConstString(string name, string encodedValue)
        {
            encodedValue = encodedValue == null ? "null" : encodedValue.CsCite();
            return AddConst(name, "string", encodedValue);
        }

        public CsMethodParameter AddField(string fieldName, Type type)
        {
            return AddField(fieldName, TypeName(type));
        }

        public CsMethodParameter AddField(string fieldName, string type)
        {
            var field = new CsMethodParameter(fieldName, type);
            Fields.Add(field);
            return field;
        }
        
        public CsMethod AddConstructor(string description = null)
        {
            var n = _name.Split('<')[0].Trim();            
            var m    = new CsMethod(n, Name)
            {
                Description = description
            };
            _methods.Add(m);
            m.IsConstructor = true;
            return m;
        }

        public CsMethod AddMethod(string name, string type, string description = null)
        {
            var isConstructor = string.IsNullOrEmpty(name) || name == _name ;
            if (isConstructor)
                name = _name;
            var m = new CsMethod(name, type)
            {
                Description = description
            };
            _methods.Add(m);
            m.IsConstructor = isConstructor;
            return m;
        }

        public CsProperty AddProperty(string propertyName, Type type)
        {
            return AddProperty(propertyName, TypeName(type));
        }

        public CsProperty AddProperty(string propertyName, string type)
        {
            var property = new CsProperty(propertyName, type);
            Properties.Add(property);
            return property;
        }

        public ISet<string> GetNamespaces(bool withParent)
        {
            var parentNamespaces = Owner?.GetNamespaces(true);
            var appendNamespace  = DotNetType?.Namespace;
            var append2          = string.IsNullOrEmpty(appendNamespace) ? null : new[] {appendNamespace};
            var copy             = GeneratorsHelper.MakeCopy(parentNamespaces, append2);
            return copy;
        }

        public CsClass GetOrCreateNested(string typeName)
        {
            var existing = _nestedClasses
                .FirstOrDefault(csClass => csClass.Name == typeName);
            if (existing != null) return existing;
            existing = new CsClass(typeName)
            {
                Owner = this
            };
            _nestedClasses.Add(existing);
            return existing;
        }

        public void MakeCode(ICodeWriter writer)
        {
            WriteSummary(writer, Description);
            WriteAttributes(writer, Attributes);
            var def = string.Join(" ", DefAttributes());
            {
                var baseAndInterfaces = new List<string>(ImplementedInterfaces);
                if (!string.IsNullOrEmpty(_baseClass))
                    baseAndInterfaces.Insert(0, _baseClass);
                if (baseAndInterfaces.Any())
                    def += " : " + string.Join(", ", baseAndInterfaces);
            }
            writer.Open(def);
            // Constructors
            var addEmptyLineBeforeRegion = Emit_constructors(writer, false);
            // Methods
            addEmptyLineBeforeRegion = Emit_methods(writer, addEmptyLineBeforeRegion);
            //Properties
            addEmptyLineBeforeRegion = Emit_properties(writer, addEmptyLineBeforeRegion);
            // Fields
            addEmptyLineBeforeRegion = Emit_fields(writer, addEmptyLineBeforeRegion);
            // Nested
            Emit_nested(writer, addEmptyLineBeforeRegion);
            writer.Close();
        }


        public override string ToString()
        {
            return "csClass " + _name;
        }

        public string TypeName(Type type)
        {
            return GeneratorsHelper.TypeName(type, this);
        }

        private void _EmitProperty(CsProperty prop, ICodeWriter writer)
        {
            var fieldName = prop.PropertyFieldName;

            var getterLines2 = prop.GetGetterLines(Features.HasFlag(LanguageFeatures.ExpressionBody));
            var header       = GetPropertyHeader(prop);

            WriteSummary(writer, prop.Description);
            WriteAttributes(writer, prop.Attributes);
            var emitField = prop.EmitField && !IsInterface;
            if (IsInterface || prop.MakeAutoImplementIfPossible && string.IsNullOrEmpty(prop.OwnSetter) &&
                string.IsNullOrEmpty(prop.OwnGetter))
            {
                var gs = prop.IsPropertyReadOnly
                    ? $"{{ {OptionalVisibility(prop.GetterVisibility)}get; }}"
                    : $"{{ {OptionalVisibility(prop.GetterVisibility)}get; {OptionalVisibility(prop.SetterVisibility)}set; }}";
                var c = header + " " + gs;
                if (!string.IsNullOrEmpty(prop.ConstValue))
                    c += " = " + prop.ConstValue + ";";
                writer.WriteLine(c).EmptyLine();
                emitField = false;
            }
            else
            {
                writer.Open(header);
                {
                    WriteGetterOrSetter(writer, getterLines2, "get", prop.GetterVisibility);
                    if (!prop.IsPropertyReadOnly)
                        WriteGetterOrSetter(writer,
                            prop.GetSetterLines(Features.HasFlag(LanguageFeatures.ExpressionBody)), "set",
                            prop.SetterVisibility);
                }
                writer.Close().EmptyLine();
            }

            if (emitField)
                writer
                    .WriteLine("// ReSharper disable once InconsistentNaming")
                    .WriteLine($"{prop.FieldVisibility.ToString().ToLower()} {prop.Type} {fieldName};")
                    .EmptyLine();
        }

        // Private Methods 

        private bool _wm(ICodeWriter writer, bool addEmptyLineBeforeRegion, IEnumerable<CsMethod> m,
            string                   region)
        {
            var csMethods = m as CsMethod[] ?? m.ToArray();
            if (!csMethods.Any()) return addEmptyLineBeforeRegion;
            writer.EmptyLine(!addEmptyLineBeforeRegion);
            addEmptyLineBeforeRegion = Action(writer, csMethods.OrderBy(a => a.Visibility).ThenBy(a => a.Name), region,
                i =>
                {
                    i.MakeCode(writer);
                    writer.EmptyLine();
                }
            );
            return addEmptyLineBeforeRegion;
        }

        private bool Action<T>(ICodeWriter writer, IEnumerable<T> list, string region, Action<T> action)
        {
            var enumerable = list as IList<T> ?? list.ToList();
            if (!enumerable.Any()) return false;
            var hasRegions = Features.HasFlag(LanguageFeatures.Regions);
            if (hasRegions)
            {
                writer.WriteLine("#region " + region);
                writer.EmptyLine();
            }

            foreach (var i in enumerable)
                action(i);
            if (hasRegions)
                writer.WriteLine("#endregion");
            return hasRegions;
        }

        private string[] DefAttributes()
        {
            var x                  = new List<string>(4);
            var visibilityAsString = VisibilityToString(Visibility);
            if (visibilityAsString != null) x.Add(visibilityAsString);
            switch (Kind)
            {
                case NamespaceMemberKind.Class:
                    if (GetIsAbstract())
                        x.Add("abstract");
                    if (IsStatic)
                        x.Add("static");
                    if (IsPartial)
                        x.Add("partial");
                    x.Add("class");
                    break;
                case NamespaceMemberKind.Interface:
                    if (IsPartial)
                        x.Add("partial");
                    x.Add("interface");
                    break;
                case NamespaceMemberKind.Struct:
                    if (IsPartial)
                        x.Add("partial");
                    x.Add("struct");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            x.Add(_name);
            return x.ToArray();
        }

        private bool Emit_constructors(ICodeWriter writer, bool addEmptyLineBeforeRegion)
        {
            var c = _methods.Where(i => i.IsConstructor).ToArray();
            if (!c.Any()) return addEmptyLineBeforeRegion;
            var m                    = c.Where(i => !i.IsStatic);
            addEmptyLineBeforeRegion = _wm(writer, addEmptyLineBeforeRegion, m, "Constructors");

            m                        = c.Where(i => i.IsStatic);
            addEmptyLineBeforeRegion = _wm(writer, addEmptyLineBeforeRegion, m, "Static constructors");
            return addEmptyLineBeforeRegion;
        }

        private bool Emit_fields(ICodeWriter writer, bool addEmptyLineBeforeRegion)
        {
            if (!_fields.Any()) return addEmptyLineBeforeRegion;
            writer.EmptyLine(!addEmptyLineBeforeRegion);
            addEmptyLineBeforeRegion = Action(writer, _fields.OrderBy(a => a.IsConst), "Fields",
                i =>
                {
                    WriteAttributes(writer, i.Attributes);
                    WriteSummary(writer, i.Description);
                    if (i.IsConst)
                    {
                        writer
                            .WriteLine("public const {0} {1} = {2};", i.Type, i.Name, i.ConstValue)
                            .EmptyLine();
                    }
                    else
                    {
                        var att = new List<string>(8)
                        {
                            i.Visibility.ToString().ToLower()
                        };
                        if (i.IsStatic) att.Add("static");
                        if (i.IsVolatile) att.Add("volatile");
                        if (i.IsReadOnly) att.Add("readonly");
                        att.Add(i.Type);
                        att.Add(i.Name);
                        if (!string.IsNullOrEmpty(i.ConstValue))
                            att.Add("= " + i.ConstValue);
                        var line  = string.Join(" ", att) + ";";
                        var lines =
                            line.Split('\r', '\n')
                                .Select(a => a.Trim())
                                .Where(a => !string.IsNullOrEmpty(a))
                                .ToArray();
                        for (var ii = 0; ii < lines.Length; ii++)
                        {
                            if (ii == 1)
                                writer.Indent++;
                            writer.WriteLine(lines[ii]);
                        }

                        if (lines.Length > 1)
                            writer.Indent--;
                        writer.EmptyLine();
                    }
                }
            );
            return addEmptyLineBeforeRegion;
        }

        private bool Emit_methods(ICodeWriter writer, bool addEmptyLineBeforeRegion)
        {
            var c = _methods.Where(i => !i.IsConstructor).ToArray();
            if (!c.Any()) return addEmptyLineBeforeRegion;
            var m                    = c.Where(i => !i.IsStatic);
            addEmptyLineBeforeRegion = _wm(writer, addEmptyLineBeforeRegion, m, "Methods");

            m                        = c.Where(i => i.IsStatic);
            addEmptyLineBeforeRegion = _wm(writer, addEmptyLineBeforeRegion, m, "Static methods");
            return addEmptyLineBeforeRegion;
        }

        private void Emit_nested(ICodeWriter writer, bool addEmptyLineBeforeRegion)
        {
            // ReSharper disable once InvertIf
            if (!_nestedClasses.Any()) return;
            writer.EmptyLine(!addEmptyLineBeforeRegion);
            Action(writer, _nestedClasses.OrderBy(a => a._name), "Nested classes",
                i =>
                {
                    i.MakeCode(writer);
                    writer.EmptyLine();
                }
            );
        }

        private bool Emit_properties(ICodeWriter writer, bool addEmptyLineBeforeRegion)
        {
            if (!_properties.Any()) return addEmptyLineBeforeRegion;
            writer.EmptyLine(!addEmptyLineBeforeRegion);
            addEmptyLineBeforeRegion = Action(writer, _properties, "Properties",
                i => _EmitProperty(i, writer));
            return addEmptyLineBeforeRegion;
        }

        private bool GetIsAbstract()
        {
            return IsAbstract || _methods.Any(i => i.IsAbstract);
        }

        private string GetPropertyHeader(CsProperty prop)
        {
            var list = new List<string>();
            if (!IsInterface && prop.Visibility != Visibilities.InterfaceDefault)
                list.Add(prop.Visibility.ToString().ToLower());
            if (prop.IsStatic)
                list.Add("static");
            if (!IsInterface && prop.IsVirtual)
                list.Add("virtual");
            list.Add(prop.Type);
            list.Add(prop.Name);
            var header = string.Join(" ", list);
            return header;
        }

        public static LanguageFeatures DefaultLanguageFeatures { get; set; }

        public IClassOwner Owner { get; set; }

        /// <summary>
        ///     Nazwa klasy
        /// </summary>
        public string Name
        {
            get { return _name; }
            private set
            {
                value = value?.Trim() ?? string.Empty;
                _name = value;
            }
        }

        /// <summary>
        ///     base class
        /// </summary>
        public string BaseClass
        {
            get { return _baseClass; }
            set { _baseClass = value?.Trim() ?? string.Empty; }
        }

        /// <summary>
        ///     Optional, real type related to code class
        /// </summary>
        public Type DotNetType { get; set; }

        /// <summary>
        ///     atrybuty
        /// </summary>
        /// <summary>
        /// </summary>
        public List<CsProperty> Properties
        {
            get { return _properties; }
            set
            {
                if (value == null) value = new List<CsProperty>();
                _properties              = value;
            }
        }

        /// <summary>
        /// </summary>
        public List<CsMethodParameter> Fields
        {
            get { return _fields; }
            set
            {
                if (value == null) value = new List<CsMethodParameter>();
                _fields                  = value;
            }
        }

        /// <summary>
        ///     is class abstract
        /// </summary>
        public bool IsAbstract { get; set; }

        /// <summary>
        ///     is class partial
        /// </summary>
        public bool IsPartial { get; set; }

        /// <summary>
        ///     is class static
        /// </summary>
        public bool IsStatic { get; set; }

        /// <summary>
        ///     is class sealed
        /// </summary>
        public bool IsSealed { get; set; }

        /// <summary>
        ///     emit as interface
        /// </summary>
        public bool IsInterface
        {
            get { return Kind == NamespaceMemberKind.Interface; }
        }

        public NamespaceMemberKind Kind { get; set; }

        public LanguageFeatures Features { get; set; } = DefaultLanguageFeatures;

        /// <summary>
        ///     Własność jest tylko do odczytu.
        /// </summary>
        public IList<string> ImplementedInterfaces { get; } = new List<string>();


        /// <summary>
        ///     obiekt, na podstawie którego wygenerowano klasę, przydatne przy dalszej obróbce
        /// </summary>
        public object GeneratorSource { get; set; }

        /// <summary>
        /// </summary>
        private readonly List<CsClass> _nestedClasses = new List<CsClass>();

        /// <summary>
        ///     methods
        /// </summary>
        private readonly List<CsMethod> _methods = new List<CsMethod>();

        private string _name      = string.Empty;
        private string _baseClass = string.Empty;

        private List<CsProperty>        _properties = new List<CsProperty>();
        private List<CsMethodParameter> _fields     = new List<CsMethodParameter>();
    }
}