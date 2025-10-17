using System;
using System.Linq;
using System.Reflection;
using Cocoar.Reflectensions.Internal;
using Cocoar.Reflectensions.Tests.TestClasses;
using Xunit;

namespace Cocoar.Reflectensions.Tests.Internal
{
    public class ExpandableObjectPropBagTests
    {
        [Fact]
        public void Constructor_WithNoArguments_InitializesWithSelf()
        {
            var propBag = new ExpandableObjectPropBag();
            
            Assert.NotNull(propBag._instance);
            Assert.NotNull(propBag._instanceType);
            Assert.Same(propBag, propBag._instance);
            Assert.Equal(typeof(ExpandableObjectPropBag), propBag._instanceType);
        }

        [Fact]
        public void Constructor_WithInstance_InitializesWithProvidedInstance()
        {
            var testInstance = new Expandable1 { Name = "Test", Age = 42 };
            var propBag = new ExpandableObjectPropBag(testInstance);
            
            Assert.NotNull(propBag._instance);
            Assert.NotNull(propBag._instanceType);
            Assert.Same(testInstance, propBag._instance);
            Assert.Equal(typeof(Expandable1), propBag._instanceType);
        }

        [Fact]
        public void Constructor_WithNullInstance_InitializesWithSelf()
        {
            var propBag = new ExpandableObjectPropBag(null);
            
            Assert.NotNull(propBag._instance);
            Assert.NotNull(propBag._instanceType);
            Assert.Same(propBag, propBag._instance);
            Assert.Equal(typeof(ExpandableObjectPropBag), propBag._instanceType);
        }

        [Fact]
        public void InstancePropertyInfo_GetsPublicInstanceProperties()
        {
            var testInstance = new Expandable1 { Name = "Test", Age = 42 };
            var propBag = new ExpandableObjectPropBag(testInstance);
            
            var properties = propBag.InstancePropertyInfo;
            
            Assert.NotNull(properties);
            Assert.Contains(properties, p => p.Name == "Name");
            Assert.Contains(properties, p => p.Name == "Age");
        }

        [Fact]
        public void InstancePropertyInfo_ExcludesExpandableBaseObjectProperties()
        {
            var testInstance = new Expandable1();
            var propBag = new ExpandableObjectPropBag(testInstance);
            
            var properties = propBag.InstancePropertyInfo;
            
            Assert.DoesNotContain(properties, p => p.DeclaringType == typeof(ExpandableBaseObject));
        }

        [Fact]
        public void InstancePropertyInfo_UsesPublicAndIgnoreCase()
        {
            var testInstance = new Expandable1();
            var propBag = new ExpandableObjectPropBag(testInstance);
            
            var properties = propBag.InstancePropertyInfo;
            
            // Verify properties are retrieved with correct binding flags
            var expectedProperties = typeof(Expandable1)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase)
                .Where(p => p.DeclaringType != typeof(ExpandableBaseObject))
                .ToArray();
            
            Assert.Equal(expectedProperties.Length, properties.Length);
        }

        [Fact]
        public void InstancePropertyInfo_IsCached()
        {
            var testInstance = new Expandable1();
            var propBag = new ExpandableObjectPropBag(testInstance);
            
            var properties1 = propBag.InstancePropertyInfo;
            var properties2 = propBag.InstancePropertyInfo;
            
            Assert.Same(properties1, properties2);
        }

        [Fact]
        public void PropertiesDictionary_InitializesEmpty()
        {
            var propBag = new ExpandableObjectPropBag();
            
            Assert.NotNull(propBag.__properties);
            Assert.Empty(propBag.__properties);
        }

        [Fact]
        public void PropertiesDictionary_SupportsCaseInsensitiveKeys()
        {
            var propBag = new ExpandableObjectPropBag();
            
            propBag.__properties["TestKey"] = "value1";
            propBag.__properties["testkey"] = "value2";
            
            // Should only have one key due to case-insensitive comparer
            Assert.Single(propBag.__properties);
            Assert.Equal("value2", propBag.__properties["TESTKEY"]);
        }

