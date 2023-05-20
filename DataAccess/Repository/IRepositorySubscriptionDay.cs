using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IRepositorySubscriptionDay
    {
        Task<List<TblSubscriptionDay>> GetSubscriptionDay();
        Task AddTblSubscriptionDay(TblSubscriptionDay m);
        Task DeleteTblSubscriptionDay(string m);
        Task UpdateTblSubscriptionDay(TblSubscriptionDay m);
        Task<List<TblSubscriptionDay>> SearchByName( int page, int pageSize);
        Task<TblSubscriptionDay> GetSubscriptionDayById(string SubscriptionDayId);
    }
}
