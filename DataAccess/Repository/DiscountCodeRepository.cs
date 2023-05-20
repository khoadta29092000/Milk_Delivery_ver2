using BusinessObject.Models;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class DiscountCodeRepository : IRepositoryDiscountCode
    {
        public Task<List<TblDiscountCode>> GetDiscountCode() => DiscountCodeDAO.GetDiscountCode();
        public Task AddTblDiscountCode(TblDiscountCode m) => DiscountCodeDAO.AddTblDiscountCode(m);
        public Task AddTblDiscountCodeOfAccount(TblDiscountOfAccount m) => DiscountCodeDAO.AddTblDiscountCodeOfAccount(m);
        public Task DeleteTblDiscountCode(string m) => DiscountCodeDAO.DeleteTblDiscountCode(m);
        public Task UpdateTblDiscountCode(TblDiscountCode m) => DiscountCodeDAO.UpdateTblDiscountCode(m);
        public Task<List<TblDiscountCode>> SearchByName(int minPoint, int maxPoint, bool? isDeleted, int page, int pageSize) => DiscountCodeDAO.SearchByName(minPoint, maxPoint, isDeleted, page, pageSize);
        public Task<List<TblOrder>> GetOrdersByDiscountCode(string DiscountCodeId, int page, int pageSize) => DiscountCodeDAO.GetOrdersByDiscountCode(DiscountCodeId, page, pageSize);
        public Task<TblDiscountCode> GetDiscountCodeById(string DiscountCodeId) => DiscountCodeDAO.GetDiscountCodeById(DiscountCodeId);
        public Task UpdateActive(string DiscountCodeId, bool active) => DiscountCodeDAO.UpdateActive(DiscountCodeId, active);
        public Task<List<TblDiscountOfAccount>> GetDiscountCodeOfAccount(string? AccountID, string? DiscountCodeId, int page, int pageSize) => DiscountCodeDAO.GetDiscountCodeOfAccount(AccountID,DiscountCodeId, page, pageSize);
        public Task<List<TblAccount>> GetAllAccountBuyDiscountCode(string DiscountCodeId, int page, int pageSize) => DiscountCodeDAO.GetAllAccountBuyDiscountCode(DiscountCodeId, page, pageSize);
        public Task<List<TblDiscountCode>> GetAllDiscountCodeBuyAccount(string AccountID, int page, int pageSize) => DiscountCodeDAO.GetAllDiscountCodeBuyAccount(AccountID, page, pageSize);
    }
}
