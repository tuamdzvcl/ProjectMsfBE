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

        [HttpGet("events")]
        public async Task<IActionResult> GetAllEvent()
        {
            var userId = Guid.Parse(User.FindFirst("id").Value);

            var result = await _userService.GetListEventByUserID(userId);
            return Ok(result);
        }
        [HttpGet()]
        public async Task<IActionResult> GetallUser(
            [FromQuery] int pageIndex,
            [FromQuery] int pageSize,
            [FromQuery] string? key)
            
        {
            var result = await _userService.GetAll(pageIndex,pageSize,key);
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userService.Delete(id);
            return Ok(result);
        }
    }
}
