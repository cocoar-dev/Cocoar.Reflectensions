//using System.Collections.Generic;
//using System.Threading.Tasks;
//using doob.Reflectensions.Common;
//using doob.Reflectensions.ExtensionMethods;
//using doob.Reflectensions.Helper;
//using Newtonsoft.Json;
//using Xunit;
//using Xunit.Abstractions;

//namespace doob.Reflectensions.Tests
//{
//    public class ConvertStringTests
//    {

//        private readonly ITestOutputHelper _output;

//        public ConvertStringTests(ITestOutputHelper output) {
//            this._output = output;

            
//            var js = JsonConvert.SerializeObject("");
//            var z = js.Length;
//        }


//        [Theory]
//        [InlineData("")]
//        [InlineData(@"\/Date(1198908717056)\/")]
//        [InlineData("2012-03-19T07:22Z")]
//        [InlineData("915148798.75")]
//        [InlineData("December 17, 1995 03:24:00")]

//        public void ConvertNullableToDateTime(string value)
//        {

//            var jsonAvailable = JsonHelpers.IsAvailable();

//            Assert.True(jsonAvailable);
//            var dt = value.ToNullableDateTime();

//            _output.WriteLine(dt.ToString());
//        }

//        [Theory]
//        [InlineData(@"\/Date(1198908717056)\/")]
//        [InlineData("2012-03-19T07:22Z")]
//        [InlineData("December 17, 1995 03:24:00")]

//        public void ConvertToDateTime(string value) {

//            var dt = value.ToDateTime();

//            _output.WriteLine(dt.ToString());
//        }

//        [Fact]
//        public async Task ConvertAsTaskOfTest() {



//            IEnumerable<string> ienum = new List<string>() { "eins", "zwei", "drei" };

//            List<string> t = await Task.Run(() => ienum).CastToTaskOf<List<string>>();

//            _output.WriteLine(new Json().ToJson(t, true));
//        }


//    }
//}
