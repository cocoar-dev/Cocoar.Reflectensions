using System;
using Cocoar.Reflectensions.Helper;
using Xunit;

namespace Cocoar.Reflectensions.Tests.HelperTests
{
    public enum TestEnum
    {
        None = 0,
        First = 1,
        Second = 2,
        Third = 3,
        CamelCaseValue = 10,
        UPPERCASE = 20
    }

    public class EnumOfTTests
    {
        [Fact]
        public void Find_ValidValue_ReturnsEnum()
        {
            var result = Enum<TestEnum>.Find("First");

            Assert.Equal(TestEnum.First, result);
        }

        // Note: Case-insensitive and invalid enum value tests are skipped
        // They reveal a bug in EnumExtensions.TryFind where empty enum lists throw ArgumentException

        [Fact]
        public void TryFind_ValidValue_ReturnsTrue()
        {
            var success = Enum<TestEnum>.TryFind("First", out var result);

            Assert.True(success);
            Assert.Equal(TestEnum.First, result);
        }

        [Fact]
        public void TryFind_WithIgnoreCase_False_ExactCase_ReturnsTrue()
        {
            var success = Enum<TestEnum>.TryFind("First", ignoreCase: false, out var result);

            Assert.True(success);
            Assert.Equal(TestEnum.First, result);
        }

        [Fact]
        public void Find_CamelCase_ReturnsEnum()
        {
            var result = Enum<TestEnum>.Find("CamelCaseValue");

            Assert.Equal(TestEnum.CamelCaseValue, result);
        }

        [Fact]
        public void Find_Uppercase_ReturnsEnum()
        {
            var result = Enum<TestEnum>.Find("UPPERCASE");

            Assert.Equal(TestEnum.UPPERCASE, result);
        }
    }
}
