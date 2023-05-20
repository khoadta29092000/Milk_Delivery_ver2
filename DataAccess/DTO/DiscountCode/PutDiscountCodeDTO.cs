

namespace DataAccess.DTO.DiscountCode
{
    public partial class PutDiscountCodeDTO
    {
        public string Id { get; set; } = null!;
        public string? CodeDescription { get; set; }
        public int DiscountPercentage { get; set; }
        public int Points { get; set; }
    }
}
