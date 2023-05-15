using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class TblVerificationCode
    {
        public int Id { get; set; }
        public string AccountId { get; set; } = null!;
        public string Code { get; set; } = null!;
        public DateTime ExpirationTime { get; set; }

        public virtual TblAccount? Account { get; set; }
    }
}
