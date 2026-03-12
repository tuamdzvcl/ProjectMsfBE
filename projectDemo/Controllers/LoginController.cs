using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using projectDemo.DTO.Request;
using projectDemo.DTO.Respone;
using projectDemo.DTO.Response;
using projectDemo.Service.Auth;
using System.Net;

namespace projectDemo.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.AuthenCase(request);

            return Ok(result);
        }
        [HttpPost("regiter")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest resquest)
        {
            var result = await _authService.Regiter(resquest);

            return Ok(result);
        }

    }
}
