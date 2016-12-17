#region using

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using isukces.code.AutoCode;
using isukces.code.CodeWrite;
using isukces.code.interfaces;

#endregion

namespace isukces.code
{
    public class CsClass : ClassMemberBase, IClassOwner
    {
        #region Constructors

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
            Name = name;
            BaseClass = baseClass;
        }

        #endregion

        #region Static Methods

        public static CsAttribute MkAttribute(string attributeName)
        {
            return new CsAttribute {Name = attributeName};
        }

        // Private Methods 

        private static bool _wm(ICodeWriter writer, bool addEmptyLineBeforeRegion, IEnumerable<CsMethod> m,
            string region)
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

        private static bool Action<T>(ICodeWriter writer, IEnumerable<T> list, string region, Action<T> action)
        {
            var enumerable = list as IList<T> ?? list.ToList();
            if (!enumerable.Any()) return false;
            writer.WriteLine("#region " + region);
            {
                writer.EmptyLine();
                foreach (var i in enumerable)
                    action(i);
            }
            writer.WriteLine("#endregion");

            return true;
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
            if ((attributes == null) || (attributes.Count == 0))
                return;
            foreach (var j in attributes)
                writer.WriteLine("[{0}]", j.Code);
        }

        private static void WriteGetterOrSetter(ICodeWriter writer, IReadOnlyList<string> lines, string keyWord,
            Visibilities? memberVisibility)
        {
            if ((lines == null) || (lines.Count <= 0)) return;
            if (lines.Count == 1)
                writer.WriteLine("{0}{1} {{ {2} }}", OptionalVisibility(memberVisibility), keyWord, lines[0]);
            else
            {
                writer.Open(keyWord);
                foreach (var iii in lines)
                    writer.WriteLine(iii);
                writer.Close();
            }
        }

        private static void WriteSummary(ICodeWriter writer, string description)
        {
            description = description?.Trim();
            if (string.IsNullOrEmpty(description)) return;
            writer.WriteLine("/// <summary>");
            writer.WriteLine("/// " + description.XmlEncode());
            writer.WriteLine("/// </summary>");
        }

        #endregion

        #region Instance Methods

        // Public Methods 

