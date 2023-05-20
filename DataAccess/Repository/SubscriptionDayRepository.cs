using BusinessObject.Models;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class SubscriptionDayRepository : IRepositorySubscriptionDay
    {
        public Task<List<TblSubscriptionDay>> GetSubscriptionDay() => SubscriptionDayDAO.GetSubscriptionDay();
        public Task AddTblSubscriptionDay(TblSubscriptionDay m) => SubscriptionDayDAO.AddTblSubscriptionDay(m);
        public Task DeleteTblSubscriptionDay(string m) => SubscriptionDayDAO.DeleteTblSubscriptionDay(m);
        public Task UpdateTblSubscriptionDay(TblSubscriptionDay m) => SubscriptionDayDAO.UpdateTblSubscriptionDay(m);
        public Task<TblSubscriptionDay> GetSubscriptionDayById(string SubscriptionDayId) => SubscriptionDayDAO.GetSubscriptionDayById(SubscriptionDayId);
        public Task<List<TblSubscriptionDay>> SearchByName(int page, int pageSize) => SubscriptionDayDAO.SearchByName(page, pageSize);
    }
}
