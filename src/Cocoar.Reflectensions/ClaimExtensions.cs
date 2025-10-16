using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Cocoar.Reflectensions
{
    public static class ClaimExtensions
    {

        public static IEnumerable<Claim> GetClaimsByType(this IEnumerable<Claim> claims, string type) {
            return claims.Where(c => c.Type.Equals(type, StringComparison.CurrentCultureIgnoreCase));
        }

        public static Claim? GetFirstClaimByType(this IEnumerable<Claim> claims, string type)
        {
            return claims.FirstOrDefault(c => c.Type.Equals(type, StringComparison.CurrentCultureIgnoreCase));
        }

        public static string? GetFirstClaimValueByType(this IEnumerable<Claim> claims, string type)
        {
            return GetFirstClaimByType(claims, type)?.Value;
        }

        public static IEnumerable<string> GetClaimValuesByType(this IEnumerable<Claim> claims, string type) {
            return GetClaimsByType(claims, type).Select(c => c.Value);
        }

        public static IEnumerable<Claim> RemoveClaimsByType(this IEnumerable<Claim> claims, string type)
        {
            return claims.Where(c => !c.Type.Equals(type, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