        public CsMethodParameter AddConst(string name, string type, string encodedValue)
        {
            var constValue = new CsMethodParameter(name, type)
            {
                ConstValue = encodedValue,
                IsConst = true
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
            var field = new CsMethodParameter(fieldName, TypeName(type));
            Fields.Add(field);
            return field;
        }

        public CsMethod AddMethod(string name, string type, string description)
        {
            var m = new CsMethod(name, type)
            {
                Description = description
            };
            _methods.Add(m);
            m.IsConstructor = name == _name;
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
            var parentNamespaces = ClassOwner?.GetNamespaces(true);
            var appendNamespace = DotNetType?.Namespace;
            var append2 = string.IsNullOrEmpty(appendNamespace) ? null : new[] {appendNamespace};
            var copy = GeneratorsHelper.MakeCopy(parentNamespaces, append2);
            return copy;
        }

        public CsClass GetOrCreateNested(string typeName)
        {
            var existing = _nestedClasses
                .FirstOrDefault(csClass => csClass.Name == typeName);
            if (existing != null) return existing;
            existing = new CsClass(typeName)
            {
                ClassOwner = this
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

        public string TypeName(Type type)
        {
            return GeneratorsHelper.TypeName(type, this);
        }

        // Private Methods 

        private void _EmitProperty(CsProperty prop, ICodeWriter writer)
        {
            var fieldName = prop.PropertyFieldName;

            var getterLines = prop.GetGetterLines();
            var visibility = IsInterface || (prop.Visibility == Visibilities.InterfaceDefault)
                ? ""
                : prop.Visibility.ToString().ToLower() + " ";
            WriteSummary(writer, prop.Description);
            WriteAttributes(writer, prop.Attributes);
            var emitField = prop.EmitField && !IsInterface;

            if (IsInterface ||
                (prop.MakeAutoImplementIfPossible && string.IsNullOrEmpty(prop.OwnSetter) &&
                 string.IsNullOrEmpty(prop.OwnGetter)))
            {
                writer.WriteLine("{0}{1} {2} {{ {3}get; {4}set; }}",
                        visibility, prop.Type, prop.Name, OptionalVisibility(prop.GetterVisibility),
                        OptionalVisibility(prop.SetterVisibility))
                    .EmptyLine();
                emitField = false;
            }
            else
            {
                writer.Open("{0}{1} {2}", visibility, prop.Type, prop.Name);
                {
                    WriteGetterOrSetter(writer, getterLines, "get", prop.GetterVisibility);
                    if (!prop.IsPropertyReadOnly)
                        WriteGetterOrSetter(writer, prop.GetSetterLines(), "set", prop.SetterVisibility);
                }
                writer.Close().EmptyLine();
            }

            if (emitField)
                writer
                    .WriteLine("// ReSharper disable once InconsistentNaming")
                    .WriteLine("{2} {0} {1};", prop.Type, fieldName, prop.FieldVisibility.ToString().ToLower())
                    .EmptyLine();
        }

        private string[] DefAttributes()
        {
            var x = new List<string>(4);
            var visibilityAsString = VisibilityToString(Visibility);
            if (visibilityAsString != null) x.Add(visibilityAsString);
            if (IsInterface)
                x.Add("interface");
            else
            {
                if (GetIsAbstract())
                    x.Add("abstract");
                if (IsPartial)
                    x.Add("partial");
                if (IsStatic)
                    x.Add("static");
                x.Add("class");
            }
            x.Add(_name);
            return x.ToArray();
        }

        private bool Emit_constructors(ICodeWriter writer, bool addEmptyLineBeforeRegion)
        {
            var c = _methods.Where(i => i.IsConstructor).ToArray();
            if (!c.Any()) return addEmptyLineBeforeRegion;
            var m = c.Where(i => !i.IsStatic);
            addEmptyLineBeforeRegion = _wm(writer, addEmptyLineBeforeRegion, m, "Constructors");

            m = c.Where(i => i.IsStatic);
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
                        writer
                            .WriteLine("public const {0} {1} = {2};", i.Type, i.Name, i.ConstValue)
                            .EmptyLine();
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
            );
            return addEmptyLineBeforeRegion;
        }

        private bool Emit_methods(ICodeWriter writer, bool addEmptyLineBeforeRegion)
        {
            var c = _methods.Where(i => !i.IsConstructor).ToArray();
            if (!c.Any()) return addEmptyLineBeforeRegion;
            var m = c.Where(i => !i.IsStatic);
            addEmptyLineBeforeRegion = _wm(writer, addEmptyLineBeforeRegion, m, "Methods");

            m = c.Where(i => i.IsStatic);
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

        #endregion

        #region Properties

        public IClassOwner ClassOwner { get; set; }

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
            set
            {
                value = value?.Trim() ?? string.Empty;
                _baseClass = value;
            }
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
                _properties = value;
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
                _fields = value;
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
        public bool IsInterface { get; set; }

        /// <summary>
        ///     Własność jest tylko do odczytu.
        /// </summary>
        public IList<string> ImplementedInterfaces { get; } = new List<string>();


        /// <summary>
        ///     obiekt, na podstawie którego wygenerowano klasę, przydatne przy dalszej obróbce
        /// </summary>
        public object GeneratorSource { get; set; }

        #endregion

        #region Fields

        /// <summary>
        /// </summary>
        private readonly List<CsClass> _nestedClasses = new List<CsClass>();

        /// <summary>
        ///     methods
        /// </summary>
        private readonly List<CsMethod> _methods = new List<CsMethod>();

        private string _name = string.Empty;
        private string _baseClass = string.Empty;

        private List<CsProperty> _properties = new List<CsProperty>();
        private List<CsMethodParameter> _fields = new List<CsMethodParameter>();

        #endregion
    }
}