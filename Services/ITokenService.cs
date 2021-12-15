using Microsoft.AspNetCore.Identity;

namespace training_api.Services
{
    public interface ITokenService
    {
        string GenerateToken(IdentityUser user);
    }
}
