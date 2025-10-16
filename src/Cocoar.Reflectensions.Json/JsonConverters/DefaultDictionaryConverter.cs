using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Cocoar.Reflectensions.JsonConverters {
    public sealed class DefaultDictionaryConverter : CustomCreationConverter<IDictionary<string, object>> {

        public override IDictionary<string, object> Create(Type objectType) {
            return new Dictionary<string, object>();
        }

        public override bool CanConvert(Type objectType) {
            // in addition to handling ExpandoObject
            // we want to handle the deserialization of dict value
            // which is of type object

            var type = objectType;
            while (type != null)
            {
                if (type.FullName?.Equals("Cocoar.Reflectensions.Internal.ExpandableBaseObject") == true)
                    return false;

                type = type.BaseType;
            }
            

            return objectType == typeof(object) || base.CanConvert(objectType);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer) {
            if (reader.TokenType == JsonToken.StartObject || reader.TokenType == JsonToken.Null)
                return base.ReadJson(reader, objectType, existingValue, serializer);

            // if the next token is not an object
            // then fall back on standard deserializer (strings, numbers etc.)
            return serializer.Deserialize(reader);
        }
    }
}
