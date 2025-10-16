using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Cocoar.Reflectensions.ExtensionMethods;
using Xunit;

namespace Cocoar.Reflectensions.Tests {
    public class CheckTypeTests {

        [Theory]
        [InlineData(typeof(int))]
        public void IsNumericType(Type type) {
            var result = type.IsNumericType();
            Assert.True(result);
        }

        [Theory]
        [InlineData(typeof(List<string>))]
        [InlineData(typeof(IEnumerable<int>))]
        [InlineData(typeof(ICollection<DateTime>))]
        [InlineData(typeof(Byte[]))]
        public void IsEnumerationType(Type type) {
            var result = type.IsEnumerableType();
            Assert.True(result);
        }


        [Theory]
        [InlineData(typeof(Dictionary<string, object>))]
        [InlineData(typeof(ExpandoObject))]
        public void IsNotEnumerationType(Type type) {
            var result = type.IsEnumerableType();
            Assert.False(result);
        }

        [Theory]
        [InlineData(typeof(Dictionary<string, object>), typeof(IDictionary<,>))]
        [InlineData(typeof(Dictionary<string, object>), typeof(IDictionary))]
        [InlineData(typeof(ExpandoObject), typeof(IEnumerable))]
        public void ImplementsInterface(Type type, Type interfaceType) {
            var result = type.ImplementsInterface(interfaceType);
            Assert.True(result);
        }


        [Theory]
        [InlineData(typeof(int?))]
        [InlineData(typeof(DateTime?))]
        public void IsNullableType(Type type) {
            var result = type.IsNullableType();
            Assert.True(result);
        }


        [Theory]
        [InlineData(typeof(int?), typeof(object))]
        [InlineData(typeof(DateTime), typeof(object))]
        public void InheritFromClass(Type type, Type from) {
            var result = type.InheritFromClass(from);
            Assert.True(result);
        }

        [Theory]
        [InlineData(typeof(int?), typeof(string))]
        [InlineData(typeof(DateTime), typeof(TimeSpan))]
        public void Not_InheritFromClass(Type type, Type from) {
            var result = type.InheritFromClass(from);
            Assert.False(result);
        }


        [Theory]
        [InlineData(typeof(IDictionary<string, string>))]
        [InlineData(typeof(Dictionary<string, string>))]
        public void IDictionaryTest(Type type) {
           
            var d = type.IsDictionaryType();

            Assert.True(d);
        }


        [Theory]
        [InlineData(typeof(List<string>))]
        public void NotIDictionaryTest(Type type) {

            Assert.True(!type.IsDictionaryType());
        }
    }
}
