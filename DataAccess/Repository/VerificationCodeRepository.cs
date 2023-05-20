using BusinessObject.Models;
using DataAccess.DAO;


namespace DataAccess.Repository
{
    public class VerificationCodeRepository : IRepositoryVerificationCode
    {
        public Task<List<TblVerificationCode>> GetListVerificationCode() => VerificationCodeDAO.GetListVerificationCode();
        public Task<TblVerificationCode> GetVerificationCodeById(string Id, string code) => VerificationCodeDAO.Instance.GetVerificationCodeById(Id, code);
        public Task DeleteVerificationCode(int Id) => VerificationCodeDAO.DeleteVerificationCode(Id);
        public Task AddVerificationCode(TblVerificationCode c) => VerificationCodeDAO.AddVerificationCode(c);
    }
}
