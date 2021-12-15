using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using training_api.Contacts.Requests;
using training_api.Contacts.Responses;
using training_api.Services;

namespace training_api.Controllers
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
                Token = authResponse.Token
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
                Token = authResponse.Token
            });
        }
    }
}
