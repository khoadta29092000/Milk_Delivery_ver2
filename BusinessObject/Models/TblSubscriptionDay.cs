using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class TblSubscriptionDay
    {
        public TblSubscriptionDay()
        {
            Packages = new HashSet<TblPackage>();
        }

        public string Id { get; set; } = null!;
        public string? Description { get; set; }
        public int DiscountPercentage { get; set; }
        public string Title { get; set; } = null!;

        public virtual ICollection<TblPackage> Packages { get; set; }
    }
}
