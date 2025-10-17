using System;
using System.Linq;
using System.Reflection;
using Cocoar.Reflectensions.ExtensionMethods;
using Xunit;

namespace Cocoar.Reflectensions.Tests
{
    public class ExtensionMethodTests
    {
        #region MethodBaseExtensions Tests
        [Fact]
        public void IsExtensionMethod_OnExtensionMethod_ReturnsTrue()
        {
            var method = typeof(StringExtensions).GetMethods()
                .First(m => m.Name == "Split" && m.GetParameters().Length == 3);

            var result = method.IsExtensionMethod();

            Assert.True(result);
        }

        [Fact]
        public void IsExtensionMethod_OnNormalMethod_ReturnsFalse()
        {
            var method = typeof(string).GetMethod("ToUpper", Type.EmptyTypes);

            var result = method!.IsExtensionMethod();

            Assert.False(result);
        }
        #endregion

        #region ParameterInfoExtensions Tests
        [AttributeUsage(AttributeTargets.Parameter)]
        private class TestParamAttribute : Attribute { }

        private void TestMethod([TestParam] string param1, string param2) { }

        [Fact]
        public void HasAttribute_WithAttribute_ReturnsTrue()
        {
            var method = GetType().GetMethod("TestMethod", BindingFlags.NonPublic | BindingFlags.Instance);
            var param = method!.GetParameters()[0];

            var result = param.HasAttribute(typeof(TestParamAttribute));

            Assert.True(result);
        }

        [Fact]
        public void HasAttribute_WithoutAttribute_ReturnsFalse()
        {
            var method = GetType().GetMethod("TestMethod", BindingFlags.NonPublic | BindingFlags.Instance);
            var param = method!.GetParameters()[1];

            var result = param.HasAttribute(typeof(TestParamAttribute));

            Assert.False(result);
        }

        [Fact]
        public void HasAttribute_ByName_WithAttribute_ReturnsTrue()
        {
            var method = GetType().GetMethod("TestMethod", BindingFlags.NonPublic | BindingFlags.Instance);
            var param = method!.GetParameters()[0];

            var result = param.HasAttribute("TestParamAttribute");

            Assert.True(result);
        }

        [Fact]
        public void HasAttribute_ByName_WithoutAttribute_ReturnsFalse()
        {
            var method = GetType().GetMethod("TestMethod", BindingFlags.NonPublic | BindingFlags.Instance);
            var param = method!.GetParameters()[1];

            var result = param.HasAttribute("TestParamAttribute");

            Assert.False(result);
        }

        [Fact]
        public void HasAttribute_NullParameter_ThrowsArgumentNullException()
        {
            ParameterInfo param = null!;

            Assert.Throws<ArgumentNullException>(() => param.HasAttribute(typeof(TestParamAttribute)));
        }

        [Fact]
        public void HasAttribute_NonAttributeType_ThrowsArgumentException()
        {
            var method = GetType().GetMethod("TestMethod", BindingFlags.NonPublic | BindingFlags.Instance);
            var param = method!.GetParameters()[0];

            Assert.Throws<ArgumentException>(() => param.HasAttribute(typeof(string)));
        }
        #endregion

        #region ParameterInfoEnumerableExtensions Tests
        private void MultiParamMethod(
            [TestParam] string param1,
            int param2,
            [TestParam] string param3,
            double param4) { }

        [Fact]
        public void WithName_FindsParameterByName()
        {
            var method = GetType().GetMethod("MultiParamMethod", BindingFlags.NonPublic | BindingFlags.Instance);
            var parameters = method!.GetParameters();

            var result = parameters.WithName("param2", ignoreCase: false);

            Assert.Single(result);
            Assert.Equal("param2", result.First().Name);
        }

        [Fact]
        public void WithName_IgnoreCase_FindsParameter()
        {
            var method = GetType().GetMethod("MultiParamMethod", BindingFlags.NonPublic | BindingFlags.Instance);
            var parameters = method!.GetParameters();

            var result = parameters.WithName("PARAM2", ignoreCase: true);

            Assert.Single(result);
            Assert.Equal("param2", result.First().Name);
        }

