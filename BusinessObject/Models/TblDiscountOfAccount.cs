using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class TblDiscountOfAccount
    {
        public string AccountId { get; set; } = null!;
        public string DiscountCodeId { get; set; } = null!;
        public string Code { get; set; } = null!;
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Quantity { get; set; }

        public virtual TblAccount Account { get; set; } = null!;
        public virtual TblDiscountCode DiscountCode { get; set; } = null!;
    }
}
