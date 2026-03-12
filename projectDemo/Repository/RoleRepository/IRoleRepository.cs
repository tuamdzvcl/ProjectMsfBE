using EventTick.Model.Enum;
using EventTick.Model.Models;

namespace projectDemo.Repository.Ipml;

public interface IRoleRepository
{
    Task<int> GetOrCreateAsync(Role roleName);
    
}
