using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using projectDemo.DTO.Request;
using projectDemo.Service.OrderService;

namespace projectDemo.Controllers
{
    [Authorize]
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Tạo Order
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var userId = Guid.Parse(User.FindFirst("id").Value);

            var result = await _orderService.CreateOrder(request, userId);
            return Ok(result);
        }

        /// <summary>
        /// Lấy Order theo ID
        /// </summary>
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder(Guid orderId)
        {
            var result = await _orderService.GetOrder(orderId);
            return Ok(result);
        }

        /// <summary>
        /// Lấy danh sách Order theo User
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetListOrderByUser(
            [FromQuery] int pageIndex ,
            [FromQuery] int pageSize )
        {
            var userId = Guid.Parse(User.FindFirst("id").Value);
            var result = await _orderService.GetListOrderbyIdUser(userId,pageIndex,pageSize);
            return Ok(result);
        }

        /// <summary>
        /// Lấy chi tiết Order
        /// </summary>
        [HttpGet("{orderId}/detail")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOrderDetail(Guid orderId)
        {
            var result = await _orderService.GetListOrderDetail(orderId);
            return Ok(result);
        }

        /// <summary>
        /// Update Order
        /// </summary>
        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateOrder(Guid orderId, [FromBody] CreateOrderRequest request)
        {
            var result = await _orderService.UpdateOrder(orderId, request);
            return Ok(result);
        }

        /// <summary>
        /// Delete Order
        /// </summary>
        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder(Guid orderId)
        {
            var result = await _orderService.DeleteOrder(orderId);
            return Ok(result);
        }
    }

}
