using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iSukces.Code.Interfaces;

// ReSharper disable once CheckNamespace
namespace iSukces.Code
{
    public class CsClass : ClassMemberBase, IClassOwner, IConditional, ITypeNameResolver,
        IAttributable, ICommentable, IAnnotableByUser, IEnumOwner
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

        private static void Emit_single_field(ICsCodeWriter writer, CsClassField field)
        {
            writer.OpenCompilerIf(field);
            writer.WriteComment(field);
            try
            {
                WriteSummary(writer, field.Description);
                writer.WriteAttributes(field.Attributes);
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


        public static CsAttribute MkAttribute(string attributeName)
        {
            return new CsAttribute(attributeName);
        }


        public static void WriteSummary(ICsCodeWriter writer, string description)
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

        // Public Methods 

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

        public CsClassField AddConstInt(string name, int encodedValue)
        {
            return AddConst(name, "int", encodedValue.ToCsString());
        }

        public CsMethod AddConstructor(string description = null)
        {
            var m = new CsMethod(GetConstructorName(), Name)
            {
                Description = description
            };
            _methods.Add(m);
            m.Kind = MethodKind.Constructor;
            return m;
        }

        public CsClassField AddConstString(string name, string plainValue)
        {
            var encodedValue = plainValue == null ? "null" : plainValue.CsEncode();
            return AddConst(name, "string", encodedValue);
        }

        public CsEnum AddEnum(CsEnum csEnum)
        {
            ((List<CsEnum>)Enums).Add(csEnum);
            csEnum.Owner = this;
            return csEnum;
        }

        public CsEvent AddEvent(string name, string type, string description = null)
        {
            // public event EventHandler<ConversionCtx.ResolveSeparateLinesEventArgs> ResolveSeparateLines;
            var ev = new CsEvent(name, type, description);
            _events.Add(ev);
            return ev;
        }

        public CsEvent AddEvent<T>(string name, string description = null)
        {
            // public event EventHandler<ConversionCtx.ResolveSeparateLinesEventArgs> ResolveSeparateLines;
            var type = this.GetTypeName<T>();
            var ev   = new CsEvent(name, type, description);
            _events.Add(ev);
            return ev;
        }

        public CsClassField AddField(string fieldName, Type type)
        {
            return AddField(fieldName, GetTypeName(type));
        }

        public CsClassField AddField(string fieldName, string type)
        {
            var field = new CsClassField(fieldName, type);
            Fields.Add(field);
            return field;
        }

        public CsMethod AddFinalizer(string description = null)
        {
            var m = new CsMethod(GetFinalizerName(), Name)
            {
                Description = description
            };
            _methods.Add(m);
            m.Kind = MethodKind.Finalizer;
            return m;
        }

        public CsMethod AddMethod(string name, Type type, string description = null)
        {
            return AddMethod(name, type == null ? null : GetTypeName(type), description);
        }

        public CsMethod AddMethod(string name, string type, string description = null)
        {
            if (string.IsNullOrEmpty(name) || name == GetConstructorName())
                return AddConstructor(description);
            if (name == GetFinalizerName())
                return AddFinalizer(description);
            if (string.IsNullOrEmpty(type))
                type = "void";
            var m = new CsMethod(name, type)
            {
                Description = description
            };
            _methods.Add(m);
            return m;
        }

        public CsProperty AddProperty(string propertyName, Type type)
        {
            return AddProperty(propertyName, GetTypeName(type));
        }

        public CsProperty AddProperty(string propertyName, string type)
        {
            propertyName = propertyName?.Trim();
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException("propertyName is empty");
            var property = new CsProperty(propertyName, type);
            if (propertyName.Contains('.'))
            {
                property.Visibility = Visibilities.InterfaceDefault;
                property.EmitField  = false;
            }

            Properties.Add(property);
            return property;
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
                    if (IsReadOnlyStruct)
                        x.Add("readonly");
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
            var w = new CsClassWriter(this);
            var c = _methods
                .Where(i => i.Kind == MethodKind.Constructor || i.Kind == MethodKind.Finalizer)
                .OrderBy(a => a.Kind != MethodKind.Constructor)
                .ToArray();
            if (!c.Any())
                return addEmptyLineBeforeRegion;
            var m = c.Where(i => !i.IsStatic);
            addEmptyLineBeforeRegion = w.WriteMethods(writer, addEmptyLineBeforeRegion, m, "Constructors");

            m                        = c.Where(i => i.IsStatic);
            addEmptyLineBeforeRegion = w.WriteMethods(writer, addEmptyLineBeforeRegion, m, "Static constructors");
            return addEmptyLineBeforeRegion;
        }

        private bool Emit_events(ICsCodeWriter writer, bool addEmptyLineBeforeRegion)
        {
            if (!_events.Any())
                return addEmptyLineBeforeRegion;
            writer.EmptyLine(!addEmptyLineBeforeRegion);
            CsClassWriter w = new CsClassWriter(this);

            addEmptyLineBeforeRegion = w.WriteMethodAction(writer, _events.OrderBy(a => a.Name), "Events",
                ev =>
                {
                    writer.OpenCompilerIf(ev);
                    try
                    {
                        WriteSummary(writer, ev.Description);
                        writer.WriteAttributes(ev.Attributes);
                        // public event EventHandler<BeforeSaveEventArgs> BeforeSave;
                        var v    = ev.Visibility.ToCsCode();
                        var code = $"{v} event {ev.Type} {ev.Name};".TrimStart();

                        if (ev.LongDefinition)
                        {
                            writer.Open(code);
                            writer.WriteLine($"add {{ {ev.FieldName} += value; }}");
                            writer.WriteLine($"remove {{ {ev.FieldName} -= value; }}");
                            writer.Close();
                        }
                        else
                        {
                            writer.WriteLine(code);
                        }
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
            CsClassWriter w   = new CsClassWriter(this);
            var           all = _fields.OrderBy(a => a.IsConst).ToList();
            foreach (var i in _events.Where(a => a.LongDefinition))
            {
                var eventField = new CsClassField(i.FieldName, i.Type, i.GetFieldDescription());
                all.Add(eventField);
            }

            if (!all.Any()) return addEmptyLineBeforeRegion;
            writer.EmptyLine(!addEmptyLineBeforeRegion);

            addEmptyLineBeforeRegion = w.WriteMethodAction(writer, all, "Fields",
                field => { Emit_single_field(writer, field); }
            );
            return addEmptyLineBeforeRegion;
        }

        private bool Emit_methods(ICsCodeWriter writer, bool addEmptyLineBeforeRegion)
        {
            CsClassWriter w = new CsClassWriter(this);
            var c = _methods
                .Where(i => i.Kind != MethodKind.Constructor && i.Kind != MethodKind.Finalizer)
                .OrderBy(a => a.Kind)
                .ToArray();
            if (!c.Any()) return addEmptyLineBeforeRegion;
            var m = c.Where(i => !i.IsStatic);
            addEmptyLineBeforeRegion = w.WriteMethods(writer, addEmptyLineBeforeRegion, m, "Methods");

            m                        = c.Where(i => i.IsStatic);
            addEmptyLineBeforeRegion = w.WriteMethods(writer, addEmptyLineBeforeRegion, m, "Static methods");
            return addEmptyLineBeforeRegion;
        }

        private void Emit_nested(ICsCodeWriter writer, bool addEmptyLineBeforeRegion)
        {
            CsClassWriter w = new CsClassWriter(this);
            // ReSharper disable once InvertIf
            if (_nestedClasses.Any())
            {
                writer.EmptyLine(!addEmptyLineBeforeRegion);
                w.WriteMethodAction(writer, _nestedClasses.OrderBy(a => a._name), "Nested classes",
                    i =>
                    {
                        i.MakeCode(writer);
                        writer.EmptyLine();
                    }
                );
            }

            if (Enums.Any())
            {
                writer.EmptyLine(!addEmptyLineBeforeRegion);
                w.WriteMethodAction(writer, Enums.OrderBy(a => a.Name), "Nested enums",
                    i =>
                    {
                        i.MakeCode(writer);
                        writer.EmptyLine();
                    }
                );
            }
        }

        private bool Emit_properties(ICsCodeWriter writer, bool addEmptyLineBeforeRegion)
        {
            CsClassWriter w = new CsClassWriter(this);
            if (!_properties.Any()) return addEmptyLineBeforeRegion;
            writer.EmptyLine(!addEmptyLineBeforeRegion);
            addEmptyLineBeforeRegion = w.WriteMethodAction(writer, _properties, "Properties",
                i =>
                {
                    var tmp = new PropertyWriter(this, i);
                    tmp.EmitProperty(writer);
                });
            return addEmptyLineBeforeRegion;
        }

        public string GetComments()
        {
            return _extraComment.ToString();
        }

        private string GetConstructorName()
        {
            return _name.Split('<')[0].Trim();
        }

        private string GetFinalizerName()
        {
            return "~" + GetConstructorName();
        }

        private bool GetIsAbstract()
        {
            return IsAbstract || _methods.Any(i => i.Overriding == OverridingType.Abstract);
        }

        public string GetNamespace()
        {
            var owner = Owner;
            while (true)
                switch (owner)
                {
                    case null:
                    case CsFile _:
                        return string.Empty;
                    case CsNamespace ns:
                        return ns.Name;
                    case CsClass cl:
                        owner = cl.Owner;
                        continue;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(owner));
                }
        }

        public CsClass GetOrCreateNested(string typeName)
        {
            return GetOrCreateNested(typeName, out _);
        }

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
            if (Owner is not CsClass cl) return result;
            var cutBegin = cl.Name + ".";
            if (result.StartsWith(cutBegin, StringComparison.Ordinal))
                result = result.Substring(cutBegin.Length);
            return result;
        }

        public bool IsKnownNamespace(string namespaceName)
        {
            return Owner?.IsKnownNamespace(namespaceName) ?? false;
        }

        public void MakeCode(ICsCodeWriter writer)
        {
            writer.OpenCompilerIf(CompilerDirective);
            writer.WriteComment(this);
            WriteSummary(writer, Description);
            writer.WriteAttributes(Attributes);
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


        public override string ToString()
        {
            return "csClass " + _name;
        }

        public CsClass WithBaseClass(string baseClass)
        {
            BaseClass = baseClass;
            return this;
        }

        #region properties

        public static CodeFormatting DefaultCodeFormatting { get; set; } = new CodeFormatting(CodeFormattingFeatures.None, 100);

        public IClassOwner Owner { get; set; }

        /// <summary>
        ///     Nazwa klasy
        /// </summary>
        public string Name
        {
            get => _name;
            private set => _name = value?.Trim() ?? string.Empty;
        }

        /// <summary>
        ///     base class
        /// </summary>
        public string BaseClass
        {
            get => _baseClass;
            set => _baseClass = value?.Trim() ?? string.Empty;
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
            get => _properties;
            set => _properties = value ?? new List<CsProperty>();
        }

        /// <summary>
        /// </summary>
        public List<CsClassField> Fields
        {
            get => _fields;
            set => _fields = value ?? new List<CsClassField>();
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
        ///     is read only struct
        /// </summary>
        public bool IsReadOnlyStruct { get; set; }

        /// <summary>
        ///     emit as interface
        /// </summary>
        public bool IsInterface => Kind == CsNamespaceMemberKind.Interface;

        public CsNamespaceMemberKind Kind { get; set; }

        public CodeFormatting Formatting { get; set; } = DefaultCodeFormatting;

        /// <summary>
        ///     Własność jest tylko do odczytu.
        /// </summary>
        public IList<string> ImplementedInterfaces { get; } = new List<string>();


        /// <summary>
        ///     obiekt, na podstawie którego wygenerowano klasę, przydatne przy dalszej obróbce
        /// </summary>
        public object GeneratorSource { get; set; }

        public IReadOnlyList<CsMethod> Methods => _methods;

        public IReadOnlyList<CsEvent> Events => _events;

        #endregion

        public IDictionary<string, object> UserAnnotations { get; } = new Dictionary<string, object>();
        public IReadOnlyList<CsEnum>       Enums           { get; } = new List<CsEnum>();

        #region Fields

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

        #endregion
    }
}
