using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class TblPackage
    {
        public TblPackage()
        {
            TblOrders = new HashSet<TblOrder>();
            TblPackageSettings = new HashSet<TblPackageSetting>();
            SubscriptionDays = new HashSet<TblSubscriptionDay>();
        }

        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public bool? IsDeleted { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<TblOrder> TblOrders { get; set; }
        public virtual ICollection<TblPackageSetting> TblPackageSettings { get; set; }

        public virtual ICollection<TblSubscriptionDay> SubscriptionDays { get; set; }
    }
}
