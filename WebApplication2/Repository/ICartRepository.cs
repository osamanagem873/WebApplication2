using WebApplication2.Models;

namespace WebApplication2.Repository
{
    public interface ICartRepository
    {
        Task<int> AddItem(int itemId, int qty);
        Task<int> RemoveItem(int itemId);
        Task<ShoppingCart> GetUserCart(string userId);
        Task<int> GetCartItemCount(string userId = "");
        Task<int> DeleteAllCartItems(string userId);
        Task<ShoppingCart> GetCart(string userId);
        Task<bool> DoCheckout();
        Task<int> DeleteCartItem(int itemId);
    }
}
