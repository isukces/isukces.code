using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace isukces.code.vssolutions
{
    public  class JsonHelper
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public static JsonSerializer DefaultSerializerFactory()
        {
            var serializer = new JsonSerializer();
            serializer.Converters.Add(new StringEnumConverter());
            serializer.NullValueHandling    = NullValueHandling.Ignore;
            serializer.DefaultValueHandling = DefaultValueHandling.Ignore;
            return serializer;
        }


        public static T Load<T>(FileInfo file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));
            if (!file.Exists)
                return default(T);

            using(var fileStream = File.Open(file.FullName, FileMode.Open))
            using(var reader = new StreamReader(fileStream))
            using(var textReader = new JsonTextReader(reader))
            {
                var serializer = DefaultSerializerFactory();
                var result     = serializer.Deserialize<T>(textReader);
                return result;
            }
        }

        public static void Save<T>(FileInfo file, T data, Formatting f = Formatting.Indented)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));
            using(var fileStream = File.Open(file.FullName, FileMode.Create))
            using(var streamWriter = new StreamWriter(fileStream))
            using(var jsonTextWriter = new JsonTextWriter(streamWriter))
            {
                jsonTextWriter.Formatting = f; // Formatting.Indented;
                var serializer = DefaultSerializerFactory();
                serializer.Serialize(jsonTextWriter, data);
            }
        }

        private static JsonSerializer MySerializerFactory()
        {
            var sf = FileHelper.DefaultSerializerFactory();
            sf.Converters.Add(new MyVersionConverter());
            return sf;
        }


        private  class MyVersionConverter : JsonConverter
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
}