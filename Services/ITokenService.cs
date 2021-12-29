using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace training_api.Services
{
    public interface ITokenService
    {
        Task<(string, string)> GenerateTokensAsync(IdentityUser user);
    }
}
