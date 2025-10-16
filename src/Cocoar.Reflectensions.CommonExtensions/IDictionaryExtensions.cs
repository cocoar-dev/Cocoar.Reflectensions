using System.Collections.Generic;
using Cocoar.Reflectensions.ExtensionMethods;

namespace Cocoar.Reflectensions.Common {
    public static class IDictionaryExtensions {

        public static T? GetValueAs<T>(this IDictionary<string, object> dictionary, string key, T? orDefault = default) {
            if(TryGetValueAs<T>(dictionary, key, out var val))
                return val;

            return orDefault;
        }

        public static bool TryGetValueAs<T>(this IDictionary<string, object> dictionary, string key, out T? value) {

            if (!dictionary.ContainsKey(key)) {
                value = default;
                return false;
            }

            if (dictionary[key].Reflect().TryAs<T>(out var converted)) {
                value = converted;
                return true;
            }

            value = default;
            return false;
        }

        public static T? GetValueTo<T>(this IDictionary<string, object> dictionary, string key, T? orDefault = default) {
            if (TryGetValueTo<T>(dictionary, key, out var val))
                return val;

            return orDefault;
        }

        public static bool TryGetValueTo<T>(this IDictionary<string, object> dictionary, string key, out T? value) {

            if (!dictionary.ContainsKey(key)) {
                value = default;
                return false;
            }

            if (dictionary[key].Reflect().TryTo<T>(out var converted)) {
                value = converted;
                return true;
            }

            value = default;
            return false;
        }
        
       
    }
}
