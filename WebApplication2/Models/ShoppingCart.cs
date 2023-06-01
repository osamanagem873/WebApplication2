using WebApplication2.Data;

namespace WebApplication2.Models
{
    public class ShoppingCart
    {

        public int Id { get; set; }

        public string UserId { get; set; }

        public bool IsDeleted { get; set; } = false;

        public ICollection<CartDetails> CartDetails { get; set; }


    }
}

