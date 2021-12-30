using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Api.Contacts.Requests;
using Api.Contacts.Responses;
using Api.Services;

namespace Api.Controllers
{
    [Route("api/identity")]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Registration([FromBody] UserRegistrationRequest request)
        {
            var authResponse = await _identityService.RegisterAsync(request.Email, request.Password);

            if(!authResponse.Success)
            {
                return BadRequest(new AuthenticationFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }

            return Ok(new AuthenticationSuccessResponse
            {
                Token = authResponse.Token,
                RefreshToken = authResponse.RefreshToken
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var authResponse = await _identityService.LoginAsync(request.Email, request.Password);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthenticationFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }

            return Ok(new AuthenticationSuccessResponse
            {
                Token = authResponse.Token,
                RefreshToken = authResponse.RefreshToken
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            var authResponse = await _identityService.RefreshTokenAsync(request.Token, request.RefreshToken);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthenticationFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }

            return Ok(new AuthenticationSuccessResponse
            {
                Token = authResponse.Token,
                RefreshToken = authResponse.RefreshToken
            });
        }
    }
}
