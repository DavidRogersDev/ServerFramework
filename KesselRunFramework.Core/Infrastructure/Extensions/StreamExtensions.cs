using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace KesselRunFramework.Core.Infrastructure.Extensions
{
    public static class StreamExtensions
    {
        public static T ReadAndDeserializeFromJson<T>(this Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            if (!stream.CanRead)
                throw new NotSupportedException("It is not possible to read from this stream.");

            using (var streamReader = new StreamReader(stream))
            using (var jsonTextReader = new JsonTextReader(streamReader))
            {
                var jsonSerializer = new JsonSerializer();

                return jsonSerializer.Deserialize<T>(jsonTextReader);
            }
        }

        public static void SerializeToJsonAndWrite<T>(this Stream stream, T package)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (package == null) throw new ArgumentNullException(nameof(package));

            if (!stream.CanRead)
                throw new NotSupportedException("It is not possible to read from this stream");

            using (var streamWriter = new StreamWriter(stream, Encoding.UTF8, 1024, true))
            using (var jsonTextWriter = new JsonTextWriter(streamWriter))
            {
                var jsonSerializer = new JsonSerializer();
                jsonSerializer.Serialize(jsonTextWriter, package);
                jsonTextWriter.Flush();
            }
        }
    }
}
