using System;
using System.Collections.Generic;
using System.Globalization;
using Cocoar.Reflectensions.ExtensionMethods;
using Cocoar.Reflectensions.Tests.TestClasses;
using Xunit;

namespace Cocoar.Reflectensions.Tests
{
    public class TypeCastingTests
    {


        [Theory]
        [InlineData(typeof(Camaro), typeof(Truck))]
        public void IsImplicitCastableTo_ReturnsTrue_WhenTypeIsCastable(Type from, Type to) {
            var isCastable = from.IsImplicitCastableTo(to);
            Assert.True(isCastable);
        }

        [Theory]
        [InlineData(typeof(Truck), typeof(Camaro))]
        public void IsImplicitCastableTo_ReturnsFalse_WhenTypeIsNotCastable(Type from, Type to) {
            var isCastable = from.IsImplicitCastableTo(to);
            Assert.False(isCastable);
        }

        [Fact]
        public void ConvertToNullableDateTime_ConvertsValidStringAndHandlesEmptyString() {
            var str = "2018-03-21T15:50:17+00:00";
            DateTime? nullDate = str.Reflect().To<DateTime?>();
            DateTime date = str.Reflect().To<DateTime>();

            Assert.NotNull(nullDate);
            Assert.True(date > DateTime.MinValue);

            var emptyStr = "";
            DateTime? nullDate2 = emptyStr.Reflect().To<DateTime?>(null);
            DateTime date2 = emptyStr.Reflect().To<DateTime>(DateTime.Now);

            Assert.Null(nullDate2);
            Assert.True(date2 > DateTime.MinValue);
        }


        [Theory]
        [InlineData("1")]
        [InlineData("12345")]
        public void ConvertStringToInt_ConvertsSuccessfully(string value) {
            int? _nullInt = value.Reflect().To<int?>();
            int _int = value.Reflect().To<int>();

            Assert.Equal(_nullInt, int.Parse(value));
            Assert.Equal(_int, int.Parse(value));
        }

        [Theory]
        [InlineData("1.123")]
        [InlineData("12345.123")]
        public void ConvertStringToDouble_ConvertsSuccessfully(string value) {
            double? _nullDouble = value.Reflect().To<double?>();
            double _double = value.Reflect().To<double>();

            Assert.Equal(_nullDouble, double.Parse(value, CultureInfo.InvariantCulture));
            Assert.Equal(_double, double.Parse(value, CultureInfo.InvariantCulture));
        }

        [Theory]
        [InlineData("a5ac3789-a5ef-4a49-81bb-fc24a6e47244")]
        public void ConvertStringToGuid_ConvertsSuccessfully(string value) {
            Guid? nullGuid = $"{value}!!".Reflect().To<Guid?>(null);
            Guid guid = value.Reflect().To<Guid>();

            Assert.Null(nullGuid);
            Assert.Equal(Guid.Parse(value), guid);
        }

        [Theory]
        [InlineData("ja", true)]
        [InlineData("yes", true)]
        [InlineData("si", false)]
        [InlineData("hai", true)]
        [InlineData("oui", false)]
        [InlineData(Simplest.Third, true)]
        [InlineData(NoFlags.Eins | NoFlags.Drei, true)]
        [InlineData(NoFlags.Drei, false)]
        [InlineData(WithFlags.Two, true)]
        public void ConvertToBoolean_WithCustomTrueValues_ConvertsCorrectly(object value, bool expectedResult)
        {
            var truevalues = new object[]
            {
                "ja",
                "yes",
                "hai",
                Simplest.Third,
                NoFlags.Eins | NoFlags.Drei,
                WithFlags.Two
            };

            var result = value.Reflect().ToBoolean(truevalues);

            Assert.Equal(expectedResult, result);
        }


        [Fact]
        public void ConvertArrayToIEnumerable_ConvertsSuccessfully()
        {
            var arr = new object[0];
            var enumerable = arr.Reflect().To<IEnumerable<object>>();

            Assert.NotNull(enumerable);
        }
    }
}
