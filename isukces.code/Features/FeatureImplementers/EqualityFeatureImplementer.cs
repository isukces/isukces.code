using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using iSukces.Code.AutoCode;
using iSukces.Code.Interfaces;

namespace iSukces.Code.FeatureImplementers
{
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
            if (CompareToExpressions == null)
                CompareToExpressions = new List<CompareToExpressionData>();
            else
                CompareToExpressions.Clear();
            CompareToExpressions.AddRange(expressions);
            return this;
        }

        public EqualityFeatureImplementer WithEqualityExpressions(IEnumerable<EqualsExpressionData> expressions)
        {
            if (EqualityExpressions == null)
                EqualityExpressions = new List<EqualsExpressionData>();
            else
                EqualityExpressions.Clear();
            EqualityExpressions.AddRange(expressions);
            return this;
        }

        public EqualityFeatureImplementer WithGetHashCodeExpressions(
            IEnumerable<GetHashCodeExpressionDataWithMemberInfo> expressions)
        {
            if (GetHashCodeExpressions == null)
                GetHashCodeExpressions = new List<GetHashCodeExpressionDataWithMemberInfo>();
            else
                GetHashCodeExpressions.Clear();
            expressions = expressions.OrderBy(a => a);
            GetHashCodeExpressions.AddRange(expressions);
            return this;
        }

        private void AddNeverBrowsable(IAttributable field)
        {
            var tn1 = _class.GetTypeName(typeof(DebuggerBrowsableAttribute));
            var tn2 = _class.GetTypeName(typeof(DebuggerBrowsableState));
            field.Attributes.Add(new CsAttribute(tn1).WithArgument(new CsDirectCode(tn2 + ".Never")));
        }

        private void AddOperatorMethod(string opertatorName, string body)
        {
            var m = _class.AddMethod(opertatorName, "bool").WithStatic().WithBody(body);
            m.Parameters.Add(new CsProperty("left", MyTypeName));
            m.Parameters.Add(new CsProperty("right", MyTypeName));
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
                    AddOperatorMethod(oper, cs.Code);
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
                    AddOperatorMethod(oper, cs.Code);
                }
            }
        }

        private string WriteCompareTo()
        {
            if (CompareToExpressions is null || CompareToExpressions.Count == 0) return null;
            var cs = new CsCodeWriter();
            if (CanBeNull)
                cs.WriteLine("if (ReferenceEquals(this, other)) return 0;")
                    .WriteLine("if (other is null) return 1;");

            var comparers = new Dictionary<string, string>();
            for (var index = 0; index < CompareToExpressions.Count; index++)
            {
                var c1   = CompareToExpressions[index];
                var expr = c1.ExpressionTemplate;
                if (!string.IsNullOrEmpty(c1.Instance))
                {
                    if (!comparers.TryGetValue(c1.Instance, out var variableName))
                    {
                        comparers[c1.Instance] = variableName = "comparer" + GetSuffix(comparers.Count);
                        cs.WriteLine("var " + variableName + " = " + c1.Instance + ";");
                    }

                    expr = expr.Format(variableName);
                }

                if (index + 1 == CompareToExpressions.Count)
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

            var m = _class.AddMethod(nameof(IComparable<int>.CompareTo), "int")
                .WithBody(cs);
            m.Parameters.Add(new CsProperty("other", _class.Name));

            cs = new CsCodeWriter()
                .WriteLine("if (obj is null) return 1;");
            if (CanBeNull)
                cs.WriteLine("if (ReferenceEquals(this, obj)) return 0;");
            cs.WriteLine(
                $"return obj is {MyTypeName} other ? CompareTo(other) : throw new ArgumentException(\"Object must be of type {MyTypeName}\");");
            m = _class.AddMethod(m.Name, "int")
                .WithBody(cs);
            m.Parameters.Add(new CsProperty("obj", "object"));

            return m.Name;
        }

        private void WriteEqualityOperators()
        {
            AddOperatorMethod("==", "return Equals(left, right);");
            AddOperatorMethod("!=", "return !Equals(left, right);");
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
                    : nameof(GetHashCode) + "()";

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

            var m = _class.AddMethod("Equals", "bool")
                .WithBody(cw);
            m.AddParam(OtherArgName, MyTypeName);
        }

        private void WriteEqualsWithObject()
        {
            var gettype = nameof(GetType) + "()";
            if (_type.GetTypeInfo().IsSealed)
                gettype = string.Format("typeof({0})", MyTypeName);
            var cw = new CsCodeWriter();
            cw.WriteLine($"if ({OtherArgName} is null) return false;");
            if (CanBeNull)
            {
                cw.WriteLine($"if (ReferenceEquals(this, {OtherArgName})) return true;");
                cw.WriteLine(
                    $"return {OtherArgName} is {MyTypeName} {OtherArgName}Casted && Equals({OtherArgName}Casted);");
            }
            else
            {
                cw.WriteLine($"if ({OtherArgName}.GetType() != {gettype}) return false;");
                cw.WriteLine($"return Equals(({MyTypeName}){OtherArgName});");
            }

            var m = _class.AddMethod("Equals", "bool")
                .WithOverride()
                .WithBody(cw);
            m.AddParam(OtherArgName, "object");
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
                var field = _class.AddField(flagFieldName, "bool");
                AddNeverBrowsable(field);
            }

            if (hasIntField)
            {
                var field = _class.AddField(GetHashCodeFieldName, "int").WithVisibility(Visibilities.Private);
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
                    _class.AddMethod("GetHashCode", "int")
                        .WithOverride()
                        .WithBody(cw1);
                    break;
                case GetHashCodeImplementationKind.Precomputed:
                    _class.AddMethod("GetHashCode", "int")
                        .WithOverride()
                        .WithBody($"return {GetHashCodeFieldName};");
                    break;
            }

            var useCalcMetod = hasIntField;
            var m            = WriteGetHashCode(useCalcMetod ? calculateHashCode : nameof(GetHashCode));
            if (useCalcMetod)
                m.Visibility = Visibilities.Private;
            else
                m.IsOverride = true;
        }


        private CsMethod WriteGetHashCode(string methodName)
        {
            CsCodeWriter WriteGetHashCodeInternal()
            {
                var cw          = new CsCodeWriter();
                var expressions = GetHashCodeExpressions;
                if (expressions.Count == 0)
                {
                    cw.WriteLine("return 0;");
                    return cw;
                }

                if (!string.IsNullOrEmpty(IsEmptyObjectPropertyName))
                {
                    if (expressions.Count == 1)
                    {
                        var q = expressions[0].Code.ExpressionWithOffset;
                        cw.WriteLine($"return {IsEmptyObjectPropertyName} ? 0 : {q};");
                        return cw;
                    }

                    cw.WriteLine($"if ({IsEmptyObjectPropertyName}) return 0;");
                }

                GetHashCodeEmiter.Write(expressions, cw);
                return cw;
            }

            var cw1 = WriteGetHashCodeInternal();
            var m = _class.AddMethod(methodName, "int")
                .WithBody(cw1);

            return m;
        }

        public List<EqualsExpressionData>    EqualityExpressions  { get; set; } = new List<EqualsExpressionData>();
        public List<CompareToExpressionData> CompareToExpressions { get; set; } = new List<CompareToExpressionData>();

        public List<GetHashCodeExpressionDataWithMemberInfo> GetHashCodeExpressions { get; set; } =
            new List<GetHashCodeExpressionDataWithMemberInfo>();

        public Features ImplementFeatures { get; set; }

        public string OtherArgName { get; set; } = "other";

        public string MyTypeName { get; }

        /// <summary>
        ///     Name of property that denotes if object is empty. If true then no other properties will be compared.
        /// </summary>
        public string IsEmptyObjectPropertyName { get; set; }

        public bool UseGetHashCodeInEqualityChecking { get; set; }

        public GetHashCodeImplementationKind CachedGetHashCodeImplementation { get; set; }
        public bool                          CanBeNull                       { get; set; }

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

        private const string GetHashCodeFieldName = "_cachedHashCode";
    }
}