using System;
using Newtonsoft.Json;

namespace isukces.code.vssolutions
{
    internal class StringVersionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(Version);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var value = reader.Value;
            return value == null ? null : Version.Parse(value.ToString());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var txt = ((Version)value).ToString();
            writer.WriteValue(txt);
        }
    }
}