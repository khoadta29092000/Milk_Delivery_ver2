using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IRepositoryDiscountCode
    {
        Task<List<TblDiscountCode>> GetDiscountCode();
        Task AddTblDiscountCode(TblDiscountCode m);
        Task AddTblDiscountCodeOfAccount(TblDiscountOfAccount m);
        Task DeleteTblDiscountCode(string m);
        Task UpdateTblDiscountCode(TblDiscountCode m);
        Task<List<TblDiscountCode>> SearchByName(int minPoint, int maxPoint, bool? isDeleted, int page, int pageSize);
        Task<List<TblOrder>> GetOrdersByDiscountCode(string DiscountCodeId, int page, int pageSize);
        Task<TblDiscountCode> GetDiscountCodeById(string DiscountCodeId);
        Task UpdateActive(string DiscountCodeId, bool active);
        Task<List<TblDiscountOfAccount>> GetDiscountCodeOfAccount(string? AccountID, string? DiscountCodeId, int page, int pageSize);
        Task<List<TblAccount>> GetAllAccountBuyDiscountCode(string DiscountCodeId, int page, int pageSize);
        Task<List<TblDiscountCode>> GetAllDiscountCodeBuyAccount(string AccountID, int page, int pageSize);

    }
}
