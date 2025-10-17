using Cocoar.Reflectensions.Helper;
using Xunit;

namespace Cocoar.Reflectensions.Tests.HelperTests
{
    public class WildcardHelperTests
    {
        [Theory]
        [InlineData("test.txt", "*.txt", true)]
        [InlineData("test.doc", "*.txt", false)]
        [InlineData("readme.md", "read*.md", true)]
        [InlineData("README.MD", "read*.md", true)] // Case insensitive by default
        [InlineData("file123.txt", "file???.txt", true)]
        [InlineData("file12.txt", "file???.txt", false)]
        [InlineData("test", "test", true)]
        [InlineData("test", "TEST", true)] // Case insensitive
        public void Match_WithVariousPatterns_MatchesCorrectly(string input, string pattern, bool expected)
        {
            var result = WildcardHelper.Match(input, pattern, ignoreCase: true);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Match_CaseSensitive_RespectsCase()
        {
            Assert.True(WildcardHelper.Match("Test", "Test", ignoreCase: false));
            Assert.False(WildcardHelper.Match("TEST", "Test", ignoreCase: false));
        }

        [Fact]
        public void Match_WithInvert_InvertsResult()
        {
            Assert.False(WildcardHelper.Match("test.txt", "*.txt", ignoreCase: true, invert: true));
            Assert.True(WildcardHelper.Match("test.doc", "*.txt", ignoreCase: true, invert: true));
        }

        [Fact]
        public void Match_NullInput_TreatsAsEmpty()
        {
            Assert.True(WildcardHelper.Match(null!, "*"));
            Assert.False(WildcardHelper.Match(null!, "test"));
        }

        [Theory]
        [InlineData("file+.txt", "file+.txt", true)]
        [InlineData("file.txt", "file+.txt", false)]
        [InlineData("fileABC.txt", "file+.txt", true)]
        public void Match_WithPlusWildcard_MatchesOneOrMore(string input, string pattern, bool expected)
        {
            var result = WildcardHelper.Match(input, pattern);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void ContainsWildcard_WithAsterisk_ReturnsTrue()
        {
            Assert.True(WildcardHelper.ContainsWildcard("test*"));
        }

        [Fact]
        public void ContainsWildcard_WithQuestionMark_ReturnsTrue()
        {
            Assert.True(WildcardHelper.ContainsWildcard("test?"));
        }

        [Fact]
        public void ContainsWildcard_WithPlus_ReturnsTrue()
        {
            Assert.True(WildcardHelper.ContainsWildcard("test+"));
        }

        [Fact]
        public void ContainsWildcard_WithoutWildcard_ReturnsFalse()
        {
            Assert.False(WildcardHelper.ContainsWildcard("test"));
        }

        [Fact]
        public void WildcardToRegex_ConvertsAsterisk()
        {
            var result = WildcardHelper.WildcardToRegex("*.txt");

            Assert.Contains(".*", result);
            Assert.StartsWith("^", result);
            Assert.EndsWith("$", result);
        }

        [Fact]
        public void WildcardToRegex_ConvertsQuestionMark()
        {
            var result = WildcardHelper.WildcardToRegex("file?.txt");

            Assert.Contains(".", result);
        }

        [Fact]
        public void WildcardToRegex_EscapesSpecialCharacters()
        {
            var result = WildcardHelper.WildcardToRegex("file(1).txt");

            // Should escape parentheses
            Assert.Contains("\\(", result);
            Assert.Contains("\\)", result);
        }
    }
}
