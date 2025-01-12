using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace iSukces.Code;

public class MethodInfoConverter
{
    private MethodInfoConverter()
    {
    }

    public static CodeDocumentationKey? GetKey(MethodInfo? m)
    {
        if (m is null)
            return null;
        var converter = new MethodInfoConverter();
        var name      = converter.GetCodeInternal(m);
        var key       = new CodeDocumentationKey(CodeDocumentationKind.Method, name);
        return key;
    }

    public static string TypeToString(Type? type)
    {
        if (type is null)
            return "";

        var typeIsByRef = type.IsByRef;
        if (typeIsByRef)
        {
            type = type.GetElementType();
            if (type is null)
                throw new Exception("type.GetElementType() returned null");
        }

        var hasDeclaringType = type.DeclaringType is not null;
        if (!type.IsGenericType)
        {
            var x = hasDeclaringType
                ? $"{TypeToString(type.DeclaringType)}.{type.Name}"
                : type.ToString();
            return typeIsByRef ? $"{x}@" : x;
        }

        var sb = new StringBuilder();
        if (hasDeclaringType)
        {
            sb.Append(type.DeclaringType);
            sb.Append(".");
        }

        var typeFullName = type.FullName;
        if (typeFullName is null)
            throw new Exception("type.FullName returned null");
        sb.Append(typeFullName.Split('`')[0]);
        sb.Append("{");
        var genericArguments = type
            .GetGenericArguments()
            .Select(TypeToString);
        var join = string.Join(",", genericArguments);
        sb.Append(join);
        sb.Append("}");
        if (typeIsByRef)
            sb.Append("@");
        return sb.ToString();
    }

    private void Append(Type? type)
    {
        _sb!.Append(TypeToString(type));
    }

    private string GetCodeInternal(MethodInfo method)
    {
        _sb = new StringBuilder();
        Append(method.DeclaringType);
        _sb.Append(".");
        _sb.Append(method.Name);
        var parameters = method.GetParameters();
        if (parameters.Length == 0)
            return _sb.ToString();
        _sb.Append("(");
        var pars = parameters
            .Select(parameterInfo =>
            {
                var parameterType = parameterInfo.ParameterType;
                var b             = TypeToString(parameterType);
                return b;
            });
        var join = string.Join(",", pars);
        _sb.Append(join);
        _sb.Append(")");
        return _sb.ToString();
    }

    #region Fields

    private StringBuilder? _sb;

    #endregion
}
