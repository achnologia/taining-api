using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace training_api.Extensions
{
    public static class ClaimExtensions
    {
        public static string GetClaimValueByName(this IEnumerable<Claim> claims, string type)
        {
            return claims.Single(claim => claim.Type == type).Value;
        }
    }
}
