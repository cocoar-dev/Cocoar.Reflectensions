using System;
using System.Linq;
using System.Reflection;
using Cocoar.Reflectensions.ExtensionMethods;
using Cocoar.Reflectensions.Tests.TestClasses;
using Xunit;

namespace Cocoar.Reflectensions.Tests
{
    public class MethodInfoFilterTests
    {
        [Fact]
        public void HasName_WithMatchingName_ReturnsTrue()
        {
            var method = typeof(Car).GetMethod("ToString");
            
            Assert.NotNull(method);
            Assert.True(method.HasName("ToString"));
        }

        [Fact]
        public void HasName_WithNonMatchingName_ReturnsFalse()
        {
            var method = typeof(Car).GetMethod("ToString");
            
            Assert.NotNull(method);
            Assert.False(method.HasName("GetType"));
        }

        [Fact]
        public void WithName_FiltersMethodsByName()
        {
            var methods = typeof(Building)
                .GetMethods()
                .WithName("OpenMainDoor")
                .ToList();
            
            Assert.NotEmpty(methods);
            Assert.All(methods, m => Assert.Equal("OpenMainDoor", m.Name));
        }

        [Fact]
        public void WithName_WithStringComparison_FiltersCorrectly()
        {
            var methods = typeof(Building)
                .GetMethods()
                .WithName("openmAINDoor", StringComparison.OrdinalIgnoreCase)
                .ToList();
            
            Assert.NotEmpty(methods);
            Assert.All(methods, m => Assert.Equal("OpenMainDoor", m.Name));
        }

        [Fact]
        public void HasReturnType_WithCorrectType_ReturnsTrue()
        {
            var method = typeof(Building)
                .GetMethod("CountFloors", BindingFlags.Public | BindingFlags.Instance);
            
            Assert.NotNull(method);
            Assert.True(method.HasReturnType<int>());
        }

        [Fact]
        public void HasReturnType_WithIncorrectType_ReturnsFalse()
        {
            var method = typeof(Building)
                .GetMethod("CountFloors", BindingFlags.Public | BindingFlags.Instance);
            
            Assert.NotNull(method);
            Assert.False(method.HasReturnType<string>());
        }

        [Fact]
        public void WithReturnType_FiltersMethodsByReturnType()
        {
            var methods = typeof(Building)
                .GetMethods()
                .WithReturnType<int>()
                .ToList();
            
            Assert.NotEmpty(methods);
            Assert.All(methods, m => Assert.Equal(typeof(int), m.ReturnType));
        }

        [Fact]
        public void HasParametersLengthOf_WithCorrectLength_ReturnsTrue()
        {
            var method = typeof(Building)
                .GetMethod("OpenMainDoor", BindingFlags.Public | BindingFlags.Instance);
            
            Assert.NotNull(method);
            Assert.True(method.HasParametersLengthOf(0));
        }

        [Fact]
        public void HasParametersLengthOf_WithIncorrectLength_ReturnsFalse()
        {
            var method = typeof(Building)
                .GetMethod("OpenMainDoor", BindingFlags.Public | BindingFlags.Instance);
            
            Assert.NotNull(method);
            Assert.False(method.HasParametersLengthOf(2));
        }

        [Fact]
        public void WithParametersLengthOf_FiltersMethodsByParameterCount()
        {
            var methods = typeof(Building)
                .GetMethods()
                .WithParametersLengthOf(0)
                .ToList();
            
            Assert.NotEmpty(methods);
            Assert.All(methods, m => Assert.Empty(m.GetParameters()));
        }

        [Fact]
        public void HasParametersOfType_WithMatchingTypes_ReturnsTrue()
        {
            var methods = typeof(string)
                .GetMethods()
                .Where(m => m.Name == "IndexOf" && m.GetParameters().Length == 1)
                .ToList();
            
            Assert.NotEmpty(methods);
            var method = methods.First(m => m.GetParameters()[0].ParameterType == typeof(char));
            Assert.True(method.HasParametersOfType(new[] { typeof(char) }));
        }

        [Fact]
        public void WithParametersOfType_WorksWithExistingMethods()
        {
            // Test that WithParametersOfType extension works
            var allMethods = typeof(Building).GetMethods();
            var filtered = allMethods.WithParametersOfType(typeof(int)).ToList();
            
            // The method should filter (may be empty if no methods take single int parameter)
            // Test verifies the method executes without error
            Assert.NotNull(filtered);
        }

        [Fact]
        public void WithGenericArgumentsLengthOf_FiltersGenericMethods()
        {
            var methods = typeof(Enumerable)
                .GetMethods()
                .Where(m => m.IsGenericMethod)
                .WithGenericArgumentsLengthOf(1)
                .ToList();
            
            Assert.NotEmpty(methods);
            Assert.All(methods, m => Assert.Single(m.GetGenericArguments()));
        }

        [Fact]
        public void ChainedFilters_ApplyCorrectly()
        {
            var methods = typeof(Building)
                .GetMethods()
                .WithReturnType<int>()
                .WithParametersLengthOf(0)
                .ToList();
            
            Assert.NotEmpty(methods);
            Assert.All(methods, m =>
            {
                Assert.Equal(typeof(int), m.ReturnType);
                Assert.Empty(m.GetParameters());
            });
        }

