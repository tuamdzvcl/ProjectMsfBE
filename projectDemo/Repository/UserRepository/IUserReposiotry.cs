using EventTick.Model.Models;
using projectDemo.DTO.Response;

namespace projectDemo.Repository.Ipml
{
    public interface IUserReposiotry
    {
         Task<string> GetRoleByUser(Guid Userid);
        Task<User> GetUserByid(Guid id);
        Task<(List<Event>, int status, string messager)> GetListEventByUserID(Guid userID);        
    }
}
