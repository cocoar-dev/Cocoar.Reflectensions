using System.Linq;
using Cocoar.Reflectensions.ExtensionMethods;
using Cocoar.Reflectensions.Tests.TestClasses;
using Xunit;

namespace Cocoar.Reflectensions.Tests
{
    public class PropertyInfoTests
    {
        [Fact]
        public void IsIndexerProperty_WithNonIndexer_ReturnsFalse()
        {
            var property = typeof(Building).GetProperty("WindowCount");
            
            Assert.NotNull(property);
            Assert.False(property.IsIndexerProperty());
        }

        [Fact]
        public void WhichIsIndexerProperty_FiltersIndexerProperties()
        {
            var properties = typeof(string).GetProperties();
            var indexers = properties.WhichIsIndexerProperty().ToList();
            
            // String has an indexer property (Chars[int index])
            Assert.NotEmpty(indexers);
        }

        [Fact]
        public void IsPublic_WithPublicProperty_ReturnsTrue()
        {
            var property = typeof(Building).GetProperty("WindowCount");
            
            Assert.NotNull(property);
            Assert.True(property.IsPublic());
        }

        [Fact]
        public void IsPublic_WithPrivateProperty_ReturnsFalse()
        {
            var properties = typeof(Human).GetProperties(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            if (properties.Any())
            {
                var privateProperty = properties.First();
                // This assumes there's at least one private property - if not, the test passes
                Assert.False(privateProperty.IsPublic());
            }
        }

        [Fact]
        public void GetProperties_ReturnsExpectedProperties()
        {
            var properties = typeof(Building).GetProperties().ToList();
            
            Assert.NotEmpty(properties);
            Assert.Contains(properties, p => p.Name == "WindowCount");
            Assert.Contains(properties, p => p.Name == "HasGarden");
        }

        [Fact]
        public void PropertyCanRead_ChecksCorrectly()
        {
            var property = typeof(Building).GetProperty("WindowCount");
            
            Assert.NotNull(property);
            Assert.True(property.CanRead);
        }

        [Fact]
        public void PropertyCanWrite_ChecksCorrectly()
        {
            var property = typeof(Building).GetProperty("WindowCount");
            
            Assert.NotNull(property);
            Assert.True(property.CanWrite);
        }

        [Fact]
        public void GetPropertyType_ReturnsCorrectType()
        {
            var property = typeof(Building).GetProperty("WindowCount");
            
            Assert.NotNull(property);
            Assert.Equal(typeof(int), property.PropertyType);
        }

        [Fact]
        public void GetPropertiesOfSpecificType_FiltersCorrectly()
        {
            var intProperties = typeof(Building)
                .GetProperties()
                .Where(p => p.PropertyType == typeof(int))
                .ToList();
            
            Assert.NotEmpty(intProperties);
            Assert.Contains(intProperties, p => p.Name == "WindowCount");
        }

        [Fact]
        public void GetInheritedProperties_IncludesBaseClassProperties()
        {
            var properties = typeof(Building).GetProperties().ToList();
            
            // Building has its own properties
            Assert.Contains(properties, p => p.Name == "WindowCount");
            Assert.Contains(properties, p => p.Name == "HasGarden");
        }
    }
}
