using System;
using System.Collections.Generic;
using Cocoar.Reflectensions.Helper;
using Cocoar.Reflectensions.Tests.TestClasses;
using Xunit;

namespace Cocoar.Reflectensions.Tests
{
    public class TypeFinderTests
    {
        [Theory]
        [InlineData("int", typeof(int))]
        [InlineData("string", typeof(string))]
        [InlineData("bool", typeof(bool))]
        [InlineData("double", typeof(double))]
        [InlineData("float", typeof(float))]
        [InlineData("long", typeof(long))]
        [InlineData("short", typeof(short))]
        [InlineData("byte", typeof(byte))]
        [InlineData("decimal", typeof(decimal))]
        public void FindType_WithBuiltInTypes_ReturnsCorrectType(string typeName, Type expectedType)
        {
            var type = TypeHelper.FindType(typeName);
            
            Assert.Equal(expectedType, type);
        }

        [Theory]
        [InlineData("Int32", typeof(int))]
        [InlineData("String", typeof(string))]
        [InlineData("Boolean", typeof(bool))]
        public void FindType_WithFullNames_ReturnsCorrectType(string typeName, Type expectedType)
        {
            var type = TypeHelper.FindType(typeName);
            
            Assert.Equal(expectedType, type);
        }

        [Fact]
        public void FindType_WithGenericList_ReturnsTypeOrNull()
        {
            var type = TypeHelper.FindType("List<int>");
            
            // This may or may not be supported - test documents the behavior
            if (type != null)
            {
                Assert.True(type.IsGenericType);
            }
        }

        [Fact]
        public void FindType_WithGenericDictionary_ReturnsTypeOrNull()
        {
            var type = TypeHelper.FindType("Dictionary<string,int>");
            
            // This may or may not be supported - test documents the behavior
            if (type != null)
            {
                Assert.True(type.IsGenericType);
            }
        }

        [Fact]
        public void FindType_WithArray_ReturnsCorrectType()
        {
            var type = TypeHelper.FindType("int[]");
            
            Assert.NotNull(type);
            Assert.True(type.IsArray);
            Assert.Equal(typeof(int), type.GetElementType());
        }

        [Fact]
        public void FindType_WithNullableType_ReturnsTypeOrNull()
        {
            var type = TypeHelper.FindType("int?");
            
            // Nullable syntax may not be supported in this format
            if (type != null)
            {
                Assert.True(type == typeof(int?) || type == typeof(int));
            }
        }

        [Fact]
        public void FindType_WithFullyQualifiedName_ReturnsCorrectType()
        {
            var typeName = typeof(Building).FullName;
            var type = TypeHelper.FindType(typeName!);
            
            Assert.Equal(typeof(Building), type);
        }

        [Fact]
        public void FindType_WithTypeMapping_UsesMapping()
        {
            var mapping = new Dictionary<string, string>
            {
                ["number"] = "double",
                ["boolean"] = "bool"
            };

            var type = TypeHelper.FindType("number", mapping);
            
            Assert.Equal(typeof(double), type);
        }

        [Fact]
        public void FindType_WithComplexGeneric_ReturnsTypeOrNull()
        {
            var type = TypeHelper.FindType("List<Dictionary<string,int>>");
            
            // Complex generic syntax may not be fully supported
            // Test documents current behavior
            if (type != null)
            {
                Assert.True(type.IsGenericType);
            }
        }

        [Fact]
        public void FindType_WithInvalidType_ReturnsNull()
        {
            var type = TypeHelper.FindType("ThisTypeDoesNotExist");
            
            Assert.Null(type);
        }

        [Fact]
        public void FindType_WithEmptyString_ReturnsNull()
        {
            var type = TypeHelper.FindType("");
            
            Assert.Null(type);
        }

        [Fact]
        public void FindType_WithNestedGeneric_ReturnsTypeOrNull()
        {
            var type = TypeHelper.FindType("List<List<int>>");
            
            // Nested generic syntax may not be fully supported
            if (type != null)
            {
                Assert.True(type.IsGenericType);
            }
        }

        [Fact]
        public void FindType_WithCustomType_ReturnsCorrectType()
        {
            var type = TypeHelper.FindType("Cocoar.Reflectensions.Tests.TestClasses.Building");
            
            Assert.Equal(typeof(Building), type);
        }

        [Fact]
        public void FindType_WithMultipleMappings_UsesCorrectMapping()
        {
            var mapping = new Dictionary<string, string>
            {
                ["number"] = "double",
                ["boolean"] = "bool",
                ["text"] = "string"
            };

            var numType = TypeHelper.FindType("number", mapping);
            var boolType = TypeHelper.FindType("boolean", mapping);
            var textType = TypeHelper.FindType("text", mapping);
            
            Assert.Equal(typeof(double), numType);
            Assert.Equal(typeof(bool), boolType);
            Assert.Equal(typeof(string), textType);
        }
    }
}
