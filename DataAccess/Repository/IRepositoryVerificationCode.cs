using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
