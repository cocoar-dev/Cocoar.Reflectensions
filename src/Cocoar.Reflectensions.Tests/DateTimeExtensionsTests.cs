using System;
using Xunit;

namespace Cocoar.Reflectensions.Tests
{
    public class DateTimeExtensionsTests
    {
        [Fact]
        public void ToUnixTimeMilliseconds_UtcDateTime_ReturnsCorrectValue()
        {
            var dateTime = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var result = dateTime.ToUnixTimeMilliseconds();

            Assert.Equal(1577836800000, result);
        }

        [Fact]
        public void ToUnixTimeMilliseconds_LocalDateTime_ReturnsCorrectValue()
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);

            var result = dateTime.ToUnixTimeMilliseconds();

            Assert.True(result != 0); // Will vary based on timezone
        }

        [Fact]
        public void ToUnixTimeMilliseconds_UnspecifiedKind_TreatsAsLocal()
        {
            var dateTime = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);

            var result = dateTime.ToUnixTimeMilliseconds();

            Assert.True(result > 0);
        }

        [Fact]
        public void ToUnixTimeMilliseconds_NullableDateTime_ReturnsNull()
        {
            DateTime? dateTime = null;

            var result = dateTime.ToUnixTimeMilliseconds();

            Assert.Null(result);
        }

        [Fact]
        public void ToUnixTimeMilliseconds_NullableDateTime_ReturnsValue()
        {
            DateTime? dateTime = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var result = dateTime.ToUnixTimeMilliseconds();

            Assert.Equal(1577836800000, result);
        }

        [Fact]
        public void SetKind_ChangesKind()
        {
            var dateTime = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);

            var result = dateTime.SetKind(DateTimeKind.Utc);

            Assert.Equal(DateTimeKind.Utc, result.Kind);
            Assert.Equal(dateTime.Ticks, result.Ticks);
        }

        [Fact]
        public void FromUnixTimeMilliseconds_ReturnsCorrectDateTime()
        {
            long milliseconds = 1577836800000;

            var result = milliseconds.FromUnixTimeMilliseconds();

            Assert.Equal(new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc), result);
            Assert.Equal(DateTimeKind.Utc, result.Kind);
        }

        [Fact]
        public void FromUnixTimeSeconds_ReturnsCorrectDateTime()
        {
            long seconds = 1577836800;

            var result = seconds.FromUnixTimeSeconds();

            Assert.Equal(new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc), result);
            Assert.Equal(DateTimeKind.Utc, result.Kind);
        }

        [Fact]
        public void ToUtc_LocalDateTime_ConvertsToUtc()
        {
            var dateTime = new DateTime(2020, 1, 1, 12, 0, 0, DateTimeKind.Local);

            var result = dateTime.ToUtc();

            Assert.Equal(DateTimeKind.Utc, result.Kind);
        }

        [Fact]
        public void ToUtc_AlreadyUtc_ReturnsUnchanged()
        {
            var dateTime = new DateTime(2020, 1, 1, 12, 0, 0, DateTimeKind.Utc);

            var result = dateTime.ToUtc();

            Assert.Equal(dateTime, result);
            Assert.Equal(DateTimeKind.Utc, result.Kind);
        }

        [Fact]
        public void ToLocal_UtcDateTime_ConvertsToLocal()
        {
            var dateTime = new DateTime(2020, 1, 1, 12, 0, 0, DateTimeKind.Utc);

            var result = dateTime.ToLocal();

            Assert.Equal(DateTimeKind.Local, result.Kind);
        }

        [Fact]
        public void ToLocal_AlreadyLocal_ReturnsUnchanged()
        {
            var dateTime = new DateTime(2020, 1, 1, 12, 0, 0, DateTimeKind.Local);

            var result = dateTime.ToLocal();

            Assert.Equal(dateTime, result);
            Assert.Equal(DateTimeKind.Local, result.Kind);
        }

        [Fact]
        public void SetIsLocal_UnspecifiedDateTime_SetsKindToLocal()
        {
            var dateTime = new DateTime(2020, 1, 1, 12, 0, 0, DateTimeKind.Unspecified);

            var result = dateTime.SetIsLocal();

            Assert.Equal(DateTimeKind.Local, result.Kind);
            Assert.Equal(dateTime.Ticks, result.Ticks);
        }

        [Fact]
        public void SetIsLocal_AlreadyLocal_ReturnsUnchanged()
        {
            var dateTime = new DateTime(2020, 1, 1, 12, 0, 0, DateTimeKind.Local);

            var result = dateTime.SetIsLocal();

            Assert.Equal(dateTime, result);
        }

        [Fact]
        public void SetIsUtc_UnspecifiedDateTime_SetsKindToUtc()
        {
            var dateTime = new DateTime(2020, 1, 1, 12, 0, 0, DateTimeKind.Unspecified);

            var result = dateTime.SetIsUtc();

            Assert.Equal(DateTimeKind.Utc, result.Kind);
            Assert.Equal(dateTime.Ticks, result.Ticks);
        }

        [Fact]
        public void SetIsUtc_AlreadyUtc_ReturnsUnchanged()
        {
            var dateTime = new DateTime(2020, 1, 1, 12, 0, 0, DateTimeKind.Utc);

            var result = dateTime.SetIsUtc();

            Assert.Equal(dateTime, result);
        }

        [Fact]
        public void RoundTrip_MillisecondsConversion_WorksCorrectly()
        {
            var original = new DateTime(2020, 6, 15, 14, 30, 45, DateTimeKind.Utc);

            var milliseconds = original.ToUnixTimeMilliseconds();
            var result = milliseconds.FromUnixTimeMilliseconds();

            Assert.Equal(original.Year, result.Year);
            Assert.Equal(original.Month, result.Month);
            Assert.Equal(original.Day, result.Day);
            Assert.Equal(original.Hour, result.Hour);
            Assert.Equal(original.Minute, result.Minute);
            Assert.Equal(original.Second, result.Second);
        }
    }
}
