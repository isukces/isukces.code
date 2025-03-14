using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using iSukces.Code.AutoCode;
using iSukces.Code.Interfaces;

namespace iSukces.Code.FeatureImplementers;

public partial class EqualityFeatureImplementer
{
    public EqualityFeatureImplementer(CsClass @class, Type type)
    {
        _class     = @class;
        _type      = type;
        MyTypeName = _class.GetTypeName(type);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string GetSuffix(int cnt) => cnt == 0 ? "" : (cnt + 1).ToCsString();

    public static string? WriteCompareTo(CsClass csClass, IReadOnlyList<CompareToExpressionData>? compareToExpressions,
        bool canBeNull)
    {
        if (compareToExpressions is null || compareToExpressions.Count == 0) return null;
        var cs = new CsCodeWriter();
        if (canBeNull)
            cs.WriteLine("if (ReferenceEquals(this, other)) return 0;")
                .WriteLine("if (other is null) return 1;");

        var typeName  = csClass.Name.Declaration;
        var comparers = new Dictionary<string, string>();
        for (var index = 0; index < compareToExpressions.Count; index++)
        {
            var c1   = compareToExpressions[index];
            var expr = c1.ExpressionTemplate;
            if (!string.IsNullOrEmpty(c1.Instance))
            {
                if (!comparers.TryGetValue(c1.Instance, out var variableName))
                {
                    comparers[c1.Instance] = variableName = $"comparer{GetSuffix(comparers.Count)}";
                    cs.WriteLine($"var {variableName} = {c1.Instance};");
                }

                expr = expr.Format(variableName);
            }

            if (index + 1 == compareToExpressions.Count)
            {
                cs.WriteLine($"return {expr};");
            }
            else
            {
                // var compar = c1.FieldName.FirstLower() + "Comparison";
                var compar = "comparisonResult"; // c1.FieldName.FirstLower() + "Comparison";
                if (index == 0)
                    cs.WriteLine($"var {compar} = {expr};");
                else
                    cs.WriteLine($"{compar} = {expr};");
                cs.WriteLine($"if ({compar} != 0) return {compar};");
            }
        }

        var m = csClass.AddMethod(nameof(IComparable<int>.CompareTo), CsType.Int32)
            .WithBody(cs);
        var ptType = csClass.Name.ToReferenceNullableIfPossible(csClass.Kind);
        var p1     = new CsMethodParameter("other", ptType);
        m.Parameters.Add(p1);

        {
            cs = new CsCodeWriter()
                .WriteLine("if (obj is null) return 1;");
            if (canBeNull)
                cs.WriteLine("if (ReferenceEquals(this, obj)) return 0;");
            cs.WriteLine(
                $"return obj is {typeName} other ? CompareTo(other) : throw new ArgumentException(\"Object must be of type {typeName}\");");
            m = csClass.AddMethod(m.Name, CsType.Int32)
                .WithBody(cs);
            m.Parameters.Add(new CsMethodParameter("obj", CsType.ObjectNullable));
        }
        return m.Name;
    }

    private void AddNeverBrowsable(IAttributable field)
    {
        var tn1 = _class.GetTypeName(typeof(DebuggerBrowsableAttribute));
        var tn2 = _class.GetTypeName(typeof(DebuggerBrowsableState));
        var attribute = new CsAttribute(tn1)
            .WithArgument(new CsDirectCode($"{tn2.Declaration}.Never"));
        field.Attributes.Add(attribute);
    }

    private void AddOperatorMethod(string opertatorName, string body, bool isExpression)
    {
        var m = _class.AddMethod(opertatorName, CsType.Bool).WithStatic().WithBody(body);
        m.IsExpressionBody = isExpression;
        m.Parameters.Add(new CsMethodParameter("left", MyTypeName));
        m.Parameters.Add(new CsMethodParameter("right", MyTypeName));
    }

    public void CreateCode()
    {
        if (ImplementFeatures.HasFlag(Features.Equality))
        {
            WriteEqualsWithObject();
            WriteEqualsWithMyType();
            WriteEqualityOperators();
            WriteGetHashCode();
        }

        if (ImplementFeatures.HasFlag(Features.CompareTo))
        {
            var methodName = WriteCompareTo();
            if (ImplementFeatures.HasFlag(Features.CompareOperators) && !string.IsNullOrEmpty(methodName))
                WriteCompareOperators(methodName);
        }
    }

    public EqualityFeatureImplementer WithCompareToExpressions(IEnumerable<CompareToExpressionData> expressions)
    {
        if (CompareToExpressions is null)
            CompareToExpressions = new List<CompareToExpressionData>();
        else
            CompareToExpressions.Clear();
        CompareToExpressions.AddRange(expressions);
        return this;
    }

    public EqualityFeatureImplementer WithEqualityExpressions(IEnumerable<EqualsExpressionData> expressions)
    {
        if (EqualityExpressions is null)
            EqualityExpressions = new List<EqualsExpressionData>();
        else
            EqualityExpressions.Clear();
        EqualityExpressions.AddRange(expressions);
        return this;
    }

    public EqualityFeatureImplementer WithGetHashCodeExpressions(
        IEnumerable<GetHashCodeExpressionDataWithMemberInfo> expressions)
    {
        if (GetHashCodeExpressions is null)
            GetHashCodeExpressions = new List<GetHashCodeExpressionDataWithMemberInfo>();
        else
            GetHashCodeExpressions.Clear();
        expressions = expressions.OrderBy(a => a);
        GetHashCodeExpressions.AddRange(expressions);
        return this;
    }

    private void WriteCompareOperators(string methodName)
    {
        var hasEqualityGeneratorAttribute = ImplementFeatures.HasFlag(Features.Equality);
        if (methodName is null)
        {
            var tmp = GeneratorsHelper.DefaultComparerMethodName(_type, _class);
            methodName = tmp.GetCode();
            foreach (var oper in CompareOperators)
            {
                var resultIfEqual = oper.Length == 2 ? "true" : "false";
                var cs            = new CsCodeWriter();
                if (hasEqualityGeneratorAttribute)
                    cs.WriteLine($"if (Equals(left, right)) return {resultIfEqual};");
                else if (CanBeNull)
                    cs.WriteLine($"if (ReferenceEquals(left, right)) return {resultIfEqual};");
                cs.WriteLine($"return {methodName}(left, right) {oper} 0;");
                AddOperatorMethod(oper, cs.Code, false);
            }
        }
        else
        {
            foreach (var oper in CompareOperators)
            {
                var resultIfEqual = oper.Length == 2 ? "true" : "false";
                var cs            = new CsCodeWriter();
                if (hasEqualityGeneratorAttribute)
                    cs.WriteLine($"if (Equals(left, right)) return {resultIfEqual};");
                else if (CanBeNull)
                    cs.WriteLine($"if (ReferenceEquals(left, right)) return {resultIfEqual};");
                // =======
                if (CanBeNull)
                {
                    var result = oper.Contains("<") ? "true" : "false";
                    cs.WriteLine("if (left is null) // null.CompareTo(NOTNULL) = -1")
                        .IncIndent()
                        .WriteLine($"return {result};")
                        .DecIndent();
                }

                cs.WriteLine($"return left.CompareTo(right) {oper} 0;");
                AddOperatorMethod(oper, cs.Code, false);
            }
        }
    }

    private string? WriteCompareTo() => WriteCompareTo(_class, CompareToExpressions, CanBeNull);

    private void WriteEqualityOperators()
    {
        AddOperatorMethod("==", "Equals(left, right)", true);
        AddOperatorMethod("!=", "!Equals(left, right)", true);
    }

    private void WriteEqualsWithMyType()
    {
        var cw = new CsCodeWriter();
        if (CanBeNull)
        {
            cw.WriteLine($"if ({OtherArgName} is null) return false;");
            cw.WriteLine($"if (ReferenceEquals(this, {OtherArgName})) return true;");
        }

        if (UseGetHashCodeInEqualityChecking)
        {
            var getHashCodeExpression = CachedGetHashCodeImplementation == GetHashCodeImplementationKind.Precomputed
                ? GetHashCodeFieldName
                : $"{nameof(GetHashCode)}()";

            cw.WriteLine(
                $"if ({getHashCodeExpression} != {OtherArgName}.{getHashCodeExpression}) return false;");
        }

        if (!string.IsNullOrEmpty(IsEmptyObjectPropertyName))
        {
            cw.WriteLine(
                $"if ({IsEmptyObjectPropertyName}) return {OtherArgName}.{IsEmptyObjectPropertyName};");
            cw.WriteLine($"if ({OtherArgName}.{IsEmptyObjectPropertyName}) return false;");
        }

        for (var i = 0; i < EqualityExpressions.Count; i++)
        {
            var code = i == 0
                ? EqualityExpressions[i].Code.GetCode(CsOperatorPrecendence.LogicalAnd, ExpressionAppend.Before)
                : EqualityExpressions[i].Code.GetCode(CsOperatorPrecendence.LogicalAnd, ExpressionAppend.After);
            var codeLine = (i == 0 ? "return " : "    && ") + code;
            if (i + 1 == EqualityExpressions.Count)
                codeLine += ";";
            cw.WriteLine(codeLine); // +" // cost "+code[i].co);
        }

        var m = _class.AddMethod("Equals", CsType.Bool)
            .WithBody(cw);
        var pt = MyTypeName.ToReferenceNullableIfPossible(_type);
        m.AddParam(OtherArgName, pt);
    }

    private void WriteEqualsWithObject()
    {
        var gettype    = $"{nameof(GetType)}()";
        var myTypeName = MyTypeName.Declaration;
        if (_type.GetTypeInfo().IsSealed)
            gettype = $"typeof({myTypeName})";
        var cw = new CsCodeWriter();
        cw.WriteLine($"if ({OtherArgName} is null) return false;");
        if (CanBeNull)
        {
            cw.WriteLine($"if (ReferenceEquals(this, {OtherArgName})) return true;");
            cw.WriteLine($"return {OtherArgName} is {myTypeName} {OtherArgName}Casted && Equals({OtherArgName}Casted);");
        }
        else
        {
            cw.WriteLine($"if ({OtherArgName}.GetType() != {gettype}) return false;");
            cw.WriteLine($"return Equals(({myTypeName}){OtherArgName});");
        }

        var m = _class.AddMethod("Equals", CsType.Bool)
            .WithOverride()
            .WithBody(cw);
        m.AddParam(OtherArgName, CsType.ObjectNullable);
    }

    private void WriteGetHashCode()
    {
        if (CachedGetHashCodeImplementation == GetHashCodeImplementationKind.Custom)
            return;
        const string flagFieldName     = "_isCachedHashCodeCalculated";
        const string calculateHashCode = "CalculateHashCode";

        var hasBoolField = CachedGetHashCodeImplementation == GetHashCodeImplementationKind.Cached;
        var hasIntField = hasBoolField
                          || CachedGetHashCodeImplementation == GetHashCodeImplementationKind.Precomputed;

        if (hasBoolField)
        {
            var field = _class.AddField(flagFieldName, CsType.Bool);
            AddNeverBrowsable(field);
        }

        if (hasIntField)
        {
            var field = _class.AddField(GetHashCodeFieldName, CsType.Int32).WithVisibility(Visibilities.Private);
            AddNeverBrowsable(field);
        }

        switch (CachedGetHashCodeImplementation)
        {
            case GetHashCodeImplementationKind.Cached:
                var cw1 = new CsCodeWriter()
                    .WriteLine($"if ({flagFieldName}) return {GetHashCodeFieldName};")
                    .WriteLine($"{GetHashCodeFieldName} = {calculateHashCode}();")
                    .WriteLine($"{flagFieldName} = true;")
                    .WriteLine($"return {GetHashCodeFieldName};");
                _class.AddMethod(nameof(GetHashCode), CsType.Int32)
                    .WithOverride()
                    .WithBody(cw1);
                break;
            case GetHashCodeImplementationKind.Precomputed:
                _class.AddMethod(nameof(GetHashCode), CsType.Int32)
                    .WithOverride()
                    .WithBodyAsExpression($"{GetHashCodeFieldName}");
                break;
        }

        var useCalcMetod = hasIntField;
        var m            = WriteGetHashCode(useCalcMetod ? calculateHashCode : nameof(GetHashCode));
        if (useCalcMetod)
            m.Visibility = Visibilities.Private;
        else
            m.Overriding = OverridingType.Override;
    }


    private CsMethod WriteGetHashCode(string methodName)
    {
        var q = WriteGetHashCodeInternal(out var isExpression);
        var m = _class.AddMethod(methodName, CsType.Int32)
            .WithBody(q);
        m.IsExpressionBody = isExpression;

        return m;

        CsCodeWriter WriteGetHashCodeInternal(out bool isExpr)
        {
            var cw          = new CsCodeWriter();
            var expressions = GetHashCodeExpressions;
            if (expressions.Count == 0)
            {
                cw.WriteLine("0");
                isExpr = true;
                return cw;
            }

            if (!string.IsNullOrEmpty(IsEmptyObjectPropertyName))
            {
                if (expressions.Count == 1)
                {
                    var q = expressions[0].Code.ExpressionWithOffset;
                    cw.WriteLine($"{IsEmptyObjectPropertyName} ? 0 : {q}");
                    isExpr = true;
                    return cw;
                }

                cw.WriteLine($"if ({IsEmptyObjectPropertyName}) return 0;");
            }

            GetHashCodeEmiter.Write(expressions, cw);
            isExpr = false;
            return cw;
        }
    }

    public List<EqualsExpressionData>    EqualityExpressions  { get; set; } = new List<EqualsExpressionData>();
    public List<CompareToExpressionData> CompareToExpressions { get; set; } = new List<CompareToExpressionData>();

    public List<GetHashCodeExpressionDataWithMemberInfo> GetHashCodeExpressions { get; set; } =
        new List<GetHashCodeExpressionDataWithMemberInfo>();

    public Features ImplementFeatures { get; set; }

    public string OtherArgName { get; set; } = "other";

    public CsType MyTypeName { get; }

    /// <summary>
    ///     Name of property that denotes if object is empty. If true then no other properties will be compared.
    /// </summary>
    public string IsEmptyObjectPropertyName { get; set; }

    public bool UseGetHashCodeInEqualityChecking { get; set; }

    public GetHashCodeImplementationKind CachedGetHashCodeImplementation { get; init; }
    public bool                          CanBeNull                       { get; init; }

    private const string GetHashCodeFieldName = "_cachedHashCode";

    public static int DefaultGethashcodeMultiply = 397;


    public static IReadOnlyList<string> CompareOperators = "> < >= <=".Split(' ');

    private readonly CsClass _class;
    private readonly Type _type;

    [Flags]
    public enum Features
    {
        None = 0,
        Equality = 1,
        CompareTo = 2,
        CompareOperators = 4,
        All = Equality | CompareTo | CompareOperators
    }
}

