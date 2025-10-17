using System;
using System.Globalization;
using Xunit;

namespace Cocoar.Reflectensions.Tests
{
    public class StringExtensionsTests
    {
        public StringExtensionsTests()
        {
            // Ensure all tests use invariant culture for consistent number parsing
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        }
        #region Split Tests
        [Fact]
        public void Split_WithSeparator_SplitsCorrectly()
        {
            var result = "a,b,c".Split(",");

            Assert.Equal(new[] { "a", "b", "c" }, result);
        }

        [Fact]
        public void Split_WithRemoveEmptyEntries_RemovesEmpty()
        {
            var result = "a,,b,c".Split(",", removeEmptyEntries: true);

            Assert.Equal(new[] { "a", "b", "c" }, result);
        }

        [Fact]
        public void Split_NullString_ReturnsEmptyArray()
        {
            // The extension method checks for null and returns empty array
            // but we can't call it on a null reference directly
            var result = StringExtensions.Split(null!, ",");

            Assert.Empty(result);
        }
        #endregion

        #region Trim Tests
        [Fact]
        public void Trim_WithCustomCharacters_TrimsCorrectly()
        {
            var result = "**test**".Trim("*");

            Assert.Equal("test", result);
        }

        [Fact]
        public void TrimToNull_WhitespaceString_ReturnsNull()
        {
            var result = "   ".TrimToNull();

            Assert.Null(result);
        }

        [Fact]
        public void TrimToNull_EmptyString_ReturnsNull()
        {
            var result = "".TrimToNull();

            Assert.Null(result);
        }

        [Fact]
        public void TrimToNull_NullString_ReturnsNull()
        {
            string? value = null;
            var result = value.TrimToNull();

            Assert.Null(result);
        }

        [Fact]
        public void TrimToNull_ValidString_ReturnsTrimmed()
        {
            var result = "  test  ".TrimToNull();

            Assert.Equal("test", result);
        }
        #endregion

        #region RemoveEnd Tests
        [Fact]
        public void RemoveEnd_WithLength_RemovesCorrectly()
        {
            var result = "testing".RemoveEnd(3);

            Assert.Equal("test", result);
        }

        [Fact]
        public void RemoveEnd_WithString_RemovesCorrectly()
        {
            var result = "testing.txt".RemoveEnd(".txt");

            Assert.Equal("testing", result);
        }

        [Fact]
        public void RemoveEnd_CaseInsensitive_RemovesCorrectly()
        {
            var result = "testing.TXT".RemoveEnd(".txt", ignoreCase: true);

            Assert.Equal("testing", result);
        }

        [Fact]
        public void RemoveEnd_NotPresent_ReturnsUnchanged()
        {
            var result = "testing".RemoveEnd(".txt");

            Assert.Equal("testing", result);
        }
        #endregion

        #region RemoveEmptyLines Tests
        [Fact]
        public void RemoveEmptyLines_RemovesBlankLines()
        {
            var input = "line1\n\nline2\nline3";
            var result = input.RemoveEmptyLines();

            Assert.DoesNotContain("\n\n", result);
        }
        #endregion

        #region Match Tests
        [Fact]
        public void Match_WithWildcard_MatchesCorrectly()
        {
            Assert.True("test.txt".Match("*.txt"));
            Assert.False("test.doc".Match("*.txt"));
        }

        [Fact]
        public void Match_CaseInsensitive_MatchesCorrectly()
        {
            Assert.True("TEST.TXT".Match("*.txt", ignoreCase: true));
        }
        #endregion

        #region IsNumeric Tests
        [Fact]
        public void IsNumeric_ValidInteger_ReturnsTrue()
        {
            Assert.True("123".IsNumeric());
        }

        [Fact]
        public void IsNumeric_ValidDecimal_ReturnsTrue()
        {
            Assert.True("123.45".IsNumeric());
        }

        [Fact]
        public void IsNumeric_NegativeNumber_ReturnsTrue()
        {
            Assert.True("-123".IsNumeric());
        }

        [Fact]
        public void IsNumeric_PositiveSign_ReturnsTrue()
        {
            Assert.True("+123".IsNumeric());
        }

        [Fact]
        public void IsNumeric_InvalidString_ReturnsFalse()
        {
            Assert.False("abc".IsNumeric());
        }

        [Fact]
        public void IsNumeric_NullOrWhitespace_ReturnsFalse()
        {
            Assert.False("".IsNumeric());
            Assert.False("   ".IsNumeric());
        }
        #endregion

