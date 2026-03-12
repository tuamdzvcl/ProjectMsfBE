using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using projectDemo.DTO.Request;
using projectDemo.DTO.UpdateRequest;
using projectDemo.Service.Auth;
using projectDemo.Service.EventService;
using projectDemo.Service.TicketTypeService;

namespace projectDemo.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/event")]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly ITypeTicketService _typeTicketService;
        public EventController(IEventService eventService,ITypeTicketService ticketService)
        {
            _eventService = eventService;
            _typeTicketService = ticketService;
        }

        [HttpPost()]
        public async Task<IActionResult> CreateEvent([FromBody] EventRequest resquest)
        {
            var userId = Guid.Parse(User.FindFirst("id").Value);

            var result = await _eventService.CreateEvent(resquest, userId);

            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(Guid id, [FromBody] EventUpdateRequest resquest)
        {
            var result = await _eventService.UpdateEvent(id, resquest);
            return Ok(result);
        }
        [HttpGet()]
        public async Task<IActionResult> GetAllEvent()
        {
            var result = await _eventService.GetEventAll();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventbyId(Guid id)
        {
            var result = await _eventService.GetEventById(id);
            return Ok(result);
        }
        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteId(Guid id)
        {
            var result = await _eventService.DeleteEvent(id);
            return Ok(result);
        }

        [HttpGet("page")]
        public async Task<IActionResult> GetPageEvent(
            [FromQuery] int pageIndex,
            [FromQuery] int pageSize,
            [FromQuery] string? key)
        {
            var result = await _eventService.GetListEventPage(pageIndex, pageSize, key);
            return Ok(result);
        }
        [HttpGet("typetick/{id}")]
        public async Task<IActionResult> GetAllEvent(Guid id)
        {
            var result = await _typeTicketService.GetListTypeTickByEventID(id);
            return Ok(result);
        }
    }
}

    
