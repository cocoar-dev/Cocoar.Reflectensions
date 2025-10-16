using System;
using System.Collections.Generic;
using System.Linq;

namespace Cocoar.Reflectensions.Internal {
    public abstract partial class ExpandableBaseObject {

        public Dictionary<string, object?> AsDictionary(bool omitNullValues = false) {
            var props = GetProperties();
            if (omitNullValues) {
                props = props.Where(p => p.Value != null);
            }
            return props.ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        public T To<T>() where T : ExpandableObject {

            var exp = Activator.CreateInstance<T>();
            foreach (var keyValuePair in AsDictionary()) {
                exp[keyValuePair.Key] = keyValuePair.Value!;
            }

            return exp;
        }

    }
}
