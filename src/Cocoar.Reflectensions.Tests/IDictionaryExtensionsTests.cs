using System.Collections.Generic;
using Xunit;

namespace Cocoar.Reflectensions.Tests
{
    public class IDictionaryExtensionsTests
    {
        #region GetValueOrDefault Tests (from ExtensionMethods.IDictionaryExtensions)
        [Fact]
        public void GetValueOrDefault_KeyExists_ReturnsValue()
        {
            var dict = new Dictionary<string, int>
            {
                ["key1"] = 10,
                ["key2"] = 20
            };

            var result = dict.GetValueOrDefault("key1");

            Assert.Equal(10, result);
        }

        [Fact]
        public void GetValueOrDefault_KeyDoesNotExist_ReturnsDefault()
        {
            var dict = new Dictionary<string, int>
            {
                ["key1"] = 10
            };

            var result = dict.GetValueOrDefault("key2");

            Assert.Equal(default(int), result);
        }

        [Fact]
        public void GetValueOrDefault_KeyDoesNotExist_ReturnsCustomDefault()
        {
            var dict = new Dictionary<string, int>
            {
                ["key1"] = 10
            };

            var result = dict.GetValueOrDefault("key2", 999);

            Assert.Equal(999, result);
        }

        [Fact]
        public void GetValueOrDefault_ReferenceType_KeyExists_ReturnsValue()
        {
            var dict = new Dictionary<string, string>
            {
                ["key1"] = "value1"
            };

            var result = dict.GetValueOrDefault("key1");

            Assert.Equal("value1", result);
        }

        [Fact]
        public void GetValueOrDefault_ReferenceType_KeyDoesNotExist_ReturnsNull()
        {
            var dict = new Dictionary<string, string>
            {
                ["key1"] = "value1"
            };

            var result = dict.GetValueOrDefault("key2");

            Assert.Null(result);
        }

        [Fact]
        public void GetValueOrDefault_ReferenceType_KeyDoesNotExist_ReturnsCustomDefault()
        {
            var dict = new Dictionary<string, string>
            {
                ["key1"] = "value1"
            };

            var result = dict.GetValueOrDefault("key2", "default");

            Assert.Equal("default", result);
        }

        [Fact]
        public void GetValueOrDefault_EmptyDictionary_ReturnsDefault()
        {
            var dict = new Dictionary<string, int>();

            var result = dict.GetValueOrDefault("key1");

            Assert.Equal(default(int), result);
        }

        [Fact]
        public void GetValueOrDefault_NullableType_KeyExists_ReturnsValue()
        {
            var dict = new Dictionary<string, int?>
            {
                ["key1"] = 10
            };

            var result = dict.GetValueOrDefault("key1");

            Assert.Equal(10, result);
        }

        [Fact]
        public void GetValueOrDefault_NullableType_KeyDoesNotExist_ReturnsNull()
        {
            var dict = new Dictionary<string, int?>
            {
                ["key1"] = 10
            };

            var result = dict.GetValueOrDefault("key2");

            Assert.Null(result);
        }

        [Fact]
        public void GetValueOrDefault_ComplexType_KeyExists_ReturnsValue()
        {
            var obj = new { Name = "Test" };
            var dict = new Dictionary<string, object>
            {
                ["key1"] = obj
            };

            var result = dict.GetValueOrDefault("key1");

            Assert.Same(obj, result);
        }

        [Fact]
        public void GetValueOrDefault_IntKeys_WorksCorrectly()
        {
            var dict = new Dictionary<int, string>
            {
                [1] = "one",
                [2] = "two"
            };

            var result = dict.GetValueOrDefault(1);

            Assert.Equal("one", result);
        }

        [Fact]
        public void GetValueOrDefault_IntKeys_MissingKey_ReturnsDefault()
        {
            var dict = new Dictionary<int, string>
            {
                [1] = "one"
            };

            var result = dict.GetValueOrDefault(2, "default");

            Assert.Equal("default", result);
        }
        #endregion

        #region GetValueAs/TryGetValueAs Tests (from IDictionaryExtensions)
        [Fact]
        public void GetValueAs_KeyExists_ConvertsCorrectly()
        {
            var dict = new Dictionary<string, object>
            {
                ["number"] = 42
            };

            var result = dict.GetValueAs<int>("number");

            Assert.Equal(42, result);
        }

        [Fact]
        public void GetValueAs_KeyExists_SameType_ReturnsValue()
        {
            var dict = new Dictionary<string, object>
            {
                ["text"] = "hello"
            };

            var result = dict.GetValueAs<string>("text");

            Assert.Equal("hello", result);
        }

        [Fact]
        public void GetValueAs_KeyDoesNotExist_ReturnsDefault()
        {
            var dict = new Dictionary<string, object>
            {
                ["key1"] = "value1"
            };

            var result = dict.GetValueAs<string>("nonexistent");

            Assert.Null(result);
        }

