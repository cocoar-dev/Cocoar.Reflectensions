using System.Reflection;
using System.Text;
using Xunit;

namespace Cocoar.Reflectensions.Tests
{
    public class AssemblyExtensionsTests
    {
        [Fact]
        public void ReadResourceAsStream_NonExistentResource_ReturnsNull()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var result = assembly.ReadResourceAsStream("NonExistent.Resource");

            Assert.Null(result);
        }

        [Fact]
        public void ReadResourceAsString_NonExistentResource_ReturnsNull()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var result = assembly.ReadResourceAsString("NonExistent.Resource");

            Assert.Null(result);
        }

        [Fact]
        public void ReadResourceAsByteArray_NonExistentResource_ReturnsNull()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var result = assembly.ReadResourceAsByteArray("NonExistent.Resource");

            Assert.Null(result);
        }

        [Fact]
        public void ReadResourceMethods_WithValidAssembly_DoNotThrow()
        {
            var assembly = Assembly.GetExecutingAssembly();

            // These should not throw even with non-existent resources
            var stream = assembly.ReadResourceAsStream("test");
            var str = assembly.ReadResourceAsString("test");
            var bytes = assembly.ReadResourceAsByteArray("test");

            Assert.Null(stream);
            Assert.Null(str);
            Assert.Null(bytes);
        }
    }
}
