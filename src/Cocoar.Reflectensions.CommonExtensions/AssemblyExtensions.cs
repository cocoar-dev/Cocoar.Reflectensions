using System.IO;
using System.Reflection;

namespace Cocoar.Reflectensions.Common
{
    public static class AssemblyExtensions
    {
        public static Stream? ReadResourceAsStream(this Assembly assembly, string fileName)
        {
            return assembly.GetManifestResourceStream(fileName);
        }
        
        public static string? ReadResourceAsString(this Assembly assembly, string fileName)
        {
            using var stream = assembly.ReadResourceAsStream(fileName);
            if (stream == null)
                return null;
            using var streamReader = new StreamReader(stream);
            return streamReader.ReadToEnd();
        }

        public static byte[]? ReadResourceAsByteArray(this Assembly assembly, string fileName)
        {
            using var stream = assembly.ReadResourceAsStream(fileName);
            if (stream == null)
                return null;

            var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }

    }
}
