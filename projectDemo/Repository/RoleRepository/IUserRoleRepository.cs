using EventTick.Model.Models;

namespace projectDemo.Repository.Ipml
{
    public interface IUserRoleRepository
    {
        Task InsertAsync(UserRole userRole);

    }
}
