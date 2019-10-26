using System;
using System.IO;
using System.Xml.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace isukces.code.vssolutions
{
    internal static class FileHelper
    {
        public static void CheckValidForRead(this FileInfo file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));
            if (!file.Exists)
                throw new FileNotFoundException(string.Format("File {0} doesn't exist", file.FullName));
        }

        public static JsonSerializer DefaultSerializerFactory()
        {
            var serializer = new JsonSerializer();
            serializer.Converters.Add(new StringEnumConverter());
            serializer.NullValueHandling    = NullValueHandling.Ignore;
            serializer.DefaultValueHandling = DefaultValueHandling.Ignore;
            return serializer;
        }

        public static XDocument Load([NotNull] FileName location)
        {
            if (location == null) throw new ArgumentNullException(nameof(location));
            lock(Locking.Lock)
            {
                return XDocument.Load(location.FullName);
            }
        }
    }
}