        [Fact]
        public void WithTypeOf_FindsParametersOfType()
        {
            var method = GetType().GetMethod("MultiParamMethod", BindingFlags.NonPublic | BindingFlags.Instance);
            var parameters = method!.GetParameters();

            var result = parameters.WithTypeOf(typeof(string));

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void WithTypeOf_Generic_FindsParametersOfType()
        {
            var method = GetType().GetMethod("MultiParamMethod", BindingFlags.NonPublic | BindingFlags.Instance);
            var parameters = method!.GetParameters();

            var result = parameters.WithTypeOf<string>();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void WithoutTypeOf_ExcludesParametersOfType()
        {
            var method = GetType().GetMethod("MultiParamMethod", BindingFlags.NonPublic | BindingFlags.Instance);
            var parameters = method!.GetParameters();

            var result = parameters.WithoutTypeOf(typeof(string));

            Assert.Equal(2, result.Count());
            Assert.DoesNotContain(result, p => p.ParameterType == typeof(string));
        }

        [Fact]
        public void WithoutTypeOf_Generic_ExcludesParametersOfType()
        {
            var method = GetType().GetMethod("MultiParamMethod", BindingFlags.NonPublic | BindingFlags.Instance);
            var parameters = method!.GetParameters();

            var result = parameters.WithoutTypeOf<string>();

            Assert.Equal(2, result.Count());
            Assert.DoesNotContain(result, p => p.ParameterType == typeof(string));
        }

        [Fact]
        public void WithAttribute_FindsParametersWithAttribute()
        {
            var method = GetType().GetMethod("MultiParamMethod", BindingFlags.NonPublic | BindingFlags.Instance);
            var parameters = method!.GetParameters();

            var result = parameters.WithAttribute("TestParamAttribute");

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void WithoutAttribute_FindsParametersWithoutAttribute()
        {
            var method = GetType().GetMethod("MultiParamMethod", BindingFlags.NonPublic | BindingFlags.Instance);
            var parameters = method!.GetParameters();

            var result = parameters.WithoutAttribute("TestParamAttribute");

            Assert.Equal(2, result.Count());
        }
        #endregion

        #region TypeEnumerableExtensions Tests
        [AttributeUsage(AttributeTargets.Class)]
        private class TestClassAttribute : Attribute { }

        [TestClass]
        private class ClassWithAttribute { }

        private class ClassWithoutAttribute { }

        private class GenericClass<T> { }

        private class DerivedClass : ClassWithAttribute { }

        [Fact]
        public void WithAttribute_Generic_FindsTypesWithAttribute()
        {
            var types = new[] { typeof(ClassWithAttribute), typeof(ClassWithoutAttribute) };

            var result = types.WithAttribute<TestClassAttribute>();

            Assert.Single(result);
            Assert.Contains(typeof(ClassWithAttribute), result);
        }

        [Fact]
        public void WithAttribute_ByType_FindsTypesWithAttribute()
        {
            var types = new[] { typeof(ClassWithAttribute), typeof(ClassWithoutAttribute) };

            var result = types.WithAttribute(typeof(TestClassAttribute));

            Assert.Single(result);
            Assert.Contains(typeof(ClassWithAttribute), result);
        }

        [Fact]
        public void WithAttributeExpanded_Generic_ReturnsTypesAndAttributes()
        {
            var types = new[] { typeof(ClassWithAttribute), typeof(ClassWithoutAttribute) };

            var result = types.WithAttributeExpanded<TestClassAttribute>();

            Assert.Single(result);
            Assert.Equal(typeof(ClassWithAttribute), result.First().Key);
            Assert.NotNull(result.First().Value);
        }

        [Fact]
        public void WithAttributeExpanded_ByType_ReturnsTypesAndAttributes()
        {
            var types = new[] { typeof(ClassWithAttribute), typeof(ClassWithoutAttribute) };

            var result = types.WithAttributeExpanded(typeof(TestClassAttribute));

            Assert.Single(result);
            Assert.Equal(typeof(ClassWithAttribute), result.First().Key);
            Assert.NotNull(result.First().Value);
        }

        [Fact]
        public void WithAttributeExpanded_NonAttributeType_ThrowsArgumentException()
        {
            var types = new[] { typeof(ClassWithAttribute) };

            Assert.Throws<ArgumentException>(() => types.WithAttributeExpanded(typeof(string)).ToList());
        }

        [Fact]
        public void WhichIsGenericTypeOf_FindsGenericTypes()
        {
            var genericType = typeof(GenericClass<int>);
            var types = new[] { genericType, typeof(ClassWithAttribute) };

            var result = types.WhichIsGenericTypeOf(typeof(GenericClass<>));

            Assert.Single(result);
            Assert.Contains(genericType, result);
        }

        [Fact]
        public void WhichIsGenericTypeOf_Generic_FindsGenericTypes()
        {
            var genericType = typeof(GenericClass<int>);
            var types = new[] { genericType, typeof(ClassWithAttribute) };

            var result = types.WhichIsGenericTypeOf(typeof(GenericClass<>));

            Assert.Single(result);
            Assert.Contains(genericType, result);
        }

        [Fact]
        public void WhichInheritFromClass_FindsInheritingTypes()
        {
            var types = new[] { typeof(DerivedClass), typeof(ClassWithAttribute), typeof(ClassWithoutAttribute) };

            var result = types.WhichInheritFromClass(typeof(ClassWithAttribute));

            Assert.Single(result);
            Assert.Contains(typeof(DerivedClass), result);
        }

        [Fact]
        public void WhichInheritFromClass_Generic_FindsInheritingTypes()
        {
            var types = new[] { typeof(DerivedClass), typeof(ClassWithAttribute), typeof(ClassWithoutAttribute) };

            var result = types.WhichInheritFromClass<ClassWithAttribute>();

            Assert.Single(result);
            Assert.Contains(typeof(DerivedClass), result);
        }
        #endregion
    }
}
