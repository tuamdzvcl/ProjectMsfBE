using projectDemo.DTO.Request;
using projectDemo.DTO.Respone;
using projectDemo.DTO.Response;
using projectDemo.DTO.UpdateRequest;

namespace projectDemo.Service.EventService
{
    public interface IEventService
    {
        Task<List<EventResponse>> GetEventAll();

        Task<ApiResponse<EventResponse>> GetEventById(Guid EventID);
        Task<ApiResponse<string>> UpdateEvent(Guid EventID, EventUpdateRequest resquest);
        Task<ApiResponse<string>> DeleteEvent(Guid EventID);
        Task<ApiResponse<EventResponse>> CreateEvent(EventRequest resquest,Guid Userid);
        bool checkVadidate(EventRequest resquest);
        Task<PageResponse<EventResponse>> GetListEventPage(int pageSize, int pageIndex, string keyWord);
    }
}
