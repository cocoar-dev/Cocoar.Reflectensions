using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Xunit;

namespace Cocoar.Reflectensions.Tests
{
    public class ClaimExtensionsTests
    {
        private List<Claim> CreateTestClaims()
        {
            return new List<Claim>
            {
                new Claim("name", "John Doe"),
                new Claim("email", "john@example.com"),
                new Claim("role", "Admin"),
                new Claim("role", "User"),
                new Claim("department", "IT")
            };
        }

        [Fact]
        public void GetClaimsByType_FindsMatchingClaims()
        {
            var claims = CreateTestClaims();

            var result = claims.GetClaimsByType("role").ToList();

            Assert.Equal(2, result.Count);
            Assert.All(result, c => Assert.Equal("role", c.Type, ignoreCase: true));
        }

        [Fact]
        public void GetClaimsByType_CaseInsensitive_FindsClaims()
        {
            var claims = CreateTestClaims();

            var result = claims.GetClaimsByType("ROLE").ToList();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetClaimsByType_NoMatches_ReturnsEmpty()
        {
            var claims = CreateTestClaims();

            var result = claims.GetClaimsByType("nonexistent").ToList();

            Assert.Empty(result);
        }

        [Fact]
        public void GetFirstClaimByType_FindsFirstMatchingClaim()
        {
            var claims = CreateTestClaims();

            var result = claims.GetFirstClaimByType("role");

            Assert.NotNull(result);
            Assert.Equal("role", result!.Type, ignoreCase: true);
            Assert.Equal("Admin", result.Value);
        }

        [Fact]
        public void GetFirstClaimByType_CaseInsensitive_FindsClaim()
        {
            var claims = CreateTestClaims();

            var result = claims.GetFirstClaimByType("NAME");

            Assert.NotNull(result);
            Assert.Equal("John Doe", result!.Value);
        }

        [Fact]
        public void GetFirstClaimByType_NoMatch_ReturnsNull()
        {
            var claims = CreateTestClaims();

            var result = claims.GetFirstClaimByType("nonexistent");

            Assert.Null(result);
        }

        [Fact]
        public void GetFirstClaimValueByType_FindsValue()
        {
            var claims = CreateTestClaims();

            var result = claims.GetFirstClaimValueByType("name");

            Assert.Equal("John Doe", result);
        }

        [Fact]
        public void GetFirstClaimValueByType_CaseInsensitive_FindsValue()
        {
            var claims = CreateTestClaims();

            var result = claims.GetFirstClaimValueByType("EMAIL");

            Assert.Equal("john@example.com", result);
        }

        [Fact]
        public void GetFirstClaimValueByType_NoMatch_ReturnsNull()
        {
            var claims = CreateTestClaims();

            var result = claims.GetFirstClaimValueByType("nonexistent");

            Assert.Null(result);
        }

        [Fact]
        public void GetClaimValuesByType_FindsAllValues()
        {
            var claims = CreateTestClaims();

            var result = claims.GetClaimValuesByType("role").ToList();

            Assert.Equal(2, result.Count);
            Assert.Contains("Admin", result);
            Assert.Contains("User", result);
        }

        [Fact]
        public void GetClaimValuesByType_CaseInsensitive_FindsValues()
        {
            var claims = CreateTestClaims();

            var result = claims.GetClaimValuesByType("ROLE").ToList();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetClaimValuesByType_NoMatches_ReturnsEmpty()
        {
            var claims = CreateTestClaims();

            var result = claims.GetClaimValuesByType("nonexistent").ToList();

            Assert.Empty(result);
        }

        [Fact]
        public void RemoveClaimsByType_RemovesMatchingClaims()
        {
            var claims = CreateTestClaims();

            var result = claims.RemoveClaimsByType("role").ToList();

            Assert.Equal(3, result.Count);
            Assert.All(result, c => Assert.False(c.Type.Equals("role", System.StringComparison.OrdinalIgnoreCase)));
        }

        [Fact]
        public void RemoveClaimsByType_CaseInsensitive_RemovesClaims()
        {
            var claims = CreateTestClaims();

            var result = claims.RemoveClaimsByType("NAME").ToList();

            Assert.Equal(4, result.Count);
            Assert.DoesNotContain(result, c => c.Type.Equals("name", System.StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void RemoveClaimsByType_NoMatches_ReturnsAll()
        {
            var claims = CreateTestClaims();

            var result = claims.RemoveClaimsByType("nonexistent").ToList();

            Assert.Equal(5, result.Count);
        }

        [Fact]
        public void RemoveClaimsByType_RemovesAll_ReturnsEmpty()
        {
            var claims = new List<Claim>
            {
                new Claim("role", "Admin"),
                new Claim("role", "User")
            };

            var result = claims.RemoveClaimsByType("role").ToList();

            Assert.Empty(result);
        }
    }
}
