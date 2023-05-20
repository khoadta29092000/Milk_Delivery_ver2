using BusinessObject.Models;
using DataAccess.DTO.Account;

namespace DataAccess.Repository
{
    public interface IRepositoryAccount
    {
        Task<List<TblAccount>> GetMembers();
        Task<TblAccount> LoginMember(string email, string password);
        Task<TblAccount> GetProfile(string AccountID);
        Task ChangePassword(string AccountID, string password);
        Task UpdateActive(string AccountID, bool active);
        Task VerificationAccount(string AccountID, bool active);
        Task DeleteMember(string m);
        Task UpdateMember(TblAccount m);
        Task AddMember(TblAccount m);
        Task AddMemberPoint(TblMemberPoint m);
        Task<List<GetAllAccountDTO>> SearchByEmail(string? search, int RoleId, int page, int pageSize);
        Task<List<TblDiscountCode>> GetDiscountCodeByAccount(string TblAccountID, int page, int pageSize);

    }
}
