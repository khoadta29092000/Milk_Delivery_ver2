using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class TblMemberPoint
    {
        public string AccountId { get; set; } = null!;
        public int PurchaseTimes { get; set; }
        public int MemberPoints { get; set; }

        public virtual TblAccount Account { get; set; } = null!;
    }
}
