#nullable disable
using System;
using Newtonsoft.Json;

namespace iSukces.Code.VsSolutions;

internal class StringVersionConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) => objectType == typeof(Version);

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
        JsonSerializer serializer)
    {
        var value = reader.Value?.ToString();
        return value == null ? null : Version.Parse(value);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var txt = ((Version)value).ToString();
        writer.WriteValue(txt);
    }
}
