using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Cocoar.Reflectensions.Internal {
    public abstract partial class ExpandableBaseObject: IDictionary<string, object?> {
        public IEnumerator<KeyValuePair<string, object?>> GetEnumerator() {
            return GetProperties().GetEnumerator()!;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<string, object?> item) {
            this[item.Key] = item.Value;
        }

        public void Clear() {
            throw new NotImplementedException();
        }
        
        public void CopyTo(KeyValuePair<string, object?>[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<string, object?> item) {
            if (IsInstanceProperty(item.Key)) {
                throw new Exception($"'{item.Key}' is an instance property, therefore it can't be removed!");
            }

            return __properties.Remove(item.Key);
        }

        public int Count => GetProperties(true).Count();

        public bool IsReadOnly { get; } = false;

        public void Add(string key, object? value) {
            this[key] = value;
        }

       

        public bool Remove(string key) {
            if (IsInstanceProperty(key)) {
                throw new Exception($"'{key}' is an instance property, therefore it can't be removed!");
            }

            return __properties.Remove(key);
        }

        public bool TryGetValue(string key, out object value) {
            
            return TryGetProperty(_instance, key, out value!);
        }

        public ICollection<string> Keys => GetKeys().ToList();
        public ICollection<object?> Values => GetValues().ToList()!;
    }
}
