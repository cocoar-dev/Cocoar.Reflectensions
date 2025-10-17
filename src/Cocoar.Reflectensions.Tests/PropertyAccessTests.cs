using System;
using Cocoar.Reflectensions.Exceptions;
using Cocoar.Reflectensions.ExtensionMethods;
using Cocoar.Reflectensions.Tests.TestClasses;
using Xunit;

namespace Cocoar.Reflectensions.Tests
{
    public class PropertyAccessTests
    {
        [Fact]
        public void GetPropertyValue_SimpleProperty_ReturnsCorrectValue()
        {
            var building = new Building { WindowCount = 10, HasGarden = true };
            
            var windowCount = building.Reflect().GetPropertyValue<int>("WindowCount");
            var hasGarden = building.Reflect().GetPropertyValue<bool>("HasGarden");
            
            Assert.Equal(10, windowCount);
            Assert.True(hasGarden);
        }

        [Fact]
        public void GetPropertyValue_NonExistentProperty_ThrowsException()
        {
            var building = new Building();
            
            Assert.Throws<PropertyNotFoundException>(() => building.Reflect().GetPropertyValue<string>("NonExistent"));
        }

        [Fact]
        public void GetPropertyValue_NullObject_ThrowsArgumentNullException()
        {
            Building? building = null;
            
            Assert.Throws<ArgumentNullException>(() => building!.Reflect().GetPropertyValue<string>("WindowCount"));
        }

        [Fact]
        public void SetPropertyValue_SimpleProperty_SetsValueCorrectly()
        {
            var building = new Building { WindowCount = 10 };
            
            building.Reflect().SetPropertyValue("WindowCount", 20);
            building.Reflect().SetPropertyValue("HasGarden", true);
            
            Assert.Equal(20, building.WindowCount);
            Assert.True(building.HasGarden);
        }

        [Fact]
        public void SetPropertyValue_NonExistentProperty_ThrowsPropertyNotFoundException()
        {
            var building = new Building();
            
            Assert.Throws<PropertyNotFoundException>(() => 
                building.Reflect().SetPropertyValue("NonExistent", "value"));
        }

        [Fact]
        public void GetPropertyValue_WithTypeConversion_ConvertsCorrectly()
        {
            var building = new Building { WindowCount = 100 };
            
            // Get int property as double
            var windowCountAsDouble = building.Reflect().GetPropertyValue<double>("WindowCount");
            
            Assert.Equal(100.0, windowCountAsDouble);
        }

        [Fact]
        public void GetPropertyValue_BoolProperty_ReturnsCorrectValue()
        {
            var building = new Building { HasGarden = false };
            
            var hasGarden = building.Reflect().GetPropertyValue<bool>("HasGarden");
            
            Assert.False(hasGarden);
        }

        [Fact]
        public void SetPropertyValue_BoolProperty_SetsCorrectly()
        {
            var building = new Building { HasGarden = false };
            
            building.Reflect().SetPropertyValue("HasGarden", true);
            
            Assert.True(building.HasGarden);
        }
    }
}
