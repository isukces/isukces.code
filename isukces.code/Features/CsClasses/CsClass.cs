using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using isukces.code.interfaces;

// ReSharper disable once CheckNamespace
namespace isukces.code
{
    public class CsClass : ClassMemberBase, IClassOwner, IConditional, ITypeNameResolver,
        IAttributable, ICommentable, IAnnotableByUser
    {
        /// <summary>
        ///     Tworzy instancję obiektu
        ///     <param name="name">Nazwa klasy</param>
        /// </summary>
        public CsClass(string name) => Name = name;

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

        public static CsAttribute MkAttribute(string attributeName) => new CsAttribute(attributeName);

        public static void WriteAttributes(ICsCodeWriter writer, ICollection<ICsAttribute> attributes)
        {
            if (attributes == null || attributes.Count == 0)
                return;
            foreach (var j in attributes)
                writer.WriteLine("[{0}]", j.Code);
        }

        private static void Emit_single_field(ICsCodeWriter writer, CsClassField field)
        {
            writer.OpenCompilerIf(field);
            writer.WriteComment(field);
            try
            {
                WriteAttributes(writer, field.Attributes);
                WriteSummary(writer, field.Description);
                if (field.IsConst)
                {
                    var v = field.Visibility.ToCsCode();
                    if (string.IsNullOrEmpty(v))
                        v = Visibilities.Public.ToCsCode();
                    writer
                        .WriteLine("{0} const {1} {2} = {3};", v, field.Type, field.Name, field.ConstValue)
                        .EmptyLine();
                }
                else
                {
                    var att = new List<string>(8)
                    {
                        field.Visibility.ToCsCode()
                    };
                    if (field.IsStatic) att.Add("static");
                    if (field.IsVolatile) att.Add("volatile");
                    if (field.IsReadOnly) att.Add("readonly");
                    att.Add(field.Type);
                    att.Add(field.Name);
                    if (!string.IsNullOrEmpty(field.ConstValue))
                        att.Add("= " + field.ConstValue);
                    var line = string.Join(" ", att) + ";";
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
            finally
            {
                writer.CloseCompilerIf(field);
            }
        }

        private static string OptionalVisibility(Visibilities? memberVisibility)
        {
            var v = memberVisibility == null ? "" : memberVisibility.Value.ToString().ToLower() + " ";
            return v;
        }


        private static void WriteGetterOrSetter(ICsCodeWriter writer, CodeLines code, string keyWord,
            Visibilities? memberVisibility)
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

        private static void WriteSummary(ICsCodeWriter writer, string description)
        {
            description = description?.Trim();
            if (string.IsNullOrEmpty(description)) return;
            writer.WriteLine("/// <summary>");
            var lines = description.Split('\r', '\n').Where(q => !string.IsNullOrEmpty(q?.Trim()));
            foreach (var line in lines)
                writer.WriteLine("/// " + line.XmlEncode());
            writer.WriteLine("/// </summary>");
        }


        public void AddComment(string x)
        {
            _extraComment.AppendLine(x);
        }

        // Public Methods 

        public CsClassField AddConst(string name, string type, string encodedValue)
        {
            var constValue = new CsClassField(name, type)
            {
                ConstValue = encodedValue,
                IsConst    = true
            };
            Fields.Add(constValue);
            return constValue;
        }

        public CsClassField AddConstInt(string name, int encodedValue) =>
            AddConst(name, "int", encodedValue.ToCsString());

        public CsMethod AddConstructor(string description = null)
        {
            var n = _name.Split('<')[0].Trim();
            var m = new CsMethod(n, Name)
            {
                Description = description
            };
            _methods.Add(m);
            m.IsConstructor = true;
            return m;
        }

        public CsClassField AddConstString(string name, string plainValue)
        {
            var encodedValue = plainValue == null ? "null" : plainValue.CsEncode();
            return AddConst(name, "string", encodedValue);
        }

        public CsEvent AddEvent(string name, string type)
        {
            // public event EventHandler<ConversionCtx.ResolveSeparateLinesEventArgs> ResolveSeparateLines;
            var ev = new CsEvent(name, type);
            _events.Add(ev);
            return ev;
        }

        public CsEvent AddEvent<T>(string name)
        {
            // public event EventHandler<ConversionCtx.ResolveSeparateLinesEventArgs> ResolveSeparateLines;
            var type = this.GetTypeName<T>();
            var ev   = new CsEvent(name, type);
            _events.Add(ev);
            return ev;
        }

        public CsClassField AddField(string fieldName, Type type) => AddField(fieldName, GetTypeName(type));

        public CsClassField AddField(string fieldName, string type)
        {
            var field = new CsClassField(fieldName, type);
            Fields.Add(field);
            return field;
        }

        public CsMethod AddMethod(string name, Type type, string description = null) =>
            AddMethod(name, GetTypeName(type), description);

        public CsMethod AddMethod(string name, string type, string description = null)
        {
            var isConstructor = string.IsNullOrEmpty(name) || name == _name;
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
            => AddProperty(propertyName, GetTypeName(type));

        public CsProperty AddProperty(string propertyName, string type)
        {
            var property = new CsProperty(propertyName, type);
            if (propertyName.Contains('.'))
            {
                property.Visibility = Visibilities.InterfaceDefault;
                property.EmitField  = false;
            }

            Properties.Add(property);
            return property;
        }

        public string GetComments() => _extraComment.ToString();

        public CsClass GetOrCreateNested(string typeName) => GetOrCreateNested(typeName, out _);

        public CsClass GetOrCreateNested(string typeName, out bool isCreatedNew)
        {
            var existing = _nestedClasses
                .FirstOrDefault(csClass => csClass.Name == typeName);
            if (existing != null)
            {
                isCreatedNew = false;
                return existing;
            }

            existing = new CsClass(typeName)
            {
                Owner = this
            };
            _nestedClasses.Add(existing);
            isCreatedNew = true;
            return existing;
        }

        public string GetTypeName(Type type)
        {
            if (Owner == null)
                throw new NullReferenceException(nameof(Owner));
            var result = Owner.GetTypeName(type);
            if (!(Owner is CsClass cl)) return result;
            var cutBegin = cl.Name + ".";
            if (result.StartsWith(cutBegin, StringComparison.Ordinal))
                result = result.Substring(cutBegin.Length);
            return result;
        }

        public bool IsKnownNamespace(string namespaceName) => Owner?.IsKnownNamespace(namespaceName) ?? false;

        public void MakeCode(ICsCodeWriter writer)
        {
            writer.OpenCompilerIf(CompilerDirective);
            writer.WriteComment(this);
            WriteSummary(writer, Description);
            WriteAttributes(writer, Attributes);
            var def = string.Join(" ", DefAttributes());
            {
                var dupa              = new HashSet<string>();
                var baseAndInterfaces = new List<string>();
                if (!string.IsNullOrEmpty(_baseClass))
                {
                    baseAndInterfaces.Add(_baseClass);
                    dupa.Add(_baseClass);
                }

                for (var index = 0; index < ImplementedInterfaces.Count; index++)
                {
                    var interfaceName = ImplementedInterfaces[index];
                    if (dupa.Add(interfaceName))
                        baseAndInterfaces.Add(interfaceName);
                }

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
            // Events
            addEmptyLineBeforeRegion = Emit_events(writer, addEmptyLineBeforeRegion);
            // Nested
            Emit_nested(writer, addEmptyLineBeforeRegion);
            writer.Close();
            writer.CloseCompilerIf(CompilerDirective);
        }


        public override string ToString() => "csClass " + _name;

        public CsClass WithBaseClass(string baseClass)
        {
            BaseClass = baseClass;
            return this;
        }

        private void _EmitProperty(CsProperty prop, ICsCodeWriter writer)
        {
            writer.OpenCompilerIf(prop);
            try
            {
                writer.WriteComment(prop);

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
                    if (!IsInterface && !string.IsNullOrEmpty(prop.ConstValue))
                        c += " = " + prop.ConstValue + ";";
                    writer.WriteLine(c);
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
                    writer.Close();
                }

                if (emitField)
                    writer
                        .EmptyLine()
                        //.WriteLine("// ReSharper disable once InconsistentNaming")
                        .WriteLine($"{prop.FieldVisibility.ToString().ToLower()} {prop.Type} {fieldName};");
            }
            finally
            {
                writer.CloseCompilerIf(prop);
            }

            writer.EmptyLine();
        }


        private bool _wm(ICsCodeWriter writer, bool addEmptyLineBeforeRegion, IEnumerable<CsMethod> m,
            string region)
        {
            var csMethods = m as CsMethod[] ?? m.ToArray();
            if (!csMethods.Any()) return addEmptyLineBeforeRegion;
            writer.EmptyLine(!addEmptyLineBeforeRegion);
            addEmptyLineBeforeRegion = Action(writer, csMethods.OrderBy(a => a.Visibility).ThenBy(a => a.Name), region,
                i =>
                {
                    i.MakeCode(writer, IsInterface);
                    writer.EmptyLine();
                }
            );
            return addEmptyLineBeforeRegion;
        }

        private bool Action<T>(ICsCodeWriter writer, IEnumerable<T> list, string region, Action<T> action)
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
            var visibilityAsString = Visibility.ToCsCode();
            if (visibilityAsString != null) x.Add(visibilityAsString);
            switch (Kind)
            {
                case CsNamespaceMemberKind.Class:
                    if (GetIsAbstract())
                    {
                        if (IsSealed)
                            throw new Exception($"Class {Name} can't be both sealed and abstract");
                        if (IsStatic)
                            throw new Exception($"Class {Name} can't be both static and abstract");
                        x.Add("abstract");
                    }
                    else if (IsStatic)
                    {
                        if (IsSealed)
                            throw new Exception($"Class {Name} can't be both static and sealed");
                        x.Add("static");
                    }
                    else if (IsSealed)
                    {
                        x.Add("sealed");
                    }

                    if (IsPartial)
                        x.Add("partial");
                    x.Add("class");
                    break;
                case CsNamespaceMemberKind.Interface:
                    if (IsPartial)
                        x.Add("partial");
                    x.Add("interface");
                    break;
                case CsNamespaceMemberKind.Struct:
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

        private bool Emit_constructors(ICsCodeWriter writer, bool addEmptyLineBeforeRegion)
        {
            var c = _methods.Where(i => i.IsConstructor).ToArray();
            if (!c.Any()) return addEmptyLineBeforeRegion;
            var m = c.Where(i => !i.IsStatic);
            addEmptyLineBeforeRegion = _wm(writer, addEmptyLineBeforeRegion, m, "Constructors");

            m                        = c.Where(i => i.IsStatic);
            addEmptyLineBeforeRegion = _wm(writer, addEmptyLineBeforeRegion, m, "Static constructors");
            return addEmptyLineBeforeRegion;
        }

        private bool Emit_events(ICsCodeWriter writer, bool addEmptyLineBeforeRegion)
        {
            if (!_events.Any())
                return addEmptyLineBeforeRegion;
            writer.EmptyLine(!addEmptyLineBeforeRegion);
            addEmptyLineBeforeRegion = Action(writer, _events.OrderBy(a => a.Name), "Events",
                ev =>
                {
                    writer.OpenCompilerIf(ev);
                    try
                    {
                        // public event EventHandler<BeforeSaveEventArgs> BeforeSave;
                        var v    = ev.Visibility.ToCsCode();
                        var code = $"{v} event {ev.Type} {ev.Name};";
                        writer.WriteLine(code.TrimStart());
                    }
                    finally
                    {
                        writer.CloseCompilerIf(ev);
                    }
                }
            );
            return addEmptyLineBeforeRegion;
        }

        private bool Emit_fields(ICsCodeWriter writer, bool addEmptyLineBeforeRegion)
        {
            if (!_fields.Any()) return addEmptyLineBeforeRegion;
            writer.EmptyLine(!addEmptyLineBeforeRegion);
            addEmptyLineBeforeRegion = Action(writer, _fields.OrderBy(a => a.IsConst), "Fields",
                field => { Emit_single_field(writer, field); }
            );
            return addEmptyLineBeforeRegion;
        }

        private bool Emit_methods(ICsCodeWriter writer, bool addEmptyLineBeforeRegion)
        {
            var c = _methods.Where(i => !i.IsConstructor).ToArray();
            if (!c.Any()) return addEmptyLineBeforeRegion;
            var m = c.Where(i => !i.IsStatic);
            addEmptyLineBeforeRegion = _wm(writer, addEmptyLineBeforeRegion, m, "Methods");

            m                        = c.Where(i => i.IsStatic);
            addEmptyLineBeforeRegion = _wm(writer, addEmptyLineBeforeRegion, m, "Static methods");
            return addEmptyLineBeforeRegion;
        }

        private void Emit_nested(ICsCodeWriter writer, bool addEmptyLineBeforeRegion)
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

        private bool Emit_properties(ICsCodeWriter writer, bool addEmptyLineBeforeRegion)
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
            if (!IsInterface)
            {
                if (prop.IsOverride)
                    list.Add("override");
                else if (prop.IsVirtual)
                    list.Add("virtual");
            }

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
            private set { _name = value?.Trim() ?? string.Empty; }
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
            set { _properties = value ?? new List<CsProperty>(); }
        }

        /// <summary>
        /// </summary>
        public List<CsClassField> Fields
        {
            get { return _fields; }
            set { _fields = value ?? new List<CsClassField>(); }
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
        ///     is class sealed
        /// </summary>
        public bool IsSealed { get; set; }

        /// <summary>
        ///     emi as interface
        /// </summary>
        public bool IsInterface
        {
            get { return Kind == CsNamespaceMemberKind.Interface; }
        }

        public CsNamespaceMemberKind Kind { get; set; }

        public LanguageFeatures Features { get; set; } = DefaultLanguageFeatures;

        /// <summary>
        ///     Własność jest tylko do odczytu.
        /// </summary>
        public IList<string> ImplementedInterfaces { get; } = new List<string>();


        /// <summary>
        ///     obiekt, na podstawie którego wygenerowano klasę, przydatne przy dalszej obróbce
        /// </summary>
        public object GeneratorSource { get; set; }

        public IReadOnlyList<CsMethod> Methods
        {
            get { return _methods; }
        }

        public IReadOnlyList<CsEvent> Events
        {
            get { return _events; }
        }

        public IDictionary<string, object> UserAnnotations { get; } = new Dictionary<string, object>();

        private readonly StringBuilder _extraComment = new StringBuilder();

        /// <summary>
        /// </summary>
        private readonly List<CsClass> _nestedClasses = new List<CsClass>();

        /// <summary>
        ///     methods
        /// </summary>
        private readonly List<CsMethod> _methods = new List<CsMethod>();

        private readonly List<CsEvent> _events = new List<CsEvent>();

        private string _name = string.Empty;
        private string _baseClass = string.Empty;

        private List<CsProperty> _properties = new List<CsProperty>();
        private List<CsClassField> _fields = new List<CsClassField>();
    }
}