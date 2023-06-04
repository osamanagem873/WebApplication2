using System.ComponentModel.DataAnnotations;
using WebApplication2.Models;

namespace WebApplication2.Data
{
    public class Items
    {
        public int Id { get; set; }
       
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string? CoverImageUrl { get; set; }
        public int Quantity { get; set; }
        [Range(0, 100)]
        public Decimal? Discount { get; set; }
        public Decimal Price { get; set; }
        public ICollection<ItemGallery> ItemGallery { get; set; }
        
        public List<OrderDetails> OrderDetails { get; set; }
        public List<CartDetails> CartDetails { get; set; }
    }
}