        [Fact]
        public void GetValueAs_KeyDoesNotExist_ReturnsCustomDefault()
        {
            var dict = new Dictionary<string, object>
            {
                ["key1"] = "value1"
            };

            var result = dict.GetValueAs<string>("nonexistent", "default");

            Assert.Equal("default", result);
        }

        [Fact]
        public void GetValueAs_ConversionFails_ReturnsDefault()
        {
            var dict = new Dictionary<string, object>
            {
                ["value"] = "not a number"
            };

            var result = dict.GetValueAs<int>("value");

            Assert.Equal(0, result);
        }

        [Fact]
        public void TryGetValueAs_KeyExists_ReturnsTrueAndValue()
        {
            var dict = new Dictionary<string, object>
            {
                ["number"] = 42
            };

            var success = dict.TryGetValueAs<int>("number", out var result);

            Assert.True(success);
            Assert.Equal(42, result);
        }

        [Fact]
        public void TryGetValueAs_KeyDoesNotExist_ReturnsFalse()
        {
            var dict = new Dictionary<string, object>
            {
                ["key1"] = "value1"
            };

            var success = dict.TryGetValueAs<string>("nonexistent", out var result);

            Assert.False(success);
            Assert.Null(result);
        }

        [Fact]
        public void TryGetValueAs_ConversionFails_ReturnsFalse()
        {
            var dict = new Dictionary<string, object>
            {
                ["value"] = "not a number"
            };

            var success = dict.TryGetValueAs<int>("value", out var result);

            Assert.False(success);
            Assert.Equal(0, result);
        }
        #endregion

        #region GetValueTo/TryGetValueTo Tests
        [Fact]
        public void GetValueTo_KeyExists_ConvertsCorrectly()
        {
            var dict = new Dictionary<string, object>
            {
                ["number"] = "42"
            };

            var result = dict.GetValueTo<int>("number");

            Assert.Equal(42, result);
        }

        [Fact]
        public void GetValueTo_KeyExists_ConvertsStringToInt()
        {
            var dict = new Dictionary<string, object>
            {
                ["number"] = "123"
            };

            var result = dict.GetValueTo<int>("number");

            Assert.Equal(123, result);
        }

        [Fact]
        public void GetValueTo_KeyDoesNotExist_ReturnsDefault()
        {
            var dict = new Dictionary<string, object>
            {
                ["key1"] = "value1"
            };

            var result = dict.GetValueTo<int>("nonexistent");

            Assert.Equal(0, result);
        }

        [Fact]
        public void GetValueTo_KeyDoesNotExist_ReturnsCustomDefault()
        {
            var dict = new Dictionary<string, object>
            {
                ["key1"] = "value1"
            };

            var result = dict.GetValueTo<int>("nonexistent", 999);

            Assert.Equal(999, result);
        }

        [Fact]
        public void GetValueTo_ConversionFails_ReturnsDefault()
        {
            var dict = new Dictionary<string, object>
            {
                ["value"] = new object()
            };

            var result = dict.GetValueTo<int>("value");

            Assert.Equal(0, result);
        }

        [Fact]
        public void TryGetValueTo_KeyExists_ReturnsTrueAndValue()
        {
            var dict = new Dictionary<string, object>
            {
                ["number"] = "42"
            };

            var success = dict.TryGetValueTo<int>("number", out var result);

            Assert.True(success);
            Assert.Equal(42, result);
        }

        [Fact]
        public void TryGetValueTo_KeyDoesNotExist_ReturnsFalse()
        {
            var dict = new Dictionary<string, object>
            {
                ["key1"] = "value1"
            };

            var success = dict.TryGetValueTo<int>("nonexistent", out var result);

            Assert.False(success);
            Assert.Equal(0, result);
        }

        [Fact]
        public void TryGetValueTo_ConversionFails_ReturnsFalse()
        {
            var dict = new Dictionary<string, object>
            {
                ["value"] = new object()
            };

            var success = dict.TryGetValueTo<int>("value", out var result);

            Assert.False(success);
            Assert.Equal(0, result);
        }

        [Fact]
        public void TryGetValueTo_StringToDouble_WorksCorrectly()
        {
            var dict = new Dictionary<string, object>
            {
                ["number"] = "42"
            };

            var success = dict.TryGetValueTo<double>("number", out var result);

            Assert.True(success);
            Assert.Equal(42.0, result);
        }

        [Fact]
        public void TryGetValueTo_StringToBoolean_WorksCorrectly()
        {
            var dict = new Dictionary<string, object>
            {
                ["flag"] = "true"
            };

            var success = dict.TryGetValueTo<bool>("flag", out var result);

            Assert.True(success);
            Assert.True(result);
        }
        #endregion
    }
}
