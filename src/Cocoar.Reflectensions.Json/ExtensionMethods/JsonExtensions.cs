using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Cocoar.Reflectensions.ExtensionMethods {
    public static class JsonExtensions {

        public static IEnumerable<object?>? ToBasicDotNetObjectEnumerable(this JArray jArray, bool ignoreErrors = false) {
            return jArray?.Select(jToken => jToken.ToBasicDotNetObject()).ToList();
        }

        public static object? ToBasicDotNetObject(this JToken? jtoken) {
            if (jtoken == null)
                return null;

            switch (jtoken.Type) {
                case JTokenType.None:
                    return null;
                case JTokenType.Object:
                    var obj = jtoken as JObject;
                    return obj?.ToBasicDotNetDictionary();
                case JTokenType.Array:
                    var arr = jtoken as JArray;
                    return arr?.ToBasicDotNetObjectEnumerable();
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

        public static Dictionary<string, object?>? ToBasicDotNetDictionary(this JObject? jObject) {
            if (jObject == null)
                return null;

            var dict = new Dictionary<string, object?>();

            foreach (var kvp in jObject) {
                if (kvp.Value!.Type == JTokenType.Object) {
                    dict.Add(kvp.Key, ((JObject)kvp.Value).ToBasicDotNetDictionary());
                } else {
                    dict.Add(kvp.Key, kvp.Value.ToBasicDotNetObject());
                }
            }

            return dict;
        }

        #region ExpandableObject
        //public static IEnumerable<object> ToExpandableObjectEnumerable(this JArray jArray, bool ignoreErrors = false) {
        //    return jArray?.Select(jToken => jToken.ToExpandableObjectProperty()).ToArray();
        //}

        //public static object ToExpandableObjectProperty(this JToken jtoken) {
        //    if (jtoken == null)
        //        return null;

        //    switch (jtoken.Type) {
        //        case JTokenType.None:
        //            return null;
        //        case JTokenType.Object:
        //            var obj = jtoken as JObject;
        //            return obj.ToExpandableObject();
        //        case JTokenType.Array:
        //            var arr = jtoken as JArray;
        //            return arr.ToExpandableObjectEnumerable();
        //        case JTokenType.Constructor:
        //            return null;
        //        case JTokenType.Property:
        //            return null;
        //        case JTokenType.Comment:
        //            return null;
        //        case JTokenType.Integer:
        //            return jtoken.ToObject<int>();
        //        case JTokenType.Float: {
                        
        //                var dec = jtoken.ToObject<float>().ToString();
        //                if (dec.IsInt()) {
        //                    return dec.ToInt();
        //                }

        //                if (dec.IsLong()) {
        //                    return dec.ToLong();
        //                }

        //                if (dec.IsDouble()) {
        //                    return dec.ToDouble();
        //                }

        //                if (decimal.TryParse(dec, out var deci)) {
        //                    return deci;
        //                }

        //                return null;
        //            }

        //        case JTokenType.String:
        //            return jtoken.ToObject<string>();
        //        case JTokenType.Boolean:
        //            return jtoken.ToObject<bool>();
        //        case JTokenType.Null:
        //            return null;
        //        case JTokenType.Undefined:
        //            return null;
        //        case JTokenType.Date:
        //            return jtoken.ToObject<DateTime>();
        //        case JTokenType.Raw:
        //            return null;
        //        case JTokenType.Bytes:
        //            return jtoken.ToObject<Byte[]>();
        //        case JTokenType.Guid:
        //            return jtoken.ToObject<Guid>();
        //        case JTokenType.Uri:
        //            return jtoken.ToObject<Uri>();
        //        case JTokenType.TimeSpan:
        //            return jtoken.ToObject<TimeSpan>();
        //        default:
        //            throw new ArgumentOutOfRangeException();
        //    }

        //}

        //public static ExpandableObject ToExpandableObject(this JObject jObject) {
        //    if (jObject == null)
        //        return null;

        //    var dict = new ExpandableObject();

        //    foreach (var kvp in jObject) {
        //        if (kvp.Value.Type == JTokenType.Object) {
        //            dict.Add(kvp.Key, ((JObject)kvp.Value).ToExpandableObject());
        //        } else {
        //            dict.Add(kvp.Key, kvp.Value.ToExpandableObjectProperty());
        //        }
        //    }

        //    return dict;
        //}

        #endregion

    }
}
