using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class TblAccount
    {
        public TblAccount()
        {
            TblVerificationCodes = new HashSet<TblVerificationCode>();
        }

        public string Id { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? HashedPassword { get; set; }
        public string? SaltPassword { get; set; }
        public string? Address { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public bool Gender { get; set; }
        public string? ImageCard { get; set; }
        public int RoleId { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsVerified { get; set; }

        public virtual TblRole? Role { get; set; }
        public virtual ICollection<TblVerificationCode> TblVerificationCodes { get; set; }
    }
}
