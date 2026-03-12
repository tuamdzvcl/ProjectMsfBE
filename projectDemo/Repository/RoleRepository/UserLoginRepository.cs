using EventTick.Model.Models;
using projectDemo.Data;
using projectDemo.Repository.Ipml;

namespace projectDemo.Repository
{
    public class UserLoginRepository : IUserLoginRepository
    {
        private readonly EventTickDbContext _context;

        public UserLoginRepository(EventTickDbContext context)
        {
            _context = context;
        }

        public async Task InsertAsync(UserLogin userLogin)
        {
           await _context.AddAsync(userLogin);
        }
        
    }
}
