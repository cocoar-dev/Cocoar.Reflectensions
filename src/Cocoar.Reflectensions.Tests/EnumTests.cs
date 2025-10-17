using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Cocoar.Reflectensions;
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
        public void GetEnumName_ReturnsCorrectNameAndCanBeParsedBack(Enum value) {
            var names = value.GetName();
            _output.WriteLine($"GetName() - '{names}'");

            var success = EnumExtensions.TryFind(value.GetType(), names, out var parsedValue);
            _output.WriteLine($"Parsed - {(parsedValue as Enum)?.ToString("F") ?? "null"}");
            
            Assert.True(success);
            Assert.Equal(value, parsedValue);
        }

        [Fact]
        public void TryFindEnum_ReturnsFalse_ForEmptyString() {
            var emptyString = "";
            
            var success = EnumExtensions.TryFind(typeof(Simplest), emptyString, out var result);
            
            _output.WriteLine($"TryFind - {success}, {(result as Enum)?.ToString("F") ?? "null"}");
            Assert.False(success);
        }

        [Fact]
        public void TryFindEnum_WithEnumMemberAttribute_FindsCorrectValue() {
            var success = EnumExtensions.TryFind(typeof(ResponseFormat), "application/json", out var result);
            
            Assert.True(success);
            Assert.Equal(ResponseFormat.Json, result);
        }

        [Fact]
        public void TryFindEnum_WithDescriptionAttribute_FindsCorrectValue() {
            var success = EnumExtensions.TryFind(typeof(WithFlags), "__Two__", out var result);
            
            Assert.True(success);
            Assert.Equal(WithFlags.Two, result);
        }

        [Fact]
        public void TryFindEnum_WithEnumMemberTakesPrecedence_OverDescription() {
            var success = EnumExtensions.TryFind(typeof(WithFlags), "_Four", out var result);
            
            Assert.True(success);
            Assert.Equal(WithFlags.Three, result);
        }

        [Fact]
        public void TryFindEnum_CaseInsensitive_FindsValue() {
            var success = EnumExtensions.TryFind(typeof(Simplest), "first", true, out var result);
            
            Assert.True(success);
            Assert.Equal(Simplest.First, result);
        }

        [Fact]
        public void TryFindEnum_WithCommaSeparated_FindsMultipleValues() {
            var success = EnumExtensions.TryFind(typeof(WithFlags), "One, Two", out var result);
            
            Assert.True(success);
            Assert.Equal(WithFlags.One | WithFlags.Two, result);
        }

        [Fact]
        public void TryFindEnum_InvalidValue_ReturnsFalse() {
            var success = EnumExtensions.TryFind(typeof(Simplest), "InvalidValue", out var result);
            
            Assert.False(success);
        }

        [Fact]
        public void GetName_WithEnumMemberAttribute_ReturnsAttributeValue() {
            var name = ResponseFormat.Json.GetName();
            
            Assert.Equal("application/json", name);
        }

        [Fact]
        public void GetName_WithDescriptionAttribute_ReturnsDescription() {
            var name = WithFlags.Two.GetName();
            
            Assert.Equal("__Two__", name);
        }

        [Fact]
        public void GetName_WithEnumMemberTakesPrecedence_ReturnsEnumMemberValue() {
            var name = WithFlags.Three.GetName();
            
            Assert.Equal("_Four", name);
        }

        [Fact]
        public void GetName_WithMultipleFlags_ReturnsCommaSeparatedNames() {
            var name = (WithFlags.One | WithFlags.Two).GetName();
            
            Assert.Contains("One", name);
            Assert.Contains("__Two__", name);
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
        One = 1,

        [Description("__Two__")]
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
