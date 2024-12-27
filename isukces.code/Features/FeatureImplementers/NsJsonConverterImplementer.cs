#nullable enable
using System;
using iSukces.Code.Interfaces;

namespace iSukces.Code.FeatureImplementers;

using WrappedTypeKind = CommonKeyType.Kind;
using JsonMaker = NsJsonConverterImplementer.PrimitiveJsonConverterMaker;

// ReSharper disable once ClassNeverInstantiated.Global
public class NsJsonConverterImplementer
{
    public NsJsonConverterImplementer(CsClass jsonConverterClass, CsType keyType)
    {
        _jsonConverterClass = jsonConverterClass;
        _keyType            = keyType;
        ReadBody            = "";
        WriteBody           = "";
    }

    private CsType GetTypeName(string namespaceName, string shortName)
    {
        var info = _jsonConverterClass.GetNamespaceInfo(namespaceName);
        return info.SearchResult == NamespaceSearchResult.Found
            ? info.AddAlias(shortName)
            : new CsType(namespaceName, shortName);
    }

    public void Make()
    {
        var cl = _jsonConverterClass;
        AutocodeTools.Seal(cl);
        cl.BaseClass = GetTypeName(NsJson, "JsonConverter");
        // CanConvert
        var m = cl.AddMethod("CanConvert", CsType.Bool)
            .WithOverride()
            .WithBodyAsExpression($"objectType == {_keyType.TypeOf()}");
        m.AddParam("objectType", (CsType)"Type");

        var body = ReadBody.Trim();
        m = cl.AddMethod("ReadJson", CsType.ObjectNullable)
            .WithOverride()
            .WithBody(body);
        m.AddParam("reader", (CsType)"JsonReader");
        m.AddParam("objectType", (CsType)"Type");
        m.AddParam("existingValue", (CsType)"object?");
        m.AddParam("serializer", (CsType)"JsonSerializer");

        m = cl.AddMethod("WriteJson", CsType.Void).WithOverride().WithBody(WriteBody);
        m.AddParam("writer", (CsType)"JsonWriter");
        m.AddParam("value", (CsType)"object?");
        m.AddParam("serializer", (CsType)"JsonSerializer");
    }


    public void Setup(WrappedTypeKind kind)
    {
        var declaration = _keyType.Declaration;
        {
            var append = string.Empty;
            if (kind == WrappedTypeKind.Guid)
                append = ".ToString(\"N\")";
            var write = new CsCodeWriter()
                .SingleLineIfThrow("value is null",
                    _jsonConverterClass.GetTypeName<NullReferenceException>(),
                    "value is null".CsEncode())
                .WriteLine($"var v = ({declaration})value;")
                .WriteLine($"writer.WriteValue(v.Value{append});");
            WriteBody = write.Code;
        }

        {
            var a = kind switch
            {
                WrappedTypeKind.Int => JsonMaker.IntCode,
                WrappedTypeKind.ULong => JsonMaker.ULongCode,
                WrappedTypeKind.Long => JsonMaker.LongCode,
                WrappedTypeKind.Guid => JsonMaker.GuidCode,
                WrappedTypeKind.String => JsonMaker.StringCode,
                _ => throw new NotSupportedException(kind.ToString())
            };
            a = a.Replace(JsonMaker.ReplaceKey, declaration);

            var d = kind != WrappedTypeKind.String ? $"{declaration}?" : declaration;
            a        = a.Replace(JsonMaker.ReplaceKeyValueNullable, d);
            ReadBody = a;
        }
    }

    #region Properties

    public string ReadBody  { get; set; }
    public string WriteBody { get; set; }

    #endregion

    #region Fields

    private const string NsJson = "Newtonsoft.Json";
    private readonly CsType _keyType;
    private readonly CsClass _jsonConverterClass;

    #endregion

    public static class PrimitiveJsonConverterMaker
    {
        #region Fields

        public const string ReplaceKey = "<TYPE_NAME>";
        public const string ReplaceKeyValueNullable = "<TYPE_NAME_NULLABLE>";

        public const string IntCode = @"
return reader.Value switch
{
    long l => new <TYPE_NAME>((int)l),
    int i => new <TYPE_NAME>(i),
    null when objectType == typeof(<TYPE_NAME_NULLABLE>) => null,
    _ => throw new NotImplementedException()
};
";

        public const string ULongCode = @"
return reader.Value switch
{
    ulong ul => new <TYPE_NAME>(ul),
    long l => new <TYPE_NAME>((ulong)l),
    int i => new <TYPE_NAME>((ulong)i),
    null when objectType == typeof(<TYPE_NAME_NULLABLE>) => null,
    _ => throw new NotImplementedException()
};
";

        public const string LongCode = @"
return reader.Value switch
{
    ulong ul => new <TYPE_NAME>((long)ul),
    long l => new <TYPE_NAME>(l),
    int i => new <TYPE_NAME>((long)i),
    null when objectType == typeof(<TYPE_NAME_NULLABLE>) => null,
    _ => throw new NotImplementedException()
};
";

        public const string GuidCode = @"
return reader.Value switch
{
    string stringValue => new <TYPE_NAME>(Guid.Parse(stringValue)),
    null when objectType == typeof(<TYPE_NAME_NULLABLE>) => null,
    _ => throw new NotImplementedException()
};
";

        public const string StringCode = @"
return reader.Value switch
{
    string string<TYPE_NAME> => new <TYPE_NAME>(string<TYPE_NAME>),
    null => null,
    _ => throw new NotImplementedException()
};
";

        #endregion
    }
}
