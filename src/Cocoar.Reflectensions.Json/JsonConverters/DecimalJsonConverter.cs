using System;
using Newtonsoft.Json;

namespace Cocoar.Reflectensions.JsonConverters {
    public class DecimalJsonConverter : Newtonsoft.Json.JsonConverter
    {

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {

            var dec = reader.ReadAsString();
            if (int.TryParse(dec, out int _int))
            {
                return _int;
            }

            if (long.TryParse(dec, out long _long))
            {
                return _long;
            }

            if (double.TryParse(dec, out double _double))
            {
                return _double;
            }

            if (decimal.TryParse(dec, out var _deci))
            {
                return _deci;
            }

            return Convert.ChangeType(dec, objectType);
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(decimal) || objectType == typeof(float) || objectType == typeof(double) || objectType == typeof(int) || objectType == typeof(long));
        }

        public override bool CanRead => false;


        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (DecimalJsonConverter.IsWholeValue(value))
            {
                writer.WriteRawValue(JsonConvert.ToString(Convert.ToInt64(value)));
            }
            else
            {
                writer.WriteRawValue(JsonConvert.ToString(value));
            }
        }

        private static bool IsWholeValue(object? value)
        {
            if (value is decimal)
            {
                decimal decimalValue = (decimal)value;
                int precision = (Decimal.GetBits(decimalValue)[3] >> 16) & 0x000000FF;
                return precision == 0;
            }
            else if (value is float || value is double)
            {
                double doubleValue = Convert.ToDouble(value);
                return doubleValue == Math.Truncate(doubleValue);
            }

            return false;
        }
    }
}
