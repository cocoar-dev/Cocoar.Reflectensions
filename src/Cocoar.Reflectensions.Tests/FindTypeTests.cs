using System;
using System.Collections.Generic;
using Cocoar.Reflectensions.Helper;
using Cocoar.Reflectensions.Tests.TestClasses;
using Xunit;

namespace Cocoar.Reflectensions.Tests {
    public class FindTypeTests {


        [Theory]
        [InlineData("string", typeof(string))]
        [InlineData("int", typeof(int))]
        [InlineData("bool", typeof(Boolean))]
        [InlineData("dynamic", typeof(object))]
        public void SimpleStringToType(string typeString, Type expectedType) {

            var type = TypeHelper.FindType(typeString);
            Assert.Equal(expectedType, type);

        }


        [Theory]
        [InlineData("string[]", typeof(string[]))]
        [InlineData("int[][]", typeof(int[][]))]
        [InlineData("bool[]", typeof(Boolean[]))]
        [InlineData("dynamic[][][]", typeof(object[][][]))]
        public void ArrayStringToType(string typeString, Type expectedType) {

            var type = TypeHelper.FindType(typeString);
            Assert.Equal(expectedType, type);

        }


        [Theory]
        [InlineData("System.Collections.Generic.Dictionary`2[System.String, System.Object]", typeof(Dictionary<string, object>))]
        [InlineData("System.Collections.Generic.Dictionary`2[string, number]", typeof(Dictionary<string, double>))]
        [InlineData("System.Collections.Generic.Dictionary`2[System.Collections.Generic.List<string>, System.Collections.Generic.IReadOnlyCollection`1[Guid[]]]", typeof(Dictionary<List<string>, IReadOnlyCollection<Guid[]>>))]
        [InlineData("System.Collections.Generic.Dictionary<System.Collections.Generic.Dictionary<string, System.Collections.Generic.List`1[number]>, System.Collections.Generic.IReadOnlyCollection`1[Guid[]]>", typeof(Dictionary<Dictionary<string, List<double>>, IReadOnlyCollection<Guid[]>>))]
        [InlineData("System.Collections.Generic.Dictionary<string, double>", typeof(Dictionary<string, double>))]
        [InlineData("System.Collections.Generic.Dictionary$2<string, System.Collections.Generic.List$1<string>>", typeof(Dictionary<string, List<string>>))]
        [InlineData("Cocoar.Reflectensions.Tests.TestClasses.CreateObjectTestClass<number>", typeof(CreateObjectTestClass<double>))]
#pragma warning disable xUnit1025 // InlineData should be unique within the Theory it belongs to
        [InlineData("Cocoar.Reflectensions.Tests.TestClasses.CreateObjectTestClass<number>", typeof(CreateObjectTestClass<System.Double>))]
#pragma warning restore xUnit1025 // InlineData should be unique within the Theory it belongs to
        public void GenericStringToType(string typeString, Type expectedType) {

            var cMap = new Dictionary<string, string> {
                ["number"] = "double"
            };
            
            var type = TypeHelper.FindType(typeString, cMap);
            Assert.Equal(expectedType, type);

        }

    }
}
