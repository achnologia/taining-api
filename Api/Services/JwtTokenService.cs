using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Api.Data;
using Api.Domain;
using Api.Options;

namespace Api.Services
{
    public class JwtTokenService : ITokenService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly DataContext _dataContext;

        public JwtTokenService(JwtOptions jwtOptions, DataContext dataContext)
        {
            _jwtOptions = jwtOptions;
            _dataContext = dataContext;
        }

        public async Task<(string, string)> GenerateTokensAsync(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("idUser", user.Id)
                }),
                Expires = DateTime.UtcNow.Add(_jwtOptions.TokenLifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken
            {
                IdJwt = token.Id,
                IdUser = user.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6)
            };

            await _dataContext.RefreshTokens.AddAsync(refreshToken);
            await _dataContext.SaveChangesAsync();

            var result = (tokenHandler.WriteToken(token), refreshToken.Token);

            return result;
        }
    }
}