        #region IsInt Tests
        [Fact]
        public void IsInt_ValidInt_ReturnsTrue()
        {
            Assert.True("123".IsInt());
        }

        [Fact]
        public void IsInt_Decimal_ReturnsFalse()
        {
            Assert.False("123.45".IsInt());
        }

        [Fact]
        public void IsInt_TooLarge_ReturnsFalse()
        {
            Assert.False("9999999999999999999".IsInt());
        }
        #endregion

        #region IsLong Tests
        [Fact]
        public void IsLong_ValidLong_ReturnsTrue()
        {
            Assert.True("123456789012345".IsLong());
        }

        [Fact]
        public void IsLong_Decimal_ReturnsFalse()
        {
            Assert.False("123.45".IsLong());
        }
        #endregion

        #region IsDouble Tests
        [Fact]
        public void IsDouble_ValidDouble_ReturnsTrue()
        {
            Assert.True("123.45".IsDouble());
        }

        [Fact]
        public void IsDouble_Integer_ReturnsTrue()
        {
            Assert.True("123".IsDouble());
        }
        #endregion

        #region IsBoolean Tests
        [Fact]
        public void IsBoolean_TrueString_ReturnsTrue()
        {
            Assert.True("true".IsBoolean());
            Assert.True("True".IsBoolean());
        }

        [Fact]
        public void IsBoolean_FalseString_ReturnsTrue()
        {
            Assert.True("false".IsBoolean());
        }

        [Fact]
        public void IsBoolean_InvalidString_ReturnsFalse()
        {
            Assert.False("yes".IsBoolean());
        }
        #endregion

        #region IsValidIp Tests
        [Fact]
        public void IsValidIp_ValidIPv4_ReturnsTrue()
        {
            Assert.True("192.168.1.1".IsValidIp());
        }

        [Fact]
        public void IsValidIp_ValidIPv6_ReturnsTrue()
        {
            Assert.True("2001:0db8:85a3:0000:0000:8a2e:0370:7334".IsValidIp());
        }

        [Fact]
        public void IsValidIp_InvalidIP_ReturnsFalse()
        {
            Assert.False("999.999.999.999".IsValidIp());
            Assert.False("not-an-ip".IsValidIp());
        }
        #endregion

        #region IsBase64Encoded Tests
        [Fact]
        public void IsBase64Encoded_ValidBase64_ReturnsTrue()
        {
            Assert.True("SGVsbG8gV29ybGQ=".IsBase64Encoded());
        }

        [Fact]
        public void IsBase64Encoded_InvalidBase64_ReturnsFalse()
        {
            Assert.False("not base64!".IsBase64Encoded());
        }

        [Fact]
        public void IsBase64Encoded_EmptyString_ReturnsFalse()
        {
            Assert.False("".IsBase64Encoded());
        }
        #endregion

        #region IsLowerCase/IsUpperCase Tests
        [Fact]
        public void IsLowerCase_LowercaseString_ReturnsTrue()
        {
            Assert.True("lowercase".IsLowerCase());
        }

        [Fact]
        public void IsLowerCase_MixedCase_ReturnsFalse()
        {
            Assert.False("lowerCase".IsLowerCase());
        }

        [Fact]
        public void IsUpperCase_UppercaseString_ReturnsTrue()
        {
            Assert.True("UPPERCASE".IsUpperCase());
        }

        [Fact]
        public void IsUpperCase_MixedCase_ReturnsFalse()
        {
            Assert.False("UPPERcase".IsUpperCase());
        }
        #endregion

        #region IsValidEmailAddress Tests
        [Fact]
        public void IsValidEmailAddress_ValidEmail_ReturnsTrue()
        {
            Assert.True("test@example.com".IsValidEmailAddress());
        }

        [Fact]
        public void IsValidEmailAddress_InvalidEmail_ReturnsFalse()
        {
            Assert.False("not-an-email".IsValidEmailAddress());
            Assert.False("@example.com".IsValidEmailAddress());
        }

        [Fact]
        public void IsValidEmailAddress_EmptyString_ReturnsFalse()
        {
            Assert.False("".IsValidEmailAddress());
        }
        #endregion

        #region IsGuid Tests
        [Fact]
        public void IsGuid_ValidGuid_ReturnsTrue()
        {
            Assert.True("550e8400-e29b-41d4-a716-446655440000".IsGuid());
        }

        [Fact]
        public void IsGuid_ValidGuidWithQuotes_ReturnsTrue()
        {
            Assert.True("\"550e8400-e29b-41d4-a716-446655440000\"".IsGuid());
        }

