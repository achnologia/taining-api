using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Api.Services
{
    public interface ITokenService
    {
        Task<(string, string)> GenerateTokensAsync(IdentityUser user);
    }
}
