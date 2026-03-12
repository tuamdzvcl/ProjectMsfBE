using EventTick.Model.Enum;
using EventTick.Model.Models;
using projectDemo.Data;
using projectDemo.Repository.Ipml;

namespace projectDemo.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly EventTickDbContext _context;

        public RoleRepository(EventTickDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetOrCreateAsync(Role roleName)
        {
          await _context.Role.AddAsync(roleName);
            return roleName.Id;
        }


        
    }
}