        [Fact]
        public void PropertiesDictionary_CanStoreAndRetrieveValues()
        {
            var propBag = new ExpandableObjectPropBag();
            
            propBag.__properties["StringValue"] = "test";
            propBag.__properties["IntValue"] = 42;
            propBag.__properties["NullValue"] = null;
            
            Assert.Equal("test", propBag.__properties["StringValue"]);
            Assert.Equal(42, propBag.__properties["IntValue"]);
            Assert.Null(propBag.__properties["NullValue"]);
        }

        [Fact]
        public void PropertiesDictionary_CanStoreComplexTypes()
        {
            var propBag = new ExpandableObjectPropBag();
            var complexObject = new Expandable1 { Name = "Complex", Age = 100 };
            
            propBag.__properties["Complex"] = complexObject;
            
            Assert.Same(complexObject, propBag.__properties["Complex"]);
        }

        [Fact]
        public void InstancePropertyInfo_WorksWithDifferentTypes()
        {
            var expandable2 = new Expandable2 { Name = "Test2", Age = 50 };
            var propBag = new ExpandableObjectPropBag(expandable2);
            
            var properties = propBag.InstancePropertyInfo;
            
            Assert.NotNull(properties);
            Assert.Contains(properties, p => p.Name == "Name");
            Assert.Contains(properties, p => p.Name == "Age");
            Assert.Contains(properties, p => p.Name == "Dates");
        }

        [Fact]
        public void InstancePropertyInfo_HandlesTypeWithNoProperties()
        {
            var simpleObject = new object();
            var propBag = new ExpandableObjectPropBag(simpleObject);
            
            var properties = propBag.InstancePropertyInfo;
            
            Assert.NotNull(properties);
            Assert.Empty(properties);
        }

        [Fact]
        public void Constructor_PreservesInstanceState()
        {
            var testInstance = new Expandable1 { Name = "Original", Age = 25 };
            var propBag = new ExpandableObjectPropBag(testInstance);
            
            // Verify the instance maintains its state
            var nameProperty = propBag.InstancePropertyInfo.First(p => p.Name == "Name");
            var ageProperty = propBag.InstancePropertyInfo.First(p => p.Name == "Age");
            
            Assert.Equal("Original", nameProperty.GetValue(propBag._instance));
            Assert.Equal(25, ageProperty.GetValue(propBag._instance));
        }

        [Fact]
        public void PropertiesDictionary_SupportsKeyOperations()
        {
            var propBag = new ExpandableObjectPropBag();
            
            propBag.__properties["Key1"] = "Value1";
            propBag.__properties["Key2"] = "Value2";
            
            Assert.True(propBag.__properties.ContainsKey("Key1"));
            Assert.True(propBag.__properties.ContainsKey("key1")); // Case-insensitive
            Assert.False(propBag.__properties.ContainsKey("Key3"));
            Assert.Equal(2, propBag.__properties.Count);
        }

        [Fact]
        public void PropertiesDictionary_SupportsRemoval()
        {
            var propBag = new ExpandableObjectPropBag();
            
            propBag.__properties["ToRemove"] = "Value";
            Assert.Single(propBag.__properties);
            
            propBag.__properties.Remove("ToRemove");
            Assert.Empty(propBag.__properties);
        }

        [Fact]
        public void PropertiesDictionary_SupportsClear()
        {
            var propBag = new ExpandableObjectPropBag();
            
            propBag.__properties["Key1"] = "Value1";
            propBag.__properties["Key2"] = "Value2";
            propBag.__properties["Key3"] = "Value3";
            
            Assert.Equal(3, propBag.__properties.Count);
            
            propBag.__properties.Clear();
            Assert.Empty(propBag.__properties);
        }
    }
}
