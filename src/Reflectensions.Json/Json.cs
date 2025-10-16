using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Cocoar.Reflectensions.Common;
using Cocoar.Reflectensions.ExtensionMethods;
using Cocoar.Reflectensions.JsonConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Cocoar.Reflectensions {
    public class Json : IJson {


        private readonly List<Type> _jsonConversters = new List<Type>();

        private JsonSerializerSettings JsonSerializerSettings { get; }

        public JsonSerializer JsonSerializer => JsonSerializer.Create(JsonSerializerSettings);

        private JsonFlattener JsonFlattener() => new JsonFlattener(JsonSerializer);



        public Json RegisterJsonConverter<T>(int? index = null) where T : JsonConverter {
            return RegisterJsonConverter(typeof(T), index);
        }

        public Json RegisterJsonConverter(JsonConverter jsonConverter, int? index = null) {
            return RegisterJsonConverter(jsonConverter?.GetType());
        }

        public Json RegisterJsonConverter(Type? jsonConverterType, int? index = null) {
            if (jsonConverterType == null)
                return this;

            if (!jsonConverterType.IsSubclassOf(typeof(JsonConverter))) {
                throw new ArgumentException($"'{jsonConverterType}' is not a sub class of '{typeof(JsonConverter)}'");
            }


            if (!_jsonConversters.Contains(jsonConverterType)) {
                if (index.HasValue) {
                    _jsonConversters.Insert(index.Value, jsonConverterType);
                } else {
                    _jsonConversters.Add(jsonConverterType);
                }
            }

            var l = _jsonConversters.Select(Activator.CreateInstance).Cast<JsonConverter>().ToList();
            JsonSerializerSettings.Converters = l;

            return this;
        }



        public Json UnRegisterJsonConverter<T>() where T : JsonConverter {
            return UnRegisterJsonConverter(typeof(T));
        }

        public Json UnRegisterJsonConverter(JsonConverter jsonConverter) {
            return UnRegisterJsonConverter(jsonConverter.GetType());
        }

        public Json UnRegisterJsonConverter(Type? jsonConverterType) {

            if (jsonConverterType == null)
                return this;

            if (_jsonConversters.Contains(jsonConverterType))
                _jsonConversters.Remove(jsonConverterType);

            var l = _jsonConversters.Select(Activator.CreateInstance).Cast<JsonConverter>().ToList();
            JsonSerializerSettings.Converters = l;
            return this;
        }


        public Json SetContractResolver<T>() where T : IContractResolver {
            JsonSerializerSettings.ContractResolver = Activator.CreateInstance<T>();
            return this;
        }

        public Json SetContractResolver(Type contractResolverType) {

            if (!typeof(IContractResolver).IsAssignableFrom(contractResolverType)) {
                throw new ArgumentException($"'{contractResolverType}' does not implement interface '{typeof(IContractResolver)}'");
            }

            return SetContractResolver((IContractResolver)Activator.CreateInstance(contractResolverType)!);
        }

        public Json SetContractResolver(IContractResolver? contractResolver) {
            JsonSerializerSettings.ContractResolver = contractResolver;
            return this;
        }


        public JsonSerializerSettings GetJsonSerializerSettings()
        {
            return JsonSerializerSettings.Copy();
        }

        public Json() {

            JsonSerializerSettings = new JsonSerializerSettings {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Include,
                CheckAdditionalContent = false,
                ContractResolver = new DefaultContractResolver()
            };

            //RegisterJsonConverter<DecimalJsonConverter>();
            RegisterJsonConverter<DefaultDictionaryConverter>();
        }

        public Json(bool ignoreNullValues) {

            JsonSerializerSettings = new JsonSerializerSettings {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = ignoreNullValues ? NullValueHandling.Ignore : NullValueHandling.Include,
                CheckAdditionalContent = false,
                ContractResolver = new DefaultContractResolver()
            };

            //RegisterJsonConverter<DecimalJsonConverter>();
            RegisterJsonConverter<DefaultDictionaryConverter>();
        }

        public JToken ToJToken(object @object) {

            if (@object == null)
                return JValue.CreateNull();

            if (@object is JToken jt)
                return jt;

            return JToken.FromObject(@object, JsonSerializer);
        }
        public JToken ToJToken(string json) {

            return JToken.Parse(json);
        }


        public JObject ToJObject(object @object) {

            var jToken = ToJToken(@object);
            return ToJObject(jToken);
        }
        public JObject ToJObject(string json) {

            var jToken = ToJToken(json);
            return ToJObject(jToken);
        }
        private JObject ToJObject(JToken jToken) {

            if (jToken.Type != JTokenType.Object)
                throw new ArgumentException($"The object is of type '{jToken.Type}', and therefore can't be converted to a JObject");

            return (JObject)jToken;
        }


        public string ToJson(object @object, bool formatted = false) {

            var formating = Formatting.None;
            if (formatted)
                formating = Formatting.Indented;

            return ToJToken(@object).ToString(formating);
        }

        public string Beautify(string json) {
            return ToJToken(json).ToString(Formatting.Indented);
        }

        public string Minify(string json) {
            return ToJToken(json).ToString(Formatting.None);
        }



        public T? ToObject<T>(string json) {
            var jToken = ToJToken(json);
            return ToObject<T>(jToken);
        }

        public T? ToObject<T>(object @object) {
            var jToken = ToJToken(@object);
            return ToObject<T>(jToken);
        }

        public object? ToObject(string json, Type type) {
            var jToken = ToJToken(json);
            return ToObject(jToken, type);
        }

        public object? ToObject(object @object, Type type) {
            var jToken = ToJson(@object);
            return ToObject(jToken, type);
        }

        public object? ToObject(JToken jToken, Type type) {
            return jToken?.ToObject(type, JsonSerializer);
        }
        public T? ToObject<T>(JToken jToken) {
            return jToken != null ? jToken.ToObject<T>(JsonSerializer) : default(T);
        }



        public Dictionary<TKey, TValue>? ToDictionary<TKey, TValue>(object? data) where TKey : notnull {
            if (data == null)
                return null;

            var jToken = ToJToken(data);
            return ToDictionary<TKey, TValue>(jToken);

        }

        public Dictionary<TKey, TValue>? ToDictionary<TKey, TValue>(string? json) where TKey : notnull {
            if (json == null)
                return null;

            var jToken = ToJToken(json);
            return ToDictionary<TKey, TValue>(jToken);
        }


        private Dictionary<TKey, TValue>? ToDictionary<TKey, TValue>(JToken jToken) where TKey : notnull {

            if (jToken.Type != JTokenType.Object)
                throw new ArgumentException($"The object is of type '{jToken.Type}', and therefore can't be converted to a Dictionary");


            return JsonConvert.DeserializeObject<Dictionary<TKey, TValue>>(ToJson(jToken), JsonSerializerSettings);

        }

        public Dictionary<string, object>? ToDictionary(object? data) {
            if (data == null)
                return null;

            var jObject = ToJObject(data);
            return ToDictionary(jObject);

        }

        public Dictionary<string, object>? ToDictionary(string? json) {
            if (json == null)
                return null;

            var jObject = ToJObject(json);
            return ToDictionary(jObject);
        }

        public Dictionary<string, object>? ToDictionary(JObject? jObject) {
            if (jObject == null)
                return null;

            return ToDictionary<string, object>(jObject);
        }


        public Dictionary<string, string> Flatten(object @object) {
            return Flatten(ToJObject(@object));
        }

        public Dictionary<string, string> Flatten(JObject jsonObject) {
            return JsonFlattener().Flatten(jsonObject);
        }

        
        public JObject Unflatten(IDictionary<string, object> keyValues) {
            return JsonFlattener().Unflatten(keyValues);
        }

        public JObject Merge(JObject destination, JObject source) {

            destination.Merge(source);
            return destination;
        }

        public JObject MergePaths(JObject destination, IDictionary<string, object> keyValues) {

            var jObject = Unflatten(keyValues);
            return Merge(destination, jObject);
        }

        public JObject MergePath(JObject destination, string path, object value) {


            var dict = new Dictionary<string, object> {
                [path] = value
            };

            return MergePaths(destination, dict);
        }


        public object? ToBasicDotNetObject(JToken? jtoken) {
            if (jtoken == null)
                return null;

            switch (jtoken.Type) {
                case JTokenType.None:
                    return null;
                case JTokenType.Object:
                    var obj = jtoken as JObject;
                    return ToBasicDotNetDictionary(obj);
                case JTokenType.Array:
                    var arr = jtoken as JArray;
                    return ToBasicDotNetObjectEnumerable(arr);
                case JTokenType.Constructor:
                    return null;
                case JTokenType.Property:
                    return null;
                case JTokenType.Comment:
                    return null;
                case JTokenType.Integer:
                    return jtoken.ToObject<int>();
                case JTokenType.Float:
                    return jtoken.ToObject<float>();
                case JTokenType.String:
                    return jtoken.ToObject<string>();
                case JTokenType.Boolean:
                    return jtoken.ToObject<bool>();
                case JTokenType.Null:
                    return null;
                case JTokenType.Undefined:
                    return null;
                case JTokenType.Date:
                    return jtoken.ToObject<DateTime>();
                case JTokenType.Raw:
                    return null;
                case JTokenType.Bytes:
                    return jtoken.ToObject<Byte[]>();
                case JTokenType.Guid:
                    return jtoken.ToObject<Guid>();
                case JTokenType.Uri:
                    return jtoken.ToObject<Uri>();
                case JTokenType.TimeSpan:
                    return jtoken.ToObject<TimeSpan>();
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        public Dictionary<string, object?>? ToBasicDotNetDictionary(JObject? jObject) {
            if (jObject == null)
                return null;

            var dict = new Dictionary<string, object?>();

            foreach (var kvp in jObject) {
                dict.Add(kvp.Key,
                    kvp.Value?.Type == JTokenType.Object
                        ? ToBasicDotNetDictionary((JObject)kvp.Value)
                        : ToBasicDotNetObject(kvp.Value));
            }

            return dict;
        }

        public IEnumerable<object?>? ToBasicDotNetObjectEnumerable(JArray? jArray, bool ignoreErrors = false) {
            return jArray?.Select(ToBasicDotNetObject).ToList();
        }

        public IEnumerable<T?>? ToBasicDotNetObjectEnumerable<T>(JArray jArray, bool ignoreErrors = false) {
            return jArray?.Select(ToObject<T>).ToList();
        }

        public T? FromJsonStreamToObject<T>(Stream? stream) {
            if (stream == null || stream.CanRead == false)
                return default;

            using var sr = new StreamReader(stream);
            using var jtr = new JsonTextReader(sr);
            var searchResult = JsonSerializer.Deserialize<T>(jtr);
            return searchResult;
        }

        public void SerializeToStream(Stream stream, JToken jtoken) {
            using var sw = new StreamWriter(stream, new UTF8Encoding(false), 8192, true);
            JsonSerializer.Serialize(sw, jtoken);
        }


        private static readonly Lazy<Reflectensions.Json> lazyJson = new Lazy<Reflectensions.Json>(() => new Reflectensions.Json()
            .RegisterJsonConverter<StringEnumConverter>()
            .RegisterJsonConverter<DefaultDictionaryConverter>()
        );

        public static Json Converter => lazyJson.Value;


        private static bool? _newtonsoftJsonAvailable;
        public static bool NewtonsoftJsonAvailable => _newtonsoftJsonAvailable ??= FindType();

        private static bool FindType() {

            

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies) {
                var foundType = assembly.GetType("Newtonsoft.Json.JsonSerializer", false, true);
                if (foundType != null) {
                    return true;
                }
            }

            return false;
        }

    }

}


