using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Cocoar.Reflectensions.Common;
using Newtonsoft.Json.Converters;
using Xunit;
using Xunit.Abstractions;

namespace Cocoar.Reflectensions.Tests
{
    public class EnumTests
    {

        private readonly ITestOutputHelper _output;

        public EnumTests(ITestOutputHelper output) {
            this._output = output;
        }


        [Theory]
        [InlineData(NoFlags.Eins)]
        [InlineData(NoFlags.Zwei)]
        [InlineData(NoFlags.Drei)]
        [InlineData(NoFlags.Eins | NoFlags.Zwei)]
        [InlineData(NoFlags.Zwei | NoFlags.Eins | NoFlags.Drei)]
        [InlineData(WithFlags.One)]
        [InlineData(WithFlags.Two)]
        [InlineData(WithFlags.Three)]
        [InlineData(WithFlags.One | WithFlags.Two)]
        [InlineData(WithFlags.Two | WithFlags.One | WithFlags.Three)]
        [InlineData(Simplest.First)]
        [InlineData(Simplest.Second)]
        [InlineData(Simplest.Third)]
        [InlineData(Simplest.First | Simplest.Second | Simplest.Zero)]
        [InlineData(Simplest.Second | Simplest.First | Simplest.Third)]

        public void GetEnumNames(Enum value) {


            var names = value.GetName();

            var json = new Json();
            json.RegisterJsonConverter<StringEnumConverter>();

            var js = json.ToJson(value);


            _output.WriteLine($"GetName() - '{names}'");

            EnumExtensions.TryFind(value.GetType(), names, out var ens);
            _output.WriteLine($"Parsed - {((Enum)ens).ToString("F")}");
        }

        [Fact]
        public void ParseEnumNames() {

            var find = "";

           
            var tryf = EnumExtensions.TryFind(typeof(Simplest), find, out var ens);
            _output.WriteLine($"TryFind - {tryf}, {((Enum)ens)?.ToString("F")}");
        }
    }

 
    public enum Simplest {

        Zero = -1,
        First,
        Second,
        Third
    }

    public enum NoFlags {
        
        Eins = 1,
        Zwei = 2,
        Drei = 4
    }

    [Flags]
    public enum WithFlags {
        //[Description("__One__")]
        //[EnumMember(Value = "_One")]
        One = 1,

        [Description("__Two__")]
        //[EnumMember(Value = "_Two")]
        Two = 2,

        [Description("__Four__")]
        [EnumMember(Value = "_Four")]
        Three = 4
    }

    public enum ResponseFormat {
        [EnumMember(Value = "*/*")]
        Unknown,

        [EnumMember(Value = "application/json")]
        Json,

        [EnumMember(Value = "application/xml")]
        Xml,

        [EnumMember(Value = "text/plain")]
        Text,

    }
}
