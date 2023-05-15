using System;
using System.Collections.Generic;
namespace SubcriptionMilk.DTO

{
    public partial class AccountDTO
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; }
        public string? Address { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public bool Gender { get; set; }
        public string? ImageCard { get; set; }
        public int RoleId { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool isVerified { get; set; }
    }
}
