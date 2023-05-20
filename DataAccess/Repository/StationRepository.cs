using BusinessObject.Models;
using DataAccess.DAO;


namespace DataAccess.Repository
{
    public class StationRepository : IRepositoryStation
    {
        public Task<List<TblStation>> GetStation() => StationDAO.GetStation();
        public Task AddTblStation(TblStation m) => StationDAO.AddTblStation(m);
        public Task DeleteTblStation(string m) => StationDAO.DeleteTblStation(m);
        public Task UpdateTblStation(TblStation m) => StationDAO.UpdateTblStation(m);
        public Task<List<TblStation>> SearchByName(string? search, bool? isDeleted, int page, int pageSize) => StationDAO.SearchByName(search, isDeleted, page, pageSize);
        public Task<List<TblAccount>> GetAccountsByStation(string stationId, int page, int pageSize) => StationDAO.GetAccountsByStation(stationId, page, pageSize);
        public Task<List<TblOrder>> GetOrdersByStation(string stationId, int page, int pageSize) => StationDAO.GetOrdersByStation(stationId, page, pageSize);
        public Task<TblStation> GetStationById(string stationId) => StationDAO.GetStationById(stationId);
        public Task UpdateActive(string stationId, bool active) => StationDAO.UpdateActive(stationId, active);
    }
}
