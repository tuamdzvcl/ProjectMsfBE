using projectDemo.DTO.Respone;
using projectDemo.DTO.Response;

namespace projectDemo.Service.UserService
{
    public interface IUserService
    {
        Task<ApiResponse<List<EventResponse>>> GetListEventByUserID(Guid guid);
    }
}
