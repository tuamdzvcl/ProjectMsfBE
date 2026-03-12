
using projectDemo.Entity.Models;
using projectDemo.DTO.Respone;
using projectDemo.DTO.Response;
using projectDemo.Service.Auth;
using projectDemo.Repository.Ipml;
using AutoMapper;
using projectDemo.DTO.Request;
using EventTick.Model.Models;
using EventTick.Model.Enum;
using projectDemo.DTO.UpdateRequest;

namespace projectDemo.Service.EventService
{

    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IUserReposiotry _userReposiotry;
        private readonly IMapper _mapper;

        public EventService(IEventRepository eventRepository, IMapper mapper, IUserReposiotry userReposiotry)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
            _userReposiotry = userReposiotry;
        }
        //check date
        public bool checkVadidate(EventRequest request)
        {
            DateTime now = DateTime.Now;

            if (request.StartDate <= now)
            {
                return false;
            }
            if (request.EndDate <= request.StartDate)
                return false;
            if (request.SaleStartDate >= request.SaleEndDate)
                return false;
            if (request.SaleEndDate >= request.StartDate)
                return false;
            if (request.EndDate - request.StartDate > TimeSpan.FromDays(3))
                return false;
            return true;
        }
        //lấy userName
        public async Task<string> rederNameByUserID(Guid UserID)
        {
            return await _userReposiotry.GetRoleByUser(UserID);
        }
        //tạo event -> 
        public async Task<ApiResponse<EventResponse>> CreateEvent(EventRequest resquest,Guid Userid)
        {
            try
            {
                var check = checkVadidate(resquest);
                

                if (!check)
                {
                    return ApiResponse<EventResponse>.FailResponse(Entity.Enum.EnumStatusCode.DATE, "Kiêm tra lại ngày và giờ");
                }
                var userid = _userReposiotry.GetUserByid(Userid);
                if (userid == null)
                {
                    return ApiResponse<EventResponse>.FailResponse(Entity.Enum.EnumStatusCode.USERNOTFOUND, "Không tìm thấy user ");

                }
                Event events = new Event
                {
                    Id = Guid.NewGuid(),
                    UserID = Userid,
                    Title = resquest.Title,
                    Status = EnumStatusEvent.DRAFT,
                    PosterUrl = resquest.PosterUrl,
                    StartDate = resquest.StartDate,
                    EndDate = resquest.EndDate,
                    SaleStartDate = resquest.SaleStartDate,
                    SaleEndDate = resquest.SaleEndDate,
                    Description = resquest.Description,
                    Location = resquest.Location,
                    CreatedDate = DateTime.Now,

                };

                await _eventRepository.CreateEvent(events);
                await _eventRepository.SaveChangesAsync();

                EventResponse response = new EventResponse
                {
                    UserID = events.UserID,
                    Status = events.Status.ToString(),
                    PosterUrl = events.PosterUrl,
                    StartDate = events.StartDate,
                    EndDate = events.EndDate,
                    SaleStartDate = events.SaleStartDate,
                    SaleEndDate = events.SaleEndDate,
                    Description = events.Description,
                    Location = events.Location,
                    EventID = events.Id,
                    Title = events.Title
                };
                return ApiResponse<EventResponse>.SuccessResponse(Entity.Enum.EnumStatusCode.SUCCESS, response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"System Error: {ex.Message}");

                return ApiResponse<EventResponse>.FailResponse(Entity.Enum.EnumStatusCode.SERVER, "lỗi", ex.Message);
            }


        }
        // xóa mềm
        public async Task<ApiResponse<string>> DeleteEvent(Guid EventID)
        {
            try
            {
                Event events = await _eventRepository.GetEventById(EventID)
                 ?? throw new DllNotFoundException();
                events.Status = EnumStatusEvent.CANNEL;
                await _eventRepository.SaveChangesAsync();
                return ApiResponse<string>.SuccessResponse(Entity.Enum.EnumStatusCode.SUCCESS, "Xóa thành công");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"System Error: {ex.Message}");

                return ApiResponse<string>.FailResponse(Entity.Enum.EnumStatusCode.SERVER, "lỗi", ex.Message);
            }
        }
        // lấy tất cả các event
        public async Task<List<EventResponse>> GetEventAll()
        {
            List<Event> listEvents = await _eventRepository.GetAllEvent();

            var result = listEvents.Select(e => new EventResponse
            {
                EventID = e.Id,
                Description = e.Description,
                Location = e.Location,
                EndDate = e.EndDate,
                SaleStartDate = e.SaleStartDate,
                SaleEndDate = e.SaleEndDate,
                StartDate = e.StartDate,
                PosterUrl = e.PosterUrl,
                Status = e.Status.ToString(),
                Title = e.Title,
                UserID = e.UserID
            }).ToList();
            return result;
        }
        //lấy event theo id
        public async Task<ApiResponse<EventResponse>> GetEventById(Guid EventID)
        {
            try
            {
                Event events = await _eventRepository.GetEventById(EventID)
                 ?? throw new DllNotFoundException();

                EventResponse response = new EventResponse
                {
                    EventID = events.Id,
                    Description = events.Description,
                    Location = events.Location,
                    EndDate = events.EndDate,
                    SaleStartDate = events.SaleStartDate,
                    SaleEndDate = events.SaleEndDate,
                    StartDate = events.StartDate,
                    PosterUrl = events.PosterUrl,
                    Status = events.Status.ToString(),
                    Title = events.Title,
                    UserID = events.UserID
                };
                return ApiResponse<EventResponse>.SuccessResponse(Entity.Enum.EnumStatusCode.SUCCESS, response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"System Error: {ex.Message}");

                return ApiResponse<EventResponse>.FailResponse(Entity.Enum.EnumStatusCode.SERVER, "lỗi", ex.Message);
            }

        }
        // lấy tất cả các event phân trang
        public async Task<PageResponse<EventResponse>> GetListEventPage(int pageSize, int pageIndex, string keyWord)
        {
            try
            {
                if (pageIndex < 1)
                    pageIndex = 1;
                if (pageSize <= 0)
                    pageSize = 10;

                var response = await _eventRepository.GetPageEvent(pageIndex, pageSize, keyWord);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("lỗi ở đây này :", ex.Message);
                throw new Exception();
            }
        }
        //update event
        public async Task<ApiResponse<string>> UpdateEvent(Guid EventID, EventUpdateRequest resquest)
        {
            try
            {
                bool result = await _eventRepository.UpdateEvent(EventID, resquest);
                if (!result)
                    return ApiResponse<string>.FailResponse(Entity.Enum.EnumStatusCode.SERVER, "lỗi");
                return ApiResponse<string>.SuccessResponse(Entity.Enum.EnumStatusCode.SUCCESS, "Update Thành công");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"System Error: {ex.Message}");
                return ApiResponse<string>.FailResponse(Entity.Enum.EnumStatusCode.SERVER, "lỗi", ex.Message);
            }
        }
    }
}
