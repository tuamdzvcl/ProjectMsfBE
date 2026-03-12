using projectDemo.DTO.Request;
using projectDemo.DTO.Respone;
using projectDemo.DTO.Response;
using projectDemo.DTO.UpdateRequest;

namespace projectDemo.Service.OrderService
{
    public interface IOrderService
    {
        Task<ApiResponse<OrderResponseCreate>> CreateOrder(CreateOrderRequest request, Guid userid);
        Task<ApiResponse<string>> UpdateOrder(Guid orderID,CreateOrderRequest request);
        Task<ApiResponse<string>> DeleteOrder(Guid OrderID);
        Task<ApiResponse<OrderResponse>> GetOrder();
        Task<PageResponse<Guid>> GetListOrderbyIdUser(Guid UserID, int pageindex, int pagesize);
        Task<ApiResponse<OrderResponse>> GetListOrderDetail(Guid OrderID);
    }
}
