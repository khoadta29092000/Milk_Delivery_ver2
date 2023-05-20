using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;


namespace DataAccess.DAO
{
    public class DiscountCodeDAO
    {
        private static DiscountCodeDAO instance = null;
        private static readonly object instanceLock = new object();
        private DiscountCodeDAO() { }
        public static DiscountCodeDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new DiscountCodeDAO();
                    }
                    return instance;
                }
            }
        }

        public static async Task<List<TblDiscountCode>> GetDiscountCode()
        {
            var DiscountCode = new List<TblDiscountCode>();

            try
            {
                using (var context = new MilkDBContext())
                {
                    DiscountCode = await context.TblDiscountCodes.ToListAsync();

                }
                return DiscountCode;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public static async Task AddTblDiscountCodeOfAccount(TblDiscountOfAccount m)
        {
            try
            {
                using (var context = new MilkDBContext())
                {
                    context.TblDiscountOfAccounts.Add(m);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task AddTblDiscountCode(TblDiscountCode m)
        {
            try
            {
                using (var context = new MilkDBContext())
                {
                    var p1 = await context.TblDiscountCodes.FirstOrDefaultAsync(c => c.Points.Equals(m.Points) && c.DiscountPercentage.Equals(m.DiscountPercentage));
                    var p2 = await context.TblDiscountCodes.FirstOrDefaultAsync(c => c.Id.Equals(m.Id));
                    if(p1 == null)
                    {
                        if (p2 == null)
                        {
                            context.TblDiscountCodes.Add(m);
                            await context.SaveChangesAsync();
                        }
                        else
                        {
                            throw new Exception("Id is Exits");
                        }
                    }
                    else
                    {
                        throw new Exception("Discount Code is Exits");
                    }
                  

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task UpdateTblDiscountCode(TblDiscountCode m)
        {
            try
            {
                using (var context = new MilkDBContext())
                {


                    var existingEntity = context.TblDiscountCodes.Local.FirstOrDefault(e => e.Id == m.Id);
                    if (existingEntity != null)
                    {
                        context.Entry(existingEntity).CurrentValues.SetValues(m);
                    }
                    else
                    {
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

        public static async Task DeleteTblDiscountCode(string p)
        {
            try
            {
                using (var context = new MilkDBContext())
                {
                    var DiscountCode = await context.TblDiscountCodes.FirstOrDefaultAsync(c => c.Id == p);
                    if (DiscountCode == null)
                    {
                        throw new Exception("Id is not Exits");
                    }
                    else
                    {
                        context.TblDiscountCodes.Remove(DiscountCode);
                        await context.SaveChangesAsync();
                    }



                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static async Task<List<TblDiscountCode>> SearchByName(int minPoint, int maxPoint, bool? isDeleted, int page, int pageSize)
        {
            using (var context = new MilkDBContext())
            {
                List<TblDiscountCode> searchResult = null;
                IEnumerable<TblDiscountCode> searchValues = await GetDiscountCode();

                if (page == 0 || pageSize == 0)
                {
                    page = 1;
                    pageSize = int.MaxValue; // Set pageSize to a large value or use another appropriate value
                }

                if (isDeleted != null)
                {
                    searchValues = searchValues.Where(discountCode => discountCode.IsDeleted.Equals(isDeleted));
                }              
                if (maxPoint != 0)
                {
                    searchValues = searchValues.Where(discountCode => discountCode.Points >= minPoint && discountCode.Points <= maxPoint);
                }
             

                searchValues = searchValues.Skip((page - 1) * pageSize).Take(pageSize);
                searchResult = searchValues.ToList();

                return searchResult;
            }
        }

        public static async Task<List<TblOrder>> GetOrdersByDiscountCode(string stationId, int page, int pageSize)
        {
            try
            {
                using (var context = new MilkDBContext())
                {
                    List<TblOrder> searchResult = null;
                    IEnumerable<TblOrder> searchValues = await context.TblOrders.ToListAsync();
                    if (page == 0 || pageSize == 0)
                    {
                        page = 1;
                        pageSize = int.MaxValue;
                    }
                    if (stationId != null)
                    {
                        searchValues = await context.TblOrders
                      .Where(o => o.StationId == stationId)
                      .ToListAsync();

                    }
                    searchValues = searchValues.Skip((page - 1) * pageSize).Take(pageSize);
                    searchResult = searchValues.ToList();
                    return searchResult;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task<List<TblDiscountOfAccount>> GetDiscountCodeOfAccount(string? AccountID, string? DiscountCodeId, int page, int pageSize)
        {
            try
            {
                using (var context = new MilkDBContext())
                {
                    List<TblDiscountOfAccount> searchResult = null;
                    IEnumerable<TblDiscountOfAccount> searchValues = await context.TblDiscountOfAccounts.ToListAsync();
                    if (page == 0 || pageSize == 0)
                    {
                        page = 1;
                        pageSize = int.MaxValue;
                    }
                    if (AccountID != null)
                    {
                        searchValues = await (from discountCode in context.TblDiscountOfAccounts
                                              where discountCode.AccountId.ToLower().Contains(AccountID.ToLower())
                                              select discountCode
                                              ).ToListAsync();
                    }

                    if (DiscountCodeId != null)
                    {
                        searchValues = await (from discountCode in context.TblDiscountOfAccounts
                                              where discountCode.DiscountCodeId.ToLower().Contains(DiscountCodeId.ToLower())
                                              select discountCode
                                                ).ToListAsync();
                    }
                    searchValues = searchValues.Skip((page - 1) * pageSize).Take(pageSize);
                    searchResult = searchValues.ToList();
                    return searchResult;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task<List<TblAccount>> GetAllAccountBuyDiscountCode(string DiscountCodeId, int page, int pageSize)
        {
            try
            {
                using (var context = new MilkDBContext())
                {
                    List<TblAccount> searchResult = null;
                    if (page == 0 || pageSize == 0)
                    {
                        page = 1;
                        pageSize = int.MaxValue;
                    }
            
                    if (DiscountCodeId != null)
                    {
                        IEnumerable<TblAccount> searchValues = await (from account in context.TblAccounts
                                                                where account.TblDiscountOfAccounts.Any(c => c.DiscountCodeId == DiscountCodeId)
                                                                select account).ToListAsync();
                        searchValues = searchValues.Skip((page - 1) * pageSize).Take(pageSize);
                        searchResult = searchValues.ToList();
                    }            
                    return searchResult;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task<List<TblDiscountCode>> GetAllDiscountCodeBuyAccount(string AccountID, int page, int pageSize)
        {
            try
            {
                using (var context = new MilkDBContext())
                {
                    List<TblDiscountCode> searchResult = null;
                    if (page == 0 || pageSize == 0)
                    {
                        page = 1;
                        pageSize = int.MaxValue;
                    }

                    if (AccountID != null)
                    {
                        IEnumerable<TblDiscountCode> searchValues = await (from discountCode in context.TblDiscountCodes
                                                                      where discountCode.TblDiscountOfAccounts.Any(c => c.AccountId == AccountID)
                                                                      select discountCode).ToListAsync();
                        searchValues = searchValues.Skip((page - 1) * pageSize).Take(pageSize);
                        searchResult = searchValues.ToList();
                    }
                    return searchResult;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task<TblDiscountCode> GetDiscountCodeById(string DiscountCodeId)
        {
            try
            {
                IEnumerable<TblDiscountCode> accounts = await GetDiscountCode();
                TblDiscountCode DiscountCode = accounts.SingleOrDefault(mb => mb.Id == DiscountCodeId);
                return DiscountCode;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task UpdateActive(string DiscountCodeId, bool active)
        {
            try
            {
                var user = new TblDiscountCode() { Id = DiscountCodeId, IsDeleted = !active };
                using (var db = new MilkDBContext())
                {
                    db.TblDiscountCodes.Attach(user);
                    db.Entry(user).Property(x => x.IsDeleted).IsModified = true;
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
