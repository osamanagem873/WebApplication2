using System.ComponentModel.DataAnnotations;
using WebApplication2.Data;

namespace WebApplication2.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int ItemsId { get; set; }
        public int Quantity { get; set; }
        public Decimal UnitPrice { get; set; }
        public Order Order { get; set; }
        public Items Items { get; set; }


    }
}
