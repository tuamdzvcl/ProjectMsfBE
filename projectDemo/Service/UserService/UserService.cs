using AutoMapper;
using projectDemo.DTO.Respone;
using projectDemo.DTO.Response;
using projectDemo.Repository.Ipml;
using projectDemo.Repository.TickTypeRepository;

namespace projectDemo.Service.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserReposiotry _userReposiotry;
        private readonly IMapper _mapper;

        public UserService(IUserReposiotry userReposiotry, IMapper mapper)
        {
            _userReposiotry = userReposiotry;
            _mapper = mapper;
        }
        // lấy tất cả user
        public async Task<ApiResponse<List<EventResponse>>> GetListEventByUserID(Guid guid)
        {
           var (entitys,status,messger) =await _userReposiotry.GetListEventByUserID(guid);
            if(status != 200)
            {
                return ApiResponse<List<EventResponse>>.FailResponse(Entity.Enum.EnumStatusCode.EVENTNOTFOUD, messger);

            }
            var response = _mapper.Map<List<EventResponse>>(entitys);
            return ApiResponse<List<EventResponse>>.SuccessResponse(Entity.Enum.EnumStatusCode.SUCCESS, response);

        }

    }
}
