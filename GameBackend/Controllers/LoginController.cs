using GameBackend.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameBackend.Controllers
{
    [ApiController]
    [Route("login")]
    public class LoginController : ControllerBase
    {
        private readonly IGameAuthenticationService _authenticationService;
        private readonly ITokenService _tokenService;

        public LoginController(IGameAuthenticationService authenticationService, ITokenService tokenService)
        {
            _authenticationService = authenticationService;
            _tokenService = tokenService;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(string username, string password)
        {
            var user = _authenticationService.Authenticate(username, password);

            if (user is null)
            {
                return BadRequest("Invalid credentials");
            }

            return Ok(_tokenService.GetToken(user));
        }
    }
}
