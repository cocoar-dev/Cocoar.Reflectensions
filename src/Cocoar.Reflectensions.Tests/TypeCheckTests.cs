using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Cocoar.Reflectensions.ExtensionMethods;
using Xunit;

namespace Cocoar.Reflectensions.Tests {
    public class TypeCheckTests {

        [Theory]
        [InlineData(typeof(int))]
        [InlineData(typeof(double))]
        [InlineData(typeof(decimal))]
        [InlineData(typeof(byte))]
        public void IsNumericType_ReturnsTrue_ForNumericTypes(Type type) {
            var result = type.IsNumericType();
            Assert.True(result);
        }

        [Theory]
        [InlineData(typeof(string))]
        [InlineData(typeof(bool))]
        [InlineData(typeof(DateTime))]
        public void IsNumericType_ReturnsFalse_ForNonNumericTypes(Type type) {
            var result = type.IsNumericType();
            Assert.False(result);
        }

        [Theory]
        [InlineData(typeof(List<string>))]
        [InlineData(typeof(IEnumerable<int>))]
        [InlineData(typeof(ICollection<DateTime>))]
        [InlineData(typeof(Byte[]))]
        public void IsEnumerableType_ReturnsTrue_ForEnumerableTypes(Type type) {
            var result = type.IsEnumerableType();
            Assert.True(result);
        }

        [Theory]
        [InlineData(typeof(Dictionary<string, object>))]
        [InlineData(typeof(ExpandoObject))]
        public void IsEnumerableType_ReturnsFalse_ForNonEnumerableTypes(Type type) {
            var result = type.IsEnumerableType();
            Assert.False(result);
        }

        [Theory]
        [InlineData(typeof(Dictionary<string, object>), typeof(IDictionary<,>))]
        [InlineData(typeof(Dictionary<string, object>), typeof(IDictionary))]
        [InlineData(typeof(ExpandoObject), typeof(IEnumerable))]
        public void ImplementsInterface_ReturnsTrue_WhenTypeImplementsInterface(Type type, Type interfaceType) {
            var result = type.ImplementsInterface(interfaceType);
            Assert.True(result);
        }

        [Theory]
        [InlineData(typeof(string), typeof(IDictionary))]
        [InlineData(typeof(int), typeof(IEnumerable))]
        public void ImplementsInterface_ReturnsFalse_WhenTypeDoesNotImplementInterface(Type type, Type interfaceType) {
            var result = type.ImplementsInterface(interfaceType);
            Assert.False(result);
        }


        [Theory]
        [InlineData(typeof(int?))]
        [InlineData(typeof(DateTime?))]
        public void IsNullableType_ReturnsTrue_ForNullableTypes(Type type) {
            var result = type.IsNullableType();
            Assert.True(result);
        }

        [Theory]
        [InlineData(typeof(int))]
        [InlineData(typeof(string))]
        public void IsNullableType_ReturnsFalse_ForNonNullableTypes(Type type) {
            var result = type.IsNullableType();
            Assert.False(result);
        }


        [Theory]
        [InlineData(typeof(int?), typeof(object))]
        [InlineData(typeof(DateTime), typeof(object))]
        public void InheritFromClass_ReturnsTrue_WhenTypeInheritsFromClass(Type type, Type from) {
            var result = type.InheritFromClass(from);
            Assert.True(result);
        }

        [Theory]
        [InlineData(typeof(int?), typeof(string))]
        [InlineData(typeof(DateTime), typeof(TimeSpan))]
        public void InheritFromClass_ReturnsFalse_WhenTypeDoesNotInheritFromClass(Type type, Type from) {
            var result = type.InheritFromClass(from);
            Assert.False(result);
        }


        [Theory]
        [InlineData(typeof(IDictionary<string, string>))]
        [InlineData(typeof(Dictionary<string, string>))]
        public void IsDictionaryType_ReturnsTrue_ForDictionaryTypes(Type type) {
            var result = type.IsDictionaryType();
            Assert.True(result);
        }

        [Theory]
        [InlineData(typeof(List<string>))]
        [InlineData(typeof(string))]
        public void IsDictionaryType_ReturnsFalse_ForNonDictionaryTypes(Type type) {
            var result = type.IsDictionaryType();
            Assert.False(result);
        }
    }
}
