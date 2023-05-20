using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class TblDiscountCode
    {
        public TblDiscountCode()
        {
            TblDiscountOfAccounts = new HashSet<TblDiscountOfAccount>();
            TblOrders = new HashSet<TblOrder>();
        }

        public string Id { get; set; } = null!;
        public string? CodeDescription { get; set; }
        public bool IsDeleted { get; set; }
        public int DiscountPercentage { get; set; }
        public int Points { get; set; }

        public virtual ICollection<TblDiscountOfAccount> TblDiscountOfAccounts { get; set; }
        public virtual ICollection<TblOrder> TblOrders { get; set; }
    }
}
