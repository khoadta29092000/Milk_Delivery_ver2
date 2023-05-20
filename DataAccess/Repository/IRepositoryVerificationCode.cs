using BusinessObject.Models;


namespace DataAccess.Repository
{
    public interface IRepositoryVerificationCode
    {
        Task<List<TblVerificationCode>> GetListVerificationCode();
        Task<TblVerificationCode> GetVerificationCodeById(string Id, string code);
        Task DeleteVerificationCode(int Id);

        Task AddVerificationCode(TblVerificationCode c);
    }
}
