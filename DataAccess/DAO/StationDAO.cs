using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;


namespace DataAccess.DAO
{
    public class StationDAO
    {
        private static StationDAO instance = null;
        private static readonly object instanceLock = new object();
        private StationDAO() { }
        public static StationDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new StationDAO();
                    }
                    return instance;
                }
            }
        }
    
        public static async Task<List<TblStation>> GetStation()
        {
            var Station = new List<TblStation>();

            try
            {
                using (var context = new MilkDBContext())
                {
                    Station = await context.TblStations.ToListAsync();

                }
                return Station;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public static async Task AddTblStation(TblStation m)
        {
            try
            {
                using (var context = new MilkDBContext())
                {
                    var p1 = await context.TblStations.FirstOrDefaultAsync(c => c.StationName.Equals(m.StationName));
                    var p2 = await context.TblStations.FirstOrDefaultAsync(c => c.Id.Equals(m.Id));
                    if (p1 == null)
                    {
                        if (p2 == null)
                        {
                            context.TblStations.Add(m);
                            await context.SaveChangesAsync();
                        }
                        else
                        {
                            throw new Exception("Id is Exits");
                        }
                    }
                    else
                    {
                        throw new Exception("Station Name is Exist");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task UpdateTblStation(TblStation m)
        {
            try
            {
                using (var context = new MilkDBContext())
                {
                    var duplicateName = await context.TblStations
                        .AnyAsync(c => c.Id != m.Id && c.StationName.Equals(m.StationName));

                    if (duplicateName)
                    {
                        throw new Exception("StationName already exists.");
                    }

                    var existingEntity = context.TblStations.Local.FirstOrDefault(e => e.Id == m.Id);
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

        public static async Task DeleteTblStation(string p)
        {
            try
            {
                using (var context = new MilkDBContext())
                {
                    var Station = await context.TblStations.FirstOrDefaultAsync(c => c.Id == p);
                    if (Station == null)
                    {
                        throw new Exception("Id is not Exits");
                    }
                    else
                    {
                        context.TblStations.Remove(Station);
                        await context.SaveChangesAsync();
                    }



                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static async Task<List<TblStation>> SearchByName(string? search,bool? isDeleted, int page, int pageSize)
        {
            using (var context = new MilkDBContext())
            {
                List<TblStation> searchResult = null;
                IEnumerable<TblStation> searchValues = await GetStation();
                if (page == 0 || pageSize == 0)
                {
                    page = 1;
                    pageSize = int.MaxValue;
                }
                if (search != null)
                {
                    searchValues = await (from Station in context.TblStations
                                          where Station.StationName.ToLower().Contains(search.ToLower()) 
                                          select Station).ToListAsync();

                }
                if (isDeleted != null)
                {
                    searchValues = await (from Station in context.TblStations
                                          where Station.IsDeleted.Equals(isDeleted)
                                          select Station).ToListAsync();

                }
                searchValues = searchValues.Skip((page - 1) * pageSize).Take(pageSize);
                searchResult = searchValues.ToList();
                return searchResult;
            }
        }
        public static async Task<List<TblAccount>> GetAccountsByStation(string stationId, int page, int pageSize)
        {
            try
            {
                using (var context = new MilkDBContext())
                {
                    List<TblAccount> searchResult = null;
                    IEnumerable<TblAccount> searchValues =  await context.TblAccounts.ToListAsync();
                    if (page == 0 || pageSize == 0)
                    {
                        page = 1;
                        pageSize = int.MaxValue;
                    }
                    if (stationId != null)
                    {
                         searchValues = await context.TblAccounts
                           .Where(a => a.StationId == stationId)
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
        public static async Task<List<TblOrder>> GetOrdersByStation(string stationId, int page, int pageSize)
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
        public static async Task<TblStation> GetStationById(string stationId)
        {
            try
            {         
                    IEnumerable<TblStation> accounts = await GetStation();
                    TblStation Station = accounts.SingleOrDefault(mb => mb.Id == stationId);                            
                    return Station;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    
        public static async Task UpdateActive(string stationId,bool active)
        {
            try
            {
                var user = new TblStation() { Id = stationId, IsDeleted = !active };
                using (var db = new MilkDBContext())
                {
                    db.TblStations.Attach(user);
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
