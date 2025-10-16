using System.Collections;
using System.Collections.Generic;
using Cocoar.Reflectensions.ExtensionMethods;
using Cocoar.Reflectensions.Internal;

namespace Cocoar.Reflectensions {

    public class ExpandableObject : ExpandableBaseObject {
        public ExpandableObject() : base() {

        }

        public ExpandableObject(object? @object) : base(PreventIDictionary(@object)) {

            
            if (@object != null && @object.GetType().IsDictionaryType()) {
                foreach (var o in (IEnumerable)@object)
                {
                    if (o.GetType().IsGenericTypeOf(typeof(KeyValuePair<,>))) {
                        var key = o.Reflect().GetPropertyValue("Key")!.ToString()!;
                        var value = o.Reflect().GetPropertyValue("Value");
                        this[key] = value;
                    }
                    
                }
            } 
        }

        public ExpandableObject(IDictionary dictionary) {
            
            foreach (DictionaryEntry o in dictionary)
            {
                this[o.Key.ToString()!] = o.Value!;
            }

        }

        public ExpandableObject(IDictionary<string, object?> dictionary) {

            foreach (var keyValuePair in dictionary) {
                this[keyValuePair.Key] = keyValuePair.Value;
            }

        }

        public ExpandableObject(Dictionary<string, object?> dictionary) {

            foreach (var keyValuePair in dictionary) {
                this[keyValuePair.Key] = keyValuePair.Value;
            }

        }


        public ExpandableObject(ExpandableObject expandableObject) {

            foreach (var keyValuePair in expandableObject.AsDictionary()) {
                this[keyValuePair.Key] = keyValuePair.Value!;
            }

        }


        public static implicit operator ExpandableObject(Dictionary<string, object?> dictionary) {

            return new ExpandableObject(dictionary);

        }

        private static object? PreventIDictionary(object? value) {

            if (value == null)
                return null;

            if (value.GetType().IsDictionaryType()) {
                return null;
            }

            return value;
        }
    }

}
