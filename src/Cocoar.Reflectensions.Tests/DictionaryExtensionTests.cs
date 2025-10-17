using System.Collections.Generic;
using Cocoar.Reflectensions.ExtensionMethods;
using Cocoar.Reflectensions.Tests.TestClasses;
using Xunit;

namespace Cocoar.Reflectensions.Tests {
    public class DictionaryExtensionTests {
        [Fact]
        public void Merge_OverridesExistingKeysAndAddsNewKeys() {

            var exp1 = new Expandable1();
            exp1.Name = "Bernhard";
            exp1.Age = 99;
            exp1["Ok"] = true;

            var dict = new Dictionary<string, object>
            {
                ["Key1"] = "Value1",
                ["Key2"] = "Value2"
            };

            var dict2 = new Dictionary<string, object>
            {
                ["Key1"] = "NewValue1",
                ["Key3"] = "NewValue3"
            };

            var mergedDict = dict.Merge(dict2);

            Assert.Equal(3, mergedDict.Count);
            Assert.Equal("NewValue1", mergedDict["Key1"]);
            Assert.Equal("NewValue3", mergedDict["Key3"]);
            Assert.Equal("Value2", mergedDict["Key2"]);
        }

   
    }
}
