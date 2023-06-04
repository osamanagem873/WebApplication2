using WebApplication2.Data;

namespace WebApplication2.Models
{
    public class CartDetails
    {
       public int Id { get; set; }
       public int ShoppingCartId { get; set; }
       public int ItemsId { get; set; }
       public Items Items { get; set; }
       public Decimal UnitPrice { get; set; }
       public int Quantity { get; set; }

       public ShoppingCart ShoppingCart { get; set; }
    }
}
