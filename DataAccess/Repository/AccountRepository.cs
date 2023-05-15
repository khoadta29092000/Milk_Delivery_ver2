using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using DataAccess.DAO;

namespace DataAccess.Repository
{
        public class AccountRepository : IRepositoryAccount
    {
        public  Task<List<TblAccount>> GetMembers() => AccountDAO.GetMembers();
        public  Task<TblAccount> LoginMember(string email, string password) => AccountDAO.Instance.Login(email, password);
        public  Task<TblAccount> GetProfile(string AccountID) => AccountDAO.Instance.GetProfile(AccountID);
        public  Task DeleteMember(string m) => AccountDAO.DeleteTblAccount(m);
        public  Task ChangePassword(string TblAccountID, string password) => AccountDAO.Instance.ChangePassword(TblAccountID, password);
        public  Task UpdateActive(string TblAccountID, bool active) => AccountDAO.Instance.UpdateActive(TblAccountID, active);
        public Task VerificationAccount(string TblAccountID, bool active) => AccountDAO.Instance.VerificationAccount(TblAccountID, active);

        public Task AddMember(TblAccount m) => AccountDAO.AddTblAccount(m);
        public  Task UpdateMember(TblAccount m) => AccountDAO.UpdateTblAccount(m);
        public  Task<List<TblAccount>> SearchByEmail(string? search,int roleId , int page, int pageSize) => AccountDAO.Instance.SearchByEmail(search, roleId, page, pageSize);
       
    }
}
