using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class TblOrder
    {
        public string Id { get; set; } = null!;
        public string AccountId { get; set; } = null!;
        public string PackageId { get; set; } = null!;
        public string? DiscountCodeId { get; set; }
        public string? Address { get; set; }
        public string? OrderDetailD { get; set; }
        public int? PriceAfterDiscount { get; set; }
        public int? Price { get; set; }
        public string? Phone { get; set; }
        public DateTime? EndDate { get; set; }
        public string? AdditionalRequest { get; set; }
        public DateTime? StartDate { get; set; }
        public string StationId { get; set; } = null!;

        public virtual TblAccount Account { get; set; } = null!;
        public virtual TblDiscountCode? DiscountCode { get; set; }
        public virtual TblOrderDetail? OrderDetailDNavigation { get; set; }
        public virtual TblPackage Package { get; set; } = null!;
        public virtual TblStation Station { get; set; } = null!;
    }
}
