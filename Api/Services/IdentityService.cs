using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Api.Data;
using Api.Domain;
using Api.Extensions;

namespace Api.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly DataContext _dataContext;

        public IdentityService(UserManager<IdentityUser> userManager, ITokenService tokenService, TokenValidationParameters tokenValidationParameters, DataContext dataContext)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _tokenValidationParameters = tokenValidationParameters;
            _dataContext = dataContext;
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

            return await GetAuthenticationResultSuccessAsync(newUser);
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

            return await GetAuthenticationResultSuccessAsync(user);
        }

        private async Task<AuthenticationResult> GetAuthenticationResultSuccessAsync(IdentityUser user)
        {
            var (token, refreshToken) = await _tokenService.GenerateTokensAsync(user);

            return new AuthenticationResult
            {
                Success = true,
                Token = token,
                RefreshToken = refreshToken
            };
        }

        public async Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken)
        {
            var validatedToken = GetClaimsPrincipalFromToken(token);

            if(validatedToken is null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "Ivalid Token" }
                };
            }

            var expiryDateUnix = long.Parse(validatedToken.Claims.GetClaimValueByName(JwtRegisteredClaimNames.Exp));
            var expiryDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(expiryDateUnix);

            if(expiryDateTime > DateTime.UtcNow)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "This Token has not expiered yet" }
                };
            }

            var jti = validatedToken.Claims.GetClaimValueByName(JwtRegisteredClaimNames.Jti);
            var storedRefreshToken = await _dataContext.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshToken);

            if(!IsTokenValid(storedRefreshToken, jti))
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "Ivalid Token" }
                };
            }

            storedRefreshToken.Used = true;
            _dataContext.RefreshTokens.Update(storedRefreshToken);
            await _dataContext.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(validatedToken.Claims.GetClaimValueByName("idUser"));
            return await GetAuthenticationResultSuccessAsync(user);
        }

        private ClaimsPrincipal GetClaimsPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);

                if(!IsTokenWithValidSecurityAlg(validatedToken))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }

        private bool IsTokenWithValidSecurityAlg(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwt) &&
                jwt.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
        }

        private bool IsTokenValid(RefreshToken storedRefreshToken, string jti)
        {
            return !(storedRefreshToken is null || DateTime.UtcNow > storedRefreshToken.ExpiryDate || storedRefreshToken.Invalidated || storedRefreshToken.Used || storedRefreshToken.IdJwt != jti);
        }
    }
}
