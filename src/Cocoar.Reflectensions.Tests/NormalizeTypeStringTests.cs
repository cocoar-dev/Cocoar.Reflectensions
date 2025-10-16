using System.Collections.Generic;
using Cocoar.Reflectensions.Helper;
using Xunit;

namespace Cocoar.Reflectensions.Tests
{
    public class NormalizeTypeStringTests
    {
        [Theory]
        [InlineData("string", "System.String")]
        [InlineData("int", "System.Int32")]
        [InlineData("bool", "System.Boolean")]
        [InlineData("dynamic", "System.Object")]
        [InlineData("System.String", "System.String")]
        [InlineData("System.Int32", "System.Int32")]
        [InlineData("System.Boolean", "System.Boolean")]
        [InlineData("System.Object", "System.Object")]
        public void NormalizeSimpleString(string typeString, string expectedType)
        {


            var type = TypeHelper.NormalizeTypeName(typeString);
            Assert.Equal(expectedType, type);

        }

        [Theory]
        [InlineData("double", "System.Double")]
        [InlineData("number", "System.Double")]
        [InlineData("System.Collections.Generic.Dictionary<System.Collections.Generic.Dictionary<string, System.Collections.Generic.List`1[number]>, System.Collections.Generic.IReadOnlyCollection`1[Guid[]]>", "System.Collections.Generic.Dictionary`2[System.Collections.Generic.Dictionary`2[System.String, System.Collections.Generic.List`1[System.Double]], System.Collections.Generic.IReadOnlyCollection`1[System.Guid[]]]")]
        public void NormalizeSimpleStringWithCustomMapping(string typeString, string expectedType)
        {

            var cMap = new Dictionary<string, string>
            {
                ["number"] = "double"
            };

            var type = TypeHelper.NormalizeTypeName(typeString, cMap);
            Assert.Equal(expectedType, type);

        }

        [Theory]
        [InlineData("string[]", "System.String[]")]
        [InlineData("int[][]", "System.Int32[][]")]
        [InlineData("bool[]", "System.Boolean[]")]
        [InlineData("dynamic[][][]", "System.Object[][][]")]
        [InlineData("System.String[]", "System.String[]")]
        [InlineData("System.Int32[][]", "System.Int32[][]")]
        [InlineData("System.Boolean[]", "System.Boolean[]")]
        [InlineData("System.Object[][][]", "System.Object[][][]")]
        [InlineData("System.Collections.Generic.Dictionary<string, double>[]", "System.Collections.Generic.Dictionary`2[System.String, System.Double][]")]
        public void NormalizeArrayString(string typeString, string expectedType)
        {


            var type = TypeHelper.NormalizeTypeName(typeString);
            Assert.Equal(expectedType, type);

        }


        [Theory]
        [InlineData("System.Collections.Generic.Dictionary`2[System.String, System.Object]", "System.Collections.Generic.Dictionary`2[System.String, System.Object]")]
        [InlineData("System.Collections.Generic.Dictionary`2[string, object]", "System.Collections.Generic.Dictionary`2[System.String, System.Object]")]
        [InlineData("System.Collections.Generic.Dictionary`2[System.Collections.Generic.List<string>, System.Collections.Generic.IReadOnlyCollection`1[Guid[]]]", "System.Collections.Generic.Dictionary`2[System.Collections.Generic.List`1[System.String], System.Collections.Generic.IReadOnlyCollection`1[System.Guid[]]]")]
        [InlineData("System.Collections.Generic.Dictionary<string, double>", "System.Collections.Generic.Dictionary`2[System.String, System.Double]")]
        public void NormalizeGenericStringType(string typeString, string expectedType)
        {


            var type = TypeHelper.NormalizeTypeName(typeString);
            Assert.Equal(expectedType, type);

        }
    }
}
