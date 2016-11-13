#region using

using System;
using System.Collections.Generic;
using System.Linq;
using isukces.code.CodeWrite;
using isukces.code.interfaces;

#endregion

namespace isukces.code
{
    public class CsClass : ClassMemberBase, IAttributable, ICsClassMember
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
            return new CsAttribute { Name = attributeName };
        }

        // Private Methods 

        private static bool _wm(ICodeWriter writer, bool addEmptyLineBeforeRegion, IEnumerable<CsMethod> m, string region)
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

        private static void WriteAttributes(ICodeWriter writer, ICollection<ICsAttribute> attributes)
        {
            if (attributes == null || attributes.Count == 0)
                return;
            foreach (var j in attributes)
                writer.WriteLine("[{0}]", j.Code);
        }

        #endregion

        #region Instance Methods

        // Public Methods 

        public void AddConst(string name, string type, string encodedValue)
        {
            var constValue = new CsMethodParameter(name, type)
            {
                ConstValue = encodedValue,
                IsConst = true
            };
            Fields.Add(constValue);
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

        public void MakeCode(ICodeWriter writer)
        {
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

        // Private Methods 

        private void _EmitProperty(CsProperty prop, ICodeWriter writer)
        {
            var fieldName = prop.PropertyFieldName;
            var reader = !string.IsNullOrEmpty(prop.OwnGetter)
                ? prop.OwnGetter
                : string.Format("return {0};", fieldName);

            var visibility = IsInterface || (prop.PropertyVisibility == Visibilities.InterfaceDefault)
                ? ""
                : prop.PropertyVisibility.ToString().ToLower() + " ";
            if (!string.IsNullOrEmpty(prop.Description))
            {
                writer.WriteLine("/// <summary>");
                writer.WriteLine("/// " + prop.Description.XmlEncode());
                writer.WriteLine("/// </summary>");
            }
            WriteAttributes(writer, prop.Attributes);
            var emitField = prop.EmitField && !IsInterface;

            if (IsInterface ||
                (prop.MakeAutoImplementIfPossible && string.IsNullOrEmpty(prop.OwnSetter) &&
                 string.IsNullOrEmpty(prop.OwnGetter)))
            {
                writer.WriteLine("{0}{1} {2} {{ get; set; }}", visibility, prop.Type, prop.Name)
                    .EmptyLine();
                emitField = false;
            }
            else
            {
                writer.Open("{0}{1} {2}", visibility, prop.Type, prop.Name);
                {
                    writer.Open("get");
                    foreach (var iii in reader.Split('\r', '\n').Where(ii => ii.Trim() != ""))
                        writer.WriteLine(iii);
                    writer.Close();
                    if (!prop.IsPropertyReadOnly)
                    {
                        writer.Open("set");
                        if (!string.IsNullOrEmpty(prop.OwnSetter))
                        {
                            foreach (var iii in prop.OwnSetter.Split('\r', '\n').Where(ii => ii.Trim() != ""))
                                writer.WriteLine(iii);
                        }
                        else
                            writer.WriteLine("{0} = value;", fieldName);
                        writer.Close();
                    }
                }
                writer.Close().EmptyLine();
            }

            if (emitField)
                writer
                    .WriteLine("// ReSharper disable once InconsistentNaming")
                    .WriteLine("protected {0} {1};", prop.Type, fieldName)
                    .EmptyLine();
        }

        private string[] DefAttributes()
        {
            var x = new List<string>(4) { "public" };

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
                    if (i.IsConst)
                        writer
                            .WriteLine("public const {0} {1} = {2};", i.Type, i.Name, i.ConstValue)
                            .EmptyLine();
                    else
                    {
                        var att = new List<string>
                        {
                            i.PropertyVisibility.ToString().ToLower()
                        };
                        if (i.IsStatic) att.Add("static");
                        if (i.IsReadOnly) att.Add("readonly");
                        att.Add(i.Type);
                        att.Add(i.Name);
                        if (!string.IsNullOrEmpty(i.ConstValue))
                            att.Add("= " + i.ConstValue);
                        var line = string.Join(" ", att) + ";";
                        writer.WriteLine(line).EmptyLine();
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
        ///     klasa Bazowa
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
        ///     czy klasa abstrakcyjna
        /// </summary>
        public bool IsAbstract { get; set; }

        /// <summary>
        ///     czy partial
        /// </summary>
        public bool IsPartial { get; set; }

        /// <summary>
        ///     czy static
        /// </summary>
        public bool IsStatic { get; set; }

        /// <summary>
        ///     czy sealed
        /// </summary>
        public bool IsSealed { get; set; }

        /// <summary>
        ///     Czy zamiast klasy emitować interfejs
        /// </summary>
        public bool IsInterface { get; set; }

        /// <summary>
        ///     metody
        /// </summary>
        public List<CsMethod> Methods
        {
            get { return _methods; }
            set
            {
                if (value == null) value = new List<CsMethod>();
                _methods = value;
            }
        }

        /// <summary>
        /// </summary>
        public List<CsClass> NestedClasses
        {
            get { return _nestedClasses; }
            set
            {
                if (value == null) value = new List<CsClass>();
                _nestedClasses = value;
            }
        }

        /// <summary>
        ///     Własność jest tylko do odczytu.
        /// </summary>
        public IList<string> ImplementedInterfaces { get; } = new List<string>();


        /// <summary>
        /// Przestrzenie nazw zadeklarowane wewnątrz klasy
        /// </summary>
        public ISet<string> Namespaces { get; set; } = new HashSet<string>();

        /// <summary>
        ///     obiekt, na podstawie którego wygenerowano klasę, przydatne przy dalszej obróbce
        /// </summary>
        public object GeneratorSource { get; set; }

        #endregion

        #region Fields

        private string _name = string.Empty;
        private string _baseClass = string.Empty;
        
        private List<CsProperty> _properties = new List<CsProperty>();
        private List<CsMethodParameter> _fields = new List<CsMethodParameter>();
        private List<CsMethod> _methods = new List<CsMethod>();
        private List<CsClass> _nestedClasses = new List<CsClass>();

        #endregion

        #region Nested

        #endregion

    }
}