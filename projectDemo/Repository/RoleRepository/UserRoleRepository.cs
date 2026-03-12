using EventTick.Model.Models;
using projectDemo.Data;
using projectDemo.Repository.Ipml;

namespace projectDemo.Repository
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly EventTickDbContext _context;

        public UserRoleRepository(EventTickDbContext context)
        {
            _context = context;
        }

        public async Task InsertAsync(UserRole userRole)
        {
            await _context.AddAsync(userRole);
            
        }
    }
}
