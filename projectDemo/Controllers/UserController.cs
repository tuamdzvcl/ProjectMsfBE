using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using projectDemo.Service.UserService;

namespace projectDemo.Controllers
{
    [Authorize]
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}/events")]
        public async Task<IActionResult> GetAllEvent(Guid id)
        {
            var result = await _userService.GetListEventByUserID(id);
            return Ok(result);
        }
    }
}
