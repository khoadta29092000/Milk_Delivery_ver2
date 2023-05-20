using BusinessObject.Models;

namespace DataAccess.Repository
{
    public interface IRepositoryStation
    {
        Task<List<TblStation>> GetStation();
        Task AddTblStation(TblStation m);
        Task DeleteTblStation(string m);
        Task UpdateTblStation(TblStation m);
        Task<List<TblStation>> SearchByName(string? search, bool? isDeleted, int page, int pageSize);
        Task<List<TblAccount>> GetAccountsByStation(string stationId, int page, int pageSize);
        Task<List<TblOrder>> GetOrdersByStation(string stationId, int page, int pageSize);
        Task<TblStation> GetStationById(string stationId);
        Task UpdateActive(string stationId, bool active);
    }
}
