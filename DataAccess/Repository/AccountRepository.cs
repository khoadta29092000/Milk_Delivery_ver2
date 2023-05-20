using BusinessObject.Models;
using DataAccess.DAO;
using DataAccess.DTO.Account;

namespace DataAccess.Repository
{
        public class AccountRepository : IRepositoryAccount
    {
        public  Task<List<TblAccount>> GetMembers() => AccountDAO.GetMembers();
        public  Task<TblAccount> LoginMember(string email, string password) => AccountDAO.Login(email, password);
        public  Task<TblAccount> GetProfile(string AccountID) => AccountDAO.GetProfile(AccountID);
        public  Task DeleteMember(string m) => AccountDAO.DeleteTblAccount(m);
        public  Task ChangePassword(string TblAccountID, string password) => AccountDAO.ChangePassword(TblAccountID, password);
        public  Task UpdateActive(string TblAccountID, bool active) => AccountDAO.UpdateActive(TblAccountID, active);
        public Task VerificationAccount(string TblAccountID, bool active) => AccountDAO.VerificationAccount(TblAccountID, active);
        public Task AddMember(TblAccount m) => AccountDAO.AddTblAccount(m);
        public Task AddMemberPoint(TblMemberPoint m) => AccountDAO.addTblAccountPoint(m);
        public  Task UpdateMember(TblAccount m) => AccountDAO.UpdateTblAccount(m);
        public  Task<List<GetAllAccountDTO>> SearchByEmail(string? search,int roleId , int page, int pageSize) => AccountDAO.SearchByEmail(search, roleId, page, pageSize);
        public Task<List<TblDiscountCode>> GetDiscountCodeByAccount(string TblAccountID, int page, int pageSize) => AccountDAO.GetDiscountCodeByAccount(TblAccountID, page, pageSize);


    }
}
