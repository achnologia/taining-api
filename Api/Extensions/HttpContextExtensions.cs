using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Api.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetIdUser(this HttpContext httpContext)
        {
            if (httpContext.User is null)
                return string.Empty;

            var claim = httpContext.User.Claims.Single(x => x.Type == "id");

            return claim.Value;
        }
    }
}
