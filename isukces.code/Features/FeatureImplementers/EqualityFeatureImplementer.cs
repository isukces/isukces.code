using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using isukces.code.interfaces;

namespace isukces.code.AutoCode
{
    public class EqualityFeatureImplementer
    {
        public EqualityFeatureImplementer(CsClass @class, Type type)
        {
            _class     = @class;
            _type      = type;
            MyTypeName = _class.GetTypeName(type);
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

        public EqualityFeatureImplementer WithCompareToExpressions(IEnumerable<C1> expressions)
        {
            if (CompareToExpressions == null)
                CompareToExpressions = new List<C1>();
            else
                CompareToExpressions.Clear();
            CompareToExpressions.AddRange(expressions);
            return this;
        }

        public EqualityFeatureImplementer WithEqualityExpressions(IEnumerable<CodePieceWithComputeCost> expressions)
        {
            if (EqualityExpressions == null)
                EqualityExpressions = new List<CodePieceWithComputeCost>();
            else
                EqualityExpressions.Clear();
            EqualityExpressions.AddRange(expressions);
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
                methodName = GeneratorsHelper.DefaultComparerMethodName(_type, _class);
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

            for (var index = 0; index < CompareToExpressions.Count; index++)
            {
                var c1 = CompareToExpressions[index];
                if (index + 1 == CompareToExpressions.Count)
                {
                    cs.WriteLine($"return {c1.CompareExpression};");
                }
                else
                {
                    var compar = c1.FieldName.FirstLower() + "Comparison";
                    cs.WriteLine($"var {compar} = {c1.CompareExpression};")
                        .WriteLine($"if ({compar} != 0) return {compar};");
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

            if (!string.IsNullOrEmpty(IsEmptyObjectPropertyName))
            {
                cw.WriteLine(
                    $"if ({IsEmptyObjectPropertyName}) return {OtherArgName}.{IsEmptyObjectPropertyName};");
                cw.WriteLine($"if ({OtherArgName}.{IsEmptyObjectPropertyName}) return false;");
            }

            for (var i = 0; i < EqualityExpressions.Count; i++)
            {
                var code = (i == 0 ? "return " : "    && ") + EqualityExpressions[i].GetCode();
                if (i + 1 == EqualityExpressions.Count)
                    code += ";";
                cw.WriteLine(code); // +" // cost "+code[i].co);
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
            const string fieldName         = "_cachedHashCode";
            const string fieldName2        = "_isCachedHashCodeCalculated";
            const string calculateHashCode = "CalculateHashCode";

            if (CachedGetHashCodeImplementation)
            {
                var field = _class.AddField(fieldName, "int");
                AddNeverBrowsable(field);
                field = _class.AddField(fieldName2, "bool");
                AddNeverBrowsable(field);

                var cw1 = new CsCodeWriter()
                    .WriteLine($"if ({fieldName2}) return {fieldName};")
                    .WriteLine($"{fieldName} = {calculateHashCode}();")
                    .WriteLine($"{fieldName2} = true;")
                    .WriteLine($"return {fieldName};");
                _class.AddMethod("GetHashCode", "int")
                    .WithOverride()
                    .WithBody(cw1);
            }

            var m = WriteGetHashCode(CachedGetHashCodeImplementation ? calculateHashCode : nameof(GetHashCode));
            if (CachedGetHashCodeImplementation)
                m.Visibility = Visibilities.Private;
            else
                m.IsOverride = true;
        }


        private CsMethod WriteGetHashCode(string methodName)
        {
            var cw = new CsCodeWriter();
            if (!string.IsNullOrEmpty(IsEmptyObjectPropertyName))
                cw.WriteLine($"if ({IsEmptyObjectPropertyName}) return 0;");

            switch (GetHashCodeExpressions.Count)
            {
                case 0:
                    cw.WriteLine("return 0;");
                    break;
                case 1:
                    cw.WriteLine($"return {GetHashCodeExpressions[0].Code};");
                    break;
                case 2:
                    cw.Open("unchecked");
                    cw.WriteLine($"return ({GetHashCodeExpressions[0].BracketsCode} * 397) ^ {GetHashCodeExpressions[1].BracketsCode};");
                    cw.Close();
                    break;
                default:
                {
                    cw.Open("unchecked");
                    {
                        for (var i = 0; i < GetHashCodeExpressions.Count; i++)
                        {
                            var hc = GetHashCodeExpressions[i];
                            cw.WriteLine(i == 0
                                ? $"var hashCode = {hc.Code};"
                                : $"hashCode = (hashCode * 397) ^ {hc.BracketsCode};");
                        }

                        cw.WriteLine("return hashCode;");
                    }
                    cw.Close();
                    break;
                }
            }

            var m = _class.AddMethod(methodName, "int")
                .WithBody(cw);

            return m;
        }

        public List<CodePieceWithComputeCost> EqualityExpressions { get; set; } = new List<CodePieceWithComputeCost>();
        public List<C1> CompareToExpressions { get; set; } = new List<C1>();
        public List<GetHashCodeExpressionData> GetHashCodeExpressions { get; set; } = new List<GetHashCodeExpressionData>();
        
        public Features ImplementFeatures { get; set; }

        public string OtherArgName { get; set; } = "other";

        public string MyTypeName { get; }

        /// <summary>
        ///     Name of property that denotes if object is empty. If true then no other properties will be compared.
        /// </summary>
        public string IsEmptyObjectPropertyName { get; set; }

        public bool     CachedGetHashCodeImplementation { get; set; }
        public bool     CanBeNull                       { get; set; }
        


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

        public struct C1
        {
            public C1(string fieldName, string compareExpression)
            {
                FieldName         = fieldName;
                CompareExpression = compareExpression;
            }

            public string FieldName         { get; }
            public string CompareExpression { get; }
        }

        public EqualityFeatureImplementer WithGetHashCodeExpressions(IEnumerable<GetHashCodeExpressionData> expressions)
        {
            if (GetHashCodeExpressions == null)
                GetHashCodeExpressions = new List<GetHashCodeExpressionData>();
            else
                GetHashCodeExpressions.Clear();
            GetHashCodeExpressions.AddRange(expressions);
            return this;
        }
    }
}