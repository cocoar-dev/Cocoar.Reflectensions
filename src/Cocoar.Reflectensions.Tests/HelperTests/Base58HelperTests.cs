using System;
using System.Text;
using Cocoar.Reflectensions.Helper;
using Xunit;

namespace Cocoar.Reflectensions.Tests.HelperTests
{
    public class Base58HelperTests
    {
        [Fact]
        public void Encode_ValidData_EncodesSuccessfully()
        {
            var data = Encoding.UTF8.GetBytes("Hello World");

            var result = Base58Helper.Encode(data);

            Assert.NotEmpty(result);
            Assert.DoesNotContain("0", result);
            Assert.DoesNotContain("O", result);
            Assert.DoesNotContain("I", result);
            Assert.DoesNotContain("l", result);
        }

        [Fact]
        public void Decode_ValidBase58_DecodesSuccessfully()
        {
            var original = "Hello World";
            var data = Encoding.UTF8.GetBytes(original);
            var encoded = Base58Helper.Encode(data);

            var decoded = Base58Helper.Decode(encoded);
            var result = Encoding.UTF8.GetString(decoded);

            Assert.Equal(original, result);
        }

        [Fact]
        public void Encode_EmptyArray_ReturnsEmpty()
        {
            var result = Base58Helper.Encode(new byte[0]);

            Assert.Empty(result);
        }

        [Fact]
        public void Encode_WithLeadingZeros_PreservesZeros()
        {
            var data = new byte[] { 0, 0, 1, 2, 3 };

            var result = Base58Helper.Encode(data);

            Assert.StartsWith("11", result);
        }

        [Fact]
        public void Decode_WithInvalidCharacter_ThrowsFormatException()
        {
            var exception = Assert.Throws<FormatException>(() => Base58Helper.Decode("Invalid0Character"));

            Assert.Contains("Invalid Base58 character", exception.Message);
        }

        [Fact]
        public void EncodeWithCheckSum_ValidData_EncodesWithChecksum()
        {
            var data = Encoding.UTF8.GetBytes("Test");

            var result = Base58Helper.EncodeWithCheckSum(data);

            Assert.NotEmpty(result);
        }

        [Fact]
        public void DecodeWithCheckSum_ValidData_DecodesSuccessfully()
        {
            var original = "Test Data";
            var data = Encoding.UTF8.GetBytes(original);
            var encoded = Base58Helper.EncodeWithCheckSum(data);

            var decoded = Base58Helper.DecodeWithCheckSum(encoded);
            var result = Encoding.UTF8.GetString(decoded);

            Assert.Equal(original, result);
        }

        [Fact]
        public void DecodeWithCheckSum_InvalidChecksum_ThrowsFormatException()
        {
            var data = Encoding.UTF8.GetBytes("Test");
            var encoded = Base58Helper.Encode(data);

            var exception = Assert.Throws<FormatException>(() => Base58Helper.DecodeWithCheckSum(encoded));

            Assert.Contains("checksum is invalid", exception.Message);
        }

        [Fact]
        public void AddCheckSum_AddsCorrectLength()
        {
            var data = new byte[] { 1, 2, 3, 4, 5 };

            var result = Base58Helper.AddCheckSum(data);

            Assert.Equal(data.Length + Base58Helper.CheckSumSizeInBytes, result.Length);
        }

        [Fact]
        public void VerifyAndRemoveCheckSum_ValidChecksum_ReturnsOriginal()
        {
            var original = new byte[] { 1, 2, 3, 4, 5 };
            var withChecksum = Base58Helper.AddCheckSum(original);

            var result = Base58Helper.VerifyAndRemoveCheckSum(withChecksum);

            Assert.NotNull(result);
            Assert.Equal(original, result);
        }

        [Fact]
        public void VerifyAndRemoveCheckSum_InvalidChecksum_ReturnsNull()
        {
            var data = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var result = Base58Helper.VerifyAndRemoveCheckSum(data);

            Assert.Null(result);
        }

        [Theory]
        [InlineData("Hello")]
        [InlineData("A")]
        [InlineData("1234567890")]
        [InlineData("Special chars: !@#$%")]
        public void RoundTrip_VariousStrings_WorksCorrectly(string input)
        {
            var data = Encoding.UTF8.GetBytes(input);
            var encoded = Base58Helper.Encode(data);
            var decoded = Base58Helper.Decode(encoded);
            var result = Encoding.UTF8.GetString(decoded);

            Assert.Equal(input, result);
        }

        [Fact]
        public void RoundTrip_WithChecksum_WorksCorrectly()
        {
            var original = "Test with checksum";
            var data = Encoding.UTF8.GetBytes(original);
            var encoded = Base58Helper.EncodeWithCheckSum(data);
            var decoded = Base58Helper.DecodeWithCheckSum(encoded);
            var result = Encoding.UTF8.GetString(decoded);

            Assert.Equal(original, result);
        }

        [Fact]
        public void Encode_LargeData_HandlesCorrectly()
        {
            var data = new byte[1000];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (byte)(i % 256);
            }

            var encoded = Base58Helper.Encode(data);
            var decoded = Base58Helper.Decode(encoded);

            Assert.Equal(data, decoded);
        }
    }
}
