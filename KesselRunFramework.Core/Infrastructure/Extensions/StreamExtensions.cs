using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace KesselRunFramework.Core.Infrastructure.Extensions
{
    public static class StreamExtensions
    {
        //public static async Task SerializeToJsonAndWrite<T>(this Stream stream, T package)
        //{
        //    if (stream == null) throw new ArgumentNullException(nameof(stream));
        //    if (package == null) throw new ArgumentNullException(nameof(package));

        //    if (!stream.CanRead)
        //        throw new NotSupportedException("It is not possible to read from this stream");

        //    using (var streamWriter = new StreamWriter(stream, Encoding.UTF8, 1024, true))
        //    {
                
        //        JsonSerializer.Serialize<T>(stream, package);
        //        jsonTextWriter.Flush();
        //    }
        //}
    }
}
