using Dapper;
using EventTick.Model.Models;
using Microsoft.EntityFrameworkCore;
using projectDemo.Data;
using projectDemo.DTO.Response;
using projectDemo.Repository.BaseData;
using projectDemo.Repository.Ipml;
using projectDemo.UnitOfWorks;
using System.Data;

namespace projectDemo.Repository
{
    public class UserRepository :  RepositoryLinqBase<User>,IUserReposiotry
    {
        private readonly RepositoryProcBase _proc;

        public UserRepository(IUnitOfWork uow): base(uow)
        {
            _proc = new RepositoryProcBase(uow);
        }

        public async Task<(List<Event>,int status,string messager)> GetListEventByUserID(Guid userID)
        {
            try 
            {
                var param = new DynamicParameters();
                param.Add("@userid", userID);
                param.Add("@status", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@messges", dbType: DbType.String, size: 250, direction: ParameterDirection.Output);


                var listevent = await _uow.connection.QueryAsync<Event>(
                    "GetListEventByUserID",
                    param,
                    commandType:CommandType.StoredProcedure
                    );

                    var status = param.Get<int>("@status");
                     var messager = param.Get<string>("@messges");
                
                    return (listevent.ToList(), status, messager);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return (new List<Event>(), 404, ex.Message);
            }
            
        }

        public async Task<string> GetRoleByUser(Guid Userid)
        {
            try
            {
                //gọi connection
                var param = new DynamicParameters();
                param.Add("@UserID", Userid);
                // gọi lệnh quey
                var result = await _uow.connection.QueryFirstOrDefaultAsync<string>(
                    "GetRoleNameByUserID",
                    param,
                    commandType: System.Data.CommandType.StoredProcedure
                    );
                return result ?? throw new DllNotFoundException();
            }
            catch( Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return ( ex.Message);
            }
        }

        public async Task<User?> GetUserByid(Guid id)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
