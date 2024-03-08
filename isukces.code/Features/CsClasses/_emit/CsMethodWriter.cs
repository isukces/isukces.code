using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iSukces.Code.Interfaces;

namespace iSukces.Code
{
    internal sealed class CsMethodWriter
    {
        public CsMethodWriter(CsMethod method)
        {
            _method = method;
        }

        private static string FormatMethodParameter(CsMethodParameter i)
        {
            var sb = new StringBuilder();
            if (i.Attributes.Any())
            {
                var joioned = i.Attributes.CommaJoin();
                sb.Append($"[{joioned}] ");
            }

            if (i.UseThis)
                sb.Append("this ");
            switch (i.CallType)
            {
                case ParameterCallTypes.Output:
                    sb.Append("out ");
                    break;
                case ParameterCallTypes.Reference:
                    sb.Append("ref ");
                    break;
            }

            sb.AppendFormat("{0} {1}", i.Type, i.Name);
            if (!string.IsNullOrEmpty(i.ConstValue))
                sb.Append(" = " + i.ConstValue);
            return sb.ToString();
        }

        private void Check()
        {
            if (_method.GenericArguments != null)
            {
                if (Kind == MethodKind.Constructor)
                    throw new Exception("Construction can't have generic arguments");
                if (Kind == MethodKind.Finalizer)
                    throw new Exception("Finalizer can't have generic arguments");
            }

            var g = Kind.GetStaticInstanceStatus();
            switch (g)
            {
                case StaticInstanceStatus.Instance
                    when _method.IsStatic:
                    throw new Exception("Method marked as " + Kind + " can't be static");
                case StaticInstanceStatus.Static
                    when !_method.IsStatic:
                    throw new Exception("Method marked as " + Kind + " have to be static");
            }

            if (Kind == MethodKind.Constructor || Kind == MethodKind.Finalizer)
                if (Overriding != OverridingType.None)
                    throw new Exception("Constructor nor finalizer can't be " + Overriding);
        }

        private string GetMethodDefinition(bool inInterface)
        {
            var query = from i in _method.Parameters
                select FormatMethodParameter(i);
            var mDefinition = string.Format("{0}{2}({1})",
                string.Join(" ", GetMethodAttributes(inInterface)),
                query.CommaJoin(),
                _method.GenericArguments.GetTriangleBracketsInfo());
            return mDefinition;
        }

