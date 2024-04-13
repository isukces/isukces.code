using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using iSukces.Code.Interfaces;
using JetBrains.Annotations;

namespace iSukces.Code;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "RedundantExtendsListEntry")]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class CsClass : ClassMemberBase, IClassOwner, IConditional, ITypeNameResolver,
    IAttributable, ICommentable, IAnnotableByUser, IEnumOwner
{
    [Obsolete("Use CsType instead of string", GlobalSettings.WarnObsolete)]
    public CsClass(string name) => Name = (CsType)name;

    /// <summary>
    ///     Tworzy instancję obiektu
    ///     <param name="name">Nazwa klasy</param>
    /// </summary>
    public CsClass(CsType name) => Name = name;

    /// <summary>
    ///     Tworzy instancję obiektu
    ///     <param name="name">Nazwa klasy</param>
    ///     <param name="baseClass">klasa Bazowa</param>
    /// </summary>
    public CsClass(CsType name, CsType baseClass)
    {
        Name      = name;
        BaseClass = baseClass;
    }

    private static void Emit_single_field(ICsCodeWriter writer, CsClassField field, CodeEmitConfig config)
    {
        writer.OpenCompilerIf(field);
        writer.WriteComment(field);
        try
        {
            WriteSummary(writer, field.Description);
            writer.WriteAttributes(field.Attributes);
            var effectiveType = field.Type.AsString(config.AllowReferenceNullable);
            if (field.IsConst)
            {
                var v = field.Visibility.ToCsCode();
                if (string.IsNullOrEmpty(v))
                    v = Visibilities.Public.ToCsCode();
                writer
                    .WriteLine("{0} const {1} {2} = {3};", v, effectiveType, field.Name, field.ConstValue)
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
                att.Add(effectiveType);
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


    [Obsolete("", true)]
    public static CsAttribute MkAttribute(string attributeName) => new(attributeName);

    public static void WriteSummary(ICsCodeWriter writer, string description)
    {
        description = description?.Trim();
        if (string.IsNullOrEmpty(description)) return;
        writer.WriteLine("/// <summary>");
        var lines = description.Split('\r', '\n')
            .Where(q => !string.IsNullOrEmpty(q.Trim()));
        foreach (var line in lines)
            writer.WriteLine("/// " + line.XmlEncode());
        writer.WriteLine("/// </summary>");
    }


    public CsMethod AddBinaryOperator(string operatorName, CsType returnType)
    {
        var m = AddMethod(operatorName, returnType)
            .WithStatic()
            .WithParameter("left", Name).WithParameter("right", Name);

        return m;
    }

    public void AddComment(string x) => _extraComment.AppendLine(x);

    // Public Methods 

    public CsClassField AddConst(string name, CsType type, string encodedValue)
    {
        var constValue = new CsClassField(name, type)
        {
            ConstValue = encodedValue,
            IsConst    = true,
            Owner      = this
        };
        Fields.Add(constValue);
        return constValue;
    }

    public CsClassField AddConstInt(string name, int encodedValue) => AddConst(name, CsType.Int32, encodedValue.ToCsString());

    public CsMethod AddConstructor(string description = null)
    {
        var m = new CsMethod(GetConstructorName(), default)
        {
            Description = description,
            Owner       = this
        };
        _methods.Add(m);
        m.Kind = MethodKind.Constructor;
        return m;
    }

    public CsClassField AddConstString(string name, string plainValue)
    {
        var encodedValue = plainValue == null ? "null" : plainValue.CsEncode();
        return AddConst(name, CsType.String, encodedValue);
    }

    public CsEnum AddEnum(CsEnum csEnum)
    {
        ((List<CsEnum>)Enums).Add(csEnum);
        csEnum.Owner = this;
        return csEnum;
    }

    public CsEvent AddEvent(string name, CsType type, string description = null)
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

    public CsClassField AddField(string fieldName, Type type) => AddField(fieldName, GetTypeName(type));

    public CsClassField AddField(string fieldName, CsType type)
    {
        var field = new CsClassField(fieldName, type)
        {
            Owner = this
        };
        Fields.Add(field);
        return field;
    }

    public CsMethod AddFinalizer(string description = null)
    {
        var m = new CsMethod(GetFinalizerName(), default)
        {
            Description = description,
            Owner       = this
        };
        _methods.Add(m);
        m.Kind = MethodKind.Finalizer;
        return m;
    }

    [Obsolete("Use CsType instead of string", GlobalSettings.WarnObsolete)]
    public CsMethod AddMethod(string name, string type, string description = null) => AddMethod(name, new CsType(type), description);

    public CsMethod AddMethod(string name, Type type, string description = null)
        => AddMethod(name, type == null ? default : GetTypeName(type), description);

    public CsMethod AddMethod(string name, CsType type, string description = null)
    {
        if (string.IsNullOrEmpty(name) || name == GetConstructorName())
            return AddConstructor(description);
        if (name == GetFinalizerName())
            return AddFinalizer(description);
        var m = new CsMethod(name, type)
        {
            Description = description,
            Owner       = this
        };
        _methods.Add(m);
        return m;
    }

    public CsProperty AddProperty(string propertyName, Type type) => AddProperty(propertyName, GetTypeName(type));

    [Obsolete("Use CsType instead of string", GlobalSettings.WarnObsolete)]
    public CsProperty AddProperty(string propertyName, string type) => AddProperty(propertyName, new CsType(type));


    public CsProperty AddProperty(string propertyName, CsType type)
    {
        propertyName = propertyName?.Trim();
        if (string.IsNullOrEmpty(propertyName))
            throw new ArgumentException("propertyName is empty");
        var property = new CsProperty(propertyName, type)
        {
            Owner = this
        };
        if (propertyName.Contains('.'))
        {
            property.Visibility = Visibilities.InterfaceDefault;
            property.EmitField  = false;
        }

        if ((DefaultCodeFormatting.Flags & CodeFormattingFeatures.MakeAutoImplementIfPossible) != 0)
            property.WithMakeAutoImplementIfPossible();

        Properties.Add(property);
        return property;
    }

    public bool AllowReferenceNullable()
    {
        if (Owner is CsClass cl)
            return cl.AllowReferenceNullable();
        var file = FindFile();
        if (file is not null)
            return file.Nullable.IsNullableReferenceEnabled();

        return false;

        [CanBeNull]
        CsFile FindFile()
        {
            object owner = Owner;
            while (true)
                switch (owner)
                {
                    case CsFile csFile:
                        return csFile;
                    case CsNamespace ns:
                        owner = ns.Owner;
                        break;
                    default:
                        return null;
                }
        }
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
                        throw new Exception($"Class {Name.Modern} can't be both sealed and abstract");
                    if (IsStatic)
                        throw new Exception($"Class {Name.Modern} can't be both static and abstract");
                    x.Add("abstract");
                }
                else if (IsStatic)
                {
                    if (IsSealed)
                        throw new Exception($"Class {Name.Modern} can't be both static and sealed");
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

            case CsNamespaceMemberKind.Record:
                if (IsPartial)
                    x.Add("partial");
                x.Add("record");
                break;

            case CsNamespaceMemberKind.RecordStruct:
                if (IsPartial)
                    x.Add("partial");
                x.Add("record struct");
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        x.Add(Name.Declaration);
        return x.ToArray();
    }

    private bool Emit_constructors(ICsCodeWriter writer, bool addEmptyLineBeforeRegion)
    {
        var w = new CsClassWriter(this);
        var c = _methods
            .Where(i => i.Kind is MethodKind.Constructor or MethodKind.Finalizer)
            .OrderBy(a => a.Kind != MethodKind.Constructor)
            .ToArray();
        if (c.Length == 0)
            return addEmptyLineBeforeRegion;
        var m = c.Where(i => !i.IsStatic);
        addEmptyLineBeforeRegion = w.WriteMethods(writer, addEmptyLineBeforeRegion, m, "Constructors");

        m                        = c.Where(i => i.IsStatic);
        addEmptyLineBeforeRegion = w.WriteMethods(writer, addEmptyLineBeforeRegion, m, "Static constructors");
        return addEmptyLineBeforeRegion;
    }

    private bool Emit_events(ICsCodeWriter writer, bool addEmptyLineBeforeRegion)
    {
        if (_events.Count == 0)
            return addEmptyLineBeforeRegion;
        writer.EmptyLine(!addEmptyLineBeforeRegion);
        var w = new CsClassWriter(this);

        var allowReferenceNullable = AllowReferenceNullable();

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
                    var code = $"{v} event {ev.Type.AsString(allowReferenceNullable)} {ev.Name};".TrimStart();

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

    private bool Emit_fields(ICsCodeWriter writer, bool addEmptyLineBeforeRegion, CodeEmitConfig config)
    {
        var w   = new CsClassWriter(this);
        var all = _fields.OrderBy(a => a.IsConst).ToList();
        foreach (var i in _events.Where(a => a.LongDefinition))
        {
            var eventField = new CsClassField(i.FieldName, i.Type, i.GetFieldDescription())
            {
                Owner = this
            };
            all.Add(eventField);
        }

        if (all.Count == 0) return addEmptyLineBeforeRegion;
        writer.EmptyLine(!addEmptyLineBeforeRegion);

        addEmptyLineBeforeRegion = w.WriteMethodAction(writer, all, "Fields",
            field => { Emit_single_field(writer, field, config); }
        );
        return addEmptyLineBeforeRegion;
    }

    private bool Emit_methods(ICsCodeWriter writer, bool addEmptyLineBeforeRegion)
    {
        var w = new CsClassWriter(this);
        var c = _methods
            .Where(i => i.Kind != MethodKind.Constructor && i.Kind != MethodKind.Finalizer)
            .OrderBy(a => a.Kind)
            .ToArray();
        if (c.Length == 0) return addEmptyLineBeforeRegion;
        var m = c.Where(i => !i.IsStatic);
        addEmptyLineBeforeRegion = w.WriteMethods(writer, addEmptyLineBeforeRegion, m, "Methods");

        m                        = c.Where(i => i.IsStatic);
        addEmptyLineBeforeRegion = w.WriteMethods(writer, addEmptyLineBeforeRegion, m, "Static methods");
        return addEmptyLineBeforeRegion;
    }

    private void Emit_nested(ICsCodeWriter writer, bool addEmptyLineBeforeRegion, CodeEmitConfig config)
    {
        var w = new CsClassWriter(this);
        // ReSharper disable once InvertIf
        if (_nestedClasses.Count != 0)
        {
            writer.EmptyLine(!addEmptyLineBeforeRegion);
            w.WriteMethodAction(writer, _nestedClasses.OrderBy(a => a.Name.Declaration), "Nested classes",
                i =>
                {
                    i.MakeCode(writer, config);
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
        var w = new CsClassWriter(this);
        if (_properties.Count == 0) return addEmptyLineBeforeRegion;
        writer.EmptyLine(!addEmptyLineBeforeRegion);
        addEmptyLineBeforeRegion = w.WriteMethodAction(writer, _properties, "Properties",
            i =>
            {
                var tmp = new PropertyWriter(this, i);
                tmp.EmitProperty(writer);
            });
        return addEmptyLineBeforeRegion;
    }

    public string GetComments() => _extraComment.ToString();

    private string GetConstructorName() => Name.Constructor;

    private string GetFinalizerName() => "~" + GetConstructorName();

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

    public CsClass GetOrCreateNested(CsType typeName)
        => GetOrCreateNested(typeName, out _);

    [Obsolete("Use CsType instead of string", GlobalSettings.WarnObsolete)]
    public CsClass GetOrCreateNested(string typeName)
        => GetOrCreateNested((CsType)typeName, out _);

    public CsClass GetOrCreateNested(CsType typeName, out bool isCreatedNew)
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

    public CsType GetTypeName(Type type)
    {
        if (Owner == null)
            throw new NullReferenceException(nameof(Owner));
        var result = Owner.GetTypeName(type);
        if (Owner is not CsClass cl) return result;
        var cutBegin = cl.Name.Declaration + ".";
        var justType = result.BaseName ?? string.Empty;
        if (justType.StartsWith(cutBegin, StringComparison.Ordinal))
            result = result.WithBaseName(justType[cutBegin.Length..]);
        return result;
    }

    public bool IsKnownNamespace(string namespaceName) => Owner?.IsKnownNamespace(namespaceName) ?? false;

    public void MakeCodeForBlazor(ICsCodeWriter writer, CodeEmitConfig config, bool addWrapper)
    {
        MakeCodeInternal(writer, config, ClassEmitStyle.Blazor, addWrapper);
    }

    public void MakeCode(ICsCodeWriter writer, CodeEmitConfig config)
    {
        MakeCodeInternal(writer, config, ClassEmitStyle.Normal, false);
    }

    private void MakeCodeInternal(ICsCodeWriter writer, CodeEmitConfig config, ClassEmitStyle style,
        bool addBlazorWrapper)
    {
        var isN = style == ClassEmitStyle.Normal;
        var isB = style == ClassEmitStyle.Blazor;

        if (isN)
        {
            writer.OpenCompilerIf(CompilerDirective);
            writer.WriteComment(this);
            WriteSummary(writer, Description);
            writer.WriteAttributes(Attributes);
        }

        if (isB)
        {
            if (addBlazorWrapper)
                writer.WriteLine("@code {");
            writer.IncIndent();
        }

        var def = string.Join(" ", DefAttributes());
        var hasBody = true;
        {
            var types = new HashSet<CsType>();
            var baseAndInterfaces = new List<CsType>();
            if (!BaseClass.IsVoid)
            {
                baseAndInterfaces.Add(BaseClass);
                types.Add(BaseClass);
            }

            for (var index = 0; index < ImplementedInterfaces.Count; index++)
            {
                var interfaceName = ImplementedInterfaces[index];
                if (types.Add(interfaceName))
                    baseAndInterfaces.Add(interfaceName);
            }

            // public sealed class CsImplicitConstructor(IReadOnlyList<CsMethodParameter> arguments)
            if (PrimaryConstructor is not null)
            {
                var allowReferenceNullable = AllowReferenceNullable();
                def += CsMethodWriter.FormatParameters(PrimaryConstructor.Arguments, allowReferenceNullable);
            }

            if (baseAndInterfaces.Count != 0)
                def += " : " + baseAndInterfaces
                    .Select(a => a.Declaration)
                    .CommaJoin();

            if (Kind is CsNamespaceMemberKind.Record or CsNamespaceMemberKind.RecordStruct)
            {
                hasBody = _properties.Count != 0
                          || _fields.Count != 0
                          || _methods.Count != 0
                          || _events.Count != 0
                          || _nestedClasses.Count != 0;

                if (!hasBody && isN)
                {
                    def += ";";
                    writer.WriteLine(def);
                }
            }
        }
        if (hasBody)
        {
            if (isN)
                writer.Open(def);
            // Constructors
            var addEmptyLineBeforeRegion = Emit_constructors(writer, false);
            // Methods
            addEmptyLineBeforeRegion = Emit_methods(writer, addEmptyLineBeforeRegion);
            //Properties
            addEmptyLineBeforeRegion = Emit_properties(writer, addEmptyLineBeforeRegion);
            // Fields
            addEmptyLineBeforeRegion = Emit_fields(writer, addEmptyLineBeforeRegion, config);
            // Events
            addEmptyLineBeforeRegion = Emit_events(writer, addEmptyLineBeforeRegion);
            // Nested
            Emit_nested(writer, addEmptyLineBeforeRegion, config);
            if (isN)
                writer.Close();
        }

        if (isB)
        {
            writer.DecIndent();
            if (addBlazorWrapper)
                writer.WriteLine("}");
        }

        if (isN)
            writer.CloseCompilerIf(CompilerDirective);
    }


    public override string ToString() => "csClass " + Name.Declaration;

    public bool TrySeal(bool replaceStatic = false)
    {
        if (IsStatic)
        {
            if (replaceStatic)
                IsStatic = false;
            else
                throw new InvalidOperationException();
        }

        if (IsSealed)
            return false;
        IsSealed = true;
        return true;
    }

    // public CsClass WithAttribute<TAttribute>() => this.WithAttribute(this, typeof(TAttribute));

    public CsClass WithBaseClass(CsType baseClass)
    {
        BaseClass = baseClass;
        return this;
    }

    #region Properties

    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public static CodeFormatting DefaultCodeFormatting { get; set; } = new(CodeFormattingFeatures.MakeAutoImplementIfPossible, 100);

    public IClassOwner Owner { get; set; }

    /// <summary>
    ///     Nazwa klasy
    /// </summary>
    public CsType Name { get; }

    /// <summary>
    ///     base class
    /// </summary>
    public CsType BaseClass { get; set; }

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
        set => _properties = value ?? [];
    }

    /// <summary>
    /// </summary>
    public List<CsClassField> Fields
    {
        get => _fields;
        set => _fields = value ?? [];
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

    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public CodeFormatting Formatting { get; set; } = DefaultCodeFormatting;

    /// <summary>
    ///     Własność jest tylko do odczytu.
    /// </summary>
    public CsTypesCollection ImplementedInterfaces { get; } = new CsTypesCollection();

    /// <summary>
    ///     obiekt, na podstawie którego wygenerowano klasę, przydatne przy dalszej obróbce
    /// </summary>
    public object GeneratorSource { get; set; }

    public IReadOnlyList<CsMethod> Methods => _methods;

    public IReadOnlyList<CsEvent> Events => _events;

    [CanBeNull]
    public CsPrimaryConstructor PrimaryConstructor { get; set; }

    #endregion

    public IDictionary<string, object> UserAnnotations { get; } = new Dictionary<string, object>();
    public IReadOnlyList<CsEnum>       Enums           { get; } = new List<CsEnum>();

    #region Fields

    private readonly StringBuilder _extraComment = new();

    /// <summary>
    /// </summary>
    private readonly List<CsClass> _nestedClasses = new();

    /// <summary>
    ///     methods
    /// </summary>
    private readonly List<CsMethod> _methods = new();

    private readonly List<CsEvent> _events = new();
    private List<CsProperty> _properties = new();
    private List<CsClassField> _fields = new();

    #endregion
}

public enum ClassEmitStyle
{
    Normal,
    Blazor
}