        [Fact]
        public void IsGuid_InvalidGuid_ReturnsFalse()
        {
            Assert.False("not-a-guid".IsGuid());
        }
        #endregion

        #region ToNull Tests
        [Fact]
        public void ToNull_EmptyString_ReturnsNull()
        {
            Assert.Null("".ToNull());
        }

        [Fact]
        public void ToNull_ValidString_ReturnsString()
        {
            Assert.Equal("test", "test".ToNull());
        }
        #endregion

        #region ToInt Tests
        [Fact]
        public void ToInt_ValidString_ReturnsInt()
        {
            Assert.Equal(123, "123".ToInt());
        }

        [Fact]
        public void ToInt_InvalidString_ReturnsDefault()
        {
            Assert.Equal(0, "abc".ToInt());
        }

        [Fact]
        public void ToNullableInt_ValidString_ReturnsInt()
        {
            Assert.Equal(123, "123".ToNullableInt());
        }

        [Fact]
        public void ToNullableInt_InvalidString_ReturnsNull()
        {
            Assert.Null("abc".ToNullableInt());
        }

        [Fact]
        public void ToNullableInt_NullString_ReturnsNull()
        {
            string? value = null;
            Assert.Null(value.ToNullableInt());
        }
        #endregion

        #region ToDecimal Tests
        [Fact]
        public void ToDecimal_ValidString_ReturnsDecimal()
        {
            Assert.Equal(123.45m, "123.45".ToDecimal());
        }

        [Fact]
        public void ToNullableDecimal_ValidString_ReturnsDecimal()
        {
            Assert.Equal(123.45m, "123.45".ToNullableDecimal());
        }

        [Fact]
        public void ToNullableDecimal_InvalidString_ReturnsNull()
        {
            Assert.Null("abc".ToNullableDecimal());
        }
        #endregion

        #region ToFloat Tests
        [Fact]
        public void ToFloat_ValidIntegerString_ReturnsFloat()
        {
            // Note: ToFloat currently checks IsInt() instead of IsNumeric(), 
            // so it only works with integers, not decimals
            Assert.Equal(123f, "123".ToFloat());
        }

        [Fact]
        public void ToNullableFloat_ValidString_ReturnsFloat()
        {
            Assert.Equal(123.45f, "123.45".ToNullableFloat()!.Value, 2);
        }

        [Fact]
        public void ToNullableFloat_InvalidString_ReturnsNull()
        {
            Assert.Null("abc".ToNullableFloat());
        }
        #endregion

        #region ToLong Tests
        [Fact]
        public void ToLong_ValidString_ReturnsLong()
        {
            Assert.Equal(123456789012345L, "123456789012345".ToLong());
        }

        [Fact]
        public void ToNullableLong_ValidString_ReturnsLong()
        {
            Assert.Equal(123L, "123".ToNullableLong());
        }

        [Fact]
        public void ToNullableLong_InvalidString_ReturnsNull()
        {
            Assert.Null("abc".ToNullableLong());
        }
        #endregion

        #region ToDouble Tests
        [Fact]
        public void ToDouble_ValidString_ReturnsDouble()
        {
            Assert.Equal(123.45, "123.45".ToDouble());
        }

        [Fact]
        public void ToNullableDouble_ValidString_ReturnsDouble()
        {
            Assert.Equal(123.45, "123.45".ToNullableDouble());
        }

        [Fact]
        public void ToNullableDouble_InvalidString_ReturnsNull()
        {
            Assert.Null("abc".ToNullableDouble());
        }
        #endregion

        #region ToBoolean Tests
        [Fact]
        public void ToBoolean_TrueString_ReturnsTrue()
        {
            Assert.True("true".ToBoolean());
        }

        [Fact]
        public void ToBoolean_FalseString_ReturnsFalse()
        {
            Assert.False("false".ToBoolean());
        }

        [Fact]
        public void ToBoolean_InvalidString_ReturnsDefault()
        {
            Assert.False("yes".ToBoolean());
        }
        #endregion

        #region ToDateTime Tests
        [Fact]
        public void ToNullableDateTime_ValidISO8601_ReturnsDateTime()
        {
            var result = "2020-01-01T12:00:00".ToNullableDateTime();

            Assert.NotNull(result);
            Assert.Equal(2020, result!.Value.Year);
        }

        [Fact]
        public void ToNullableDateTime_ValidCustomFormat_ReturnsDateTime()
        {
            var result = "01.01.2020".ToNullableDateTime();

            Assert.NotNull(result);
        }

