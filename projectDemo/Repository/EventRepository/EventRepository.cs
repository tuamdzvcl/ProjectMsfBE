using Dapper;
using EventTick.Model.Enum;
using EventTick.Model.Models;
using Microsoft.EntityFrameworkCore;
using projectDemo.Data;
using projectDemo.DTO.Respone;
using projectDemo.DTO.Response;
using projectDemo.DTO.UpdateRequest;
using projectDemo.Repository.Ipml;
using System.Data;

namespace projectDemo.Repository
{
    public class EventRepository : IEventRepository

    {
        private readonly EventTickDbContext _context;

        public EventRepository(EventTickDbContext context)
        {
            _context = context;
        }

        public async Task CreateEvent(Event entity)
        {
            await _context.AddAsync(entity);
        }

        public void DeleteEvent(Event entity)
        {
            _context.Event.Remove(entity);
        }

        public async Task<List<Event>> GetAllEvent()
        {
            return await _context.Event
                .Where(e => e.Status != EnumStatusEvent.CANNEL && e.IsDeleted==false)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<Event> GetEventById(Guid eventId)
        {
            
            try
            {
                return await _context.Event
                .FirstOrDefaultAsync(e => e.Id == eventId && e.Status != EnumStatusEvent.CANNEL)
                ?? new Event();
                 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Event();
            }
        }

        public async Task<PageResponse<EventResponse>> GetPageEvent(int pageIndex, int pageSize, string key)
        {

            using var connection = _context.Database.GetDbConnection();
            var param = new DynamicParameters();
            param.Add("@PageIndex", pageIndex);
            param.Add("@PageSize", pageSize);
            param.Add("@key", key);
            param.Add("@totalRow", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var result = await connection.QueryAsync<EventResponse>(
                "GetEventPaging",
                param,
                commandType: CommandType.StoredProcedure);
            int totalRow = param.Get<int>("@totalRow");
            return new PageResponse<EventResponse>
            {
                Items = result.ToList(),
                TotalRecords = totalRow,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }

        public async Task<bool> UpdateEvent(Guid EventID, EventUpdateRequest request)
        {
           
            try
            {

                using var connection = _context.Database.GetDbConnection();
                connection.Open();
                var param = new DynamicParameters();

                param.Add("@Id", EventID);
                param.Add("@Title", request.Title);
                param.Add("@Description", request.Description);
                param.Add("@Location", request.Location);
                param.Add("@StartDate", request.StartDate);
                param.Add("@EndDate", request.EndDate);
                param.Add("@SaleStartDate", request.SaleStartDate);
                param.Add("@SaleEndDate", request.SaleEndDate);
                param.Add("@PosterUrl", request.PosterUrl);
                param.Add("@Status", request.Status);
                param.Add("@RowsAffected", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await connection.ExecuteAsync(
                   "UpdateEvent",
                   param,
                   commandType: CommandType.StoredProcedure
               );
                var rows = param.Get<Int32>("@RowsAffected");
                connection.Close();
                return rows > 0;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"lỗi{ ex.ToString()}");
                return false;
            }
        }
        public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
    }
}
