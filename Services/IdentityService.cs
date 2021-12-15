using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using training_api.Domain;

namespace training_api.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenService _tokenService;

        public IdentityService(UserManager<IdentityUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<AuthenticationResult> RegisterAsync(string email, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if(existingUser is not null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User with this email already exists" }
                };
            }

            var newUser = new IdentityUser
            {
                Email = email,
                UserName = email
            };

            var createdUser = await _userManager.CreateAsync(newUser, password);

            if(!createdUser.Succeeded)
            {
                return new AuthenticationResult
                {
                    Errors = createdUser.Errors.Select(x => x.Description)
                };
            }

            return GetAuthenticationResultSuccess(newUser);
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User does not exist" }
                };
            }

            var validPasswordProvided = await _userManager.CheckPasswordAsync(user, password);

            if(!validPasswordProvided)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User/password mismatch" }
                };
            }

            return GetAuthenticationResultSuccess(user);
        }

        private AuthenticationResult GetAuthenticationResultSuccess(IdentityUser user)
        {
            var token = _tokenService.GenerateToken(user);

            return new AuthenticationResult
            {
                Success = true,
                Token = token
            };
        }
    }
}
