using IdentityService.BusinessLogic.Interfaces;
using IdentityService.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService authorizationService;
        private readonly ITokenService tokenService;

        public AuthenticationController(IAuthenticationService authorizationService, ITokenService tokenService)
        {
            this.authorizationService = authorizationService;
            this.tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            if (request is null)
            {
                return BadRequest();
            }

            var result = await authorizationService.AuthenticateUser(request.Username, request.Password);

            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
        {
            if (request is null)
            {
                return BadRequest();
            }

            var result = await authorizationService.CreateUser(request.Username, request.Password, request.Email);

            return Ok(result);
        }


        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            if (request is null)
            {
                return BadRequest();
            }

            var result = await authorizationService.RefreshToken(request.AccessToken, request.RefreshToken);

            return Ok(result);
        }

        [HttpGet("public-validation-key")]
        public IActionResult GetPublicKey()
        {
            var publicKey = tokenService.GetPublicKey();

            return Ok(publicKey);
        }

        [HttpGet("validate-token")]
        public IActionResult ValidateToken(string token)
        {
            var validationResult = tokenService.ValidateToken(token);

            return Ok(validationResult.ToString());
        }
    }
}