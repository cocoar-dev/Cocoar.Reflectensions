using System;

namespace Cocoar.Reflectensions.Common
{
    public static class DateTimeExtensions
    {
        public static long ToUnixTimeMilliseconds(this DateTime dateTime)
        {
            DateTimeOffset epcoOff = new DateTimeOffset(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            if (dateTime.Kind == DateTimeKind.Unspecified)
            {
                dateTime = dateTime.SetKind(DateTimeKind.Local);
            }
            else
            {
                dateTime = dateTime.SetKind(dateTime.Kind);
            }

            DateTimeOffset dto = new DateTimeOffset(dateTime);
            return (dto.UtcTicks - epcoOff.UtcTicks) / TimeSpan.TicksPerMillisecond;
        }

        public static long? ToUnixTimeMilliseconds(this DateTime? dateTime)
        {
            return dateTime?.ToUnixTimeMilliseconds();
        }

        public static DateTime SetKind(this DateTime datetime, DateTimeKind kind)
        {
            return new DateTime(datetime.Ticks, kind);
        }

        public static DateTime FromUnixTimeMilliseconds(this long milliseconds)
        {
            DateTimeOffset epcoOff = new DateTimeOffset(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));

            DateTime dt = epcoOff.AddMilliseconds(milliseconds).DateTime;


            return dt.SetIsUtc();
        }

        public static DateTime FromUnixTimeSeconds(this long seconds)
        {
            DateTimeOffset epcoOff = new DateTimeOffset(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));

            DateTime dt = epcoOff.AddSeconds(seconds).DateTime;


            return dt.SetIsUtc();
        }

        public static DateTime ToUtc(this DateTime dt)
        {
            if (dt.Kind != DateTimeKind.Utc)
            {
                dt = dt.ToUniversalTime();
            }
            return dt;
        }

        public static DateTime ToLocal(this DateTime dt)
        {
            if (dt.Kind != DateTimeKind.Local)
            {
                dt = dt.ToLocalTime();
            }
            return dt;
        }

        public static DateTime SetIsLocal(this DateTime dt)
        {
            if (dt.Kind != DateTimeKind.Local)
                dt = dt.SetKind(DateTimeKind.Local);

            return dt;
        }

        public static DateTime SetIsUtc(this DateTime dt)
        {
            if (dt.Kind != DateTimeKind.Utc)
                dt = dt.SetKind(DateTimeKind.Utc);

            return dt;
        }

    }
}
