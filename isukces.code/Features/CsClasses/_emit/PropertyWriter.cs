using System;
using System.Collections.Generic;
using System.Linq;
using iSukces.Code.Interfaces;

namespace iSukces.Code;

internal class PropertyWriter
{
    public PropertyWriter(CsClass csClass, CsProperty property)
    {
        _csClass  = csClass;
        _property = property;
        var flags = _csClass.Formatting.Flags;
        _allowExpressionBodies  = (flags & CodeFormattingFeatures.ExpressionBody) != 0;
        _allowReferenceNullable = csClass.AllowReferenceNullable();
    }

    private static string OptionalVisibility(Visibilities? memberVisibility)
    {
        var v = memberVisibility is null ? "" : memberVisibility.Value.ToString().ToLower() + " ";
        return v;
    }

    internal void EmitProperty(ICsCodeWriter writer, CodeEmitState state)
    {
        state.StartItem(writer, _property);
        writer.WriteComment(_property);

        var emitField = WriteProperty(writer);

        if (emitField && !IsInterface && !UseBackField)
        {
            var fieldType = _property.FieldTypeOverride;
            if (fieldType.IsVoid)
                fieldType = _property.Type;
            var f = new[]
            {
                _property.FieldVisibility.ToString().ToLower(),
                _property.IsStatic ? "static" : null,
                _property.IsReadOnly ? "readonly" : null,
                fieldType.AsString(_allowReferenceNullable),
                _property.PropertyFieldName,
                string.IsNullOrWhiteSpace(_property.ConstValue) ? null : $"= {_property.ConstValue}"
            };

            var code = string.Join(" ", f.Where(a => !string.IsNullOrWhiteSpace(a))) + ";";
            writer.EmptyLine().WriteLine(code);
        }

        state.WriteEmptyLine = true;
        // writer.EmptyLine();
    }

    private PropertyCodeLines GetGetterLines()
    {
        if (!string.IsNullOrEmpty(_property.OwnGetter))
            return new PropertyCodeLines(_property.OwnGetter.SplitToLines(), _property.OwnGetterIsExpression);
        if (UseBackField)
            return PropertyCodeLines.AsWriteAsAutoProperty();
        return new PropertyCodeLines(_property.PropertyFieldName, true);
    }

    private string GetPropertyHeader()
    {
        var list = new List<string>();
        if (!IsInterface && _property.Visibility != Visibilities.InterfaceDefault)
            list.Add(_property.Visibility.ToString().ToLower());
        if (_property.IsRequired)
            list.Add("required");
        if (_property.IsStatic)
            list.Add("static");
        if (!IsInterface)
        {
            if (_property.IsOverride)
                list.Add("override");
            else if (_property.IsVirtual)
                list.Add("virtual");
        }

        list.Add(_property.Type.AsString(_allowReferenceNullable));
        list.Add(_property.Name);
        var header = string.Join(" ", list);
        return header;
    }

    private PropertyCodeLines GetSetterLines()
    {
        var useField = UseBackField;
        var setter   = _property.OwnSetter;
        if (!string.IsNullOrEmpty(setter))
        {
            if (useField && _property.OwnSetterIsExpression)
            {
                if (!setter.StartsWith("field = ", StringComparison.Ordinal))
                    setter = "field = " + setter;
            }
            var split = setter.Replace("\r\n", "\n").Trim().Split('\r', '\n');
            return new PropertyCodeLines(split, _property.OwnSetterIsExpression);
        }

        if (useField)
            return new PropertyCodeLines("field = value", true)
            {
                WriteAsAutoProperty = true
            };
        return new PropertyCodeLines($"{_property.PropertyFieldName} = value", true);
    }


    private void WriteGetterOrSetter(ICsCodeWriter writer, GsKind kind)
    {
        var isGetter = kind is GsKind.Getter;
        var lines    = isGetter ? GetGetterLines() : GetSetterLines();
        if (lines is null || lines.IsEmpty) return;
        var keyWord          = isGetter ? "get" : "set";
        var memberVisibility = isGetter ? _property.GetterVisibility : _property.SetterVisibility;
        keyWord = OptionalVisibility(memberVisibility) + keyWord;
        if ((lines.IsExpressionBody || lines.WriteAsAutoProperty) && _allowExpressionBodies)
        {
            if (lines.Lines.Count != 1)
                throw new NotSupportedException("Multiline expression getter/setter");
            if (lines.WriteAsAutoProperty)
                writer.WriteLine(keyWord + ";");
            else
                lines.WriteExpressionLines(keyWord + " =>", true, writer);

            return;
        }

        if (lines.Lines.Count == 1)
        {
            var singleLine = lines.Lines[0].Trim();
            if (lines.IsExpressionBody)
                singleLine = isGetter ? $"return {singleLine};" : $"{singleLine};";

            writer.WriteLine($"{keyWord} {{ {singleLine} }}");
        }
        else
        {
            if (lines.IsExpressionBody)
                throw new NotSupportedException("Multiline expression getter/setter");
            writer.Open(keyWord);
            foreach (var iii in lines.Lines)
                writer.WriteLine(iii);
            writer.Close();
        }
    }

    private bool WriteProperty(ICsCodeWriter writer)
    {
        var header = GetPropertyHeader();
        CsClass.WriteSummary(writer, _property.Description);
        writer.WriteAttributes(_property.Attributes);

        if (IsInterface || _property.MakeAutoImplementIfPossible && string.IsNullOrEmpty(_property.OwnSetter) &&
            string.IsNullOrEmpty(_property.OwnGetter))
        {
            string gs;
            var    tmp = OptionalVisibility(_property.GetterVisibility);
            if (_property.SetterType == PropertySetter.None)
                gs = $"{{ {tmp}get; }}";
            else
            {
                var keyword = _property.SetterType == PropertySetter.Set ? "set" : "init";
                gs = $"{{ {tmp}get; {OptionalVisibility(_property.SetterVisibility)}{keyword}; }}";
            }

            var c = header + " " + gs;
            if (!IsInterface && !string.IsNullOrEmpty(_property.ConstValue))
                c += " = " + _property.ConstValue + ";";
            writer.WriteLine(c);
            return false;
        }

        var emitField = _property.EmitField && !UseBackField;

        if (_allowExpressionBodies && _property.SetterType == PropertySetter.None)
        {
            var lines = GetGetterLines();
            if (lines.IsExpressionBody)
            {
                writer.WriteLambda(header, lines.GetExpression(), _csClass.Formatting.MaxLineLength, true);
                return emitField;
            }
        }

        {
            writer.Open(header);
            {
                WriteGetterOrSetter(writer, GsKind.Getter);
                if (_property.SetterType != PropertySetter.None)
                    WriteGetterOrSetter(writer, GsKind.Setter);
            }

            var append = "";
            if (!IsInterface && !string.IsNullOrEmpty(_property.ConstValue) && !emitField)
                append = "= " + _property.ConstValue + ";";

            writer.Close(appendText: append);
        }

        return emitField;
    }

    private bool UseBackField => _property.EffectiveBackingField; 
    private bool IsInterface => _csClass.IsInterface;

    private readonly CsClass _csClass;
    private readonly CsProperty _property;
    private readonly bool _allowExpressionBodies;
    private readonly bool _allowReferenceNullable;

    private enum GsKind
    {
        Getter,
        Setter
    }
}
