using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;

namespace DataAccess.Repository
{
    public interface IRepositoryAccount
    {
         Task<List<TblAccount>>  GetMembers();
         Task<TblAccount>  LoginMember(string email,string password);
         Task<TblAccount>  GetProfile(string AccountID);
         Task ChangePassword(string AccountID, string password);
         Task UpdateActive(string AccountID, bool active);
         Task DeleteMember(string m);

         Task UpdateMember(TblAccount m);
         Task AddMember(TblAccount m);
         Task<List<TblAccount>> SearchByEmail(string? search,int RoleId, int page, int pageSize);
    
    }
}
