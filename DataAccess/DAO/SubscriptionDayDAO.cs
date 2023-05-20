using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;


namespace DataAccess.DAO
{
    public class SubscriptionDayDAO
    {
        private static SubscriptionDayDAO instance = null;
        private static readonly object instanceLock = new object();
        private SubscriptionDayDAO() { }
        public static SubscriptionDayDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new SubscriptionDayDAO();
                    }
                    return instance;
                }
            }
        }

        public static async Task<List<TblSubscriptionDay>> GetSubscriptionDay()
        {
            var SubscriptionDay = new List<TblSubscriptionDay>();

            try
            {
                using (var context = new MilkDBContext())
                {
                    SubscriptionDay = await context.TblSubscriptionDays.ToListAsync();

                }
                return SubscriptionDay;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public static async Task AddTblSubscriptionDay(TblSubscriptionDay m)
        {
            try
            {
                using (var context = new MilkDBContext())
                {
                    var p1 = await context.TblSubscriptionDays.FirstOrDefaultAsync(c => c.Title.Equals(m.Title));
                    var p2 = await context.TblSubscriptionDays.FirstOrDefaultAsync(c => c.Id.Equals(m.Id));
                    if (p1 == null)
                    {
                        if (p2 == null)
                        {
                            context.TblSubscriptionDays.Add(m);
                            await context.SaveChangesAsync();
                        }
                        else
                        {
                            throw new Exception("Id is Exits");
                        }
                    }
                    else
                    {
                        throw new Exception("SubscriptionDay Name is Exist");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task UpdateTblSubscriptionDay(TblSubscriptionDay m)
        {
            try
            {
                using (var context = new MilkDBContext())
                {
                    var duplicateName = await context.TblSubscriptionDays
                        .AnyAsync(c => c.Id != m.Id && c.Title.Equals(m.Title));

                    if (duplicateName)
                    {
                        throw new Exception("SubscriptionDayName already exists.");
                    }

                    var existingEntity = context.TblSubscriptionDays.Local.FirstOrDefault(e => e.Id == m.Id);
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

        public static async Task DeleteTblSubscriptionDay(string p)
        {
            try
            {
                using (var context = new MilkDBContext())
                {
                    var SubscriptionDay = await context.TblSubscriptionDays.FirstOrDefaultAsync(c => c.Id == p);
                    if (SubscriptionDay == null)
                    {
                        throw new Exception("Id is not Exits");
                    }
                    else
                    {
                        context.TblSubscriptionDays.Remove(SubscriptionDay);
                        await context.SaveChangesAsync();
                    }



                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static async Task<List<TblSubscriptionDay>> SearchByName( int page, int pageSize)
        {
            using (var context = new MilkDBContext())
            {
                List<TblSubscriptionDay> searchResult = null;
                IEnumerable<TblSubscriptionDay> searchValues = await GetSubscriptionDay();
                if (page == 0 || pageSize == 0)
                {
                    page = 1;
                    pageSize = 100000000;
                }           
                searchValues = searchValues.Skip((page - 1) * pageSize).Take(pageSize);
                searchResult = searchValues.ToList();
                return searchResult;
            }
        }
        
        public static async Task<TblSubscriptionDay> GetSubscriptionDayById(string SubscriptionDayId)
        {
            try
            {
                IEnumerable<TblSubscriptionDay> accounts = await GetSubscriptionDay();
                TblSubscriptionDay SubscriptionDay = accounts.SingleOrDefault(mb => mb.Id == SubscriptionDayId);
                return SubscriptionDay;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

       
    }
}
