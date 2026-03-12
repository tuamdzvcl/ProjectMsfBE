using Dapper;
using EventTick.Model.Models;
using Microsoft.EntityFrameworkCore;
using projectDemo.Data;
using projectDemo.DTO.Response;
using projectDemo.Repository.Ipml;
using System.Data;

namespace projectDemo.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly EventTickDbContext _context;

        public AuthRepository(EventTickDbContext context)
        {
            _context = context;
        }


        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.User
           .Include(x => x.UserRoles)
               .ThenInclude(x => x.Role)
           .FirstOrDefaultAsync(x => x.Email == email);
        }
        

        public async Task<Guid> InsertAsync(User user)
        {
            _context.User.Add(user);
            return user.Id;
        }

        public async Task<List<PermissionResponse>> GetPermissionNameAsyncByUserId(Guid UserID)
        {
            using var connection = _context.Database.GetDbConnection();

            var result = await connection.QueryAsync<PermissionResponse>(
                "GetPermissonNameByid",
                new { UserID = UserID },
                commandType: CommandType.StoredProcedure);

            return result.ToList();
                
        }
        public async Task AddAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
