using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class TblStation
    {
        public TblStation()
        {
            TblAccounts = new HashSet<TblAccount>();
            TblOrders = new HashSet<TblOrder>();
        }

        public string Id { get; set; } = null!;
        public string StationAddress { get; set; } = null!;
        public string StationName { get; set; } = null!;
        public string StationDescription { get; set; } = null!;
        public bool IsDeleted { get; set; }

        public virtual ICollection<TblAccount> TblAccounts { get; set; }
        public virtual ICollection<TblOrder> TblOrders { get; set; }
    }
}
