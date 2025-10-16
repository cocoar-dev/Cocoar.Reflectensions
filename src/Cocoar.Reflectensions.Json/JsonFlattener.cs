using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Cocoar.Reflectensions {
    internal class JsonFlattener {

        private readonly JsonSerializer _jsonSerializer;

        public JsonFlattener(JsonSerializer jsonSerializer) {
            _jsonSerializer = jsonSerializer;
        }

        public Dictionary<string, string> Flatten(JObject jsonObject) {
            var jTokens = jsonObject.Descendants().Where(p => !p.Any());
            var results = jTokens.Aggregate(new Dictionary<string, string>(), (properties, jToken) => {
                properties.Add(jToken.Path, jToken.ToString());
                return properties;
            });
            return results;
        }
        
        public JObject Unflatten(IDictionary<string, object> keyValues) {
            JContainer? result = null;
            var setting = new JsonMergeSettings {
                MergeArrayHandling = MergeArrayHandling.Merge
            };
            foreach (var pathValue in keyValues) {
                if (result == null) {
                    result = UnflatenSingle(pathValue!);
                } else {
                    result.Merge(UnflatenSingle(pathValue!)!, setting);
                }
            }
            return result as JObject ?? new JObject();
        }



        private JContainer? UnflatenSingle(KeyValuePair<string, object?> keyValue) {
            string path = keyValue.Key;
            JToken value = keyValue.Value == null ? JValue.CreateNull() : JToken.FromObject(keyValue.Value, _jsonSerializer);
            var pathSegments = SplitPath(path);

            JContainer? lastItem = null;
            //build from leaf to root
            foreach (var pathSegment in pathSegments.Reverse()) {
                var type = GetJsonType(pathSegment);
                switch (type) {
                    case JTokenType.Object:
                        var obj = new JObject();
                        if (null == lastItem) {
                            obj.Add(pathSegment, value);
                        } else {
                            obj.Add(pathSegment, lastItem);
                        }
                        lastItem = obj;
                        break;
                    case JTokenType.Array:
                        var array = new JArray();
                        int index = GetArrayIndex(pathSegment);
                        array = FillEmpty(array, index);
                        if (lastItem == null) {
                            array[index] = value;
                        } else {
                            array[index] = lastItem;
                        }
                        lastItem = array;
                        break;
                }
            }
            return lastItem;
        }

        private IList<string> SplitPath(string path) {
            IList<string> result = new List<string>();
            var reg = new Regex(@"(?!\.)([^. ^\[\]]+)|(?!\[)(\d+)(?=\])");
            foreach (Match match in reg.Matches(path)) {
                result.Add(match.Value);
            }
            return result;
        }

        private JArray FillEmpty(JArray array, int index) {
            for (var i = 0; i <= index; i++) {
                array.Add(JValue.CreateNull());
            }
            return array;
        }

        private JTokenType GetJsonType(string pathSegment) {
            return int.TryParse(pathSegment, out _) ? JTokenType.Array : JTokenType.Object;
        }

        private int GetArrayIndex(string pathSegment) {
            if (int.TryParse(pathSegment, out var result)) {
                return result;
            }
            throw new Exception("Unable to parse array index: " + pathSegment);
        }
    }
}
