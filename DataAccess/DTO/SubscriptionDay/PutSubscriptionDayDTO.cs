
namespace DataAccess.DTO.SubscriptionDay
{
    public partial class PutSubscriptionDayDTO
    {
        public string Id { get; set; } = null!;
        public string? Description { get; set; }
        public int DiscountPercentage { get; set; }
        public string Title { get; set; } = null!;
    }
}
