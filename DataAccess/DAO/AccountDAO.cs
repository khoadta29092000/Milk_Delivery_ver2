using System.Text;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using DataAccess.DTO.Account;

namespace DataAccess.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance = null;
        private static readonly object instanceLock = new object();
        private AccountDAO() { }
        public static AccountDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new AccountDAO();
                    }
                    return instance;
                }
            }
        }


        public static async Task<List<TblAccount>> GetMembers()
        {
            var members = new List<TblAccount>();

            try
            {
                using (var context = new MilkDBContext())
                {
                    members = await context.TblAccounts.ToListAsync();

                }
                return members;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public static string GenerateHashedPassword(string password, string salt)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltBytes = Convert.FromBase64String(salt);

            byte[] hashedPasswordBytes;
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordWithSaltBytes = new byte[passwordBytes.Length + saltBytes.Length];
                Buffer.BlockCopy(passwordBytes, 0, passwordWithSaltBytes, 0, passwordBytes.Length);
                Buffer.BlockCopy(saltBytes, 0, passwordWithSaltBytes, passwordBytes.Length, saltBytes.Length);

                hashedPasswordBytes = sha256.ComputeHash(passwordWithSaltBytes);
            }

            return Convert.ToBase64String(hashedPasswordBytes);
        }


        public static async Task<TblAccount> Login(string email, string password)
        {

            IEnumerable<TblAccount> members = await GetMembers();
            TblAccount member = members.SingleOrDefault(mb => mb.Email.Equals(email) && GenerateHashedPassword(password, mb.SaltPassword) == mb.HashedPassword);
            return member;
        }
        public static async Task AddTblAccount(TblAccount m)
        {
            try
            {
                using (var context = new MilkDBContext())
                {
                    var p1 = await context.TblAccounts.FirstOrDefaultAsync(c => c.Email.Equals(m.Email));
                    var p2 = await context.TblAccounts.FirstOrDefaultAsync(c => c.Id.Equals(m.Id));
                    if (p1 == null)
                    {
                        if (p2 == null)
                        {
                            context.TblAccounts.Add(m);
                            await context.SaveChangesAsync();
                        }
                        else
                        {
                            throw new Exception("Id is Exits");
                        }
                    }
                    else
                    {
                        throw new Exception("Email is Exist");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task addTblAccountPoint(TblMemberPoint m)
        {
            try
            {
                using (var context = new MilkDBContext())
                {
                    context.TblMemberPoints.Add(m);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task UpdateTblAccount(TblAccount m)
        {
            try
            {
                using (var context = new MilkDBContext())
                {
                    // Kiểm tra xem đối tượng m đã được theo dõi trong context hay chưa
                    var existingEntity = context.TblAccounts.Local.FirstOrDefault(e => e.Id == m.Id);
                    if (existingEntity != null)
                    {
                        // Nếu đã được theo dõi, cập nhật trực tiếp trên đối tượng đó
                        context.Entry(existingEntity).CurrentValues.SetValues(m);
                    }
                    else
                    {
                        // Nếu chưa được theo dõi, đính kèm và đánh dấu là sửa đổi
                        context.Attach(m);
                        context.Entry(m).State = EntityState.Modified;
                    }

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task DeleteTblAccount(string p)
        {
            try
            {
                using (var context = new MilkDBContext())
                {
                    var member = await context.TblAccounts.FirstOrDefaultAsync(c => c.Id == p);
                    if (member == null)
                    {
                        throw new Exception("Id is not Exits");
                    }
                    else
                    {
                        context.TblAccounts.Remove(member);
                        await context.SaveChangesAsync();
                    }



                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static async Task<List<GetAllAccountDTO>> SearchByEmail(string? search, int RoleId, int page, int pageSize)
        {
            using (var context = new MilkDBContext())
            {
                List<TblAccount> searchResult = null;
                IEnumerable<TblAccount> searchValues = await GetMembers();
                if (page == 0 || pageSize == 0)
                {
                    page = 1;
                    pageSize = 100000000;
                }
                searchValues = await (from member in context.TblAccounts
                                      join point in context.TblMemberPoints on member.Id equals point.AccountId
                                      select new TblAccount
                                      {
                                          Id = member.Id,
                                          FullName = member.FullName,
                                          Email = member.Email,
                                          Address = member.Address,
                                          PhoneNumber = member.PhoneNumber,
                                          Gender = member.Gender,
                                          ImageCard = member.ImageCard,
                                          RoleId = member.RoleId,
                                          CreateDate = member.CreateDate,
                                          IsDeleted = member.IsDeleted,
                                          IsVerified = member.IsVerified,
                                          StationId = member.StationId,
                                          TblMemberPoint = new TblMemberPoint
                                          {
                                              PurchaseTimes = point.PurchaseTimes,
                                              MemberPoints = point.MemberPoints
                                          },
                                      }).ToListAsync();
                if (search != null)
                {
                    searchValues = await (from member in context.TblAccounts
                                          where member.Email.ToLower().Contains(search.ToLower())
                                          join point in context.TblMemberPoints on member.Id equals point.AccountId
                                          select new TblAccount
                                          {
                                              Id = member.Id,
                                              FullName = member.FullName,
                                              Email = member.Email,
                                              Address = member.Address,
                                              PhoneNumber = member.PhoneNumber,
                                              Gender = member.Gender,
                                              ImageCard = member.ImageCard,
                                              RoleId = member.RoleId,
                                              CreateDate = member.CreateDate,
                                              IsDeleted = member.IsDeleted,
                                              IsVerified = member.IsVerified,
                                              StationId = member.StationId,
                                              TblMemberPoint = new TblMemberPoint
                                              {
                                                  PurchaseTimes = point.PurchaseTimes,
                                                  MemberPoints = point.MemberPoints
                                              },
                                          }).ToListAsync();

                }
                if (RoleId != 0)
                {
                    searchValues = searchValues.Where(c => c.RoleId == RoleId).ToList();
                }
                searchValues = searchValues.Skip((page - 1) * pageSize).Take(pageSize);
                searchResult = searchValues.ToList();

                return searchResult.Select(member => new GetAllAccountDTO
                {
                    Id = member.Id,
                    FullName = member.FullName,
                    Email = member.Email,
                    Address = member.Address,
                    PhoneNumber = member.PhoneNumber,
                    Gender = member.Gender,
                    ImageCard = member.ImageCard,
                    RoleId = member.RoleId,
                    CreateDate = member.CreateDate,
                    IsDeleted = member.IsDeleted,
                    IsVerified = member.IsVerified,
                    StationId = member.StationId,
                    PurchaseTimes = member.TblMemberPoint.PurchaseTimes,
                    MemberPoints = member.TblMemberPoint.MemberPoints,
                }).ToList();
            }
        }
        public static async Task<TblAccount> GetProfile(string TblAccountID)
        {
            try
            {
                using (var context = new MilkDBContext())
                {
                    IEnumerable<TblAccount> accounts = await GetMembers();
                    TblAccount member = accounts.SingleOrDefault(mb => mb.Id == TblAccountID);
                    TblMemberPoint value = context.TblMemberPoints.SingleOrDefault(x => x.AccountId == member.Id);
                    TblAccount lastAccount = new TblAccount
                    {
                        Id = member.Id,
                        FullName = member.FullName,
                        Email = member.Email,
                        Address = member.Address,
                        PhoneNumber = member.PhoneNumber,
                        Gender = member.Gender,
                        ImageCard = member.ImageCard,
                        RoleId = member.RoleId,
                        CreateDate = member.CreateDate,
                        IsDeleted = member.IsDeleted,
                        IsVerified = member.IsVerified,
                        StationId = member.StationId,
                        TblMemberPoint = new TblMemberPoint
                        {
                            PurchaseTimes = value.PurchaseTimes,
                            MemberPoints = value.MemberPoints
                        }
                    };
                return lastAccount;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task<List<TblDiscountCode>> GetDiscountCodeByAccount(string TblAccountID, int page, int pageSize)
        {
            var searchResult = new List<TblDiscountCode>();
            List<TblDiscountCode> searchResult1 = null;

            // IEnumerable<MemberObject> searchResult = null;
            try
            {
                if (page == 0 || pageSize == 0)
                {
                    page = 1;
                    pageSize = int.MaxValue;
                }
                using (var context = new MilkDBContext())
                {

                    IEnumerable<TblDiscountCode> searchValues = await (from discountCode in context.TblDiscountCodes
                                                            where discountCode.TblDiscountOfAccounts.Any(c => c.AccountId == TblAccountID)
                                                            select discountCode).ToListAsync();
                    searchValues = searchValues.Skip((page - 1) * pageSize).Take(pageSize);
                    searchResult1 = searchValues.ToList();
                    // searchValues =  searchValues.Skip((page - 1) * pageSize).Take(pageSize);

                }
                return searchResult1;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static async Task ChangePassword(string TblAccountID, string password)
        {
            try
            {
                var user = new TblAccount() { Id = TblAccountID, HashedPassword = password };
                using (var db = new MilkDBContext())
                {
                    db.TblAccounts.Attach(user);
                    db.Entry(user).Property(x => x.HashedPassword).IsModified = true;
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task UpdateActive(string TblAccountID, bool acticve)
        {

            try
            {

                var user = new TblAccount() { Id = TblAccountID, IsDeleted = !acticve };
                using (var db = new MilkDBContext())
                {
                    db.TblAccounts.Attach(user);
                    db.Entry(user).Property(x => x.IsDeleted).IsModified = true;
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task VerificationAccount(string TblAccountID, bool acticve)
        {

            try
            {

                var user = new TblAccount() { Id = TblAccountID, IsVerified = true };
                using (var db = new MilkDBContext())
                {
                    db.TblAccounts.Attach(user);
                    db.Entry(user).Property(x => x.IsVerified).IsModified = true;
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
