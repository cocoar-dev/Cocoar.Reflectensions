using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Cocoar.Reflectensions.Helper;

namespace Cocoar.Reflectensions {
    /// <summary>
    /// Provides extension methods for string manipulation, validation, and type conversion operations.
    /// </summary>
    public static class StringExtensions {

        /// <summary>
        /// Splits a string using a string separator.
        /// </summary>
        /// <param name="value">The string to split.</param>
        /// <param name="split">The string separator.</param>
        /// <param name="options">The string split options.</param>
        /// <returns>An array of substrings.</returns>
        public static string[] Split(this string value, string split, StringSplitOptions options) {

            if (value == null) {
                return Array.Empty<string>();
            }

            return value.Split(new[] { split }, options);

        }

        /// <summary>
        /// Splits a string using a string separator with optional removal of empty entries.
        /// </summary>
        /// <param name="value">The string to split.</param>
        /// <param name="split">The string separator.</param>
        /// <param name="removeEmptyEntries">If true, removes empty entries from the result.</param>
        /// <returns>An array of substrings.</returns>
        public static string[] Split(this string value, string split, bool removeEmptyEntries = false) {
            return Split(value, split, removeEmptyEntries ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);
        }
        
        /// <summary>
        /// Trims the specified characters from the start and end of the string.
        /// </summary>
        /// <param name="value">The string to trim.</param>
        /// <param name="trimCharacters">The strings containing characters to trim.</param>
        /// <returns>The trimmed string.</returns>
        public static string Trim(this string value, params string[] trimCharacters) {
            return value?.Trim(String.Join("", trimCharacters).ToCharArray()) ?? "";
        }
        
        /// <summary>
        /// Trims the string and returns null if the result is empty.
        /// </summary>
        /// <param name="value">The string to trim.</param>
        /// <returns>The trimmed string or null if empty.</returns>
        public static string? TrimToNull(this string? value) {
            if (value == null)
                return null;

            return ToNull(value.Trim());
        }

        /// <summary>
        /// Removes empty lines from the string.
        /// </summary>
        /// <param name="value">The string to process.</param>
        /// <returns>The string with empty lines removed.</returns>
        public static string RemoveEmptyLines(this string value) {
            var val = Regex.Replace(value, @"^\s*$\n|\r", "", RegexOptions.Multiline);
            return val;
        }

        /// <summary>
        /// Removes the specified number of characters from the end of the string.
        /// </summary>
        /// <param name="value">The string to process.</param>
        /// <param name="length">The number of characters to remove.</param>
        /// <returns>The string with characters removed from the end.</returns>
        public static string RemoveEnd(this string value, int length)
        {
            return value.Substring(0, value.Length - length);
        }

        /// <summary>
        /// Removes the specified suffix from the end of the string if present.
        /// </summary>
        /// <param name="value">The string to process.</param>
        /// <param name="remove">The suffix to remove.</param>
        /// <param name="ignoreCase">If true, performs case-insensitive comparison.</param>
        /// <returns>The string with the suffix removed if it was present.</returns>
        public static string RemoveEnd(this string value, string remove, bool ignoreCase = true)
        {

            if (value.EndsWith(remove, ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture))
            {
                return RemoveEnd(value, remove.Length);
            }

            return value;
        }
        public static bool Match(this string value, string matchString, bool ignoreCase = true, bool invert = false)
        {
            return WildcardHelper.Match(value, matchString, ignoreCase, invert);
        }

        #region StringIs
        public static bool IsNullOrWhiteSpace(this string value) {
            return String.IsNullOrWhiteSpace(value);
        }

        public static bool IsNumeric(this string value) {
            if (IsNullOrWhiteSpace(value))
                return false;

            bool firstchar = true;
            foreach (char c in value) {
                if (firstchar) {
                    if (!char.IsDigit(c) && c != '.' && c != '-' && c != '+') {
                        return false;
                    }
                } else {
                    if (!char.IsDigit(c) && c != '.') {
                        return false;
                    }
                }
                firstchar = false;

            }

            return true;
        }

        public static bool IsInt(this string value) {
            if (!IsNumeric(value))
                return false;

            int n;
            return int.TryParse(value, out n);

        }

        public static bool IsLong(this string value) {
            if (!IsNumeric(value))
                return false;

            long n;
            return long.TryParse(value, out n);

        }

        public static bool IsDouble(this string value) {
            if (!IsNumeric(value))
                return false;

            double n;
            return double.TryParse(value, out n);

        }

        public static bool IsDateTime(this string value, string? customFormat = null) {

            return ToNullableDateTime(value) != null;

        }

        public static bool IsBoolean(this string value) {
            bool ret;
            return bool.TryParse(value, out ret);
        }

        public static bool IsValidIp(this string value) {
            return System.Net.IPAddress.TryParse(value, out var _);
        }


        public static bool IsBase64Encoded(this string value) {
            if (string.IsNullOrEmpty(value))
                return false;

            try {
                byte[] data = Convert.FromBase64String(value);
                return (value.Replace(" ", "").Length % 4 == 0);
            } catch {
                return false;
            }
        }

        public static bool IsLowerCase(this string value) {
            return !string.IsNullOrEmpty(value) && !value.Any(char.IsUpper);
        }

        public static bool IsUpperCase(this string value) {
            return !string.IsNullOrEmpty(value) && !value.Any(char.IsLower);
        }

        private static string DomainMapper(Match match) {
            var idn = new IdnMapping();
            string domainName = match.Groups[2].Value;
            domainName = idn.GetAscii(domainName);
            return match.Groups[1].Value + domainName;
        }