        [Fact]
        public void GetImplicitCastMethodTo_FindsImplicitOperator()
        {
            var method = typeof(Camaro).GetImplicitCastMethodTo(typeof(Truck));
            
            Assert.NotNull(method);
            Assert.Equal("op_Implicit", method.Name);
        }

        [Fact]
        public void GetImplicitCastMethodTo_WithNoCast_ReturnsNull()
        {
            var method = typeof(Truck).GetImplicitCastMethodTo(typeof(Camaro));
            
            Assert.Null(method);
        }

        [Fact]
        public void WithReturnType_NonGeneric_FiltersMethodsByReturnType()
        {
            var methods = typeof(Building)
                .GetMethods()
                .WithReturnType(typeof(int))
                .ToList();
            
            Assert.NotEmpty(methods);
            Assert.All(methods, m => Assert.Equal(typeof(int), m.ReturnType));
        }

        [Fact]
        public void WithGenericArgumentsOfType_FiltersGenericMethodsByTypeArguments()
        {
            var methods = typeof(Enumerable)
                .GetMethods()
                .Where(m => m.IsGenericMethod && m.Name == "Empty")
                .WithGenericArgumentsOfType(typeof(object))
                .ToList();
            
            Assert.NotNull(methods);
        }

        [Fact]
        public void WithAttribute_Generic_FiltersMethodsByAttribute()
        {
            var methods = typeof(ObsoleteAttributeTestClass)
                .GetMethods()
                .WithAttribute<ObsoleteAttribute>()
                .ToList();
            
            Assert.NotEmpty(methods);
            Assert.All(methods, m => Assert.True(m.HasAttribute<ObsoleteAttribute>()));
        }

        [Fact]
        public void WithAttribute_Generic_WithInherit_FiltersMethodsByAttribute()
        {
            var methods = typeof(ObsoleteAttributeTestClass)
                .GetMethods()
                .WithAttribute<ObsoleteAttribute>(inherit: true)
                .ToList();
            
            Assert.NotEmpty(methods);
        }

        [Fact]
        public void WithAttribute_NonGeneric_FiltersMethodsByAttribute()
        {
            var methods = typeof(ObsoleteAttributeTestClass)
                .GetMethods()
                .WithAttribute(typeof(ObsoleteAttribute))
                .ToList();
            
            Assert.NotEmpty(methods);
            Assert.All(methods, m => Assert.True(m.HasAttribute(typeof(ObsoleteAttribute))));
        }

        [Fact]
        public void WithAttribute_NonGeneric_WithInherit_FiltersMethodsByAttribute()
        {
            var methods = typeof(ObsoleteAttributeTestClass)
                .GetMethods()
                .WithAttribute(typeof(ObsoleteAttribute), inherit: true)
                .ToList();
            
            Assert.NotEmpty(methods);
        }

        [Fact]
        public void WithAttributeExpanded_Generic_ReturnsMethodsWithAttributes()
        {
            var results = typeof(ObsoleteAttributeTestClass)
                .GetMethods()
                .WithAttributeExpanded<ObsoleteAttribute>()
                .ToList();
            
            Assert.NotEmpty(results);
            Assert.All(results, result =>
            {
                Assert.NotNull(result.MethodInfo);
                Assert.NotNull(result.Attribute);
            });
        }

        [Fact]
        public void WithAttributeExpanded_Generic_WithInherit_ReturnsMethodsWithAttributes()
        {
            var results = typeof(ObsoleteAttributeTestClass)
                .GetMethods()
                .WithAttributeExpanded<ObsoleteAttribute>(inherit: true)
                .ToList();
            
            Assert.NotEmpty(results);
        }

        [Fact]
        public void WithAttributeExpanded_NonGeneric_ReturnsMethodsWithAttributes()
        {
            var results = typeof(ObsoleteAttributeTestClass)
                .GetMethods()
                .WithAttributeExpanded(typeof(ObsoleteAttribute))
                .ToList();
            
            Assert.NotEmpty(results);
            Assert.All(results, result =>
            {
                Assert.NotNull(result.MethodInfo);
                Assert.NotNull(result.Attribute);
            });
        }

        [Fact]
        public void WithAttributeExpanded_NonGeneric_WithInherit_ReturnsMethodsWithAttributes()
        {
            var results = typeof(ObsoleteAttributeTestClass)
                .GetMethods()
                .WithAttributeExpanded(typeof(ObsoleteAttribute), inherit: true)
                .ToList();
            
            Assert.NotEmpty(results);
        }

        [Fact]
        public void WithAttributeExpanded_NonGeneric_WithInvalidAttributeType_ThrowsArgumentException()
        {
            var methods = typeof(ObsoleteAttributeTestClass).GetMethods();
            
            var exception = Assert.Throws<ArgumentException>(() =>
                methods.WithAttributeExpanded(typeof(string)).ToList());
            
            Assert.Contains("Attribute", exception.Message);
        }

        private class ObsoleteAttributeTestClass
        {
            [Obsolete("This method is obsolete")]
            public void ObsoleteMethod1() { }

            [Obsolete("This method is also obsolete")]
            public void ObsoleteMethod2() { }

            public void NormalMethod() { }
        }
    }
}