        private string[] GetMethodAttributes(bool inInterface)
        {
            var a = new List<string>();
            if (Name == CsMethod.Implicit || Name == CsMethod.Explicit)
            {
                //  public static implicit operator double(Force src)
                if (Visibility != Visibilities.InterfaceDefault)
                    a.Add(Visibility.ToString().ToLower());
                a.Add("static");
                a.Add(Name);
                a.Add("operator");
                a.Add(ResultType);
                return a.ToArray();
            }

            if (CsMethod.IsOperator(Name))
            {
                //  public static Meter operator +(Meter a, Meter b)
                // public static Fraction operator +(Fraction a, Fraction b)
                if (Visibility != Visibilities.InterfaceDefault)
                    a.Add(Visibility.ToString().ToLower());
                a.Add("static");
                a.Add(ResultType);
                a.Add("operator");
                a.Add(Name);
                return a.ToArray();
            }

            bool EmitVisibility()
            {
                if (Visibility == Visibilities.InterfaceDefault)
                    return false;
                if (Kind == MethodKind.Finalizer)
                    return false;
                if (Kind == MethodKind.Constructor)
                    return !IsStatic;
                return !inInterface;
            }

            if (EmitVisibility())
                a.Add(Visibility.ToString().ToLower());
            if (IsStatic)
                a.Add("static");
            if (Kind == MethodKind.Normal)
            {
                if (!inInterface)
                    switch (Overriding)
                    {
                        case OverridingType.None:
                            break;
                        case OverridingType.Virtual:
                            a.Add("virtual");
                            break;
                        case OverridingType.Abstract:
                            a.Add("abstract");
                            break;
                        case OverridingType.Override:
                            a.Add("override");
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                if (_method.IsAsync)
                    a.Add("async");
                a.Add(ResultType);
            }

            a.Add(Name);
            return a.ToArray();
        }

        /// <summary>
        ///     Tworzy kod
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public void MakeCode(ICsCodeWriter writer, CsClass owner)
        {
            var inInterface = owner.IsInterface;
            var features    = owner.Formatting;
            writer.OpenCompilerIf(_method.CompilerDirective);
            Check();
            WriteMethodDescription(writer);
            foreach (var i in _method.Attributes)
                writer.WriteLine("[{0}]", i);
            // ================
            writer.WriteComment(_method);
            // writer.SplitWriteLine(_method.GetComments());
            var mDefinition = GetMethodDefinition(inInterface);
            WriteBody();
            writer.CloseCompilerIf(_method.CompilerDirective);

            void WriteBody()
            {
                if (inInterface)
                {
                    if (Kind != MethodKind.Normal || _method.IsStatic) return;
                    if (_method.GenericArguments.HasConstraints())
                    {
                        writer.WriteLine(mDefinition);
                        _method.GenericArguments?.WriteCode(writer, true, owner);
                    }
                    else
                        writer.WriteLine(mDefinition + ";");

                    return;
                }

                if (Overriding == OverridingType.Abstract && Kind == MethodKind.Normal)
                {
                    writer.WriteLine(mDefinition + ";");
                    return;
                }

                var bodyLines = CodeLines.Parse(_method.Body, _method.IsExpressionBody);
                if (bodyLines.IsExpressionBody)
                    if (bodyLines.LinesCount != 1)
                        throw new Exception("Expression body should have exactly one line");

                var isExpressionBody = bodyLines.IsExpressionBody;
                var allowExpressionBody = isExpressionBody
                                          && (features.Flags & CodeFormattingFeatures.ExpressionBody) != 0
                                          && Kind != MethodKind.Constructor;

                if (isExpressionBody && !allowExpressionBody)
                {
                    var line = bodyLines.GetExpression();
                    var c    = _method.ResultType is "" or "void" ? $"{line};" : $"return {line};";
                    bodyLines = new CodeLines(new[] { c });
                }

                if (Kind == MethodKind.Constructor)
                {
                    writer.OpenConstructor(mDefinition, _method.BaseConstructorCall);
                    writer.WriteLines(bodyLines.Lines).Close();
                    return;
                }

                if (allowExpressionBody)
                {
                    if (_method.GenericArguments.HasConstraints())
                    {
                        var w = new CsCodeWriter();
                        WriteMDefinition(w);
                        writer.WriteLambda("", bodyLines.GetExpressionLines("", false), features.MaxLineLength);
                    }
                    else
                        writer.WriteLambda(mDefinition, bodyLines.GetExpression(), features.MaxLineLength, true);
                }
                else
                {
                    WriteMDefinition(writer);
                    writer.WriteLine(writer.LangInfo.OpenText);
                    writer.IncIndent().WriteLines(bodyLines.Lines).Close();

                }
                    
                void WriteMDefinition(ICsCodeWriter xWriter)
                {
                    xWriter.WriteLine(mDefinition);
                    var gen = _method.GenericArguments;
                    if (gen.HasConstraints())
                        gen?.WriteCode(xWriter, false, owner);
                }
            }
        }

        private void WriteMethodDescription(ICsCodeWriter writer)
        {
            var anyParameterHasDescription = _method.Parameters.Any(a => !string.IsNullOrEmpty(a.Description));
            var hasMethodDescription       = !string.IsNullOrEmpty(_method.Description);
            if (!hasMethodDescription && !anyParameterHasDescription) return;
            if (hasMethodDescription)
            {
                writer.WriteLine("/// <summary>");
                var lines = _method.Description.Replace("\r\n", "\n").Split('\r', '\n');
                foreach (var i in lines)
                    writer.WriteLine("/// " + i.XmlEncode());
                writer.WriteLine("/// </summary>");
            }

            foreach (var i in _method.Parameters)
                writer.WriteLine("/// <param name=\"{0}\">{1}</param>", i.Name.XmlEncode(),
                    i.Description.XmlEncode());
        }

        #region properties

        private bool           IsStatic   => _method.IsStatic;
        private MethodKind     Kind       => _method.Kind;
        private OverridingType Overriding => _method.Overriding;
        private Visibilities   Visibility => _method.Visibility;
        private string         Name       => _method.Name;
        private string         ResultType => _method.ResultType;

        #endregion

        #region Fields

        private readonly CsMethod _method;

        #endregion
    }
}