        public static Boolean IsValidEmailAddress(this string value) {
            if (String.IsNullOrEmpty(value))
                return false;

            try {
                value = Regex.Replace(value, @"(@)(.+)$", DomainMapper);
            } catch {
                return false;
            }

            try {
                return Regex.IsMatch(value,
                        @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                        @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
                        RegexOptions.IgnoreCase);
            } catch {
                return false;
            }
        }


        public static bool IsGuid(this string value) {
            value = Trim(value, "\"");

            return Guid.TryParse(value, out _);
        }

        #endregion

        #region StringTo
        public static string? ToNull(this string value) {
            return String.IsNullOrEmpty(value) ? null : value;
        }

        public static int ToInt(this string value) {
            return !IsInt(value) ? default : int.Parse(value, CultureInfo.InvariantCulture);
        }
        public static int? ToNullableInt(this string? value) {
            if (value == null)
                return null;

            if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var i)) {
                return i;
            }
            return null;
        }

        public static decimal ToDecimal(this string value) {
            return !IsNumeric(value) ? default : decimal.Parse(value, CultureInfo.InvariantCulture);
        }
        public static decimal? ToNullableDecimal(this string value) {
            if (value == null)
                return null;

            if (decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out var i)) {
                return i;
            }
            return null;
        }

        public static float ToFloat(this string value) {
            return !IsInt(value) ? default : float.Parse(value, CultureInfo.InvariantCulture);
        }
        public static float? ToNullableFloat(this string value) {
            if (value == null)
                return null;

            if (float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var i)) {
                return i;
            }
            return null;
        }

        public static long ToLong(this string value) {
            return !IsLong(value) ? default : long.Parse(value, CultureInfo.InvariantCulture);
        }
        public static long? ToNullableLong(this string value) {
            if (value == null)
                return null;

            if (long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var i)) {
                return i;
            }
            return null;
        }

        public static double ToDouble(this string value) {
            if (!IsDouble(value))
                return default;

            return double.Parse(value, CultureInfo.InvariantCulture);

        }
        public static double? ToNullableDouble(this string value) {
            if (value == null)
                return null;

            if (double.TryParse(value, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var i)) {
                return i;
            }
            return null;
        }


        public static bool ToBoolean(this string value) {
            if (!IsBoolean(value))
                return default;

            return bool.Parse(value);
        }

        public static DateTime? ToNullableDateTime(this string value, string? customFormat = null) {

            if (String.IsNullOrWhiteSpace(value?.Trim('"')))
                return null;

            DateTime dateTime = default(DateTime);

            List<string> formats = new List<string>();

            if (customFormat != null && !string.IsNullOrEmpty(customFormat)) {
                formats.Add(customFormat);
            } else {
                formats = new List<string>{

                    "d.M.yyyy",
                    "dd.M.yyyy",
                    "d.MM.yyyy",
                    "dd.MM.yyyy",

                    "d.M.yyyy HH:mm:ss.FFFFF",
                    "d.MM.yyyy HH:mm:ss.FFFFF",
                    "dd.M.yyyy HH:mm:ss.FFFFF",
                    "dd.MM.yyyy HH:mm:ss.FFFFF",

                    "d.M.yyyy H:mm:ss.FFFFF",
                    "d.MM.yyyy H:mm:ss.FFFFF",
                    "dd.M.yyyy H:mm:ss.FFFFF",
                    "dd.MM.yyyy H:mm:ss.FFFFF",

                    "M/d/yyyy h:mm:ss tt",
                    "M/d/yyyy h:mm:ss.FFFFF tt",
                    "M/d/yyyy h:mm tt",
                    "MM/dd/yyyy hh:mm:ss.FFFFF",
                    "M/d/yyyy h:mm:ss.FFFFF",
                    "M/d/yyyy hh:mm tt",
                    "M/d/yyyy hh tt",
                    "M/d/yyyy h:mm",
                    "M/d/yyyy h:mm",
                    "MM/dd/yyyy hh:mm",
                    "M/dd/yyyy hh:mm"
                };
            }


            if (DateTime.TryParseExact(value, formats.ToArray(),
                CultureInfo.CurrentCulture,
                System.Globalization.DateTimeStyles.AssumeLocal, out dateTime)) {
                return dateTime;
            }

            // Try standard DateTime.Parse as last resort (handles ISO 8601 and other standard formats)
            if (DateTime.TryParse(value, CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dateTime)) {
                return dateTime;
            }

            return null;
        }

        public static DateTime ToDateTime(this string value, string? customFormat = null) {
            return ToNullableDateTime(value, customFormat) ?? throw new InvalidCastException();
        }

        public static string EncodeToBase64(this string value) {
            if (string.IsNullOrEmpty(value))
                return "";

            var toEncodeAsBytes = System.Text.Encoding.UTF8.GetBytes(value);
            var returnValue = Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        public static string DecodeFromBase64(this string value) {
            if (string.IsNullOrEmpty(value))
                return "";

            var encodedDataAsBytes = Convert.FromBase64String(value);
            var returnValue = System.Text.Encoding.UTF8.GetString(encodedDataAsBytes);
            return returnValue;
        }

        public static string EncodeToBase58(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            var toEncodeAsBytes = System.Text.Encoding.UTF8.GetBytes(value);
            var returnValue = Base58Helper.Encode(toEncodeAsBytes);
            return returnValue;
        }

        public static string DecodeFromBase58(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            var encodedDataAsBytes = Base58Helper.Decode(value);
            var returnValue = System.Text.Encoding.UTF8.GetString(encodedDataAsBytes);
            return returnValue;
        }

        public static Guid ToGuid(this string value) {
            value = Trim(value, "\"");

            if (Guid.TryParse(value, out var g)) {
                return g;
            }

            throw new InvalidCastException($"Can't cast '{value}' to GUID");
        }



        #endregion

    }
}
