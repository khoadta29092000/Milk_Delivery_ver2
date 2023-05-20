
namespace DataAccess.DTO.SubscriptionDay
{
    public partial class PostSubscriptionDayDTO
    {
        public string? Description { get; set; }
        public int DiscountPercentage { get; set; }
        public string Title { get; set; } = null!;
    }
}
