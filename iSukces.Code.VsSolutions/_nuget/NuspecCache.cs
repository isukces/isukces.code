using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace iSukces.Code.VsSolutions
{
    public class NuspecCache
    {
        #region Static Methods

        public static Dictionary<string, Nuspec> GetForDirectory(DirectoryInfo directory)
        {
            var result = new Dictionary<string, Nuspec>(StringComparer.OrdinalIgnoreCase);
            try
            {
                var fn     = GetFileName(directory);
                if (!fn.Exists)
                    return result;
                var fromFile = JsonHelper.Load<Dictionary<string, Nuspec>>(fn);
                if (fromFile != null)
                    foreach (var i in fromFile)
                        result[i.Key] = i.Value;
                return result;
            }
            catch
            {
                return result;
            }
        }

        public static void Save(DirectoryInfo directory, Dictionary<string, Nuspec> cache)
        {
            var fn = GetFileName(directory);
            fn.Directory?.Create();
            using(var fileStream = File.Open(fn.FullName, FileMode.Create))
            using(var streamWriter = new StreamWriter(fileStream))
            using(var jsonTextWriter = new JsonTextWriter(streamWriter))
            {
                jsonTextWriter.Formatting =  Formatting.Indented;
                var serializer = JsonHelper.DefaultSerializerFactory();
                serializer.Converters.Add(new DirectoryInfoConverter());
                serializer.Serialize(jsonTextWriter, cache);
            }
        }

        private static FileInfo GetFileName(DirectoryInfo directory) =>
            new FileInfo(Path.Combine(directory.FullName, "$packageScannerCache.cache"));

        #endregion
    }

    public class DirectoryInfoConverter : JsonConverter<DirectoryInfo>
    {
        public override void WriteJson(JsonWriter writer, DirectoryInfo value, JsonSerializer serializer)
        {
            writer.WriteValue(value.FullName);
        }

        public override DirectoryInfo ReadJson(JsonReader reader, Type objectType, DirectoryInfo existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            return new DirectoryInfo((string)reader.Value);
        }
    }
}