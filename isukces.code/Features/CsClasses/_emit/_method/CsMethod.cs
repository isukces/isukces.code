using System;
using System.Collections.Generic;
using System.Text;
using iSukces.Code.Interfaces;
using JetBrains.Annotations;

namespace iSukces.Code;

public class CsMethod : ClassMemberBase, ICommentable, IAnnotableByUser, IGenericDefinition
{
    static CsMethod()
    {
        Operators = new HashSet<string>();
        const string tmp = "+, -, !, ~, ++, --, +, -, *, /, %, &, |, ^, <<, >>,==, !=, <, >, <=, >=,&&, ||";
        foreach (var i in tmp.Split(','))
            Operators.Add(i.Trim());
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
    public CsMethod(string name, CsType resultType)
    {
        Name       = name;
        ResultType = resultType;
        if (IsOperator(name))
        {
            Kind     = MethodKind.Operator;
            IsStatic = true;
        }
    }

    public static bool IsOperator(string name) => Operators.Contains(name);

    public void AddComment(string x)
    {
        _extraComment.AppendLine(x);
    }

    public CsMethodParameter AddParam(string name, CsType type, string description = null)
    {
        var parameter = new CsMethodParameter(name, type, description);
        _parameters.Add(parameter);
        return parameter;
    }

    public CsMethodParameter AddParam<T>(string name, CsClass owner, string description = null)
        => AddParam(name, typeof(T), owner, description);

    public CsMethodParameter AddParam(string name, Type type, ITypeNameResolver resolver, string description = null)
    {
        var parameter = new CsMethodParameter(name, resolver.GetTypeName(type), description);
        _parameters.Add(parameter);
        return parameter;
    }

    public string GetComments() => _extraComment.ToString();

    public CsMethod WithAsync(bool isAsync = true)
    {
        IsAsync = isAsync;
        return this;
    }

    public CsMethod WithBodyAsExpression(string body)
    {
        IsExpressionBody = true;
        Body             = body;
        return this;
    }

    #region Properties

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
            switch (_name)
            {
                case Implicit:
                    Kind       = MethodKind.Implicit;
                    IsStatic   = true;
                    Overriding = OverridingType.None;
                    break;
                case Explicit:
                    Kind       = MethodKind.Explicit;
                    Overriding = OverridingType.None;
                    IsStatic   = true;
                    break;
                default:
                    if (IsOperator(_name))
                    {
                        Kind       = MethodKind.Operator;
                        Overriding = OverridingType.None;
                        IsStatic   = true;
                    }

                    break;
            }
        }
    }

    /// <summary>
    /// </summary>
    public CsType ResultType { get; set; }
    

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

    public bool IsExpressionBody { get; set; }

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

    public bool IsAsync { get; set; }

    #endregion

    public IDictionary<string, object> UserAnnotations { get; } = new Dictionary<string, object>();

    [CanBeNull]
    public CsGenericArguments GenericArguments { get; set; }

    #region Fields

    public const string Implicit = "implicit";
    public const string Explicit = "explicit";

    private static readonly HashSet<string> Operators;
    private readonly StringBuilder _extraComment = new StringBuilder();

    private string _name = string.Empty;
    private List<CsMethodParameter> _parameters = new List<CsMethodParameter>();
    private string _body = string.Empty;
    private string _baseConstructorCall = string.Empty;
    private MethodKind _kind;

    #endregion
}