using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace DataAccess.DAO
{
    public class VerificationCodeDAO
    {
        private static VerificationCodeDAO instance = null;
        private static readonly object instanceLock = new object();
        private VerificationCodeDAO() { }
        public static VerificationCodeDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new VerificationCodeDAO();
                    }
                    return instance;
                }
            }
        }
        public static async Task<List<TblVerificationCode>> GetListVerificationCode()
        {
            var codes = new List<TblVerificationCode>();

            try
            {
                using (var context = new MilkDBContext())
                {
                    codes = await context.TblVerificationCodes.ToListAsync();

                }
                return codes;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        } 

            public async Task<TblVerificationCode> GetVerificationCodeById(string AccountID, string verificationCode)
        {

            IEnumerable<TblVerificationCode> codes = await GetListVerificationCode();
                TblVerificationCode code = codes.LastOrDefault(mb => mb.AccountId == AccountID && mb.Code == verificationCode);
            return code;
        }
        public static async Task AddVerificationCode(TblVerificationCode m)
        {
            try
            {
                using (var context = new MilkDBContext())
                {                

                            context.TblVerificationCodes.Add(m);
                            await context.SaveChangesAsync();                
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task DeleteVerificationCode(int Id)
        {
            try
            {
                using (var context = new MilkDBContext())
                {
                    var code = await context.TblVerificationCodes.FirstOrDefaultAsync(c => c.Id == Id);
                    if (code == null)
                    {
                        throw new Exception("Id is not Exits");
                    }
                    else
                    {
                        context.TblVerificationCodes.Remove(code);
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
       
 
    }
}
