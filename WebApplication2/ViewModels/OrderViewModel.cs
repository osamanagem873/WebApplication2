using WebApplication2.Models;

namespace WebApplication2.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public int OrderStatusId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public bool IsDeleted { get; set; }
        public List<OrderDetails> OrderDetails { get; set; }
        public AppUser User { get; set; }
    }
}
