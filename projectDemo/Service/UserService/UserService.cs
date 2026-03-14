using AutoMapper;
using EventTick.Model.Enum;
using EventTick.Model.Models;
using projectDemo.DTO.Request;
using projectDemo.DTO.Respone;
using projectDemo.DTO.Response;
using projectDemo.DTO.UpdateRequest;
using projectDemo.Repository;
using projectDemo.Repository.Ipml;
using projectDemo.Repository.PemisstionRepository;
using projectDemo.Repository.RolePermissionRepository;
using projectDemo.Repository.TickTypeRepository;
using projectDemo.UnitOfWorks;
using System.Net.WebSockets;

namespace projectDemo.Service.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserReposiotry _userReposiotry;
        private readonly IMapper _mapper;
        private readonly IUserRoleRepository _roleUserRepo;
        private readonly IRoleRepository _roleRepository;
        private readonly IPemisstionRepository _petRepository;
        private readonly IUserLoginRepository _userLoginRepository;
        private readonly IRolePermissionRepository _rolePermission;
        private readonly IUnitOfWork _uow;
        
        public UserService(IUserRoleRepository userRole, IUnitOfWork uow, IUserReposiotry userReposiotry, IMapper mapper, IRoleRepository roleRepository, IPemisstionRepository petRepository, IUserLoginRepository userLoginRepository, IRolePermissionRepository rolePermission)
        {
            _userReposiotry = userReposiotry;
            _mapper = mapper;
            _roleRepository = roleRepository;
            _petRepository = petRepository;
            _userLoginRepository = userLoginRepository;
            _rolePermission = rolePermission;
            _uow = uow;
            _roleUserRepo = userRole;
        }


        // lấy tất cả các e event do user tạo
        public async Task<ApiResponse<List<EventResponse>>> GetListEventByUserID(Guid guid)
        {
            try
            {
                var (entitys, status, messger) = await _userReposiotry.GetListEventByUserID(guid);
                if (status != 200)
                {
                    return ApiResponse<List<EventResponse>>.FailResponse(Entity.Enum.EnumStatusCode.EVENTNOTFOUD, messger);

                }
                var response = _mapper.Map<List<EventResponse>>(entitys);
                return ApiResponse<List<EventResponse>>.SuccessResponse(Entity.Enum.EnumStatusCode.SUCCESS, response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return ApiResponse<List<EventResponse>>.FailResponse(Entity.Enum.EnumStatusCode.SUCCESS,"thích làm test lỗi không");

            }

        }

         async Task<ApiResponse<UserResponse>> IUserService.Create(UserRequest request,Guid userid)
        {
            await _uow.BeginTransactionAsync();
            try
            {
                var user = await _userReposiotry.GetUserByid(userid);

                   
               

                if (user == null)
                {
                    return ApiResponse<UserResponse>.FailResponse(Entity.Enum.EnumStatusCode.USERNOTFOUND, "không tìm thấy user");

                }
                var passwordhash = BCrypt.Net.BCrypt.HashPassword("123456");

                var entity = new User
                {
                    Id = Guid.NewGuid(),
                    Username = request.UserName,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    AvatarUrl = request.AvataUrl,
                    IsActive = true,
                    IsLock = false,
                    CreatedDate = DateTime.UtcNow,
                    DateLock = null,
                    Isfalse = 0,
                    PasswordHash = passwordhash,
                    CreatedBy = user.Username
                };



                 await _userReposiotry.Create(entity);
                var Listrole = new List<int>();
                foreach (var item in request.RoleName)
                {
                    var role = await _roleRepository.GetRole(item.ToUpper());
                    if (role == null)
                    {
                        continue;
                    }
                    Listrole.Add(role.Id);
                }
                var userRoles = Listrole
                .Distinct()
                .Select(x => new UserRole
                {
                     UserId = entity.Id,
                     RoleId = x
                })
                .ToList();
                await _roleUserRepo.InserList(userRoles);
                
                await _uow.SaveChangesAsync();
                await _uow.CommitAsync();

                var response = new UserResponse
                {
                    ID = entity.Id,
                    Username = entity.Username,
                    Email= entity.Email,
                    FirstName= entity.FirstName,
                    LastName= entity.LastName,
                    RoleName = request.RoleName
                    
                };

                return ApiResponse<UserResponse>.SuccessResponse(Entity.Enum.EnumStatusCode.SUCCESS, response);

            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync();
                return ApiResponse<UserResponse>.FailResponse(Entity.Enum.EnumStatusCode.SERVER, ex.Message);
               
            }

        }
        //xóa
         async Task<ApiResponse<string>> IUserService.Delete(Guid id)
        {
           var user = await _userReposiotry.GetUserByid(id);
            user.IsDeleted = true;
            return PageResponse<string>.SuccessResponse(Entity.Enum.EnumStatusCode.SUCCESS,"xóa thành công");
        }
        //get all
        async Task<PageResponse<UserResponse>> IUserService.GetAll(int pageindex, int pagesize, string key)
        {
            var (users, total) = await _userReposiotry.GetAll(pageindex, pagesize, key);

            var response = users.Select(x => new UserResponse
            {
                ID = x.Id,
                Username = x.Username,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email
            }).ToList();

            var page = new PageResponse<UserResponse>
            {
                Items = response,
                PageIndex = pageindex,
                PageSize = pagesize,
                Message="list user"
            };

            return page;
        }

        

        Task<ApiResponse<UserResponse>> IUserService.Update(Guid id, UserUpdateRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
