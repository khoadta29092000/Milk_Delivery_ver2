

namespace DataAccess.DTO.Account

{
    public partial class GetAllAccountDTO
    {
        public string Id { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Address { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public bool Gender { get; set; }
        public string? ImageCard { get; set; }
        public int RoleId { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool? IsVerified { get; set; }
        public string? StationId { get; set; }
        public int PurchaseTimes { get; set; }
        public int MemberPoints { get; set; }
    }
}