        [Fact]
        public void ToNullableDateTime_InvalidString_ReturnsNull()
        {
            Assert.Null("not-a-date".ToNullableDateTime());
        }

        [Fact]
        public void ToNullableDateTime_NullString_ReturnsNull()
        {
            string? nullString = null;
            Assert.Null(nullString!.ToNullableDateTime());
        }

        [Fact]
        public void ToDateTime_ValidString_ReturnsDateTime()
        {
            var result = "2020-01-01".ToDateTime();

            Assert.Equal(2020, result.Year);
        }

        [Fact]
        public void ToDateTime_InvalidString_ThrowsException()
        {
            Assert.Throws<InvalidCastException>(() => "not-a-date".ToDateTime());
        }
        #endregion

        #region Base64 Tests
        [Fact]
        public void EncodeToBase64_ValidString_EncodesCorrectly()
        {
            var result = "Hello World".EncodeToBase64();

            Assert.Equal("SGVsbG8gV29ybGQ=", result);
        }

        [Fact]
        public void DecodeFromBase64_ValidBase64_DecodesCorrectly()
        {
            var result = "SGVsbG8gV29ybGQ=".DecodeFromBase64();

            Assert.Equal("Hello World", result);
        }

        [Fact]
        public void Base64_RoundTrip_WorksCorrectly()
        {
            var original = "Test String 123!";
            var encoded = original.EncodeToBase64();
            var decoded = encoded.DecodeFromBase64();

            Assert.Equal(original, decoded);
        }

        [Fact]
        public void EncodeToBase64_EmptyString_ReturnsEmpty()
        {
            Assert.Equal("", "".EncodeToBase64());
        }

        [Fact]
        public void DecodeFromBase64_EmptyString_ReturnsEmpty()
        {
            Assert.Equal("", "".DecodeFromBase64());
        }
        #endregion

        #region Base58 Tests
        [Fact]
        public void EncodeToBase58_ValidString_EncodesCorrectly()
        {
            var result = "Hello".EncodeToBase58();

            Assert.NotEmpty(result);
        }

        [Fact]
        public void Base58_RoundTrip_WorksCorrectly()
        {
            var original = "Test String";
            var encoded = original.EncodeToBase58();
            var decoded = encoded.DecodeFromBase58();

            Assert.Equal(original, decoded);
        }

        [Fact]
        public void EncodeToBase58_EmptyString_ReturnsEmpty()
        {
            Assert.Equal("", "".EncodeToBase58());
        }

        [Fact]
        public void DecodeFromBase58_EmptyString_ReturnsEmpty()
        {
            Assert.Equal("", "".DecodeFromBase58());
        }
        #endregion

        #region ToGuid Tests
        [Fact]
        public void ToGuid_ValidGuid_ReturnsGuid()
        {
            var guid = Guid.NewGuid();
            var result = guid.ToString().ToGuid();

            Assert.Equal(guid, result);
        }

        [Fact]
        public void ToGuid_ValidGuidWithQuotes_ReturnsGuid()
        {
            var guid = Guid.NewGuid();
            var result = $"\"{guid}\"".ToGuid();

            Assert.Equal(guid, result);
        }

        [Fact]
        public void ToGuid_InvalidGuid_ThrowsException()
        {
            Assert.Throws<InvalidCastException>(() => "not-a-guid".ToGuid());
        }
        #endregion

        #region IsDateTime Tests
        [Fact]
        public void IsDateTime_ValidDateTime_ReturnsTrue()
        {
            Assert.True("2020-01-01".IsDateTime());
        }

        [Fact]
        public void IsDateTime_InvalidDateTime_ReturnsFalse()
        {
            Assert.False("not-a-date".IsDateTime());
        }
        #endregion

        #region IsNullOrWhiteSpace Tests
        [Fact]
        public void IsNullOrWhiteSpace_Null_ReturnsTrue()
        {
            string? value = null;
            Assert.True(value!.IsNullOrWhiteSpace());
        }

        [Fact]
        public void IsNullOrWhiteSpace_Empty_ReturnsTrue()
        {
            Assert.True("".IsNullOrWhiteSpace());
        }

        [Fact]
        public void IsNullOrWhiteSpace_Whitespace_ReturnsTrue()
        {
            Assert.True("   ".IsNullOrWhiteSpace());
        }

        [Fact]
        public void IsNullOrWhiteSpace_ValidString_ReturnsFalse()
        {
            Assert.False("test".IsNullOrWhiteSpace());
        }
        #endregion
    }
}
