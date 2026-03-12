using EventTick.Model.Models;

namespace projectDemo.Repository.Ipml
{
    public interface IUserLoginRepository
    {
        Task InsertAsync(UserLogin userLogin);

    }
}
