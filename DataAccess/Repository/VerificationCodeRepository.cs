using BusinessObject.Models;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
