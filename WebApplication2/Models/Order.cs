using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        public int OrderStatusId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public bool IsDeleted { get; set; }
        public List<OrderDetails> OrderDetails { get; set; }
        public AppUser User { get; set; }
        
    }
}
